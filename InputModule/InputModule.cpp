// InputModule.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <time.h>
#include <assert.h>

#include "InputModule.h"

BOOL running = true;

EXPORT int _stdcall InitializeInputModule(int h_wnd)
{
	hwnd = (HWND)h_wnd;
	hinstance = GetModuleHandle(0);

	HRESULT hr = DirectInput8Create(hinstance, DIRECTINPUT_VERSION, IID_IDirectInput8A, (void**)&dInput, NULL);
	assert(SUCCEEDED(hr) && "Failed to initialize direct input");
	hr = dInput->Initialize(hinstance, DIRECTINPUT_VERSION);
	assert(SUCCEEDED(hr) && "Failed to initialize direct input");


	dInput->EnumDevices(DI8DEVCLASS_GAMECTRL, DIEnumDevicesCallback, &dInput, DIEDFL_ATTACHEDONLY);
	dInput->EnumDevices(DI8DEVCLASS_KEYBOARD, DIEnumDevicesCallback, &dInput, DIEDFL_ATTACHEDONLY);
	dInput->EnumDevices(DI8DEVCLASS_POINTER, DIEnumDevicesCallback, &dInput, DIEDFL_ATTACHEDONLY);

	std::cout << "Input Module Initialized Sucessfully" << std::endl;
	return hr;
}

int main()
{
	InitializeInputModule((int)GetConsoleWindow());

	std::cin.get();
	while (running)
	{
		InputUpdate();
	}

	Dispose();
}

void Dispose()
{
	dInput->Release();
	while (attachedDevices.size() > 0)
	{
		UnaquireDevice(*attachedDevices.begin());
		attachedDevices.erase(attachedDevices.begin());
	}
}

void UnaquireDevice(InputDevice* pDevice)
{
	std::cout << "Device " << (char*)pDevice->dvInfo.tszInstanceName << " unaquired" << std::endl;

	pDevice->pDevice->Unacquire();
	pDevice->pDevice->Release();
	delete pDevice;
}

EXPORT void __stdcall InputUpdate()
{
	static long double totalElapsed = 0.0;
	static HRESULT hr = S_OK;

	clock_t now = clock();
	static clock_t then = now;
	clock_t delta = now - then;
	long double elapsed = (long double)delta / CLOCKS_PER_SEC;
	totalElapsed += elapsed;

	if (GetAsyncKeyState(VK_ESCAPE))
		running = false;
	for (auto i = attachedDevices.begin(); i != attachedDevices.end();)
	{
		InputDevice* pDevice = *i;
		hr = ProcessDevice(pDevice);

		if (hr == DIERR_INPUTLOST)
		{
			UnaquireDevice(pDevice);
			i = attachedDevices.erase(i);
		}
		else
			++i;
	}

#ifdef _IMOUTPUT_VERBOSE
	std::cout << "Num devices: " << attachedDevices.size() << std::endl;

	system("cls");
#endif

	then = now;
}

HRESULT ProcessDevice(InputDevice* pDevice)
{
	static HRESULT hr = S_OK;
	pDevice->pDevice->Poll();
	switch (pDevice->deviceType)
	{
	case DI8DEVTYPE_1STPERSON:
	case DI8DEVTYPE_GAMEPAD:
		hr = pDevice->pDevice->GetDeviceState(sizeof(DIJOYSTATE), (void*)&pDevice->deviceState);
		if (hr == DIERR_INPUTLOST)
			return hr;

		auto gamepadState = pDevice->deviceState.joypadState1;

#ifdef _IMOUTPUT_VERBOSE
		std::cout << "Device Name:" << (char*)pDevice->dvInfo.tszInstanceName << std::endl;

		std::cout << "Device Left Stick:\t {" << gamepadState.lX << ":" << gamepadState.lY << "}" << std::endl;
		std::cout << "Device Right Stick:\t {" << gamepadState.lRx << ":" << gamepadState.lRy << "}" << std::endl;
		std::cout << "Device Slider A:" << gamepadState.rglSlider[0] << std::endl;
		std::cout << "Device Slider B:" << gamepadState.rglSlider[1] << std::endl;
		for (int i = 0; i < 10; i++)
		{
			std::cout << "Button[" << i << "]:" << (gamepadState.rgbButtons[i] == 0 ? "Up" : "Down") << std::endl;
		}
		for (uint i = 0; i < 4; i++)
		{
			std::cout << "6DOF Axis " << i << ":" << gamepadState.rgdwPOV[i] << std::endl;
		}
#endif
		break;
	case DI8DEVTYPE_MOUSE:
		hr = pDevice->pDevice->GetDeviceState(sizeof(DIMOUSESTATE), (void*)&pDevice->deviceState);
		if (hr == DIERR_INPUTLOST)
			return hr;

		auto mouseState = pDevice->deviceState.mouseState;
#ifdef _IMOUTPUT_VERBOSE
		std::cout << "Device Name:" << (char*)pDevice->dvInfo.tszInstanceName << std::endl;

		std::cout << "Delta Position:\t{ " << mouseState.lX << ":" << mouseState.lY << " }" << std::endl;
		std::cout << "Wheel Delta: " << (mouseState.lZ % 120) << std::endl;
		for (uint i = 0; i < 4; i++)
			std::cout << "Button[" << i << "]" << (mouseState.rgbButtons[i] == 0 ? "Up" : "Down") << std::endl;
#endif
		break;
	case DI8DEVTYPE_KEYBOARD:
		hr = pDevice->pDevice->GetDeviceState(KEYBOARD_BUFFER_SIZE, (void*)&pDevice->deviceState);
		if (hr == DIERR_INPUTLOST)
			return hr;


#if _IMOUTPUT_VERBOSE
		std::cout << "Device Name:" << (char*)pDevice->dvInfo.tszInstanceName << std::endl;

		std::cout << "{ ";
		for (uint i = 0; i < KEYBOARD_BUFFER_SIZE; i++)
			if (pDevice->deviceState.keys[i] != 0)
				std::cout << char(i) << " ,";
		std::cout << " } keys are down" << std::endl;
#endif
		break;
	}

	return S_OK;
}

EXPORT BOOL _stdcall IsKeyDown(int keycode)
{
	for (auto i = attachedDevices.begin(); i != attachedDevices.end(); i++)
		if ((*i)->deviceType == DI8DEVTYPE_KEYBOARD)
			return (*i)->deviceState.keys[keycode] != 0;

	return false;
}

BOOL CALLBACK DIEnumDevicesCallback(LPCDIDEVICEINSTANCE lpddi, PVOID pvRef)
{
	for (auto i = attachedDevices.begin(); i != attachedDevices.end(); i++)
	{
		if ((*i)->dvInfo.guidInstance == lpddi->guidInstance)
			return true;
	}
	//std::cout << "Device Detected " << " Name: " << (char*)lpddi->tszInstanceName << std::endl;

	uint fullByte = 0xFF;
	uint deviceType = fullByte & lpddi->dwDevType;
	uint deviceSubType = (fullByte << 8) & lpddi->dwDevType;
	deviceSubType = deviceSubType >> 8;

	IDirectInputDevice8* device = nullptr;
	HRESULT hr = dInput->CreateDevice(lpddi->guidInstance, &device, NULL);
	assert(SUCCEEDED(hr) && "Failed to create input device");
	switch (deviceType)
	{
	case DI8DEVTYPE_GAMEPAD:
	case DI8DEVTYPE_1STPERSON:
		//std::cout << '\t' << "Device " << (char*)lpddi->tszInstanceName << " is a gamepad" << std::endl;
		if (deviceSubType == DI8DEVTYPE1STPERSON_SIXDOF)
		{
			//std::cout << "\t\t-has 6DOF" << std::endl;
			device->SetDataFormat(&c_dfDIJoystick);
		}
		else
			device->SetDataFormat(&c_dfDIJoystick);
		break;
	case DI8DEVTYPE_KEYBOARD:
		//std::cout << '\t' << "Device " << (char*)lpddi->tszInstanceName << " is a keyboard" << std::endl;
		device->SetDataFormat(&c_dfDIKeyboard);
		break;
	case DI8DEVTYPE_MOUSE:
		//std::cout << '\t' << "Device " << (char*)lpddi->tszInstanceName << " is a mouse" << std::endl;
		device->SetDataFormat(&c_dfDIMouse);
		break;
	}
	hr = device->Acquire();
	assert(SUCCEEDED(hr) && "Failed to aquire device");
	// Enum Properties
	//std::cout << "\tDevice Properties:" << std::endl;
	//device->EnumObjects(DIEnumDeviceObjectsCallback, device, DIDFT_ALL);

#if 0
	DIACTIONFORMATW actionFormat;

	actionFormat.dwSize = sizeof(DIACTIONFORMATW);
	actionFormat.dwActionSize = sizeof(DIACTION);
	wchar_t UserName[] = L"User";

	hr = device->BuildActionMap(&actionFormat, UserName, DIDBAM_PRESERVE);
	assert(SUCCEEDED(hr) && "Failed to build action map for device");
	std::cout << "\tDevice has " << actionFormat.dwNumActions << " actions" << std::endl;
	for (uint i = 0; i < actionFormat.dwNumActions; i++)
	{
		DIACTION action = actionFormat.rgoAction[i];
		std::cout << "\t\tName: " << action.lptszActionName << std::endl;
}
#endif

	InputDevice* pInputDevice = new InputDevice;
	memset(pInputDevice, 0, sizeof(InputDevice));
	pInputDevice->dvInfo = *lpddi;

	pInputDevice->pDevice = device;
	pInputDevice->deviceType = deviceType;
	pInputDevice->deviceSubType = deviceSubType;
	attachedDevices.push_back(pInputDevice);

	//std::cout << "Initialized device " << (char*)lpddi->tszInstanceName << std::endl;



	return true;
}

BOOL CALLBACK DIEnumDeviceObjectsCallback(LPCDIDEVICEOBJECTINSTANCE lpddoi, LPVOID pvRef)
{
	IDirectInputDevice8* device = (IDirectInputDevice8*)pvRef;
	std::cout << "\t\tName:" << (char*)lpddoi->tszName;
	std::cout << std::endl;

	return true;
}