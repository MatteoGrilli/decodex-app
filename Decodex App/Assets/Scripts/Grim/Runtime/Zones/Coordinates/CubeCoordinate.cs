using System;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

namespace Grim.Zones.Coordinates
{
    /// <summary>
    /// A class for cube coordinates. See https://www.redblobgames.com/grids/hexagons/
    /// In this application:
    /// q == x : aligned on the x axis (to the right)
    /// r == y : 120° counterclockwise from q
    /// s == z : 120° counterclockwise from r
    /// </summary>
    public class CubeCoordinate : ICoordinate
    {
        public Vector3Int Components { get; private set; }

        public CubeCoordinate(int q, int r, int s)
        {
            Components = new Vector3Int(q, r, s);
            if (!CubeCoordinateSpace.IsValid(this))
            {
                throw new ArgumentException($"Coordinate ${Components} is not a valid cube coordinate.");
            }
        }

        public CubeCoordinate(Vector3Int qrs)
        {
            Components = qrs;
        }

        public Vector3 ToCartesian()
        {
            return new Vector3((float)Math.Sqrt(3) * 0.5f * Components.x, 0, 0.5f * (Components.z - Components.y));
        }

        public bool Equals(ICoordinate other)
        {
            if (other == null) return false;
            if (other is not CubeCoordinate)
            {
                throw new ArgumentException("Cannot compare coordinates of different types");
            }
            return (other as CubeCoordinate).Components.Equals(Components);
        }
        public override bool Equals(object obj)
        {
            if (obj is CubeCoordinate)
            {
                return Equals((CubeCoordinate)obj);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Components.x, Components.y, Components.z);
        }
    }    
}
