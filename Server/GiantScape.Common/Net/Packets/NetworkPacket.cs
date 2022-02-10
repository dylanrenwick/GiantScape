using System;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace GiantScape.Common.Net.Packets
{

    public abstract partial class NetworkPacket
    {
        public abstract PacketType Type { get; }
        public abstract ushort PacketLength { get; }

        public abstract byte[] GetContentBytes();

        public abstract void FromBytes(byte[] bytes);

        public byte[] GetBytes()
        {
            return GetHeaderBytes().Concat(GetContentBytes()).ToArray();
        }

        private byte[] GetHeaderBytes()
        {
            byte[] typeBytes = BitConverter.GetBytes((ushort)Type);
            byte[] lengthBytes = BitConverter.GetBytes(PacketLength);

            return typeBytes.Concat(lengthBytes).ToArray();
        }

        protected static ushort GetSize(params object[] contents)
        {
            return (ushort)contents.ToList().Aggregate(0, (sum, next) => sum + GetSingleSize(next));
        }

        private static ushort GetSingleSize(object obj)
        {
            if (obj is string str) return (ushort)Encoding.ASCII.GetByteCount(str);
            else return (ushort)System.Runtime.InteropServices.Marshal.SizeOf(obj);
        }

        protected static byte[] ContentsToBytes(params object[] contents)
        {
            int size = GetSize(contents);
            byte[] bytes = new byte[size];
            int offset = 0;

            for (int i = 0; i < contents.Length; i++)
            {
                int singleSize = GetSingleSize(contents[i]);
                ToBytes(contents[i], bytes, offset, singleSize);
                offset += singleSize;
            }

            return bytes;
        }

        private static void ToBytes(object obj, byte[] buffer, int offset, int length)
        {
            byte[] bytes = GetSingleBytes(obj);
            Array.Copy(bytes, 0, buffer, offset, length);
        }

        private static byte[] GetSingleBytes(object obj)
        {
            Type objType = obj.GetType();

            if (objType == typeof(string)) return Encoding.ASCII.GetBytes((string)obj);
            else if (objType == typeof(bool)) return BitConverter.GetBytes((bool)obj);
            else if (objType == typeof(char)) return BitConverter.GetBytes((char)obj);
            else if (objType == typeof(double)) return BitConverter.GetBytes((double)obj);
            else if (objType == typeof(float)) return BitConverter.GetBytes((float)obj);
            else if (objType == typeof(long)) return BitConverter.GetBytes((long)obj);
            else if (objType == typeof(int)) return BitConverter.GetBytes((int)obj);
            else if (objType == typeof(short)) return BitConverter.GetBytes((short)obj);
            else if (objType == typeof(ulong)) return BitConverter.GetBytes((ulong)obj);
            else if (objType == typeof(uint)) return BitConverter.GetBytes((uint)obj);
            else if (objType == typeof(ushort)) return BitConverter.GetBytes((ushort)obj);
            else if (objType == typeof(byte)) return new byte[1] { (byte)obj };
            else return Array.Empty<byte>();
        }
    }
}
