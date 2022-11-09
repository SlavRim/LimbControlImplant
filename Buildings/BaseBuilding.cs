using Steamworks;
using static NeuralInterceptMatrix.ImplantState;

namespace NeuralInterceptMatrix.Buildings;

public partial class BaseBuilding : Building
{
    public Territory Territory;
    public Territory.Locator<Pawn> PawnLocator;

    float radius;
    public float Radius
    {
        get => Territory.Radius = radius;
        set => Territory.Radius = radius = value;
    }

    new Def def => base.def as Def;

    public CompForbiddable Forbiddable => GetComp<CompForbiddable>();
    public CompPowerTrader Power => GetComp<CompPowerTrader>();

    public virtual ImplantState EnterState { get; } = new() { ValueName = "enter_state" };
    public virtual Switcher EnterSwitcher { get; } = new();
    public virtual ImplantState ExitState { get; } = new() { ValueName = "exit_state" };
    public virtual Switcher ExitSwitcher { get; } = new();
    public virtual RadiusResizer ResizerIncrease { get; } = new();
    public virtual RadiusResizer ResizerDecrease { get; } = new();

    public BaseBuilding()
    {
        EnterSwitcher.action = () => EnterState.Switch(this);
        EnterSwitcher.render = x => x.ApplyText(EnterState, EnterToggleLabel, EnterToggleDescription);

        ExitSwitcher.action = () => ExitState.Switch(null);
        ExitSwitcher.render = x => x.ApplyText(ExitState, ExitToggleLabel, ExitToggleDescription);

        ResizerIncrease.action = () => Radius = def.ProperRadius(Mathf.Min(Radius + 1, Map.Size.x/2));
        ResizerIncrease.render = x => x.ApplyText(radius, ResizeIncreaseToggleLabel, ResizeIncreaseToggleDescription);

        ResizerDecrease.action = () => Radius = def.ProperRadius(Mathf.Max(Radius - 1, 0));
        ResizerDecrease.render = x => x.ApplyText(radius, ResizeDecreaseToggleLabel, ResizeDecreaseToggleDescription);
    }

    public override void ExposeData()
    {
        base.ExposeData();
        EnterState.ExposeData();
        ExitState.ExposeData();
        Scribe_Values.Look(ref radius, "radius", def.DefaultRadius);
    }

    public override IEnumerable<Gizmo> GetGizmos()
    {
        var gizmos = base.GetGizmos();
        gizmos ??= new List<Gizmo>();
        if(def.RadiusResizable)
        {
            gizmos = gizmos
                .Prepend(ResizerDecrease)
                .Prepend(ResizerIncrease);
        }
        gizmos = gizmos
            .Prepend(ExitSwitcher)
            .Prepend(EnterSwitcher)
            .Where(x => x is not null);
        foreach (var gizmo in gizmos)
            gizmo.disabled = !Active;
        return gizmos;
    }

    public override void DrawExtraSelectionOverlays()
    {
        base.DrawExtraSelectionOverlays();
        if (Radius > 0.1f) GenDraw.DrawRadiusRing(Position, Radius, Color.green);
    }

    public bool Active => !Forbiddable.Forbidden && Power.PowerOn;

    private void PawnStaying(Pawn pawn)
    {
        TrySetState(pawn, true);
    }
    private void PawnExit(Pawn pawn)
    {
        TrySetState(pawn, false);
    }
    void TrySetState(Pawn pawn, bool enter)
    {
        if (!Active) return;
        pawn.TryGetImplant()?.TrySetState(enter ? EnterState : ExitState, enter ? this : null);
    }

    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
        base.SpawnSetup(map, respawningAfterLoad);
        Territory ??= new(Map, Position, def.specialDisplayRadius);
        var events = Territory.PawnEvents;
        events.OnEnter += PawnStaying;
        events.OnStay += PawnStaying;
        events.OnExit += PawnExit;
        PawnLocator = new(Territory, ThingRequest.ForGroup(ThingRequestGroup.Pawn))
        {
            Predicate = pawn => !pawn.health.Dead && pawn.IsUnderControl()
        };
    }

    bool lastActive;
    public override void Tick()
    {
        base.Tick();
        def.specialDisplayRadius = def.RadiusMax;
        if (!lastActive)
        {
            foreach (var thing in Territory.EnteredThings.ToList())
            {
                Territory.SetEnterState(thing, false);
                if(thing is Pawn pawn) 
                    pawn.TryGetImplant()?.TrySetState(ExitState, this);
            }
        }
        lastActive = Active;
        var consumption = 0f;
#if v1_4
        consumption = Power.Props.PowerConsumption;
#else
        consumption = Power.Props.basePowerConsumption;
#endif
        Power.PowerOutput = -(consumption + (GenRadial.NumCellsInRadius(radius) * def.TilePower));
        if(Active) PawnLocator.Tick();
    }
}
