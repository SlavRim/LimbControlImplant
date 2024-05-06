namespace NeuralInterceptMatrix;

partial class Definitions
{
    public static ImplantState_Properties NIM_StateProperties;
}
public class ImplantState_Properties : Def
{
    public const string DefName = nameof(Defs.NIM_StateProperties);
    public ImplantState_Properties()
    {
        defName = DefName;
    }
    public string toggleIconPath;
}