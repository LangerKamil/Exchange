using System;
using System.Collections.Generic;
using System.Text;

namespace Exchange.Class
{
    public static class RateCalculator
    {
        public static double Calculate(double rate, double amount)
        {
            return rate * amount;
        }
    }
}
