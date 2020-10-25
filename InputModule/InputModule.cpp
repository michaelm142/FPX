// InputModule.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <time.h>
#include <assert.h>

#include "InputModule.h"

BOOL running = true;

void Initialize()
{
	hwnd = GetConsoleWindow();
	hinstance = GetModuleHandle(0);

	HRESULT hr = DirectInput8Create(hinstance, DIRECTINPUT_VERSION, IID_IDirectInput8A, (void**)&dInput, NULL);
	assert(SUCCEEDED(hr) && "Failed to initialize direct input");
	hr = dInput->Initialize(hinstance, DIRECTINPUT_VERSION);
	assert(SUCCEEDED(hr) && "Failed to initialize direct input");


	std::cout << "Initialized Sucessfully" << std::endl;

	dInput->EnumDevices(DI8DEVCLASS_GAMECTRL, DIEnumDevicesCallback, &dInput, DIEDFL_ATTACHEDONLY);
	dInput->EnumDevices(DI8DEVCLASS_KEYBOARD, DIEnumDevicesCallback, &dInput, DIEDFL_ATTACHEDONLY);
	dInput->EnumDevices(DI8DEVCLASS_POINTER, DIEnumDevicesCallback, &dInput, DIEDFL_ATTACHEDONLY);


	std::cout << "Total attached devices " << attachedDevices.size();
}

int main()
{
	Initialize();

	std::cin.get();
	while (running)
	{
		Loop();
	}

	Dispose();
}

void Dispose()
{
	dInput->Release();
	for (auto i = attachedDevices.begin(); i != attachedDevices.end(); i++)
	{
		InputDevice* pDevice = *i;
		pDevice->pDevice->Unacquire();
		pDevice->pDevice->Release();
		delete pDevice;
	}
}

void Loop()
{
	static long double totalElapsed = 0.0;

	clock_t now = clock();
	static clock_t then = now;
	clock_t delta = now - then;
	long double elapsed = (long double)delta / CLOCKS_PER_SEC;
	totalElapsed += elapsed;

	if (GetAsyncKeyState(VK_ESCAPE))
		running = false;
	for (auto i = attachedDevices.begin(); i != attachedDevices.end(); i++)
	{
		InputDevice* pDevice = *i;
		ProcessDevice(pDevice);
	}

	system("cls");

	then = now;
}

void ProcessDevice(InputDevice* pDevice)
{
	pDevice->pDevice->Poll();
	switch (pDevice->deviceType)
	{
	case DI8DEVTYPE_1STPERSON:
	case DI8DEVTYPE_GAMEPAD:
		HRESULT hr = pDevice->pDevice->GetDeviceState(sizeof(DIJOYSTATE), (void*)&pDevice->joypadState1);
		assert(SUCCEEDED(hr) && "Failed to get device data");

		std::cout << "Device Name:" << (char*)pDevice->dvInfo.tszInstanceName << std::endl;

		auto gamepadState = pDevice->joypadState1;

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
		break;

	}
}

BOOL CALLBACK DIEnumDevicesCallback(LPCDIDEVICEINSTANCE lpddi, PVOID pvRef)
{
	std::cout << "Device Detected " << " Name: " << (char*)lpddi->tszInstanceName << std::endl;

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
		std::cout << '\t' << "Device " << (char*)lpddi->tszInstanceName << " is a gamepad" << std::endl;
		if (deviceSubType == DI8DEVTYPE1STPERSON_SIXDOF)
		{
			std::cout << "\t\t-has 6DOF" << std::endl;
			device->SetDataFormat(&c_dfDIJoystick);
		}
		else
			device->SetDataFormat(&c_dfDIJoystick);
		break;
	case DI8DEVTYPE_KEYBOARD:
		std::cout << '\t' << "Device " << (char*)lpddi->tszInstanceName << " is a keyboard" << std::endl;
		device->SetDataFormat(&c_dfDIKeyboard);
		break;
	case DI8DEVTYPE_MOUSE:
		std::cout << '\t' << "Device " << (char*)lpddi->tszInstanceName << " is a mouse" << std::endl;
		device->SetDataFormat(&c_dfDIMouse);
		break;
	}
	hr = device->Acquire();
	assert(SUCCEEDED(hr) && "Failed to aquire device");
	std::cout << "\tDevice Properties:" << std::endl;
	device->EnumObjects(DIEnumDeviceObjectsCallback, NULL, DIDFT_ALL);

	InputDevice* pInputDevice = new InputDevice;
	memset(pInputDevice, 0, sizeof(InputDevice));
	pInputDevice->dvInfo = *lpddi;

	pInputDevice->pDevice = device;
	pInputDevice->deviceType = deviceType;
	pInputDevice->deviceSubType = deviceSubType;
	attachedDevices.push_back(pInputDevice);
	
	std::cout << "Initialized device " << (char*)lpddi->tszInstanceName << std::endl;



	return true;
}

BOOL CALLBACK DIEnumDeviceObjectsCallback(LPCDIDEVICEOBJECTINSTANCE lpddoi, LPVOID pvRef)
{
	std::cout << "\t\tName:" << (char*)lpddoi->tszName << std::endl;

	return true;
}