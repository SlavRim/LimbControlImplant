namespace NeuralInterceptMatrix;

public partial class DisabilityHediff : Hediff
{
    public DisabilityHediff()
    {
        def = Defs.NIM_DisabilityHediff;
    }
    public DisabilityHediff(HediffStage stage) : this()
    {
        (Stage = stage).partEfficiencyOffset = -100;
    }
    public override bool ShouldRemove => false;

    public HediffStage Stage;
    public override HediffStage CurStage => Stage;
}