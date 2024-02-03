#pragma once
#include "Service.h"
#include <cinttypes>

namespace
{
	struct Buffer
	{
		char const* Data;
		int Lenght;
	};
}

extern "C" __declspec(dllexport) ProtobufLibrary::Service* __stdcall start_service()
{
	return new ProtobufLibrary::Service();
}

extern "C" __declspec(dllexport) void __stdcall send_message(
	ProtobufLibrary::Service* service,
	Buffer message
)
{
	service->HandelMessage(message.Data, message.Lenght);
}

extern "C" __declspec(dllexport) bool __stdcall get_response(
	ProtobufLibrary::Service * service,
	Buffer& response
)
{
	auto answer = service->GetResponse();
	if (answer == nullptr)
		return false;

	response.Data = answer->c_str();
	response.Lenght = answer->size();
	return true;
}

extern "C" __declspec(dllexport) void __stdcall pop_response(
	ProtobufLibrary::Service * service
)
{
	service->PopResponse();
}

extern "C" __declspec(dllexport) void __stdcall release_service(ProtobufLibrary::Service* service)
{
	delete service;
}