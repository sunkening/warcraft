using System.Collections;
using System.Collections.Generic;
 
using System;
public class MathUtil   {
    public static Random random = new Random();
    public static int GetIntRandomNumber(int min, int max)
    {
        int ran = random.Next(min, max + 1);
        return ran;
    }
    public static double GetDoubleRandomNumber(int min, int max)
    {
        double m = random.NextDouble() * max;
        double n = random.NextDouble() * min;
        if (m - n > 2.0)
            return m;
        else
            return n + 3.0;
    }
}
