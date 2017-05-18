using System;
using System.Text;
 

namespace skntcp
{
    public class ProtocalBuffer
    {
        public const int MAX_LENGTH=1024; 
        public int length;
        public int curIndex;
        public byte[] buffer;
        public ProtocalBuffer( )
        {
            buffer = new byte[MAX_LENGTH];
        }
        public ProtocalBuffer(byte[] bytes)
        {
            buffer = bytes;
            length = buffer.Length;
        }
        public ProtocalBuffer(int cap)
        {
            buffer = new byte[cap];
        }

        public void putInt(int i)
        {
            byte[] temp = System.BitConverter.GetBytes(i);
            Buffer.BlockCopy(temp, 0, buffer, curIndex, 4);
            curIndex += 4;
            length += 4;
        }

        public void putFloat(float i)
        {
            byte[] temp = System.BitConverter.GetBytes(i);
            Buffer.BlockCopy(temp, 0, buffer, curIndex, 4);
            curIndex += 4;
            length += 4;
        }

        public void putString(string s)
        {
            byte[] temp = Encoding.Unicode.GetBytes(s);
            putInt(temp.Length);
            Buffer.BlockCopy(temp, 0, buffer, curIndex, temp.Length);
            curIndex += temp.Length;
            length += temp.Length;
        }

        public int getInt()
        {
            int i = System.BitConverter.ToInt32(buffer, curIndex);
            curIndex += 4;
            return i;
        }

        public float getFloat()
        {
            float i = System.BitConverter.ToSingle(buffer, curIndex);
            curIndex += 4;
            return i;
        }

        public string getString()
        {
            int length = getInt();
            string s = Encoding.Unicode.GetString(buffer, curIndex, length);
            curIndex += length;
            return s;
        }
    }
}
