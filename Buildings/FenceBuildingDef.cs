namespace NeuralInterceptMatrix.Buildings;

public class FenceDef : BaseBuilding.Def
{
    public const string DefName = nameof(Defs.NIM_FenceBuilding);
    public FenceDef()
    {
        defName = DefName;
        thingClass = typeof(FenceBuilding);
        RadiusResizable = false;
        RadiusMax = 0;
        RadiusMin = 0;
        TilePower = 0f;
    }
}