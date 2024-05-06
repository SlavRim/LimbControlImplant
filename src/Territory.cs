namespace NeuralInterceptMatrix;

public partial class Territory
{
    public virtual Map Map { get; }
    public virtual IntVec3 Position { get; }
    public virtual float Radius { get; set; }

    public delegate void StateUpdateDelegate<in T>(T thing) where T : Thing;

    public virtual Events<Thing> ThingEvents { get; } = new ();
    public virtual Events<Pawn> PawnEvents { get; } = new();

    public Territory(Map map, IntVec3 position, float radius)
    {
        Map = map;
        Position = position;
        Radius = radius;
    }

    private readonly HashSet<Thing> enteredThings = new();
    public IReadOnlyCollection<Thing> EnteredThings => enteredThings;

    public bool IsEntered(Thing thing) => enteredThings.Contains(thing);
    public virtual bool IsInside(Thing thing) => thing.Position.DistanceTo(Position) <= Radius;
    public bool SetEnterState(Thing thing, bool? inside = null)
    {
        var result = inside ?? IsInside(thing);
        if (result) TryEnter(thing);
        else TryExit(thing);
        return result;
    }
    private void TryEnter(Thing thing)
    {
        if (IsEntered(thing))
        {
            CallEvents(EventType.Stay, thing);
            return;
        }
        enteredThings.Add(thing);
        CallEvents(EventType.Enter, thing);
    }

    private void TryExit(Thing thing)
    {
        if (!IsEntered(thing)) return;
        enteredThings.Remove(thing);
        CallEvents(EventType.Exit, thing);
    }

    public virtual void CallEvents(EventType type, Thing thing)
    {
        void Execute<T>(Events<T> events) where T : Thing => events.Call(type, thing);
        Execute(ThingEvents);
        Execute(PawnEvents);
#if DEBUG
        if (thing is not Pawn pawn || type is EventType.Stay) return;
        Log.Message($"{pawn.NameFullColored} {type+"".ToLower()} {Position}");
#endif
    }

    public enum EventType
    {
        Enter,
        Stay,
        Exit
    }
    public class Events<T>
        where T : Thing
    {
        public event StateUpdateDelegate<T> OnEnter, OnStay, OnExit;
        public StateUpdateDelegate<T> Get(EventType type) => type switch
        {
            EventType.Enter => OnEnter,
            EventType.Stay => OnStay,
            _ => OnExit
        };
        public void Call(EventType type, Thing thing) => InvokeEventSafely(Get(type), thing);
        private static void InvokeEventSafely(StateUpdateDelegate<T> @event, Thing thing)
        {
            if (thing is not T concreteThing) return;
            try
            {
                @event.Invoke(concreteThing);
            }
            catch { }
        }
    }
}
