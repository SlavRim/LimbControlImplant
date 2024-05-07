namespace NeuralInterceptMatrix.Recipes;

public abstract class ImplantSurgery : RecipeDef
{
    public override void ResolveReferences()
    {
        base.ResolveReferences();
        recipeUsers = new() { ThingDefOf.Human };
        appliedOnFixedBodyParts = new()
        {
            Defs.Neck
        };
        descriptionHyperlinks = Defs.Hyperlinks;
    }
    public ImplantSurgery() { }
}
