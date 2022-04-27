namespace UVL.Scalar.Rendering
{
    public class ScalarStackRenderer : ScalarRenderer
    {
        public int length { get; private set; }
        public int index { get; private set; }

        private ScalarStackRender stack;

        public void Render (ScalarStackRender stack)
        {
            this.stack = stack;

            length = stack.slices.Length;
            index = length / 2;

            base.SetWindow(this.stack.minValue, this.stack.maxValue);
            base.SetCutoff(this.stack.minValue, this.stack.maxValue);

            ViewSliceAtIndex(index);
        }

        public void ViewSliceAtIndex (int value)
        {
            if (stack == null) return;

            // Clamp index to stack length
            index = value;
            if (index < 0) index = 0;
            if (index >= length) index = length - 1;

            base.Render(stack.slices[index].texture, stack.slices[index].physicalSize);
        }
    }
}