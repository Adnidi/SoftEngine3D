using FluentAssertions;
using SoftEngine3D.Primitives;
using Xunit;

namespace SoftEngine3DTests.Primitives
{
    public class Vector3LengthTest
    {
        [Theory]
        [InlineData(1, 0, 0)]
        [InlineData(0, 1, 0)]
        [InlineData(0, 0, 1)]
        public void UnitVectorLengthIsOne(int x, int y, int z)
        {
            var vector = new Vector3(x, y, z);

            var length = vector.Length;

            length.Should().Be(1);
        }

        [Fact]
        public void ZeroVectorLengthIsZero()
        {
            var vector = new Vector3(0, 0, 0);

            var length = vector.Length;

            length.Should().Be(0);
        }

        [Theory]
        [InlineData(1, 1, 1, 1.7320)]
        [InlineData(1, 2, 3, 3.7417)]
        [InlineData(3, 2, 1, 3.7417)]
        [InlineData(2, 3, 1, 3.7417)]
        [InlineData(1.2, 2.6, 1, 3.0332)]
        public void VectorLengthShouldBeCorrectEuclideanLength(float x, float y, float z, float expected)
        {
            var vector = new Vector3(x, y, z);

            var length = vector.Length;

            length.Should().BeApproximately(expected, 0.0001f);
        }
    }
}