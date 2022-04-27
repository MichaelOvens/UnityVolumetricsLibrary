namespace UVL.Scalar.Rendering
{
    public class ScalarVolumeRenderer : ScalarRenderer
    {
        public void Render (ScalarVolumeRender volume)
        {
            base.Render(volume.texture, volume.physicalSize);
            base.SetWindow(volume.minValue, volume.maxValue);
            base.SetCutoff(volume.minValue, volume.maxValue);
        }
    }
}