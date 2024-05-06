namespace NeuralInterceptMatrix;

partial class Definitions
{
    public static ImplantHediffDef NIM_ImplantHediff;
}
public partial class ImplantHediffDef : HediffDef
{
    public const string DefName = nameof(Defs.NIM_ImplantHediff);
    public override void ResolveReferences()
    {
        base.ResolveReferences();
        descriptionHyperlinks = Defs.Hyperlinks;
        spawnThingOnRemoved = Defs.NIM_Implant;
    }
    public ImplantHediffDef()
    {
        defName = DefName;
        hediffClass = typeof(ImplantHediff);
        defaultLabelColor = new Color32(244, 210, 131, 255);
    }
}