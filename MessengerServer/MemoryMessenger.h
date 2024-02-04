#pragma once

#include "Messenger.h"
#include <string>
#include <queue>
namespace MessengerServer
{
	class MemoryMessenger : public Messenger
	{
	public:
		virtual ~MemoryMessenger() = default;

		void AddResponse(std::string const& response)
		{
			m_responses.push(response);
		}

		std::string const* GetResponse() const override
		{
			if (m_responses.empty())
				return nullptr;
			return &m_responses.front();
		}

		void PopResponse() override
		{
			m_responses.pop();
		}

	private:
		std::queue<std::string> m_responses;
	};
}