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
        public static double DegreesToRadians(double degrees)
        {
            return degrees * System.Math.PI / 180;
        }

        public static double RadiansToDegrees(double radians)
        {
            return radians * 180 / System.Math.PI;
        }

        public static double Cos(double x)
        {
            // This function is based on the work described in
            // http://www.ganssle.com/approx/approx.pdf

            x = x % (System.Math.PI * 2.0);

            // Make X positive if negative
            if (x < 0) { x = 0.0F - x; }

            // Get quadrand

            // Quadrand 0,  >-- Pi/2
            byte quadrand = 0;

            // Quadrand 1, Pi/2 -- Pi
            if ((x > (System.Math.PI / 2F)) & (x < (System.Math.PI)))
            {
                quadrand = 1;
                x = System.Math.PI - x;
            }

            // Quadrand 2, Pi -- 3Pi/2
            if ((x > (System.Math.PI)) & (x < ((3F * System.Math.PI) / 2)))
            {
                quadrand = 2;
                x = System.Math.PI - x;
            }

            // Quadrand 3 - 3Pi/2 -->
            if ((x > ((3F * System.Math.PI) / 2)))
            {
                quadrand = 3;
                x = (2F * System.Math.PI) - x;
            }

            // Constants used for approximation
            const double c1 = 0.99999999999925182;
            const double c2 = -0.49999999997024012;
            const double c3 = 0.041666666473384543;
            const double c4 = -0.001388888418000423;
            const double c5 = 0.0000248010406484558;
            const double c6 = -0.0000002752469638432;
            const double c7 = 0.0000000019907856854;

            // X squared
            double x2 = x * x; ;

            // Check quadrand
            if ((quadrand == 0) | (quadrand == 3))
            {
                // Return positive for quadrand 0, 3
                return (c1 + x2 * (c2 + x2 * (c3 + x2 * (c4 + x2 * (c5 + x2 * (c6 + c7 * x2))))));
            }
            else
            {
                // Return negative for quadrand 1, 2
                return 0.0F - (c1 + x2 * (c2 + x2 * (c3 + x2 * (c4 + x2 * (c5 + x2 * (c6 + c7 * x2))))));
            }
        }

        public static double Sin(double x)
        {
            return Cos((System.Math.PI / 2.0F) - x);
        }

        public static double Abs(double x)
        {
            if (x >= 0.0F)
                return x;
            else
                return (-x);
        }

        public static double Sqrt(double x)
        {
            //cut off any special case
            if (x <= 0.0f)
                return 0.0f;

            //here is a kind of base-10 logarithm
            //so that the argument will fall between
            //1 and 100, where the convergence is fast
            float exp = 1.0f;

            while (x < 1.0f)
            {
                x *= 100.0f;
                exp *= 0.1f;
            }

            while (x > 100.0f)
            {
                x *= 0.01f;
                exp *= 10.0f;
            }

            //choose the best starting point
            //upon the actual argument value
            double prev;

            if (x > 10f)
            {
                //decade (10..100)
                prev = 5.51f;
            }
            else if (x == 1.0f)
            {
                //avoid useless iterations
                return x * exp;
            }
            else
            {
                //decade (1..10)
                prev = 1.741f;
            }

            //apply the Newton-Rhapson method
            //just for three times
            prev = 0.5f * (prev + x / prev);
            prev = 0.5f * (prev + x / prev);
            prev = 0.5f * (prev + x / prev);

            //adjust the result multiplying for
            //the base being cut off before
            return prev * exp;
        }
    }
}
