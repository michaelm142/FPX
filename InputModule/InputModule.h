#ifndef _INPUT_MODULE_H
#define _INPUT_MODULE_H

#include <windows.h>
#include <dinput.h>
#include <consoleapi3.h>
#include <list>

#define KEYBOARD_BUFFER_SIZE 256

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

EXPORT void __stdcall InputUpdate();
EXPORT int __stdcall InitializeInputModule(int); 
EXPORT BOOL _stdcall IsKeyDown(int);
EXPORT void _stdcall GetMouseButtons(int*);
EXPORT void _stdcall GetMouseDeltas(LONG*);
EXPORT void _stdcall GetMousePosition(int*);

InputDevice* FindDeviceByType(uint type);
void Dispose();
HRESULT ProcessDevice(InputDevice* pDevice);
void UnaquireDevice(InputDevice* pDevice);

static std::list<InputDevice*> attachedDevices;

BOOL CALLBACK DIEnumDevicesCallback(LPCDIDEVICEINSTANCE lpddi, PVOID pvRef);
BOOL CALLBACK DIEnumDeviceObjectsCallback(LPCDIDEVICEOBJECTINSTANCE lpddoi, LPVOID pvRef);
#endif