using System;

namespace UVL
{
    public static class Async
    {
        public const int FRAMES_PER_SECOND_MIN = 30;
        public const double MS_PER_FRAME_MAX = (1f / FRAMES_PER_SECOND_MIN) * 1000f;

        public static bool FrameLimitExceeded(DateTime lastFrame)
        {
            return FrameLimitExceeded(DateTime.Now, lastFrame);
        }

        public static bool FrameLimitExceeded(DateTime start, DateTime end)
        {
            return (end - start).TotalMilliseconds > MS_PER_FRAME_MAX;
        }
    }
}