using System;
using System.Collections.Generic;
using System.Text;

namespace FS.Common.Math
{
    public static class Functions
    {
        public static double NORMSDIST(double X)
        {

            double L = 0.0;

            double K = 0.0;

            double dCND = 0.0;

            const double a1 = 0.31938153;

            const double a2 = -0.356563782;

            const double a3 = 1.781477937;

            const double a4 = -1.821255978;

            const double a5 = 1.330274429;

            L = System.Math.Abs(X);

            K = 1.0 / (1.0 + 0.2316419 * L);

            dCND = 1.0 - 1.0 / System.Math.Sqrt(2 * Convert.ToDouble(System.Math.PI.ToString())) * System.Math.Exp(-L * L / 2.0) * (a1 * K + a2 * K * K + a3 * System.Math.Pow(K, 3.0) + a4 * System.Math.Pow(K, 4.0) + a5 * System.Math.Pow(K, 5.0));

            if (X < 0)
            {

                return System.Math.Abs((1.0 - dCND));

            }

            else
            {

                return dCND;

            }

        }
    }
}
