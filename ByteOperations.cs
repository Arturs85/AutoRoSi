﻿using System;

public class ByteOperations
{
	public ByteOperations()
	{
    }
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
        Debug.Print(hi.ToString());

        int word = ((short)hi);
        word = word << 8;
        word = word | lo;
        return (short)word;
    }

}
