using System;
using Microsoft.SPOT;

namespace RoombaServer
{
   public static class Common
    {
        public static byte highByteFromWord(short word)
        {

            byte highByte = (byte)(word >> 8);
            return highByte;
        }
        public static byte lowByteFromWord(short word)
        {

            byte lowByte = (byte)(word & 0xFF);
            return lowByte;
        }
        public static short bytesToWord(byte hi, byte lo)
        {

            int word = ((short)hi);
            word = word << 8;
            word = word | lo;
            return (short)word;
        }

        public static void GetByteArrayFromInt(int value, byte[] destinationArray, int startPos)
        {
            for (int i = 0; i < 4; i++)
            {
                destinationArray[startPos + i] = (byte)(value >> (8 * i));
            }
        }

        public static int GetIntFromByteArray(byte[] sourceArray, int startPos)
        {
            int result = 0;

            for (int i = 0; i < 4; i++)
            {
                result |= sourceArray[startPos + i] << (i * 8);
            }
            return result;
        }

    }
}
