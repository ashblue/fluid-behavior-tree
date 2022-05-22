namespace CleverCrow.Fluid.BTs.Trees.Core.Interfaces
{
    public interface IRandomHandler
    {
        int Seed { get; }
        int PreviousInt { get; }
        int CurrentInt { get; }
        float PreviousFloat { get; }
        float CurrentFloat { get; }

        int NextInt();
        int NextInt(int maxExclusive);
        int NextInt(int minInclusive, int maxExclusive);

        float NextFloat();
        float NextFloat(float maxInclusive);
        float NextFloat(float minInclusive, float maxInclusive);

        void Reset(int seed = 0);

    }
}
