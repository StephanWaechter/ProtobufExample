using Google.Protobuf;
using MessengerClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufCore
{
    public class Client
    {
        public Client() 
        {
            this.messenger = new MemoryMessenger();
        }

        public delegate void DataInfoHandler(DataInfo message);
        public event DataInfoHandler RequestDataRecived;
        public void SendRequestData()
        {
            var message = new ClientMessage
            {
                RequestData = new RequestData()
            };
            messenger.SendMessage(message.ToByteArray());
            while (HandleResponse(messenger.GetResponse())) { };
        }

        private bool HandleResponse(byte[]? data)
        {
            if (data == null)
                return true;
            var message = ServerMessage.Parser.ParseFrom(data);
            if (message.DataInfo != null)
            {
                RequestDataRecived.Invoke(message.DataInfo);
            } 
            else if (message.UnknownMessage != null)
            {

            }
            return false;
        }

        private IMessenger messenger;
    }
}
