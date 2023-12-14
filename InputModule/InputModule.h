#ifndef _INPUT_MODULE_H
#define _INPUT_MODULE_H

#pragma region DECL
#include <windows.h>
#include <dinput.h>
#include <list>
#include <XInput.h>

#define KEYBOARD_BUFFER_SIZE 256
#define MAX_INPUT_DEVICES 8
#define CHECK_CONNECTED_DEVICE_INTERVAL 5

#define EXPORT extern "C" _declspec(dllexport)

typedef unsigned int uint;

struct InputDevice
{
	IDirectInputDevice8* pDevice;
	uint deviceType;
	uint deviceSubType;
	uint deviceIndex;
	DIDEVICEINSTANCE dvInfo;

	union
	{
		char keys[KEYBOARD_BUFFER_SIZE];
		DIJOYSTATE joypadState1;
		DIJOYSTATE2 joypadState2;
		DIMOUSESTATE mouseState;
		XINPUT_STATE xInputState;
	} deviceState;
};
#pragma endregion

#pragma region Fields
struct
{
	HWND hwnd;
	HINSTANCE hinstance;
	IDirectInput8* dInput;
}app;


static std::list<InputDevice*> attachedDevices;

static union
{
	InputDevice* a[MAX_INPUT_DEVICES];
	struct
	{
		InputDevice* device0;
		InputDevice* device1;
		InputDevice* device2;
		InputDevice* device3;
		InputDevice* device4;
		InputDevice* device5;
		InputDevice* device6;
		InputDevice* device7;
	};
} connectedMouses, connectedKeyboards, connectedGamepads;
#pragma endregion

#pragma region External
EXPORT void __stdcall InputUpdate();
EXPORT int __stdcall InitializeInputModule(int); 
EXPORT BOOL _stdcall IsKeyDown(int);
EXPORT void _stdcall GetMouseState(void*);
EXPORT void _stdcall GetMousePosition(int*);
EXPORT void _stdcall GetGamepadState(void*, uint index = 0);
EXPORT void _stdcall Close();
EXPORT BOOL _stdcall IsDeviceConnected(uint type, uint index);
#pragma endregion

#pragma region Devices
void QueryDevices();
InputDevice* FindDeviceByType(uint type, uint index = UINT_MAX);
HRESULT ProcessDevice(InputDevice* pDevice);
void UnaquireDevice(InputDevice* pDevice);

void AddMouse(InputDevice* pMouse);
void AddKeyboard(InputDevice* pKeyboard);
void AddGamepad(InputDevice* pGamepad);

void RemoveMouse(InputDevice* pMouse);
void RemoveKeyboard(InputDevice* pKeyboard);
void RemoveGamepad(InputDevice* pGamepad);
#pragma endregion

#pragma region Enum
BOOL CALLBACK DIEnumDevicesCallback(LPCDIDEVICEINSTANCE lpddi, PVOID pvRef);
BOOL CALLBACK DIEnumDeviceObjectsCallback(LPCDIDEVICEOBJECTINSTANCE lpddoi, LPVOID pvRef);
#pragma endregion

#endif