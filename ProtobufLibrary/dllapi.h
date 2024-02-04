#pragma once
#include "Server.h"
#include <cinttypes>

namespace
{
	struct Buffer
	{
		char const* Data;
		int Lenght;
	};
}

extern "C" __declspec(dllexport) MessengerServer::Messenger* __stdcall start_service()
{
	return new MessengerServer::MemoryMessenger();
}

extern "C" __declspec(dllexport) void __stdcall send_message(
	MessengerServer::Messenger * server,
	Buffer message
)
{
	server->AddResponse(
		ProtobufLibrary::HandelMessage(message.Data, message.Lenght)
	);
}

extern "C" __declspec(dllexport) bool __stdcall get_response(
	MessengerServer::Messenger* server,
	Buffer& response
)
{
	auto answer = server->GetResponse();
	if (answer == nullptr)
		return false;

	response.Data = answer->c_str();
	response.Lenght = answer->size();
	return true;
}

extern "C" __declspec(dllexport) void __stdcall pop_response(
	MessengerServer::Messenger* server
)
{
	server->PopResponse();
}

extern "C" __declspec(dllexport) void __stdcall release_service(MessengerServer::Messenger* server)
{
	delete server;
}