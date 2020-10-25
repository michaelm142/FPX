#ifndef _INPUT_MODULE_H
#define _INPUT_MODULE_H

#include <windows.h>
#include <dinput.h>
#include <consoleapi3.h>
#include <list>

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

	DIJOYSTATE joypadState1;
};

void Loop();
void Initialize();
void Dispose();
HRESULT ProcessDevice(InputDevice* pDevice);
void UnaquireDevice(InputDevice* pDevice);

static std::list<InputDevice*> attachedDevices;

BOOL CALLBACK DIEnumDevicesCallback(LPCDIDEVICEINSTANCE lpddi, PVOID pvRef); 
BOOL CALLBACK DIEnumDeviceObjectsCallback(LPCDIDEVICEOBJECTINSTANCE lpddoi, LPVOID pvRef);
#endif