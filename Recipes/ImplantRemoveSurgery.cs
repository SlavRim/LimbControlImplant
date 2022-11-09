namespace NeuralInterceptMatrix.Recipes;

public class ImplantRemoveSurgery : ImplantSurgery
{
    public const string DefName = nameof(Defs.NIM_ImplantRemove);
    public override void ResolveReferences()
    {
        base.ResolveReferences();
        removesHediff = Defs.NIM_ImplantHediff;
    }
    public ImplantRemoveSurgery()
    {
        defName = DefName;
        workerClass = typeof(Recipe_RemoveImplant);
    }
}