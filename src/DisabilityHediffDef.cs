namespace NeuralInterceptMatrix;

partial class Definitions
{
    public static DisabilityHediffDef NIM_DisabilityHediff;
}
public partial class DisabilityHediffDef : HediffDef
{
    public const string DefName = nameof(Defs.NIM_DisabilityHediff);
    public DisabilityHediffDef()
    {
        defName = DefName;
        hediffClass = typeof(DisabilityHediff);
    }
}
