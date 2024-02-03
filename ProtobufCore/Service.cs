using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufCore
{
    public class Service
    {
        public Service() 
        {
            messenger.MessageRecived += MessageHandler;
        }

        public delegate void DataInfoHandler(DataInfo message);
        public event DataInfoHandler RequestDataRecived;
        public void SendRequestData()
        {
            var wrapper = new ClientMessage();
            wrapper.RequestData = new RequestData();

            SendMessage(wrapper);
        }

        private void SendMessage(IMessage message)
        {
            messenger.Send(message.ToByteArray());
        }

        private void MessageHandler(byte[] data)
        {
            var message = ServerMessage.Parser.ParseFrom(data);
            if (message.DataInfo != null)
            {
                RequestDataRecived.Invoke(message.DataInfo);
                return;
            } else if (message.UnknownMessage != null)
            {

            }

        }

        private Messenger messenger = new();
    }
}
