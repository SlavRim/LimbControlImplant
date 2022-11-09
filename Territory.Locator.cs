namespace NeuralInterceptMatrix;

partial class Territory
{
    public record struct Locator<T>(Territory Territory, ThingRequest ThingRequest)
        where T : Thing
    {
        public Predicate<T> Predicate;
        public Func<IEnumerable<T>> PoolSelector;
        public Func<T, bool?> Entered; 

        IEnumerable<T> Pool => PoolSelector?.Invoke() ?? Territory.Map.listerThings.ThingsMatching(ThingRequest).OfType<T>();

        public void Locate()
        {
            var pool = Pool.ToList();
            foreach (var thing in pool)
                if (Predicate?.Invoke(thing) ?? true)
                    Territory.SetEnterState(thing, Entered?.Invoke(thing));
        }
        int tick;
        public void Tick()
        {
            if (++tick < 10) return;
            tick = 0;
            try
            {
                Locate();
            }
            catch { }
        }
    }
}