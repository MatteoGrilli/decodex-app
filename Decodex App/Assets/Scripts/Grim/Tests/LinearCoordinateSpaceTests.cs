using Grim.Zones.Coordinates;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Grim.Tests
{
    public class LinearCoordinateSpaceTests
    {
        [Test]
        public void IsValid()
        {
            Assert.DoesNotThrow(() => new LinearCoordinate(3));
            Assert.DoesNotThrow(() => new LinearCoordinate(-2));
        }

        [Test]
        public void ManhattanDistance()
        {
            LinearCoordinate c1 = new LinearCoordinate(1);
            LinearCoordinate c2 = new LinearCoordinate(4);
            Assert.AreEqual(0, LinearCoordinateSpace.ManhattanDistance(c1, c1));
            Assert.AreEqual(3, LinearCoordinateSpace.ManhattanDistance(c1, c2));
        }

        [Test]
        public void GetBallZero()
        {
            LinearCoordinate c = new LinearCoordinate(0);
            List<LinearCoordinate> self = LinearCoordinateSpace.GetBall(c, 0, true);
            Assert.True(self.Exists(coord => coord.Equals(new LinearCoordinate(0))));
            Assert.AreEqual(1, self.Count);
            List<LinearCoordinate> none = LinearCoordinateSpace.GetBall(c, 0, false);
            Assert.AreEqual(0, none.Count);
        }

        [Test]
        public void GetBallOne()
        {
            LinearCoordinate c = new LinearCoordinate(0);
            List<LinearCoordinate> adjacentExcludeOrigin = LinearCoordinateSpace.GetBall(c, 1, false);
            Assert.False(adjacentExcludeOrigin.Exists(coord => coord.Equals(new LinearCoordinate(0))));
            Assert.True(adjacentExcludeOrigin.Exists(coord => coord.Equals(new LinearCoordinate(1))));
            Assert.True(adjacentExcludeOrigin.Exists(coord => coord.Equals(new LinearCoordinate(-1))));
            Assert.AreEqual(2, adjacentExcludeOrigin.Count);

            List<LinearCoordinate> adjacentIncludeOrigin = LinearCoordinateSpace.GetBall(c, 1, true);
            Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new LinearCoordinate(0))));
            Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new LinearCoordinate(1))));
            Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new LinearCoordinate(-1))));
            Assert.AreEqual(3, adjacentIncludeOrigin.Count);
        }

        [Test]
        public void GetBallTwo()
        {
            LinearCoordinate c = new LinearCoordinate(0);
            List<LinearCoordinate> adjacentExcludeOrigin = LinearCoordinateSpace.GetBall(c, 2, false);
            Assert.False(adjacentExcludeOrigin.Exists(coord => coord.Equals(new LinearCoordinate(0))));
            Assert.True(adjacentExcludeOrigin.Exists(coord => coord.Equals(new LinearCoordinate(1))));
            Assert.True(adjacentExcludeOrigin.Exists(coord => coord.Equals(new LinearCoordinate(-1))));
            Assert.True(adjacentExcludeOrigin.Exists(coord => coord.Equals(new LinearCoordinate(2))));
            Assert.True(adjacentExcludeOrigin.Exists(coord => coord.Equals(new LinearCoordinate(-2))));
            Assert.AreEqual(4, adjacentExcludeOrigin.Count);

            List<LinearCoordinate> adjacentIncludeOrigin = LinearCoordinateSpace.GetBall(c, 2, true);
            Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new LinearCoordinate(0))));
            Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new LinearCoordinate(1))));
            Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new LinearCoordinate(-1))));
            Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new LinearCoordinate(2))));
            Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new LinearCoordinate(-2))));
            Assert.AreEqual(5, adjacentIncludeOrigin.Count);
        }

        [Test]
        public void GetSegmentZero()
        {
            LinearCoordinate c = new LinearCoordinate(0);
            int dir = LinearCoordinateSpace.Right;
            List<LinearCoordinate> segmentExcludeOrigin = LinearCoordinateSpace.GetSegment(c, dir, 0, false);
            Assert.AreEqual(0, segmentExcludeOrigin.Count);

            List<LinearCoordinate> segmentIncludeOrigin = LinearCoordinateSpace.GetSegment(c, dir, 0, true);
            Assert.True(segmentIncludeOrigin.Exists(coord => coord.Equals(new LinearCoordinate(0))));
            Assert.AreEqual(1, segmentIncludeOrigin.Count);
        }

        [Test]
        public void GetSegmentOne()
        {
            LinearCoordinate c = new LinearCoordinate(0);
            int dir = LinearCoordinateSpace.Right;
            List<LinearCoordinate> segmentExcludeOrigin = LinearCoordinateSpace.GetSegment(c, dir, 1, false);
            Assert.False(segmentExcludeOrigin.Exists(coord => coord.Equals(new LinearCoordinate(0))));
            Assert.True(segmentExcludeOrigin.Exists(coord => coord.Equals(new LinearCoordinate(1))));
            Assert.AreEqual(1, segmentExcludeOrigin.Count);

            List<LinearCoordinate> segmentIncludeOrigin = LinearCoordinateSpace.GetSegment(c, dir, 1, true);
            Assert.True(segmentIncludeOrigin.Exists(coord => coord.Equals(new LinearCoordinate(0))));
            Assert.True(segmentIncludeOrigin.Exists(coord => coord.Equals(new LinearCoordinate(1))));
            Assert.AreEqual(2, segmentIncludeOrigin.Count);

            LinearCoordinate c2 = new LinearCoordinate(-2);
            List<LinearCoordinate> segmentDifferentOrigin = LinearCoordinateSpace.GetSegment(c2, dir, 1, false);
            Assert.True(segmentDifferentOrigin.Exists(coord => coord.Equals(new LinearCoordinate(-1))));
            Assert.AreEqual(1, segmentDifferentOrigin.Count);
        }

        [Test]
        public void GetSegmentTwo()
        {
            LinearCoordinate c = new LinearCoordinate(0);
            int dir = LinearCoordinateSpace.Right;
            List<LinearCoordinate> segmentExcludeOrigin = LinearCoordinateSpace.GetSegment(c, dir, 2, false);
            Assert.False(segmentExcludeOrigin.Exists(coord => coord.Equals(new LinearCoordinate(0))));
            Assert.True(segmentExcludeOrigin.Exists(coord => coord.Equals(new LinearCoordinate(1))));
            Assert.True(segmentExcludeOrigin.Exists(coord => coord.Equals(new LinearCoordinate(2))));
            Assert.AreEqual(2, segmentExcludeOrigin.Count);

            List<LinearCoordinate> segmentIncludeOrigin = LinearCoordinateSpace.GetSegment(c, dir, 2, true);
            Assert.True(segmentIncludeOrigin.Exists(coord => coord.Equals(new LinearCoordinate(0))));
            Assert.True(segmentIncludeOrigin.Exists(coord => coord.Equals(new LinearCoordinate(1))));
            Assert.True(segmentIncludeOrigin.Exists(coord => coord.Equals(new LinearCoordinate(2))));
            Assert.AreEqual(3, segmentIncludeOrigin.Count);
        }
    }
}
