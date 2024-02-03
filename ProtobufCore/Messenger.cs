using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufCore
{
    public class Messenger : IDisposable
    {
        public delegate void MessageHander(byte[] message);
        public event MessageHander MessageRecived;
        public delegate void NativeMessageSender(IntPtr data, int length);

        [DllImport("ProtobufLibrary")]
        public static extern IntPtr start_service();

        [DllImport("ProtobufLibrary")]
        public static extern void set_callback(IntPtr service, NativeMessageSender callback);

        [DllImport("ProtobufLibrary")]
        public static extern void release_service(IntPtr serviceHandle);

        [DllImport("ProtobufLibrary")]
        public static extern void send_message(IntPtr serviceHandle, IntPtr data, int length);

        public void Dispose()
        {
            release_service(serviceImpl);
        }

        void OnMessageRecived(IntPtr data, int length)
        {
            byte[] buffer = new byte[length];
            Marshal.Copy(data, buffer, 0, length);
            MessageRecived(buffer);
        }

        public void Send(byte[] data)
        {
            var buffer = Marshal.UnsafeAddrOfPinnedArrayElement(data, 0);
            send_message(serviceImpl, buffer, data.Length);
        }

        IntPtr serviceImpl;

        public Messenger()
        {
            serviceImpl = start_service();
            set_callback(serviceImpl,
                (IntPtr data, int length) => OnMessageRecived(data, length)
            );
        }

        




        
    }
}
