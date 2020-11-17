using System.Drawing;

namespace SoftEngine3D.Imaging
{
    public static class InterpolationHelpers
    {
        public static Color InterpolateColor(Color c0, Color c1)
        {
            return Color.FromArgb(
                InterpolateBytes(c0.A, c1.A),
                InterpolateBytes(c0.R, c1.R),
                InterpolateBytes(c0.G, c1.G),
                InterpolateBytes(c0.B, c1.B));
        }

        public static byte InterpolateBytes(byte b0, byte b1)
        {
            var newValue = ((int)b0 + (int)b1) / 2;

            return (byte)newValue;
        }
    }
}