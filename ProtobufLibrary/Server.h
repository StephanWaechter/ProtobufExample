#pragma once
#include "ProtobufFormatV1.porto.pb.h"
#include "MemoryMessenger.h"

namespace ProtobufLibrary
{
	std::string HandelMessage(char const* data, int length)
	{
		std::cout << "Recived Message\n";
		ProtoGuiCore::ClientMessage message;
		ProtoGuiCore::ServerMessage answer;
		if (message.ParseFromArray(data, length))
		{
			if (message.has_requestdata())
			{
				std::cout << "Recived RequestData Message\n";
				auto data_info = answer.mutable_datainfo();
				for (int k = 0; k < 10; k++)
				{
					auto item = data_info->add_items();
					item->set_id(k);
					item->set_name("Item " + std::to_string(k));
				}
			}
			else
			{
				std::cout << "Recived Unknown Message\n";
				auto errorMessage = answer.mutable_unknownmessage();
				errorMessage->set_answer("Invalid Servier Request");
			}

			return answer.SerializeAsString();
		}
	}
}