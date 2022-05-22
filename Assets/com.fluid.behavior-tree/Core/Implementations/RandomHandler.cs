using System;
using CleverCrow.Fluid.BTs.Trees.Core.Interfaces;
using CleverCrow.Fluid.BTs.Trees.Utils;

namespace CleverCrow.Fluid.BTs.Trees.Core.Implementations
{
    public class RandomHandler : IRandomHandler
    {
        public int Seed { get; private set; }
        public int PreviousInt { get; private set; }
        public int CurrentInt { get; private set; }
        public float PreviousFloat { get; private set; }
        public float CurrentFloat { get; private set; }

        private Random _random;

        public RandomHandler(int seed = 0)
        {
            Reset(seed);
        }

        public int NextInt()
        {
            PreviousInt = CurrentInt;
            CurrentInt = _random.Next();
            return CurrentInt;
        }

        public int NextInt(int maxExclusive)
        {
            PreviousInt = CurrentInt;
            CurrentInt = _random.Next(maxExclusive);
            return CurrentInt;
        }

        public int NextInt(int minInclusive, int maxExclusive)
        {
            PreviousInt = CurrentInt;
            CurrentInt = _random.Next(minInclusive, maxExclusive);
            return CurrentInt;
        }

        public float NextFloat()
        {
            PreviousFloat = CurrentFloat;
            CurrentFloat = _random.NextFloat();
            return CurrentFloat;
        }

        public float NextFloat(float maxInclusive)
        {
            PreviousFloat = CurrentFloat;
            CurrentFloat = _random.NextFloat(0, maxInclusive);
            return CurrentFloat;
        }

        public float NextFloat(float minInclusive, float maxInclusive)
        {
            PreviousFloat = CurrentFloat;
            CurrentFloat = _random.NextFloat(minInclusive, maxInclusive);
            return CurrentFloat;
        }

        public void Reset(int seed = 0)
        {
            Seed = seed;
            _random = new Random(Seed);
        }
    }
}
