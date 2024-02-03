#pragma once
#include "ProtobufFormatV1.porto.pb.h"
#include <cinttypes>
#include <sstream>
#include <iostream>

namespace ProtobufLibrary
{
	class Service
	{
	public:
		Service()
		{
			std::cout << "Created Servie\n";
		}

		typedef void (*MessageCallback)(char const* data, int lenth);
		void SetCallback(MessageCallback callback)
		{
			m_callback = callback;
		}

		void HandelMessage(uint8_t const* data, int length)
		{
			std::cout << "Recived Message\n";
			ProtoGuiCore::ClientMessage message;
			if (message.ParseFromArray(data, length))
			{
				if (message.has_requestdata())
				{
					std::cout << "Recived RequestData Message\n";
					auto data_info = std::make_unique<ProtoGuiCore::DataInfo>();
					for (int k = 0; k < 10; k++)
					{
						auto item = data_info->add_items();
						item->set_id(k);
						item->set_name("Item " + std::to_string(k));
					}

					auto answer = ProtoGuiCore::ServerMessage();
					answer.set_allocated_datainfo(data_info.release());
					auto return_msg = answer.SerializeAsString();
					
					std::cout << "Call Callback\n";
					m_callback(
						return_msg.c_str(),
						return_msg.size()
					);
				}
			}
		}

	private:
		MessageCallback m_callback;
	};
}