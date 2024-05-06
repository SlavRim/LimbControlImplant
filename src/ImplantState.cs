using NeuralInterceptMatrix.Buildings;

namespace NeuralInterceptMatrix;

public partial class ImplantState : IExposable
{
    public enum State
    {
        Disabled,
        Arms,
        Legs
    }

    private State _value = State.Disabled;
    public object ChangedBy { get; protected set; }
    public virtual State Value
    {
        get => _value;
        protected set
        {
            if (_value == value) return;
            _value = value;
            StateUpdated?.Invoke();
        }
    }
    public event Action StateUpdated;

    public string ValueName = "state";
    public virtual void ExposeData()
    {
        Scribe_Values.Look(ref _value, ValueName);
    }

    private static readonly int MaxState = Enum.GetValues(typeof(State)).OfType<State>().Max(x => (int)x);
    public virtual State Next
    {
        get
        {
            var newState = this + 1;
            if (newState > MaxState)
                newState = (int)default(State);
            return (State)newState;
        }
    }

    public virtual void Set(State state, object by = null)
    {
        var allow = (ChangedBy, by) switch
        {
            (Pawn, not null and not Pawn) => false, // lord can force current state inside any zone
            (FenceBuilding, TowerBuilding) => false, // fence is more important than tower
            (BaseBuilding, BaseBuilding) => state == State.Disabled || Value < state,
            _ => true
        };
        if (!allow) return;
        Value = state;
        ChangedBy = by;
    }
    public virtual void Switch(object by) => Set(Next, by);

    public static implicit operator State(ImplantState state) => state.Value;
    public static implicit operator int(ImplantState state) => (int)state.Value;
    public override string ToString() => Value.ToString();
}
