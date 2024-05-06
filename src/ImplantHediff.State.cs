using static NeuralInterceptMatrix.ImplantState;

namespace NeuralInterceptMatrix;

public partial class ImplantHediff
{
    public virtual ImplantState State { get; } = new();

    public bool AbleToSetState => (!Health.Dead) && Pawn.IsUnderControl();

    public override void ExposeData()
    {
        base.ExposeData();
        State.ExposeData();
    }

    void TrySwitchState()
    {
        TrySetState(State.Next, Pawn);
    }
    public void TrySetState(State state, object by)
    {
        if (!AbleToSetState) return;
        SetState(state, by);
    }
    public void SetState(State state, object by)
    {
        State.Set(state, by);
    }

    public void ApplyState()
    {
        if (Pawn is null) return;
        RemoveDisabillities();
        State state = State;
        switch (state)
        {
            case ImplantState.State.Arms:
                AddDisability(BodyPartTagDefOf.ManipulationLimbCore, GetDisabilityStage(PawnCapacityDefOf.Manipulation));
                break;
            case ImplantState.State.Legs:
                AddDisability(BodyPartTagDefOf.MovingLimbCore, GetDisabilityStage(PawnCapacityDefOf.Moving));
                break;
            default:
                break;
        }
    }

    public override void Tick()
    {
        if (!AbleToSetState) 
        base.Tick();
    }
}