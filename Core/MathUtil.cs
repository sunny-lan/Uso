

namespace Uso.Core
{
    static class MathUtil
    {
        /// <summary>
        /// takes x in range 0 to 1 and maps to range a and b
        /// </summary>
        /// <param name="x"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Lerp(double x, double a, double b)
        {
            return a + (b-a) * x;
        }

        
        /// <summary>
        /// Takes x from a to b and maps to the range 0 to 1
        /// </summary>
        /// <param name="x"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Unlerp( double x, double a, double b)
        {
            return (x - a) / (b - a);
        }

        public static double Map(double x, double s1, double s2, double d1, double d2)
        {
            return Lerp(Unlerp(x, s1, s2), d1, d2);
        }
    }
}
