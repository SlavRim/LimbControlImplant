namespace NeuralInterceptMatrix.Buildings;

public class FenceBuilding : BaseBuilding
{
    public FenceBuilding()
    {
    }

    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
        base.SpawnSetup(map, respawningAfterLoad);
        PawnLocator = PawnLocator with { Entered = x => Position.GetThingList(Map).Contains(x) };
    }
    public override void PrintForPowerGrid(SectionLayer layer)
    {
        base.PrintForPowerGrid(layer);
        FenceOverlay.Instance.Print(layer, this, 0f);
    }
}
