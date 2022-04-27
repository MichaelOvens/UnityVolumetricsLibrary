namespace UVL.Scalar.Rendering
{
    public class ScalarSliceRenderer : ScalarRenderer
    {
        public void Render(ScalarSliceRender slice)
        {
            base.Render(slice.texture, slice.physicalSize);
            base.SetWindow(slice.minValue, slice.maxValue);
            base.SetCutoff(slice.minValue, slice.maxValue);
        }
    }
}