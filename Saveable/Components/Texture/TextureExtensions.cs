using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace SaveSystem.Saveable.Components.Texture
{
    internal static class TextureExtensions
    {
        public static Texture2D FromRenderTexture(RenderTexture renderTexture)
        {
            RenderTexture.active = renderTexture;

            Texture2D destination = new Texture2D(renderTexture.width, renderTexture.height, renderTexture.graphicsFormat, TextureCreationFlags.None);
            destination.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);

            RenderTexture.active = null;
            return destination;
        }

        public static Texture2D FromTexture(UnityEngine.Texture texture)
        {
            RenderTexture temporaryBuffer = RenderTexture.GetTemporary(texture.width, texture.height, 0, texture.graphicsFormat);
            Graphics.Blit(texture, temporaryBuffer);

            Texture2D destination = FromRenderTexture(temporaryBuffer);
            RenderTexture.ReleaseTemporary(temporaryBuffer);

            return destination;
        }
    }
}