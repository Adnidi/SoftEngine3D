using FluentAssertions;
using SoftEngine3D.Primitives;
using Xunit;

namespace SoftEngine3DTests.Primitives
{
    public class Vector3NormalizeTest
    {
        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(1, 0, 0)]
        [InlineData(1, 1, 1)]
        [InlineData(55.42, 54738.2, 0.24)]
        public void LengthAfterNormalizeIsOne(float x, float y, float z)
        {
            var vector = new Vector3(x, y, z);

            var length = vector.Normalize().Length;

            length.Should().BeApproximately(1.0f, 0.0001f);
        }
    }
}