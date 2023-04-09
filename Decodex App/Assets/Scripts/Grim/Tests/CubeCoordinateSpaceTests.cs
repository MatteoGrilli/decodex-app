using Grim.Zones.Coordinates;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CubeCoordinateSpaceTests
{
    [Test]
    public void IsValid()
    {
        Assert.DoesNotThrow(() => new CubeCoordinate(1, 1, -2));
        Assert.Throws<ArgumentException>(() => new CubeCoordinate(1,2,3));
    }

    [Test]
    public void ManhattanDistance()
    {
        CubeCoordinate c1 = new CubeCoordinate(1, 1, -2);
        CubeCoordinate c2 = new CubeCoordinate(2, 2, -4);
        Assert.AreEqual(0, CubeCoordinateSpace.ManhattanDistance(c1, c1));
        Assert.AreEqual(4, CubeCoordinateSpace.ManhattanDistance(c1, c2));
    }

    [Test]
    public void GetBallZero()
    {
        CubeCoordinate c = new CubeCoordinate(0, 0, 0);
        List<CubeCoordinate> self = CubeCoordinateSpace.GetBall(c, 0, true);
        Assert.True(self.Exists(coord => coord.Equals(new CubeCoordinate(0, 0, 0))));
        Assert.AreEqual(1, self.Count);
        List<CubeCoordinate> none = CubeCoordinateSpace.GetBall(c, 0, false);
        Assert.AreEqual(0, none.Count);
    }

    [Test]
    public void GetBallOne()
    {
        CubeCoordinate c = new CubeCoordinate(0, 0, 0);
        List<CubeCoordinate> adjacentExcludeOrigin = CubeCoordinateSpace.GetBall(c, 1, false);
        Assert.False(adjacentExcludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(0,0,0))));
        Assert.True(adjacentExcludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(1,-1,0))));
        Assert.True(adjacentExcludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(1,0,-1))));
        Assert.True(adjacentExcludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(0,1,-1))));
        Assert.True(adjacentExcludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(0,-1,1))));
        Assert.True(adjacentExcludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(-1,1,0))));
        Assert.True(adjacentExcludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(-1,0,1))));
        Assert.AreEqual(6, adjacentExcludeOrigin.Count);

        List<CubeCoordinate> adjacentIncludeOrigin = CubeCoordinateSpace.GetBall(c, 1, true);
        Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(0, 0, 0))));
        Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(1, -1, 0))));
        Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(1, 0, -1))));
        Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(0, 1, -1))));
        Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(0, -1, 1))));
        Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(-1, 1, 0))));
        Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(-1, 0, 1))));
        Assert.AreEqual(7, adjacentIncludeOrigin.Count);
    }

    [Test]
    public void GetBallTwo()
    {
        CubeCoordinate c = new CubeCoordinate(0, 0, 0);
        List<CubeCoordinate> adjacentExcludeOrigin = CubeCoordinateSpace.GetBall(c, 2, false);
        Assert.False(adjacentExcludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(0,0,0))));
        Assert.True(adjacentExcludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(1,-1,0))));
        Assert.True(adjacentExcludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(1,0,-1))));
        Assert.True(adjacentExcludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(0,1,-1))));
        Assert.True(adjacentExcludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(0,-1,1))));
        Assert.True(adjacentExcludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(-1,1,0))));
        Assert.True(adjacentExcludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(-1,0,1))));
        Assert.True(adjacentExcludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(-1, -1, 2))));
        Assert.True(adjacentExcludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(-1, 2, -1))));
        Assert.True(adjacentExcludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(2, -1, -1))));
        Assert.True(adjacentExcludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(0, 2, -2))));
        Assert.True(adjacentExcludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(0, -2, 2))));
        Assert.True(adjacentExcludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(2, 0, -2))));
        Assert.True(adjacentExcludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(2, -2, 0))));
        Assert.True(adjacentExcludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(-2, 2, 0))));
        Assert.True(adjacentExcludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(-2, 0, 2))));
        Assert.True(adjacentExcludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(1, 1, -2))));
        Assert.True(adjacentExcludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(1, -2, 1))));
        Assert.True(adjacentExcludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(-2, 1, 1))));
        Assert.AreEqual(18, adjacentExcludeOrigin.Count);

        List<CubeCoordinate> adjacentIncludeOrigin = CubeCoordinateSpace.GetBall(c, 2, true);
        Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(0, 0, 0))));
        Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(1, -1, 0))));
        Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(1, 0, -1))));
        Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(0, 1, -1))));
        Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(0, -1, 1))));
        Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(-1, 1, 0))));
        Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(-1, 0, 1))));
        Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(-1, -1, 2))));
        Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(-1, 2, -1))));
        Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(2, -1, -1))));
        Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(0, 2, -2))));
        Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(0, -2, 2))));
        Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(2, 0, -2))));
        Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(2, -2, 0))));
        Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(-2, 2, 0))));
        Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(-2, 0, 2))));
        Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(1, 1, -2))));
        Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(1, -2, 1))));
        Assert.True(adjacentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(-2, 1, 1))));
        Assert.AreEqual(19, adjacentIncludeOrigin.Count);
    }

    [Test]
    public void GetSegmentZero()
    {
        CubeCoordinate c = new CubeCoordinate(0, 0, 0);
        Vector3Int dir = CubeCoordinateSpace.Up;
        List<CubeCoordinate> segmentExcludeOrigin = CubeCoordinateSpace.GetSegment(c, dir, 0, false);
        Assert.AreEqual(0, segmentExcludeOrigin.Count);

        List<CubeCoordinate> segmentIncludeOrigin = CubeCoordinateSpace.GetSegment(c, dir, 0, true);
        Assert.True(segmentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(0, 0, 0))));
        Assert.AreEqual(1, segmentIncludeOrigin.Count);
    }

    [Test]
    public void GetSegmentOne()
    {
        CubeCoordinate c = new CubeCoordinate(0, 0, 0);
        Vector3Int dir = CubeCoordinateSpace.Up;
        List<CubeCoordinate> segmentExcludeOrigin = CubeCoordinateSpace.GetSegment(c, dir, 1, false);
        Assert.False(segmentExcludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(0, 0, 0))));
        Assert.True(segmentExcludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(0, -1, 1))));
        Assert.AreEqual(1, segmentExcludeOrigin.Count);

        List<CubeCoordinate> segmentIncludeOrigin = CubeCoordinateSpace.GetSegment(c, dir, 1, true);
        Assert.True(segmentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(0, 0, 0))));
        Assert.True(segmentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(0, -1, 1))));
        Assert.AreEqual(2, segmentIncludeOrigin.Count);

        CubeCoordinate c2 = new CubeCoordinate(1, 1, -2);
        List<CubeCoordinate> segmentDifferentOrigin = CubeCoordinateSpace.GetSegment(c2, dir, 1, false);
        Assert.True(segmentDifferentOrigin.Exists(coord => coord.Equals(new CubeCoordinate(1, 0, -1))));
        Assert.AreEqual(1, segmentDifferentOrigin.Count);
    }

    [Test]
    public void GetSegmentTwo()
    {
        CubeCoordinate c = new CubeCoordinate(0, 0, 0);
        Vector3Int dir = CubeCoordinateSpace.Up;
        List<CubeCoordinate> segmentExcludeOrigin = CubeCoordinateSpace.GetSegment(c, dir, 2, false);
        Assert.False(segmentExcludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(0, 0, 0))));
        Assert.True(segmentExcludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(0, -1, 1))));
        Assert.True(segmentExcludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(0, -2, 2))));
        Assert.AreEqual(2, segmentExcludeOrigin.Count);

        List<CubeCoordinate> segmentIncludeOrigin = CubeCoordinateSpace.GetSegment(c, dir, 2, true);
        Assert.True(segmentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(0, 0, 0))));
        Assert.True(segmentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(0, -1, 1))));
        Assert.True(segmentIncludeOrigin.Exists(coord => coord.Equals(new CubeCoordinate(0, -2, 2))));
        Assert.AreEqual(3, segmentIncludeOrigin.Count);
    }
}
