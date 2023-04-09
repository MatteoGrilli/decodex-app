using System;
using UnityEngine;

namespace Grim.Zones.Coordinates
{
    public class LinearCoordinate : ICoordinate
    {
        public static int Right => 1;
        public static int Left => -1;

        public int Value { get; private set; }

        public LinearCoordinate(int x)
        {
            Value = x;
        }

        public Vector3 ToCartesian() => new Vector3(Value, 0, 0);

        public bool Equals(ICoordinate other)
        {
            if (other == null) return false;
            if (other is not LinearCoordinate)
            {
                throw new ArgumentException("Cannot compare coordinates of different types");
            }
            return (other as LinearCoordinate).Value.Equals(Value);
        }

        public override bool Equals(object obj)
        {
            if (obj is LinearCoordinate)
            {
                return Equals((LinearCoordinate)obj);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }
    }
}
