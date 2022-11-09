namespace NeuralInterceptMatrix.Recipes;

public class ImplantInstallSurgery : ImplantSurgery
{
    public const string DefName = nameof(Defs.NIM_ImplantInstall);
    public override void ResolveReferences()
    {
        base.ResolveReferences();
        addsHediff = Defs.NIM_ImplantHediff;
    }
    public ImplantInstallSurgery()
    {
        defName = DefName;
        workerClass = typeof(Recipe_InstallImplant);
    }
}