#pragma once
#include "Service.h"
#include <cinttypes>


extern "C" __declspec(dllexport) ProtobufLibrary::Service* __stdcall start_service()
{
	return new ProtobufLibrary::Service();
}

extern "C" __declspec(dllexport) void __stdcall set_callback(
	ProtobufLibrary::Service* servcie,
	ProtobufLibrary::Service::MessageCallback callback
)
{
	servcie->SetCallback(callback);
}

extern "C" __declspec(dllexport) void __stdcall send_message(ProtobufLibrary::Service* service, uint8_t const* data, int length)
{
	service->HandelMessage(data, length);
}

extern "C" __declspec(dllexport) void __stdcall release_service(ProtobufLibrary::Service* service)
{
	delete service;
}