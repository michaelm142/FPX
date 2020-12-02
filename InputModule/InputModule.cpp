// InputModule.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include "InputModule.h"
#include <iostream>
#include <time.h>
#include <assert.h>
#include <Windows.h>
#include <XInput.h>
#include <wbemidl.h>
#include <oleauto.h>
#include <wbemidl.h>

#define SAFE_RELEASE(x) if(x) { x->Release(); x = NULL; }

static BOOL running = true;

BOOL IsXInputDevice(const GUID * pGuidProductFromDirectInput);

int main()
{
	InitializeInputModule((int)GetConsoleWindow());

	std::cin.get();
	while (running)
	{
		InputUpdate();
	}

	Close();
}


void QueryDevices()
{
	app.dInput->EnumDevices(DI8DEVCLASS_GAMECTRL, DIEnumDevicesCallback, &app.dInput, DIEDFL_ATTACHEDONLY);
	app.dInput->EnumDevices(DI8DEVCLASS_KEYBOARD, DIEnumDevicesCallback, &app.dInput, DIEDFL_ATTACHEDONLY);
	app.dInput->EnumDevices(DI8DEVCLASS_POINTER, DIEnumDevicesCallback, &app.dInput, DIEDFL_ATTACHEDONLY);

	// set up device indices
	for (uint i = 0; i < MAX_INPUT_DEVICES; i++)
	{
		if (connectedGamepads.a[i])
			connectedGamepads.a[i]->deviceIndex = i;
	}
}

EXPORT int _stdcall InitializeInputModule(int h_wnd)
{
	app.hwnd = (HWND)h_wnd;
	app.hinstance = GetModuleHandle(0);

	HRESULT hr = DirectInput8Create(app.hinstance, DIRECTINPUT_VERSION, IID_IDirectInput8A, (void**)&app.dInput, NULL);
	assert(SUCCEEDED(hr) && "Failed to initialize direct input");
	hr = app.dInput->Initialize(app.hinstance, DIRECTINPUT_VERSION);
	assert(SUCCEEDED(hr) && "Failed to initialize direct input");

	QueryDevices();

	std::cout << "Input Module Initialized Sucessfully" << std::endl;
	return hr;
}

#pragma region Add/Remove Devices
void AddMouse(InputDevice* pMouse)
{
	for (uint i = 0; i < MAX_INPUT_DEVICES; i++)
	{
		if (!connectedMouses.a[i])
		{
			connectedMouses.a[i] = pMouse;
			return;
		}
	}
}

void AddKeyboard(InputDevice* pKeyboard)
{
	for (uint i = 0; i < MAX_INPUT_DEVICES; i++)
	{
		if (!connectedKeyboards.a[i])
		{
			connectedKeyboards.a[i] = pKeyboard;
			return;
		}
	}
}

void AddGamepad(InputDevice* pGamepad)
{
	for (uint i = 0; i < MAX_INPUT_DEVICES; i++)
	{
		if (!connectedGamepads.a[i])
		{
			connectedGamepads.a[i] = pGamepad;
			return;
		}
	}
}

void RemoveMouse(InputDevice* pMouse)
{
	bool found = false;
	for (uint i = 0; i < MAX_INPUT_DEVICES; i++)
	{
		if (connectedMouses.a[i] == pMouse)
		{
			connectedMouses.a[i] = NULL;
			found = true;
		}
		else if (found && i < MAX_INPUT_DEVICES - 1)
			connectedMouses.a[i] = connectedMouses.a[i + 1];
	}
}

void RemoveKeyboard(InputDevice* pKeyboard)
{
	bool found = false;
	for (uint i = 0; i < MAX_INPUT_DEVICES; i++)
	{
		if (connectedKeyboards.a[i] == pKeyboard)
		{
			connectedKeyboards.a[i] = NULL;
			found = true;
		}
		else if (found && i < MAX_INPUT_DEVICES - 1)
			connectedKeyboards.a[i] = connectedKeyboards.a[i + 1];
	}
}

void RemoveGamepad(InputDevice* pGamepad)
{
	bool found = false;
	for (uint i = 0; i < MAX_INPUT_DEVICES; i++)
	{
		if (connectedGamepads.a[i])
			connectedGamepads.a[i]->deviceIndex = i;
		if (connectedGamepads.a[i] == pGamepad)
		{
			connectedGamepads.a[i] = NULL;
			found = true;
		}
		else if (found && i < MAX_INPUT_DEVICES - 1)
			connectedGamepads.a[i] = connectedGamepads.a[i + 1];
	}
}
#pragma endregion

HRESULT ProcessDevice(InputDevice* pDevice)
{
	static HRESULT hr = S_OK;
	pDevice->pDevice->Poll();
	switch (pDevice->deviceType)
	{
	case DI8DEVTYPE_1STPERSON:
	case DI8DEVTYPE_GAMEPAD:
		hr = pDevice->pDevice->GetDeviceState(sizeof(DIJOYSTATE2), (void*)&pDevice->deviceState);
		if (hr == DIERR_INPUTLOST)
			return hr;
		XInputGetState(pDevice->deviceIndex, &pDevice->deviceState.xInputState);

#ifdef _IMOUTPUT_VERBOSE
		auto gamepadState = pDevice->deviceState.xInputState;

		std::cout << "Left Trigger: " <<  (int)gamepadState.Gamepad.bLeftTrigger << std::endl;
		std::cout << "Right Trigger: " << (int)gamepadState.Gamepad.bRightTrigger << std::endl;
		std::cout << "Left Trigger: { X:" << gamepadState.Gamepad.sThumbLX << ", Y:" << gamepadState.Gamepad.sThumbLY << std::endl;
		std::cout << "Right Trigger: { X:" << gamepadState.Gamepad.sThumbRX << ", Y:" << gamepadState.Gamepad.sThumbRY << std::endl;

		for (uint i = 0; i < 16; i++)
		{
			uint bitfield = (1 << i);
			if ((gamepadState.Gamepad.wButtons & bitfield) != 0)
			{
				switch (gamepadState.Gamepad.wButtons)
				{
				case XINPUT_GAMEPAD_DPAD_UP:
					std::cout << "Dpad up is down" << std::endl;
					break;
				case XINPUT_GAMEPAD_DPAD_DOWN:
					std::cout << "Dpad down is down" << std::endl;
					break;
				case XINPUT_GAMEPAD_DPAD_LEFT:
					std::cout << "Dpad left is down" << std::endl;
					break;
				case XINPUT_GAMEPAD_DPAD_RIGHT:
					std::cout << "Dpad right is down" << std::endl;
					break;
				case XINPUT_GAMEPAD_START:
					std::cout << "Start is down" << std::endl;
					break;
				case XINPUT_GAMEPAD_BACK:
					std::cout << "Back is down" << std::endl;
					break;
				case XINPUT_GAMEPAD_LEFT_THUMB:
					std::cout << "LS is down" << std::endl;
					break;
				case XINPUT_GAMEPAD_RIGHT_THUMB:
					std::cout << "RS is down" << std::endl;
					break;
				case XINPUT_GAMEPAD_LEFT_SHOULDER:
					std::cout << "LB is down" << std::endl;
					break;
				case XINPUT_GAMEPAD_RIGHT_SHOULDER:
					std::cout << "RB is down" << std::endl;
					break;
				case XINPUT_GAMEPAD_A:
					std::cout << "A is down" << std::endl;
					break;
				case XINPUT_GAMEPAD_B:
					std::cout << "B is down" << std::endl;
					break;
				case XINPUT_GAMEPAD_X:
					std::cout << "X is down" << std::endl;
					break;
				case XINPUT_GAMEPAD_Y:
					std::cout << "Y is down" << std::endl;
					break;
				}
			}
		}


#endif
		break;
	case DI8DEVTYPE_MOUSE:
		hr = pDevice->pDevice->GetDeviceState(sizeof(DIMOUSESTATE), (void*)&pDevice->deviceState);
		if (hr == DIERR_INPUTLOST)
			return hr;

#ifdef _IMOUTPUT_VERBOSE
		auto mouseState = pDevice->deviceState.mouseState;
		std::cout << "Device Name:" << (char*)pDevice->dvInfo.tszInstanceName << std::endl;

		std::cout << "Delta Position:\t{ " << mouseState.lX << ":" << mouseState.lY << " }" << std::endl;
		std::cout << "Wheel Delta: " << mouseState.lZ << std::endl;
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

void UnaquireDevice(InputDevice* pDevice)
{
	std::cout << "Device " << (char*)pDevice->dvInfo.tszInstanceName << " unaquired" << std::endl;


	switch (pDevice->deviceType)
	{
	case DI8DEVTYPE_GAMEPAD:
		RemoveGamepad(pDevice);
		break;
	case DI8DEVTYPE_KEYBOARD:
		RemoveKeyboard(pDevice);
		break;
	case DI8DEVTYPE_MOUSE:
		RemoveMouse(pDevice);
		break;
	}

	pDevice->pDevice->Unacquire();
	pDevice->pDevice->Release();
	delete pDevice;
}

InputDevice* FindDeviceByType(uint deviceType, uint index)
{
	if (index == UINT_MAX)
	{
		switch (deviceType)
		{
		case DI8DEVTYPE_GAMEPAD:
		case DI8DEVTYPE_1STPERSON:
			return connectedGamepads.device0;
		case DI8DEVTYPE_KEYBOARD:
			return connectedKeyboards.device0;
		case DI8DEVTYPE_MOUSE:
			return connectedMouses.device0;
		}
	}
	else
	{
		switch (deviceType)
		{
		case DI8DEVTYPE_GAMEPAD:
		case DI8DEVTYPE_1STPERSON:
			return connectedGamepads.a[index];
		case DI8DEVTYPE_KEYBOARD:
			return connectedKeyboards.a[index];
		case DI8DEVTYPE_MOUSE:
			return connectedMouses.a[index];
		}
	}

	return nullptr;
}

#pragma region External
EXPORT void _stdcall Close()
{
	app.dInput->Release();
	while (attachedDevices.size() > 0)
	{
		UnaquireDevice(*attachedDevices.begin());
		attachedDevices.erase(attachedDevices.begin());
	}
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

	if (fmod(totalElapsed, CHECK_CONNECTED_DEVICE_INTERVAL) < 0.1)
		QueryDevices();
	//if (GetAsyncKeyState(VK_ESCAPE))
	//	running = false;
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
	std::cout << "Elapsed Time: " << totalElapsed << std::endl;

	system("cls");
#endif

	then = now;
}

EXPORT BOOL _stdcall IsKeyDown(int keycode)
{
	InputDevice* pKeyboard = nullptr;
	pKeyboard = FindDeviceByType(DI8DEVTYPE_KEYBOARD);
	if (!pKeyboard) return false;
	return pKeyboard->deviceState.keys[keycode] != 0;
}

EXPORT BOOL _stdcall IsDeviceConnected(uint type, uint index)
{
	switch (type)
	{
	case DI8DEVTYPE_GAMEPAD:
	case DI8DEVTYPE_1STPERSON:
		return connectedGamepads.a[index] != NULL;
	case DI8DEVTYPE_KEYBOARD:
		return connectedKeyboards.a[index] != NULL;
	case DI8DEVTYPE_MOUSE:
		return connectedMouses.a[index] != NULL;
	}
}

EXPORT void _stdcall GetMouseState(void* pOut)
{
	InputDevice* pDevice = nullptr;
	pDevice = FindDeviceByType(DI8DEVTYPE_MOUSE);
	LONG* dataPtr = reinterpret_cast<LONG*>(&pDevice->deviceState.mouseState);
	memcpy(pOut, dataPtr, sizeof(DIMOUSESTATE));
}

EXPORT void _stdcall GetMousePosition(int* pOut)
{
	POINT pos;
	BOOL result = GetPhysicalCursorPos(&pos);
	//PhysicalToLogicalPointForPerMonitorDPI(hwnd, &pos);
	memcpy(pOut, &pos, sizeof(POINT));
}

EXPORT void _stdcall GetGamepadState(void* pOut, uint index)
{
	InputDevice* pGamepad = nullptr;
	pGamepad = FindDeviceByType(DI8DEVTYPE_GAMEPAD, index);
	
	memcpy(pOut, &pGamepad->deviceState.xInputState.Gamepad, sizeof(XINPUT_GAMEPAD));
}
#pragma endregion

BOOL CALLBACK DIEnumDevicesCallback(LPCDIDEVICEINSTANCE lpddi, PVOID pvRef)
{
	for (auto i = attachedDevices.begin(); i != attachedDevices.end(); i++)
	{
		if ((*i)->dvInfo.guidInstance == lpddi->guidInstance)
			return true;
	}
	std::cout << "Device Detected " << " Name: " << (char*)lpddi->tszInstanceName << std::endl;

	uint fullByte = 0xFF;
	uint deviceType = fullByte & lpddi->dwDevType;
	uint deviceSubType = (fullByte << 8) & lpddi->dwDevType;
	deviceSubType = deviceSubType >> 8;
	IDirectInputDevice8* device = nullptr;
	HRESULT hr = app.dInput->CreateDevice(lpddi->guidInstance, &device, NULL);
	assert(SUCCEEDED(hr) && "Failed to create input device");
	switch (deviceType)
	{
	case DI8DEVTYPE_GAMEPAD:
#if _IMOUTPUT_VERBOSE
		std::cout << '\t' << "Device " << (char*)lpddi->tszInstanceName << " is a gamepad" << std::endl;
#endif
		device->SetDataFormat(&c_dfDIJoystick2);
		break;
	case DI8DEVTYPE_KEYBOARD:
#if _IMOUTPUT_VERBOSE
		std::cout << '\t' << "Device " << (char*)lpddi->tszInstanceName << " is a keyboard" << std::endl;
#endif
		device->SetDataFormat(&c_dfDIKeyboard);
		break;
	case DI8DEVTYPE_MOUSE:
#if _IMOUTPUT_VERBOSE
		std::cout << '\t' << "Device " << (char*)lpddi->tszInstanceName << " is a mouse" << std::endl;
#endif
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
	switch (deviceType)
	{
	case DI8DEVTYPE_GAMEPAD:
	case DI8DEVTYPE_1STPERSON:
		AddGamepad(pInputDevice);
		break;
	case DI8DEVTYPE_KEYBOARD:
		AddKeyboard(pInputDevice);
		break;
	case DI8DEVTYPE_MOUSE:
		AddMouse(pInputDevice);
		break;
	}

#if _IMOUTPUT_VERBOSE
	std::cout << "Initialized device " << (char*)lpddi->tszInstanceName << std::endl;
#endif


	return true;
	}

BOOL CALLBACK DIEnumDeviceObjectsCallback(LPCDIDEVICEOBJECTINSTANCE lpddoi, LPVOID pvRef)
{
	IDirectInputDevice8* device = (IDirectInputDevice8*)pvRef;
	std::cout << "\t\tName:" << (char*)lpddoi->tszName;
	std::cout << std::endl;

	return true;
}

BOOL IsXInputDevice(const GUID* pGuidProductFromDirectInput)
{
	IWbemLocator* pIWbemLocator = NULL;
	IEnumWbemClassObject* pEnumDevices = NULL;
	IWbemClassObject* pDevices[20] = { 0 };
	IWbemServices* pIWbemServices = NULL;
	BSTR                    bstrNamespace = NULL;
	BSTR                    bstrDeviceID = NULL;
	BSTR                    bstrClassName = NULL;
	DWORD                   uReturned = 0;
	bool                    bIsXinputDevice = false;
	UINT                    iDevice = 0;
	VARIANT                 var;
	HRESULT                 hr;

	// CoInit if needed
	hr = CoInitialize(NULL);
	bool bCleanupCOM = SUCCEEDED(hr);

	// So we can call VariantClear() later, even if we never had a successful IWbemClassObject::Get().
	VariantInit(&var);

	// Create WMI
	hr = CoCreateInstance(__uuidof(WbemLocator),
		NULL,
		CLSCTX_INPROC_SERVER,
		__uuidof(IWbemLocator),
		(LPVOID*)&pIWbemLocator);
	if (FAILED(hr) || pIWbemLocator == NULL)
		goto LCleanup;

	bstrNamespace = SysAllocString(L"\\\\.\\root\\cimv2"); if (bstrNamespace == NULL) goto LCleanup;
	bstrClassName = SysAllocString(L"Win32_PNPEntity");   if (bstrClassName == NULL) goto LCleanup;
	bstrDeviceID = SysAllocString(L"DeviceID");          if (bstrDeviceID == NULL)  goto LCleanup;

	// Connect to WMI 
	hr = pIWbemLocator->ConnectServer(bstrNamespace, NULL, NULL, 0L,
		0L, NULL, NULL, &pIWbemServices);
	if (FAILED(hr) || pIWbemServices == NULL)
		goto LCleanup;

	// Switch security level to IMPERSONATE. 
	CoSetProxyBlanket(pIWbemServices, RPC_C_AUTHN_WINNT, RPC_C_AUTHZ_NONE, NULL,
		RPC_C_AUTHN_LEVEL_CALL, RPC_C_IMP_LEVEL_IMPERSONATE, NULL, EOAC_NONE);

	hr = pIWbemServices->CreateInstanceEnum(bstrClassName, 0, NULL, &pEnumDevices);
	if (FAILED(hr) || pEnumDevices == NULL)
		goto LCleanup;

	// Loop over all devices
	for (;; )
	{
		// Get 20 at a time
		hr = pEnumDevices->Next(10000, 20, pDevices, &uReturned);
		if (FAILED(hr))
			goto LCleanup;
		if (uReturned == 0)
			break;

		for (iDevice = 0; iDevice < uReturned; iDevice++)
		{
			// For each device, get its device ID
			hr = pDevices[iDevice]->Get(bstrDeviceID, 0L, &var, NULL, NULL);
			if (SUCCEEDED(hr) && var.vt == VT_BSTR && var.bstrVal != NULL)
			{
				// Check if the device ID contains "IG_".  If it does, then it's an XInput device
					// This information can not be found from DirectInput 
				if (wcsstr(var.bstrVal, L"IG_"))
				{
					// If it does, then get the VID/PID from var.bstrVal
					DWORD dwPid = 0, dwVid = 0;
					WCHAR* strVid = wcsstr(var.bstrVal, L"VID_");
					if (strVid && swscanf(strVid, L"VID_%4X", &dwVid) != 1)
						dwVid = 0;
					WCHAR* strPid = wcsstr(var.bstrVal, L"PID_");
					if (strPid && swscanf(strPid, L"PID_%4X", &dwPid) != 1)
						dwPid = 0;

					// Compare the VID/PID to the DInput device
					DWORD dwVidPid = MAKELONG(dwVid, dwPid);
					if (dwVidPid == pGuidProductFromDirectInput->Data1)
					{
						bIsXinputDevice = true;
						goto LCleanup;
					}
				}
			}
			VariantClear(&var);
			SAFE_RELEASE(pDevices[iDevice]);
		}
	}

LCleanup:
	VariantClear(&var);
	if (bstrNamespace)
		SysFreeString(bstrNamespace);
	if (bstrDeviceID)
		SysFreeString(bstrDeviceID);
	if (bstrClassName)
		SysFreeString(bstrClassName);
	for (iDevice = 0; iDevice < 20; iDevice++)
		SAFE_RELEASE(pDevices[iDevice]);
	SAFE_RELEASE(pEnumDevices);
	SAFE_RELEASE(pIWbemLocator);
	SAFE_RELEASE(pIWbemServices);

	if (bCleanupCOM)
		CoUninitialize();

	return bIsXinputDevice;
}