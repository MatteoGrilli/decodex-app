using Grim.Zones.Coordinates;
using NUnit.Framework;
using UnityEngine;

namespace Grim.Tests
{
    public class CubeCoordinatesTests
    {
        [Test]
        public void Constructor()
        {
            CubeCoordinate c = new CubeCoordinate(1, 1, -2);
            Assert.AreEqual(new Vector3Int(1, 1, -2), c.Components);
        }

        [Test]
        public void EqualsCube()
        {
            CubeCoordinate c1 = new CubeCoordinate(1, 1, -2);
            CubeCoordinate c2 = new CubeCoordinate(3, 3, -6);
            CubeCoordinate c3 = new CubeCoordinate(3, 3, -6);
            Assert.False(c1.Equals(c2));
            Assert.False(c1.Equals(c3));
            Assert.True(c2.Equals(c3));
        }

        [Test]
        public void Equals()
        {
            ICoordinate c1 = new CubeCoordinate(1, 1, -2);
            ICoordinate c2 = new CubeCoordinate(3, 3, -6);
            ICoordinate c3 = new CubeCoordinate(3, 3, -6);
            Assert.False(c1.Equals(c2));
            Assert.False(c1.Equals(c3));
            Assert.True(c2.Equals(c3));
        }

        [Test]
        public void ToCartesian()
        {
            ICoordinate c1 = new CubeCoordinate(1, 2, -3);
            Assert.AreEqual(new Vector3(0.5f * Mathf.Sqrt(3f), 0f, -2.5f), c1.ToCartesian());
        }
    }
}