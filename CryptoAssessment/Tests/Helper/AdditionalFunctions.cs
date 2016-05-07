using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAssessment.Tests
{
    internal static class Contants
    {
        internal static double SQRT_2 = 1.41421356237;
    }

    internal static class AdditionalFunctions
    {
        // Source: https://msdn.microsoft.com/en-us/magazine/dn520240.aspx
        internal static double Erf(double x)
        {
            // assume x > 0.0
            // Abramowitz and Stegun eq. 7.1.26
            double p = 0.3275911;
            double a1 = 0.254829592;
            double a2 = -0.284496736;
            double a3 = 1.421413741;
            double a4 = -1.453152027;
            double a5 = 1.061405429;
            double t = 1.0 / (1.0 + p * x);
            double err = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);
            return err;
        }

        internal static double Erfc(double x)
        {
            return 1 - Erf(x);
        }

        internal static double CountChiStat(double[] inputTable)
        {
            double chi = 0;
            foreach (double value in inputTable)
            {
                chi = chi + ((value - 0.5) * (value - 0.5));
            }

            return chi;
        }

        internal static double CalculateStdDev(IEnumerable<double> values, out double average)
        {
            double ret = 0;
            if (values.Count() > 0)
            {
                double avg = values.Average();
                double sum = values.Sum(d => Math.Pow(d - avg, 2));
                ret = Math.Sqrt((sum) / (values.Count() - 1));
                average = avg;
            }
            else
            {
                average = 0;
            }

            return ret;
        }

        internal static int CountOnes(byte[] input)
        {
            int count = 0;
            foreach (byte b in input)
            {
                count += CountOnes(b);
            }
            return count;
        }

        internal static int CountOnes(byte input)
        {
            int count = 0;
            while (input != 0)
            {
                count++;
                input &= (byte)(input - 1);
            }
            return count;
        }
    }

    public class GammaFunctions
    {
        public static double GammaLower(double a, double x)
        {
            // incomplete Gamma 'P' (lower) aka 'igam'
            if (x < 0.0 || a <= 0.0)
                throw new Exception("Bad args in GammaLower");
            if (x < a + 1)
                return GammaLowerSer(a, x); // no surprise
            else
                return 1.0 - GammaUpperCont(a, x); // indirectly is faster
        }

        public static double GammaUpper(double a, double x)
        {
            // incomplete Gamma 'Q' (upper) == (1 - GammaP) but computed for efficiency
            // aka 'igamc' (incomplete gamma complement)
            if (x < 0.0 || a <= 0.0)
                throw new Exception("Bad args in GammaUpper");
            if (x < a + 1)
                return 1.0 - GammaLowerSer(a, x); // indirect is faster
            else
                return GammaUpperCont(a, x);
        }

        // -------------------------------------------------------------------------------

        private static double LogGamma(double x)
        {
            // Log of Gamma from Lanczos with g=5, n=6/7
            // not in A & S 
            double[] coef = new double[6] { 76.18009172947146, -86.50532032941677, 
        24.01409824083091, -1.231739572450155, 
        0.1208650973866179E-2, -0.5395239384953E-5 };
            double LogSqrtTwoPi = 0.91893853320467274178;
            double denom = x + 1;
            double y = x + 5.5;
            double series = 1.000000000190015;
            for (int i = 0; i < 6; ++i)
            {
                series += coef[i] / denom;
                denom += 1.0;
            }
            return (LogSqrtTwoPi + (x + 0.5) * Math.Log(y) - y + Math.Log(series / x));
        }

        private static double GammaLowerSer(double a, double x)
        {
            // incomplete gamma lower (computed by series expansion)
            if (x < 0.0)
                throw new Exception("x param less than 0.0 in GammaLowerSer");

            double gln = LogGamma(a);
            double ap = a;
            double del = 1.0 / a;
            double sum = del;
            for (int n = 1; n <= 10000; ++n)
            {
                ++ap;
                del *= x / ap;
                sum += del;
                if (Math.Abs(del) < Math.Abs(sum) * 3.0E-7) // close enough?
                    return sum * Math.Exp(-x + a * Math.Log(x) - gln);
            }
            throw new Exception("Unable to compute GammaLowerSer to desired accuracy");
        }

        private static double GammaUpperCont(double a, double x)
        {
            // incomplete gamma upper computed by continuing fraction
            if (x < 0.0)
                throw new Exception("x param less than 0.0 in GammaUpperCont");
            double gln = LogGamma(a);
            double b = x + 1.0 - a;
            double c = 1.0 / 1.0E-30; // div by close to double.MinValue
            double d = 1.0 / b;
            double h = d;
            for (int i = 1; i <= 10000; ++i)
            {
                double an = -i * (i - a);
                b += 2.0;
                d = an * d + b;
                if (Math.Abs(d) < 1.0E-30) d = 1.0E-30; // got too small?
                c = b + an / c;
                if (Math.Abs(c) < 1.0E-30) c = 1.0E-30;
                d = 1.0 / d;
                double del = d * c;
                h *= del;
                if (Math.Abs(del - 1.0) < 3.0E-7)
                    return Math.Exp(-x + a * Math.Log(x) - gln) * h;  // close enough?
            }
            throw new Exception("Unable to compute GammaUpperCont to desired accuracy");
        }
    }
}
