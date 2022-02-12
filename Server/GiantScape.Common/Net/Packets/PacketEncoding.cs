﻿using System;
using System.Linq;
using System.Text;

namespace GiantScape.Common.Net.Packets
{
    internal static class PacketEncoding
    {
        public static ushort GetSize(params object[] contents)
        {
            return (ushort)contents.ToList().Aggregate(0, (sum, next) => sum + GetSingleSize(next));
        }

        public static ushort GetSingleSize(object obj)
        {
            if (obj is string str) return (ushort)(sizeof(ushort) + Encoding.ASCII.GetByteCount(str));
            else return (ushort)System.Runtime.InteropServices.Marshal.SizeOf(obj);
        }

        public static byte[] ContentsToBytes(params object[] contents)
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

        public static void ToBytes(object obj, byte[] buffer, int offset, int length)
        {
            byte[] bytes = GetSingleBytes(obj);
            Array.Copy(bytes, 0, buffer, offset, length);
        }

        public static byte[] GetSingleBytes(object obj)
        {
            Type objType = obj.GetType();

            if (objType == typeof(string)) return GetStringBytes((string)obj);
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

        public static byte[] GetStringBytes(string str)
        {
            byte[] stringBytes = Encoding.ASCII.GetBytes(str);
            byte[] lengthBytes = BitConverter.GetBytes((ushort)stringBytes.Length);

            return lengthBytes.Concat(stringBytes).ToArray();
        }
    }
}
