using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufCore
{
    public interface IBufferMessenger
    {
        void SendMessage(byte[] data);
        byte[]? GetResponse();
    }

    public class Service
    {
        public Service(IBufferMessenger messenger) 
        {
            this.messenger = messenger;
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
            messenger.SendMessage(message.ToByteArray());
            while (MessageHandler(messenger.GetResponse())) { };
        }

        private bool MessageHandler(byte[]? data)
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

        private IBufferMessenger messenger;
    }
}
