using UnityEngine;

namespace UVL.Scalar.Rendering
{
    public class ScalarSliceRender
    {
        public Texture2D texture;
        public Vector2 physicalSize;
        public float minValue, maxValue;
    }

    public class ScalarStackRender
    {
        public ScalarSliceRender[] slices;
        public float minValue, maxValue;
    }

    public class ScalarVolumeRender
    {
        public Texture3D texture;
        public Vector3 physicalSize;
        public float minValue, maxValue;
    }

    public static class MaskDataToScalarRender
    {
        public static AsyncResult<ScalarSliceRender> ConvertToSliceAsync(MaskSliceData data)
            => throw new System.NotImplementedException();

        public static AsyncResult<ScalarStackRender> ConvertToStackAsync(MaskStackData data)
            => throw new System.NotImplementedException();

        public static AsyncResult<ScalarStackRender> ConvertToStackAsync(MaskVolumeData data)
            => throw new System.NotImplementedException();

        public static AsyncResult<ScalarVolumeRender> ConvertToVolumeAsync(MaskStackData data)
            => throw new System.NotImplementedException();

        public static AsyncResult<ScalarVolumeRender> ConvertToVolumeAsync(MaskVolumeData data)
            => throw new System.NotImplementedException();
    }

    public static class ScalarDataToScalarRender
    {
        public static AsyncResult<ScalarSliceRender> ToScalarSliceRenderAsync(this ScalarSliceData data)
            => Conversion.ScalarSliceDataToScalarSliceRenderConversion.RenderAsync(data);

        public static AsyncResult<ScalarStackRender> ToScalarStackRenderAsync(this ScalarStackData data)
            => Conversion.ScalarStackDataToScalarStackRenderConversion.RenderAsync(data);

        public static AsyncResult<ScalarStackRender> ToScalarStackRenderAsync(this ScalarVolumeData data)
            => Conversion.ScalarVolumeDataToScalarStackRenderConversion.RenderAsync(data);

        public static AsyncResult<ScalarVolumeRender> ToScalarVolumeRenderAsync(this ScalarStackData data)
            => Conversion.ScalarStackDataToScalarVolumeRenderConversion.RenderAsync(data);

        public static AsyncResult<ScalarVolumeRender> ToScalarVolumeRenderAsync(this ScalarVolumeData data)
            => Conversion.ScalarVolumeDataToScalarVolumeRenderConversion.RenderAsync(data);
    }
}