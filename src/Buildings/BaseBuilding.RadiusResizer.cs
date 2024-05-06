namespace NeuralInterceptMatrix.Buildings;

public partial class BaseBuilding
{
    public class RadiusResizer : Command_Action
    {
        Texture2D toggleIcon;
        public Texture2D ToggleIcon => toggleIcon ??= ContentFinder<Texture2D>.Get(Defs.NIM_StateProperties.toggleIconPath, false);

        public Action<RadiusResizer> render;

        public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
        {
            try
            {
                icon = ToggleIcon;
                render?.Invoke(this);
            }
            catch (Exception ex) { Log.Error(ex + ""); }
            return base.GizmoOnGUI(topLeft, maxWidth, parms);
        }

        public void ApplyText(
             float radius,
             string labelTranslation = ResizeDecreaseToggleLabel,
             string descTranslation = ResizeDecreaseToggleDescription)
        {
            var intRadius = Mathf.RoundToInt(radius);
            defaultLabel = labelTranslation.Translate(intRadius);
            defaultDesc = descTranslation.Translate(intRadius);
        }
    }
}
