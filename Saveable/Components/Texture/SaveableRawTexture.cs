using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace SaveSystem.Saveable.Components.Texture
{
    [JsonObject(MemberSerialization.OptOut)]
    [DataContract]
    [Serializable]
    public struct SaveableRawTexture : IReadOnlySaveable<Texture2D>
    {
        public byte[] rawTextureData;
        public int width;
        public int height;
        public GraphicsFormat graphicsFormat;

        internal SaveableRawTexture(byte[] rawTextureData, int width, int height, GraphicsFormat graphicsFormat)
        {
            this.rawTextureData = rawTextureData;
            this.width = width;
            this.height = height;
            this.graphicsFormat = graphicsFormat;
        }

        public readonly Texture2D AsTexture()
        {
            Texture2D texture = new Texture2D(width, height, graphicsFormat, TextureCreationFlags.None);
            return AsTexture(texture);
        }

        public readonly Texture2D AsTexture(Texture2D texture)
        {
            texture.LoadRawTextureData(rawTextureData);
            texture.Apply();
            return texture;
        }

        public readonly Texture2D GetData() => AsTexture();

        public readonly SaveableRawTexture With(byte[] rawTextureData) =>
            new SaveableRawTexture(rawTextureData, width, height, graphicsFormat);

        public static SaveableRawTexture From(Texture2D texture)
        {
            return new SaveableRawTexture(texture.GetRawTextureData(), texture.width, texture.height, texture.graphicsFormat);
        }

        public static SaveableRawTexture From(UnityEngine.Texture texture)
        {
            return From(TextureExtensions.FromTexture(texture));
        }

        public static SaveableRawTexture From(RenderTexture texture)
        {
            return From(TextureExtensions.FromRenderTexture(texture));
        }
    }

    //internal struct SaveableQoiImage
}