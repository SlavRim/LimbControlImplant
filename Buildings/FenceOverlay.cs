namespace NeuralInterceptMatrix.Buildings;

[StaticConstructorOnStartup]
public class FenceOverlay : Graphic_Linked
{
    public static readonly FenceOverlay Instance;
    static FenceOverlay()
    {
        Instance = new(GraphicDatabase.Get<Graphic_Single>("Things/Building/Linked/NIM_Fence_OverlayAtlas", ShaderDatabase.MetaOverlay, Vector2.one, Color.yellow));
    }
    public FenceOverlay() { }
    public FenceOverlay(Graphic subGraphic) : base(subGraphic) { }
}