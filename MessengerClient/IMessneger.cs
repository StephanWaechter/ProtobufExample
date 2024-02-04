using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerClient
{
    public interface IMessenger
    {
        void SendMessage(byte[] data);
        byte[]? GetResponse();
    }
}
