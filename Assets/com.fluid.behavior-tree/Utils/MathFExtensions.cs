using System;

namespace CleverCrow.Fluid.BTs.Trees.Utils
{
    public static class MathFExtensions
    {
        public const float Zero = 0f;

        private const float HalfValue = 0.5f;
        private const float TwiceValue = 2f;

        public static float Half(this int original) => original * HalfValue;
        public static float Half(this long original) => original * HalfValue;
        public static float Half(this float original) => original * HalfValue;
        public static float Half(this double original) => (float)(original * HalfValue);
        public static float Half(this decimal original) => (float)original * HalfValue;

        public static float Twice(this int original) => original * TwiceValue;
        public static float Twice(this long original) => original * TwiceValue;
        public static float Twice(this float original) => original * TwiceValue;
        public static float Twice(this double original) => (float)(original * TwiceValue);
        public static float Twice(this decimal original) => (float)original * TwiceValue;

        public static float NextFloat(this Random random, float minValue = float.MinValue, float maxValue = float.MaxValue)
        {
            var result = (random.NextDouble() * (maxValue - (double)minValue)) + minValue;
            return (float)result;
        }
    }
}
