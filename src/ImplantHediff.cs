using static NeuralInterceptMatrix.ImplantState;

namespace NeuralInterceptMatrix;

public partial class ImplantHediff : Hediff_Implant
{
    public ImplantHediff()
    {
        Switcher.action = TrySwitchState;
        Switcher.render = x =>
        {
            var label = Defs.NIM_ImplantHediff.label;
            x.defaultLabel = ImplantToggleLabel.Translate(label, (StatePrefix + State).Translate());
            x.defaultDesc = ImplantToggleDescription.Translate(label, (StatePrefix + State.Next).Translate());
            x.disabled = !AbleToSetState;
        };
        State.StateUpdated += ApplyState;
    }

    public Pawn Pawn => pawn;
    public Pawn_HealthTracker Health => pawn.health;
    public BodyDef Body => Part.body;

    public virtual Switcher Switcher { get; } = new();

    public override IEnumerable<Gizmo> GetGizmos()
    {
        var gizmos = base.GetGizmos();
        gizmos ??= new List<Gizmo>();
        return gizmos.Prepend(Switcher);
    }

    private static PawnCapacityModifier GetBadCapacityModifier(PawnCapacityDef capacity) => new() {
        capacity = capacity,
        offset = -1,
        setMax = 0.0f
    };

    private static HediffStage GetDisabilityStage(PawnCapacityDef capacity) => new() {
        capMods = new() {
            GetBadCapacityModifier(capacity)
        }
    };

    [NoTranslate]
    private readonly List<DisabilityHediff> disabilities = new();
    public IEnumerable<DisabilityHediff> Disabilities
    {
        get
        {
#if v1_4
            List<DisabilityHediff> hediffs = new();
            Health.hediffSet.GetHediffs<DisabilityHediff>(ref hediffs);
            return disabilities
                .Concat(hediffs)
                .Distinct();
#else
            return disabilities
                .Concat(Health.hediffSet.GetHediffs<DisabilityHediff>())
                .Distinct();
#endif
        }
    }
    public void RemoveDisabillities()
    {
        foreach (var disability in Disabilities)
            RemoveDisability(disability);

        disabilities.Clear();
    }
    public void RemoveDisability(DisabilityHediff hediff) => Health.RemoveHediff(hediff);
    public void AddDisability(BodyPartTagDef tag, HediffStage stage)
    {
        foreach (var part in Body.GetPartsWithTag(tag))
        {
            try
            {
                var hediff = new DisabilityHediff(stage);
                Health.AddHediff(hediff, part);
                disabilities.Add(hediff);
            }
            catch { }
        }
    }

    public override void PostTick()
    {
        base.PostTick();
        if (State == ImplantState.State.Disabled) return;
        if (Pawn is null) return;
        DisallowBadJob(Pawn.jobs);
    }

    public void DisallowBadJob(Pawn_JobTracker jobs, JobCondition condition = JobCondition.InterruptForced)
    {
        if (jobs.curJob.jobGiver is
            JobGiver_AIFightEnemy or
            JobGiver_AIFightEnemies or
            JobGiver_Haul or
            JobGiver_Work or
            JobGiver_Kidnap
            ||
            jobs.curDriver is
            JobDriver_AttackMelee or
            JobDriver_AttackStatic or
            JobDriver_CastAbility or
            JobDriver_Equip or
            JobDriver_Kill or
            JobDriver_Ignite)
            jobs.EndCurrentJob(condition);
    }
}