using SaveSystem.Saveable.Components.Texture;
using System;
using UnityEngine;

namespace SaveSystem.Saveable.Components
{
    public readonly struct SaveableTexture : ISaveable<IReadOnlySaveable<Texture2D>>
    {
        [Serializable]
        public enum EncodingType
        {
            Png = TextureEncodingType.Png,
            Jpg = TextureEncodingType.Jpg,
            Exr = TextureEncodingType.Exr,
            Tga = TextureEncodingType.Tga,
            Qoi,
            Raw,
        }

        private readonly RenderTexture _renderTexture;
        private readonly EncodingType _encodingType;

        public SaveableTexture(RenderTexture renderTexture, EncodingType encodingType)
        {
            _encodingType = encodingType;
            _renderTexture = renderTexture;
        }

        public IReadOnlySaveable<Texture2D> GetData() => _encodingType switch
        {
            EncodingType.Png => SaveableEncodedTexture.From(_renderTexture, (TextureEncodingType)_encodingType),
            EncodingType.Jpg => SaveableEncodedTexture.From(_renderTexture, (TextureEncodingType)_encodingType),
            EncodingType.Exr => SaveableEncodedTexture.From(_renderTexture, (TextureEncodingType)_encodingType),
            EncodingType.Tga => SaveableEncodedTexture.From(_renderTexture, (TextureEncodingType)_encodingType),
            EncodingType.Qoi => SaveableEncodedTexture<QoiTextureEncoder>.From(_renderTexture, QoiTextureEncoder.Instance),
            EncodingType.Raw => SaveableRawTexture.From(_renderTexture),
            _ => SaveableEncodedTexture.From(_renderTexture, TextureEncodingType.Png),
        };

        public bool TrySetData(IReadOnlySaveable<Texture2D> saveData)
        {
            Graphics.Blit(saveData.GetData(), _renderTexture);
            return true;
        }
    }
}
