using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufCore
{
    public interface IClient
    {
        void HandelAnswer(ReadOnlySpan<byte> message);
    }

    public class MemoryMessenger : IMessenger, IDisposable
    {
        [DllImport("ProtobufLibrary")]
        public static extern IntPtr start_service();

        [DllImport("ProtobufLibrary")]
        public static extern void release_service(IntPtr serviceHandle);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Buffer
        {
            public IntPtr Data;
            public int Length;
        }

        [DllImport("ProtobufLibrary")]
        internal static extern void send_message(IntPtr serviceHandle, Buffer message);

        [DllImport("ProtobufLibrary")]
        internal static extern bool get_response(IntPtr serviceHandle, out Buffer message);

        [DllImport("ProtobufLibrary")]
        internal static extern bool pop_response(IntPtr serviceHandle);

        internal struct PinnedArray : IDisposable
        {
            internal PinnedArray(byte[] data)
            {
                this.handel = GCHandle.Alloc(data, GCHandleType.Pinned);
                this.data = data;
            }

            internal Buffer GetBuffer()
            {
                return new Buffer
                {
                    Data = Marshal.UnsafeAddrOfPinnedArrayElement(data, 0),
                    Length = data.Length
                };
            }

            private GCHandle handel;
            private byte[] data;

            public void Dispose()
            {
                handel.Free();
            }
        }

        public void Dispose()
        {
            release_service(serviceImpl);
        }

        public void SendMessage(byte[] message)
        {
            using var pinned_message = new PinnedArray(message);
            send_message(serviceImpl, pinned_message.GetBuffer());
        }

        public byte[]? GetResponse()
        {
            Buffer buffer;
            if( get_response(serviceImpl, out buffer) )
            {
                var response = new byte[buffer.Length];
                Marshal.Copy(buffer.Data, response, 0, buffer.Length);
                pop_response(serviceImpl);
                return response;
            }
            return null;
        }

        IntPtr serviceImpl;

        public Messenger()
        {
            serviceImpl = start_service();
        }
    }
}
