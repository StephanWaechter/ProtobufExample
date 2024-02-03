#pragma once
#include "ProtobufFormatV1.porto.pb.h"
#include <cinttypes>
#include <sstream>
#include <iostream>
#include <queue>

namespace ProtobufLibrary
{
	class Service
	{
	public:
		Service()
		{
			std::cout << "Created Servie\n";
		}

		void HandelMessage(char const* data, int length)
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

				Answers.push(answer.SerializeAsString());
				return;
			}
		}

		std::string const* GetResponse() const
		{
			if (Answers.empty())
				return nullptr;
			return &Answers.front();
		}

		void PopResponse()
		{
			Answers.pop();
		}

	private:
		std::queue<std::string> Answers;
	};
}