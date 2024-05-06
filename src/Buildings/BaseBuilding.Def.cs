namespace NeuralInterceptMatrix.Buildings;

public partial class BaseBuilding
{
    public class Def : ThingDef
    {
        public Def()
        {      
            tickerType = TickerType.Normal;
        }
        public bool RadiusResizable = true;
        public int DefaultRadius;
        public int RadiusMin, RadiusMax;
        public int ProperRadius(float radius) => Mathf.Clamp(Mathf.RoundToInt(radius), RadiusMin, Math.Max(RadiusMax, RadiusMin));
        public float TilePower;
        public override void PostLoad()
        {
            base.PostLoad();
            comps.AddRange(new List<CompProperties>
            {
                new CompProperties_Flickable(),
                new CompProperties_Forbiddable()
            });
        }
    }
}
