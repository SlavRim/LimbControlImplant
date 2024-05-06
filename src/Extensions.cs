namespace NeuralInterceptMatrix;

public static partial class Extensions
{
    public static bool IsUnderControl(this Pawn pawn) => pawn.IsColonist || pawn.IsSlaveOfColony || pawn.IsPrisonerOfColony;
    public static ImplantHediff TryGetImplant(this Pawn pawn)
    {
        var hediffSet = pawn?.health.hediffSet;
#if v1_4
        var NIM = hediffSet?.GetFirstHediff<ImplantHediff>();
#else
        var NIM = hediffSet?.GetHediffs<ImplantHediff>().FirstOrDefault();
#endif
        return NIM;
    }
}