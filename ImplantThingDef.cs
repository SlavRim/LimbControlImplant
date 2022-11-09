using NeuralInterceptMatrix.Recipes;

namespace NeuralInterceptMatrix;

partial class Definitions
{
    public static ImplantThingDef NIM_Implant;
    public static ImplantRemoveSurgery NIM_ImplantRemove;
    public static ImplantInstallSurgery NIM_ImplantInstall;


    static List<DefHyperlink> hyperlinks;
    public static List<DefHyperlink> Hyperlinks => hyperlinks ??= new()
    {
        new DefHyperlink(NIM_Implant)
    };
}
public class ImplantThingDef : ThingDef
{
    public const string DefName = nameof(Defs.NIM_Implant);
    public override void ResolveReferences()
    {
        base.ResolveReferences();
    }
    public ImplantThingDef()
    {
        defName = DefName;
    }
}
