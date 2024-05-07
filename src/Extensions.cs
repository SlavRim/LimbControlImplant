namespace NeuralInterceptMatrix;

public static partial class Extensions
{
    public static bool IsUnderControl(this Pawn pawn) => pawn.IsColonist || pawn.IsSlaveOfColony || pawn.IsPrisonerOfColony;
    public static ImplantHediff TryGetImplant(this Pawn pawn)
    {
        var hediffSet = pawn?.health.hediffSet;
#if v1_3
        var NIM = hediffSet?.GetHediffs<ImplantHediff>().FirstOrDefault();
#else
        var NIM = hediffSet?.GetFirstHediff<ImplantHediff>();
#endif
        return NIM;
    }
}