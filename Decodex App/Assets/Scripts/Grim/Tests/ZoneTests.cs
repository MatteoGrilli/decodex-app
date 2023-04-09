using Grim.Zones;
using Grim.Zones.Coordinates;
using NUnit.Framework;
using NSubstitute;
using Grim.Zones.Items;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEditor.Progress;

public class ZoneTests
{
    private Zone<LinearCoordinate, IItem> CreateLinear()
    {
        List<LinearCoordinate> layout = LinearCoordinateSpace.GetSegment(new LinearCoordinate(0), LinearCoordinateSpace.Right, 9, true);
        return new("hand", layout);
    }

    private Zone<CubeCoordinate, IItem> CreateCube()
    {
        List<CubeCoordinate> layout = CubeCoordinateSpace.GetBall(new CubeCoordinate(0, 0, 0), 2, true);
        return new("board", layout);
    }

    /* -------------------- LINEAR COORDINATE ZONE -------------------- */

    [Test]
    public void CreateLinearCoorinateZone()
    {
        var zone = CreateLinear();
        Assert.AreEqual(10, zone.NumSlots);
        Assert.AreEqual(0, zone.ItemsCount());
        Assert.False(zone.IsFull());
        Assert.AreEqual("hand", zone.Id);
    }
    
    [Test]
    public void PutLinearCoordinateZone()
    {
        var zone = CreateLinear();
        var item = Substitute.For<IItem>();
        zone.Put(new LinearCoordinate(0), item);
        Assert.AreEqual(item, zone.Get(new LinearCoordinate(0)));
        Assert.AreEqual(1, zone.ItemsCount());
        var item2 = Substitute.For<IItem>();
        Assert.False(zone.Put(new LinearCoordinate(0), item2));
        Assert.AreEqual(item, zone.Get(new LinearCoordinate(0)));
        Assert.AreNotEqual(item2, zone.Get(new LinearCoordinate(0)));
        // Try put without specific coord:
        zone.Put(item2);
        Assert.AreEqual(item2, zone.Get(new LinearCoordinate(1)));
        Assert.AreEqual(2, zone.ItemsCount());
    }

    [Test]
    public void PutLinearCoordianteZoneException()
    {
        var zone = CreateLinear();
        var item = Substitute.For<IItem>();
        Assert.Throws<ArgumentNullException>(() => zone.Put(new LinearCoordinate(0), null));
        Assert.Throws<ArgumentException>(() => zone.Put(new LinearCoordinate(-1), item)); 
    }

    [Test]
    public void RemoveLinearCoordinateZone()
    {
        var zone = CreateLinear();
        var item = Substitute.For<IItem>();
        zone.Put(new LinearCoordinate(0), item);
        zone.Put(new LinearCoordinate(1), item);
        zone.Put(new LinearCoordinate(2), item);
        zone.Put(new LinearCoordinate(3), item);
        Assert.AreEqual(4, zone.ItemsCount());
        zone.Remove(new LinearCoordinate(2));
        Assert.AreEqual(null, zone.Get(new LinearCoordinate(2)));
        Assert.AreEqual(3, zone.ItemsCount());
        Assert.False(zone.Remove(new LinearCoordinate(4)));
    }

    [Test]
    public void RemoveLinearCoordinateZoneException()
    {
        var zone = CreateLinear();
        Assert.Throws<ArgumentNullException>(() => zone.Remove(null));
        Assert.Throws<ArgumentException>(() => zone.Remove(new LinearCoordinate(-1)));
    }

    [Test]
    public void RemoveAllLinearCoordinateZone()
    {
        var zone = CreateLinear();
        var item = Substitute.For<IItem>();
        zone.Put(new LinearCoordinate(0), item);
        zone.Put(new LinearCoordinate(1), item);
        zone.Put(new LinearCoordinate(2), item);
        zone.Put(new LinearCoordinate(3), item);
        Assert.AreEqual(4, zone.ItemsCount());
        zone.RemoveAll();
        Assert.AreEqual(0, zone.ItemsCount());
    }

    [Test]
    public void GetAllLinearCoordinateZone()
    {
        var zone = CreateLinear();
        var item1 = Substitute.For<IItem>();
        var item2 = Substitute.For<IItem>();
        var item3 = Substitute.For<IItem>();
        var item4 = Substitute.For<IItem>();
        zone.Put(new LinearCoordinate(0), item1);
        zone.Put(new LinearCoordinate(1), item2);
        zone.Put(new LinearCoordinate(2), item3);
        zone.Put(new LinearCoordinate(3), item4);
        var all = zone.GetAll();
        Assert.AreEqual(4, all.Count);
        Assert.True(all.Exists(i => item1.Equals(i)));
        Assert.True(all.Exists(i => item2.Equals(i)));
        Assert.True(all.Exists(i => item3.Equals(i)));
        Assert.True(all.Exists(i => item4.Equals(i)));
    }

    [Test]
    public void ShuffleLinearCoordinateZone()
    {
        var zone = CreateLinear();
        var item1 = Substitute.For<IItem>();
        var item2 = Substitute.For<IItem>();
        var item3 = Substitute.For<IItem>();
        var item4 = Substitute.For<IItem>();
        zone.Put(new LinearCoordinate(0), item1);
        zone.Put(new LinearCoordinate(1), item2);
        zone.Put(new LinearCoordinate(2), item3);
        zone.Put(new LinearCoordinate(3), item4);

        UnityEngine.Random.InitState(0);
        zone.Shuffle();
        var all = zone.GetAll();

        Assert.AreEqual(item3, all[0]);
        Assert.AreEqual(item2, all[1]);
        Assert.AreEqual(item4, all[2]);
        Assert.AreEqual(item1, all[3]);
    }

    /* -------------------- CUBE COORDINATE ZONE -------------------- */

    [Test]
    public void CreateCubeCoordinateZone()
    {
        var zone = CreateCube();
        Assert.AreEqual(19, zone.NumSlots);
        Assert.AreEqual(0, zone.ItemsCount());
        Assert.AreEqual("board", zone.Id);
    }

    [Test]
    public void PutCubeCoordinateZone()
    {
        var zone = CreateCube();
        var item = Substitute.For<IItem>();
        zone.Put(new CubeCoordinate(0, 0, 0), item);
        Assert.AreEqual(item, zone.Get(new CubeCoordinate(0, 0, 0)));
        Assert.AreEqual(1, zone.ItemsCount());
        var item2 = Substitute.For<IItem>();
        Assert.False(zone.Put(new CubeCoordinate(0, 0, 0), item2));
        Assert.AreEqual(item, zone.Get(new CubeCoordinate(0, 0, 0)));
        Assert.AreNotEqual(item2, zone.Get(new CubeCoordinate(0, 0, 0)));
    }

    [Test]
    public void PutCubeCoordianteZoneException()
    {
        var zone = CreateCube();
        var item = Substitute.For<IItem>();
        Assert.Throws<ArgumentNullException>(() => zone.Put(new CubeCoordinate(0,0,0), null));
        Assert.Throws<ArgumentException>(() => zone.Put(new CubeCoordinate(5,5,-10), item));
    }

    [Test]
    public void RemoveCubeCoordinateZone()
    {
        var zone = CreateCube();
        var item = Substitute.For<IItem>();
        zone.Put(new CubeCoordinate(0,0,0), item);
        zone.Put(new CubeCoordinate(1,1,-2), item);
        zone.Put(new CubeCoordinate(1,-1,0), item);
        zone.Put(new CubeCoordinate(1,0,-1), item);
        Assert.AreEqual(4, zone.ItemsCount());
        zone.Remove(new CubeCoordinate(1,1,-2));
        Assert.AreEqual(null, zone.Get(new CubeCoordinate(1,1,-2)));
        Assert.AreEqual(3, zone.ItemsCount());
        Assert.False(zone.Remove(new CubeCoordinate(0,1,-1)));
    }

    [Test]
    public void RemoveCubeCoordinateZoneException()
    {
        var zone = CreateCube();
        Assert.Throws<ArgumentNullException>(() => zone.Remove(null));
        Assert.Throws<ArgumentException>(() => zone.Remove(new CubeCoordinate(3,3,-6)));
    }

    [Test]
    public void RemoveAllCubeCoordinateZone()
    {
        var zone = CreateCube();
        var item = Substitute.For<IItem>();
        zone.Put(new CubeCoordinate(0, 0, 0), item);
        zone.Put(new CubeCoordinate(1, 1, -2), item);
        zone.Put(new CubeCoordinate(1, -1, 0), item);
        zone.Put(new CubeCoordinate(1, 0, -1), item);
        Assert.AreEqual(4, zone.ItemsCount());
        zone.RemoveAll();
        Assert.AreEqual(0, zone.ItemsCount());
    }

    [Test]
    public void GetAllCubeCoordinateZone()
    {
        var zone = CreateCube();
        var item1 = Substitute.For<IItem>();
        var item2 = Substitute.For<IItem>();
        var item3 = Substitute.For<IItem>();
        var item4 = Substitute.For<IItem>();
        zone.Put(new CubeCoordinate(0, 0, 0), item1);
        zone.Put(new CubeCoordinate(1, 1, -2), item2);
        zone.Put(new CubeCoordinate(1, -1, 0), item3);
        zone.Put(new CubeCoordinate(1, 0, -1), item4);
        var all = zone.GetAll();
        Assert.AreEqual(4, all.Count);
        Assert.True(all.Exists(i => item1.Equals(i)));
        Assert.True(all.Exists(i => item2.Equals(i)));
        Assert.True(all.Exists(i => item3.Equals(i)));
        Assert.True(all.Exists(i => item4.Equals(i)));
    }

    [Test]
    public void ShuffleCubeCoordinateZone()
    {
        var zone = CreateCube();
        var item1 = Substitute.For<IItem>();
        var item2 = Substitute.For<IItem>();
        var item3 = Substitute.For<IItem>();
        var item4 = Substitute.For<IItem>();
        zone.Put(new CubeCoordinate(0, 0, 0), item1);
        zone.Put(new CubeCoordinate(1, 1, -2), item2);
        zone.Put(new CubeCoordinate(1, -1, 0), item3);
        zone.Put(new CubeCoordinate(1, 0, -1), item4);

        UnityEngine.Random.InitState(0);
        zone.Shuffle();
        var all = zone.GetAll();

        Assert.AreEqual(item4, all[0]);
        Assert.AreEqual(item3, all[1]);
        Assert.AreEqual(item1, all[2]);
        Assert.AreEqual(item2, all[3]);
    }
}