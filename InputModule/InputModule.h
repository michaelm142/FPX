#ifndef _INPUT_MODULE_H
#define _INPUT_MODULE_H

#include <windows.h>
#include <dinput.h>
#include <consoleapi3.h>
#include <list>

#define KEYBOARD_BUFFER_SIZE 256
#define MAX_INPUT_DEVICES 8

#define EXPORT extern "C" _declspec(dllexport)

typedef unsigned int uint;

HWND hwnd;
HINSTANCE hinstance;

static IDirectInput8* dInput = (IDirectInput8*)(NULL);

struct InputDevice
{
	IDirectInputDevice8* pDevice;
	uint deviceType;
	uint deviceSubType;
	uint pad;
	DIDEVICEINSTANCE dvInfo;

	union
	{
		char keys[KEYBOARD_BUFFER_SIZE];
		DIJOYSTATE joypadState1;
		DIJOYSTATE2 joypadState2;
		DIMOUSESTATE mouseState;
	} deviceState;
};

static union
{
	InputDevice* a[MAX_INPUT_DEVICES];
	struct
	{
		InputDevice* mouse0;
		InputDevice* mouse1;
		InputDevice* mouse2;
		InputDevice* mouse3;
		InputDevice* mouse4;
		InputDevice* mouse5;
		InputDevice* mouse6;
		InputDevice* mouse7;
	};
} connectedMouses;

static union
{
	InputDevice* a[MAX_INPUT_DEVICES];
	struct
	{
		InputDevice* keyboard0;
		InputDevice* keyboard1;
		InputDevice* keyboard2;
		InputDevice* keyboard3;
		InputDevice* keyboard4;
		InputDevice* keyboard5;
		InputDevice* keyboard6;
		InputDevice* keyboard7;
	};
} connectedKeyboards;

static union
{
	InputDevice* a[MAX_INPUT_DEVICES];
	struct
	{
		InputDevice* Gamepad0;
		InputDevice* Gamepad1;
		InputDevice* Gamepad2;
		InputDevice* Gamepad3;
		InputDevice* Gamepad4;
		InputDevice* Gamepad5;
		InputDevice* Gamepad6;
		InputDevice* Gamepad7;
	};
} connectedGamepads;

EXPORT void __stdcall InputUpdate();
EXPORT int __stdcall InitializeInputModule(int); 
EXPORT BOOL _stdcall IsKeyDown(int);
EXPORT void _stdcall GetMouseButtons(int*);
EXPORT void _stdcall GetMouseDeltas(LONG*);
EXPORT void _stdcall GetMousePosition(int*);
EXPORT void _stdcall GetGamepadState(void*);

InputDevice* FindDeviceByType(uint type, uint index = UINT_MAX);
void Dispose();
HRESULT ProcessDevice(InputDevice* pDevice);
void UnaquireDevice(InputDevice* pDevice);
void AddMouse(InputDevice* pMouse);
void AddKeyboard(InputDevice* pKeyboard);
void AddGamepad(InputDevice* pGamepad);

static std::list<InputDevice*> attachedDevices;

BOOL CALLBACK DIEnumDevicesCallback(LPCDIDEVICEINSTANCE lpddi, PVOID pvRef);
BOOL CALLBACK DIEnumDeviceObjectsCallback(LPCDIDEVICEOBJECTINSTANCE lpddoi, LPVOID pvRef);
#endif