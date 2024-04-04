using QoiSharp;
using QoiSharp.Codec;
using UnityEngine.Experimental.Rendering;

namespace SaveSystem.Saveable.Components.Texture
{
    internal readonly struct QoiTextureEncoder : ITextureEncoder
    {
        public static readonly QoiTextureEncoder Instance = new QoiTextureEncoder();

        public byte[] Encode(byte[] rawData, int width, int height, GraphicsFormat graphicsFormat) =>
            QoiEncoder.Encode(new QoiImage(
                rawData,
                width,
                height,
                ((((uint)graphicsFormat >> 2) << 2) == (uint)graphicsFormat) ? Channels.RgbWithAlpha : Channels.Rgb,
                ColorSpace.Linear));

        public byte[] Decode(byte[] encodedData, out int width, out int height, out GraphicsFormat graphicsFormat)
        {
            QoiImage image = QoiDecoder.Decode(encodedData);
            width = image.Width;
            height = image.Height;
            graphicsFormat = image.Channels == Channels.RgbWithAlpha ? GraphicsFormat.R8G8B8A8_UNorm : GraphicsFormat.R8G8B8_UNorm;
            return image.Data;
        }
    }
}