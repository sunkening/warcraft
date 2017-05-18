using System;

namespace skntcp
{
    public class PackageBuffer
    {
        public const int HEAD_LEN = 4;
        public const int BODY_LEN = 1024;
        public const int MAX_LEN = HEAD_LEN + BODY_LEN;

        private byte[] bytes;
        private  int length;
        private int curIndex ;
        private int markIndex;
        public byte[] getBytes()
        {
            return bytes;
        }

        public PackageBuffer()
        {
            bytes = new byte[MAX_LEN];
        }

        public int getInt()
        {
            int i= System.BitConverter.ToInt32(bytes, curIndex);
            curIndex += 4;
            return i;
        }
        public int getCurIndex()
        {
            return curIndex;
        }
        public void mark()
        {
            markIndex = curIndex;
        }

        public void reset()
        {
            curIndex = markIndex;
        }

        public void read()
        {
            int remain = remaining();
            Buffer.BlockCopy(bytes, curIndex
                    , bytes, 0, remain);
            curIndex = 0;
            markIndex = 0;
            length = remain;
        }
        public void addLength(int i)
        {
            length += i;
        }

        public int getLength( )
        {
            return length  ;
        }
        public int remaining()
        {
            return length-curIndex;
        }

        public void get(byte[] dst)
        {
            Buffer.BlockCopy(bytes,curIndex ,
                dst, 0, dst.Length);
            curIndex += dst.Length;
        }
      
    }
}
