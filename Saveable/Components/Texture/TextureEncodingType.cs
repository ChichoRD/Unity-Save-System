using System;

namespace SaveSystem.Saveable.Components.Texture
{
    [Serializable]
    internal enum TextureEncodingType : byte
    {
        Png,
        Jpg,
        Exr,
        Tga,
    }
}