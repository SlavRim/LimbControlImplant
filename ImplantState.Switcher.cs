namespace NeuralInterceptMatrix;

partial class ImplantState
{
    public class Switcher : Command_Action
    {
        Texture2D toggleIcon;
        public Texture2D ToggleIcon => toggleIcon ??= ContentFinder<Texture2D>.Get(Defs.NIM_StateProperties.toggleIconPath, false);

        public Action<Switcher> render;

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
            ImplantState state,
            string labelTranslation = ImplantToggleLabel,
            string descTranslation = ImplantToggleDescription)
        {
            var label = Defs.NIM_ImplantHediff.label;
            defaultLabel = labelTranslation.Translate(label, (StatePrefix + state).Translate());
            defaultDesc = descTranslation.Translate(label, (StatePrefix + state.Next).Translate());
        }
    }
}
