#ifndef _INPUT_MODULE_H
#define _INPUT_MODULE_H

#include <windows.h>
#include <dinput.h>
#include <consoleapi3.h>
#include <vector>

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
void ProcessDevice(InputDevice* pDevice);
void Dispose();

static std::vector<InputDevice*> attachedDevices;

BOOL CALLBACK DIEnumDevicesCallback(LPCDIDEVICEINSTANCE lpddi, PVOID pvRef); 
BOOL CALLBACK DIEnumDeviceObjectsCallback(LPCDIDEVICEOBJECTINSTANCE lpddoi, LPVOID pvRef);
#endif