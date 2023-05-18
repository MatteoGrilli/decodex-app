using Grim.Zones.Coordinates;
using NUnit.Framework;
using UnityEngine;

namespace Grim.Tests
{
    public class LinearCoordinateTests
    {
        [Test]
        public void Constructor()
        {
            LinearCoordinate c = new LinearCoordinate(12345);
            Assert.AreEqual(12345, c.Value);
        }

        [Test]
        public void EqualsLinear()
        {
            LinearCoordinate c1 = new LinearCoordinate(1);
            LinearCoordinate c2 = new LinearCoordinate(3);
            LinearCoordinate c3 = new LinearCoordinate(3);
            Assert.False(c1.Equals(c2));
            Assert.False(c1.Equals(c3));
            Assert.True(c2.Equals(c3));
        }

        [Test]
        public void Equals()
        {
            ICoordinate c1 = new LinearCoordinate(1);
            ICoordinate c2 = new LinearCoordinate(3);
            ICoordinate c3 = new LinearCoordinate(3);
            Assert.False(c1.Equals(c2));
            Assert.False(c1.Equals(c3));
            Assert.True(c2.Equals(c3));
        }

        [Test]
        public void ToCartesian()
        {
            ICoordinate c1 = new LinearCoordinate(1);
            Assert.AreEqual(Vector3.right, c1.ToCartesian());
        }
    }
}