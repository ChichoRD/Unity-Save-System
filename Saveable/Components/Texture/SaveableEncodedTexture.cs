using System.Runtime.Serialization;
using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using Newtonsoft.Json;

namespace SaveSystem.Saveable.Components.Texture
{
    [JsonObject(MemberSerialization.OptOut)]
    [DataContract]
    [Serializable]
    public struct SaveableEncodedTexture : IReadOnlySaveable<Texture2D>
    {
        public byte[] encodedTextureData;
        public GraphicsFormat graphicsFormat;

        internal SaveableEncodedTexture(byte[] encodedTextureData, GraphicsFormat graphicsFormat)
        {
            this.encodedTextureData = encodedTextureData;
            this.graphicsFormat = graphicsFormat;
        }

        public readonly Texture2D AsTexture()
        {
            Texture2D texture = new Texture2D(2, 2, graphicsFormat, TextureCreationFlags.None);
            return AsTexture(texture);
        }

        public readonly Texture2D AsTexture(Texture2D texture)
        {
            ImageConversion.LoadImage(texture, encodedTextureData);
            return texture;
        }

        public readonly Texture2D GetData() => AsTexture();

        internal static SaveableEncodedTexture From(Texture2D texture, TextureEncodingType encodingType) =>
            new SaveableEncodedTexture(encodingType switch
            {
                TextureEncodingType.Png => texture.EncodeToPNG(),
                TextureEncodingType.Jpg => texture.EncodeToJPG(),
                TextureEncodingType.Exr => texture.EncodeToEXR(),
                TextureEncodingType.Tga => texture.EncodeToTGA(),
                _ => texture.EncodeToPNG(),
            },
            texture.graphicsFormat);

        internal static SaveableEncodedTexture From(UnityEngine.Texture texture, TextureEncodingType encodingType) =>
            From(TextureExtensions.FromTexture(texture), encodingType);

        internal static SaveableEncodedTexture From(RenderTexture texture, TextureEncodingType encodingType) =>
            From(TextureExtensions.FromRenderTexture(texture), encodingType);
    }

    [JsonObject(MemberSerialization.OptOut)]
    [DataContract]
    [Serializable]
    public struct SaveableEncodedTexture<TEncoder> : IReadOnlySaveable<Texture2D>
        where TEncoder : ITextureEncoder
    {
        public byte[] encodedTextureData;
        public int width;
        public int height;
        public GraphicsFormat graphicsFormat;

        [JsonIgnore]
        [IgnoreDataMember]
        [NonSerialized]
        private readonly TEncoder _encoder;

        internal SaveableEncodedTexture(byte[] encodedTextureData, int width, int height, GraphicsFormat graphicsFormat, TEncoder encoder)
        {
            this.encodedTextureData = encodedTextureData;
            this.width = width;
            this.height = height;
            this.graphicsFormat = graphicsFormat;
            _encoder = encoder;
        }

        public readonly Texture2D AsTexture()
        {
            Texture2D texture = new Texture2D(width, height, graphicsFormat, TextureCreationFlags.None);
            return AsTexture(ref texture);
        }

        public readonly Texture2D AsTexture(ref Texture2D texture)
        {
            GraphicsFormat graphicsFormat = this.graphicsFormat;
            byte[] rawTextureData = _encoder.Decode(encodedTextureData, out int width, out int height, ref graphicsFormat);
            texture = new Texture2D(width, height, graphicsFormat, TextureCreationFlags.None);
            texture.LoadRawTextureData(rawTextureData);
            texture.Apply();

            return texture;
        }

        public readonly Texture2D GetData() => AsTexture();

        internal static SaveableEncodedTexture<UEncoder> From<UEncoder>(Texture2D texture, UEncoder encoder)
            where UEncoder : ITextureEncoder =>
            new SaveableEncodedTexture<UEncoder>(
                encoder.Encode(texture.GetRawTextureData(), texture.width, texture.height, texture.graphicsFormat), texture.width, texture.height, texture.graphicsFormat,
                encoder);

        internal static SaveableEncodedTexture<UEncoder> From<UEncoder>(UnityEngine.Texture texture, UEncoder encoder)
            where UEncoder : ITextureEncoder =>
            From(TextureExtensions.FromTexture(texture), encoder);

        internal static SaveableEncodedTexture<UEncoder> From<UEncoder>(RenderTexture texture, UEncoder encoder)
            where UEncoder : ITextureEncoder =>
            From(TextureExtensions.FromRenderTexture(texture), encoder);
    }
}