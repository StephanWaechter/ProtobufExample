#pragma once
#include <string>
namespace MessengerServer
{
	class Messenger
	{
	public:
		virtual void AddResponse(std::string const& response) = 0;
		virtual std::string const* GetResponse() const = 0;
		virtual void PopResponse() = 0;
		Messenger() = default;
		virtual ~Messenger() = default;
	};
}