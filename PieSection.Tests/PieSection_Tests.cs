using System;
using Xunit;

namespace RaphsTools.UnitTests.Services
{
    public class PieSection_Tests
    {
        private readonly RaphsTools.PieSection _pieSection;

        public PieSection_Tests()
        {
            _pieSection = new RaphsTools.PieSection(90, 360);
        }

        [Fact]
        public void SetCenter_InputIs180_SetsBounds()
        {
            _pieSection.center = 180;

            Assert.Equal(180, _pieSection.center);
            Assert.Equal(180 - 45, _pieSection.lowerBound);
            Assert.Equal(180 + 45, _pieSection.upperBound);
        }

        [Fact]
        public void SetLower_InputIs180_SetsUpperAndCenter()
        {
            _pieSection.lowerBound = 180;

            Assert.Equal(180, _pieSection.lowerBound);
            Assert.Equal(180 + 45, _pieSection.center);
            Assert.Equal(180 + 90, _pieSection.upperBound);
        }

        [Fact]
        public void SetUpper_InputIs180_SetsLowerAndCenter()
        {
            _pieSection.upperBound = 180;

            Assert.Equal(180, _pieSection.upperBound);
            Assert.Equal(180 - 45, _pieSection.center);
            Assert.Equal(180 - 90, _pieSection.lowerBound);
        }

        [Fact]
        public void SetCenter_InputIsNearFullCircle_SetsAndWrapsUpperBound()
        {
            _pieSection.center = 350;

            Assert.Equal(350, _pieSection.center);
            Assert.Equal(350 - 45, _pieSection.lowerBound);
            Assert.Equal((350 + 45) % 360, _pieSection.upperBound);
        }

        [Fact]
        public void SetLower_InputIsNearFullCircle_SetsAndWrapsUpperBoundAndCenter()
        {
            _pieSection.lowerBound = 350;

            Assert.Equal(350, _pieSection.lowerBound);
            Assert.Equal((350 + 45) % 360, _pieSection.center);
            Assert.Equal((350 + 90) % 360, _pieSection.upperBound);
        }

        [Fact]
        public void SetUpper_InputIsNearZero_SetsAndWrapsLowerBoundAndCenter()
        {
            _pieSection.upperBound = 10;

            Assert.Equal(10, _pieSection.upperBound);
            Assert.Equal(10 - 45 + 360, _pieSection.center);
            Assert.Equal(10 - 90 + 360, _pieSection.lowerBound);
        }

        [Fact]
        public void SetUpper_InputKeepsCenterFromWrapping_SetsCenterAndSetsAndWrapsLowerBound()
        {
            _pieSection.upperBound = 60;

            Assert.Equal(60, _pieSection.upperBound);
            Assert.Equal(60 - 45, _pieSection.center);
            Assert.Equal(60 - 90 + 360, _pieSection.lowerBound);
        }

        [Fact]
        public void SetCenter_InputIsNearZero_SetsAndWrapsLowerBound()
        {
            _pieSection.center = 10;

            Assert.Equal(10, _pieSection.center);
            Assert.Equal(10 - 45 + 360, _pieSection.lowerBound);
            Assert.Equal(10 + 45, _pieSection.upperBound);
        }

        [Fact]
        public void SetCenter_InputIsZero_SetsAndWrapsBounds()
        {
            _pieSection.center = 0;

            Assert.Equal(0, _pieSection.center);
            Assert.Equal(-45 + 360, _pieSection.lowerBound);
            Assert.Equal(45, _pieSection.upperBound);
        }

        [Fact]
        public void SetLower_InputIsZero_SetsCenterAndBounds()
        {
            _pieSection.lowerBound = 0;

            Assert.Equal(0, _pieSection.lowerBound);
            Assert.Equal(45, _pieSection.center);
            Assert.Equal(90, _pieSection.upperBound);
        }

        [Fact]
        public void SetUpper_InputIsZero_SetsAndWrapsCenterAndLowerBound()
        {
            _pieSection.upperBound = 0;

            Assert.Equal(0, _pieSection.upperBound);
            Assert.Equal(-45 + 360, _pieSection.center);
            Assert.Equal(-90 + 360, _pieSection.lowerBound);
        }

        [Fact]
        public void DiffWithCenterSectionIsNotCrossingZero_InputIsAtCenter_ReturnsZero()
        {
            _pieSection.center = 180;
            float diff = _pieSection.diffWithCenter(_pieSection.center);

            Assert.Equal(0, diff);
        }

        [Fact]
        public void DiffWithCenterSectionIsNotCrossingZero_InputIsAtUpperBound_ReturnsHalfSectionSize()
        {
            _pieSection.center = 180;
            float diff = _pieSection.diffWithCenter(_pieSection.upperBound);

            Assert.Equal(45, diff);
        }

        [Fact]
        public void DiffWithCenterSectionIsNotCrossingZero_InputIsAtLowerBound_ReturnsMinusHalfSectionSize()
        {
            _pieSection.center = 180;
            float diff = _pieSection.diffWithCenter(_pieSection.lowerBound);

            Assert.Equal(-45, diff);
        }

        [Fact]
        public void DiffWithCenterSectionCrossingZeroCenterPositive_InputIsAtCenter_ReturnsZero()
        {
            _pieSection.center = 10;
            float diff = _pieSection.diffWithCenter(_pieSection.center);

            Assert.Equal(0, diff);
        }

        [Fact]
        public void DiffWithCenterSectionCrossingZeroCenterNegative_InputIsAtCenter_ReturnsZero()
        {
            _pieSection.center = -10;
            float diff = _pieSection.diffWithCenter(_pieSection.center);

            Assert.Equal(0, diff);
        }

        [Fact]
        public void DiffWithCenterSectionCrossingZeroCenterPositive_InputIsAtUpperBound_ReturnsHalfSectionSize()
        {
            _pieSection.center = 10;
            float diff = _pieSection.diffWithCenter(_pieSection.upperBound);

            Assert.Equal(45, diff);
        }

        [Fact]
        public void DiffWithCenterSectionCrossingZeroCenterNegative_InputIsAtUpperBound_ReturnsHalfSectionSize()
        {
            _pieSection.center = -10;
            float diff = _pieSection.diffWithCenter(_pieSection.upperBound);

            Assert.Equal(45, diff);
        }

        [Fact]
        public void DiffWithCenterSectionCrossingZeroCenterPositive_InputIsAtLowerBound_ReturnsMinusHalfSectionSize()
        {
            _pieSection.center = 10;
            float diff = _pieSection.diffWithCenter(_pieSection.lowerBound);

            Assert.Equal(-45, diff);
        }

        [Fact]
        public void DiffWithCenterSectionCrossingZeroCenterNegative_InputIsAtLowerBound_ReturnsMinusHalfSectionSize()
        {
            _pieSection.center = -10;
            float diff = _pieSection.diffWithCenter(_pieSection.lowerBound);

            Assert.Equal(-45, diff);
        }

        [Fact]
        public void DiffWithCenterSectionNotCrossingZero_InputIsAtUpperBound_ReturnsHalfSectionSize()
        {
            _pieSection.center = 180;
            float diff = _pieSection.diffWithCenter(_pieSection.upperBound);

            Assert.Equal(45, diff);
        }

        [Fact]
        public void DiffWithCenterSectionNotCrossingZero_InputIsAtLowerBound_ReturnsMinusHalfSectionSize()
        {
            _pieSection.center = 180;
            float diff = _pieSection.diffWithCenter(_pieSection.lowerBound);

            Assert.Equal(-45, diff);
        }

        [Fact]
        public void DiffWithCenterSectionCrossingZero_InputIsOutsideBoundsNearLowerBound_ReturnsMinusHalfSectionSizeAndSetsLowerBoundToValue()
        {
            _pieSection.center = 10;
            float value = _pieSection.lowerBound - 10;
            float diff = _pieSection.diffWithCenter(value);

            Assert.Equal(-45, diff);
            Assert.Equal(value, _pieSection.lowerBound);
            Assert.NotEqual(value, _pieSection.upperBound);
        }

        [Fact]
        public void DiffWithCenterSectionCrossingZero_InputIsOutsideBoundsNearUpperBound_ReturnsHalfSectionSizeAndSetsUpperBoundToValue()
        {
            _pieSection.center = 10;
            float value = _pieSection.upperBound + 10;
            float diff = _pieSection.diffWithCenter(value);

            Assert.Equal(45, diff);
            Assert.Equal(value, _pieSection.upperBound);
            Assert.NotEqual(value, _pieSection.lowerBound);
        }

        [Fact]
        public void DiffWithCenterSectionNotCrossingZero_InputIsOutsideBoundsNearLowerBound_ReturnsMinusHalfSectionSizeAndSetsLowerBoundToValue()
        {
            _pieSection.upperBound = 350;
            float value = _pieSection.lowerBound - 10;
            float diff = _pieSection.diffWithCenter(value);

            Assert.Equal(-45, diff);
            Assert.Equal(value, _pieSection.lowerBound);
            Assert.NotEqual(value, _pieSection.upperBound);
        }

        [Fact]
        public void DiffWithCenterSectionNotCrossingZero_InputIsOutsideBoundsNearUpperBound_ReturnsHalfSectionSizeAndSetsUpperBoundToValue()
        {
            _pieSection.lowerBound = 10;
            float value = _pieSection.upperBound + 10;
            float diff = _pieSection.diffWithCenter(value);

            Assert.Equal(45, diff);
            Assert.Equal(value, _pieSection.upperBound);
            Assert.NotEqual(value, _pieSection.lowerBound);
        }

        [Fact]
        public void DiffWithCenterSectionNotCrossingZero_InputIsOutsideBoundsNearUpperBoundCrossingZero_ReturnsHalfSectionSizeAndSetsUpperBoundToValue()
        {
            _pieSection.upperBound = 350;
            float diff = _pieSection.diffWithCenter(10);

            Assert.Equal(45, diff);
            Assert.Equal(10, _pieSection.upperBound);
            Assert.NotEqual(10, _pieSection.lowerBound);
        }

        [Fact]
        public void DiffWithCenterSectionNotCrossingZero_InputIsOutsideBoundsNearLowerBoundCrossingZero_ReturnsMinusHalfSectionSizeAndSetsLowerBoundToValue()
        {
            _pieSection.lowerBound = 10;
            float diff = _pieSection.diffWithCenter(350);

            Assert.Equal(-45, diff);
            Assert.Equal(350, _pieSection.lowerBound);
            Assert.NotEqual(350, _pieSection.upperBound);
        }

        [Fact]
        public void Constructor_InputPieSizeIsEqualToSectionSize_ThrowsException()
        {
            Assert.Throws<Exception>(() => new PieSection(5, 5));
        }

        [Fact]
        public void Constructor_InputPieSizeSmallerToSectionSize_ThrowsException()
        {
            Assert.Throws<Exception>(() => new PieSection(10, 5));
        }
    }
}