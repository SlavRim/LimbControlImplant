namespace NeuralInterceptMatrix.Buildings;

public class TowerDef : BaseBuilding.Def
{
    public const string DefName = nameof(Defs.NIM_TowerBuilding);
    public TowerDef()
    {
        defName = DefName;
        thingClass = typeof(TowerBuilding);
    }
}