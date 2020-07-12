using System;

namespace RedsAndBlues.GameArea
{
    [Serializable]
    public struct GameAreaSettings
    {
        public float Width;
        public float Height;

        public GameAreaSettings(float width, float height)
        {
            Height = height;
            Width = width;
        }
    }
}