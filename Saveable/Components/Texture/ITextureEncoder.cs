using UnityEngine.Experimental.Rendering;

namespace SaveSystem.Saveable.Components.Texture
{
    public interface ITextureEncoder
    {
        byte[] Encode(byte[] rawData, int width, int height, GraphicsFormat graphicsFormat);
        byte[] Decode(byte[] encodedData, out int width, out int height, ref GraphicsFormat graphicsFormat);
    }
}