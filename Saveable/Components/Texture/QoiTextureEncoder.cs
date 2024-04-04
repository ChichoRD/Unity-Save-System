using QoiSharp;
using QoiSharp.Codec;
using UnityEngine.Experimental.Rendering;

namespace SaveSystem.Saveable.Components.Texture
{
    internal readonly struct QoiTextureEncoder : ITextureEncoder
    {
        public static readonly QoiTextureEncoder Instance = new QoiTextureEncoder();
        public byte[] Encode(byte[] rawData, int width, int height, GraphicsFormat graphicsFormat) =>
            ChannelsCountFrom(graphicsFormat) switch
            {
                1 => QoiEncoder.Encode(new QoiImage(RGBFromR(rawData), width, height, Channels.Rgb, ColorSpace.Linear)),
                2 => QoiEncoder.Encode(new QoiImage(RGBFromRG(rawData), width, height, Channels.Rgb, ColorSpace.Linear)),
                3 => QoiEncoder.Encode(new QoiImage(rawData, width, height, Channels.Rgb, ColorSpace.Linear)),
                4 => QoiEncoder.Encode(new QoiImage(rawData, width, height, Channels.RgbWithAlpha, ColorSpace.Linear)),
                _ => new byte[0],
            };

        public byte[] Decode(byte[] encodedData, out int width, out int height, ref GraphicsFormat graphicsFormat)
        {
            QoiImage image = QoiDecoder.Decode(encodedData);
            width = image.Width;
            height = image.Height;
            return ChannelsCountFrom(graphicsFormat) switch
            {
                1 => RFromRGB(image.Data),
                2 => RGFromRGB(image.Data),
                3 => image.Data,
                4 => image.Data,
                _ => new byte[0],
            };
        }

        private static int ChannelsCountFrom(GraphicsFormat graphicsFormat) => (((int)graphicsFormat - 1) % 4) + 1;

        private static byte[] RGBFromR(byte[] bytes)
        {
            byte[] rgbBytes = new byte[bytes.Length * 3];
            for (int i = 0; i < bytes.Length; i++)
            {
                rgbBytes[i * 3] = bytes[i];
                rgbBytes[i * 3 + 1] = bytes[i];
                rgbBytes[i * 3 + 2] = bytes[i];
            }
            return rgbBytes;
        }

        private static byte[] RGBFromRG(byte[] bytes)
        {
            byte[] rgbBytes = new byte[bytes.Length * 3 / 2];
            for (int i = 0; i < bytes.Length / 2; i++)
            {
                rgbBytes[i * 3] = bytes[i * 2];
                rgbBytes[i * 3 + 1] = bytes[i * 2 + 1];
                rgbBytes[i * 3 + 2] = bytes[i * 2];
            }
            return rgbBytes;
        }

        private static byte[] RFromRGB(byte[] bytes)
        {
            byte[] rBytes = new byte[bytes.Length / 3];
            for (int i = 0; i < rBytes.Length; i++)
            {
                rBytes[i] = bytes[i * 3];
            }
            return rBytes;
        }

        private static byte[] RGFromRGB(byte[] bytes)
        {
            byte[] rgBytes = new byte[bytes.Length / 3 * 2];
            for (int i = 0; i < rgBytes.Length / 2; i++)
            {
                rgBytes[i * 2] = bytes[i * 3];
                rgBytes[i * 2 + 1] = bytes[i * 3 + 1];
            }
            return rgBytes;
        }
    }
}