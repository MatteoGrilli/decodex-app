using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grim.Zones.Coordinates
{
    public class CubeCoordinateSpace
    {
        public static Vector3Int Up => new Vector3Int(0, -1, 1);
        public static Vector3Int Down => new Vector3Int(0, 1, -1);
        public static Vector3Int UpRight => new Vector3Int(1, -1, 0);
        public static Vector3Int DownRight => new Vector3Int(1, 0, -1);
        public static Vector3Int UpLeft => new Vector3Int(-1, 0, 1);
        public static Vector3Int DownLeft => new Vector3Int(-1, 1, 0);

        /// <summary>
        /// Returns true if the coordinate is a valid coordinate.
        /// </summary>
        /// <returns></returns>
        public static bool IsValid(CubeCoordinate coord) => coord.Components.x + coord.Components.y + coord.Components.z == 0;

        /// <summary>
        /// Returns a line of coordinates starting from this coordinate in the
        /// chosen direction.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        /// <param name="length"></param>
        /// <param name="includeOrigin"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static List<CubeCoordinate> GetSegment(CubeCoordinate origin, Vector3Int direction, int length, bool includeOrigin)
        {
            if (direction.magnitude == 0)
            {
                throw new ArgumentException($"[CubeCoordinateSpace.GetSegment] Direction ${direction} is invalid.");
            }

            List<CubeCoordinate> selection = new();

            if (includeOrigin)
            {
                selection.Add(origin);
            }

            for (int i = 1; i <= length; i++)
            {
                selection.Add(new CubeCoordinate(origin.Components + direction * i));
            }

            return selection;
        }

        /// <summary>
        /// Returns a range of coordinates around this coordinate.
        /// Result is going to be a "bubble" in the coordinate's space
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="radius"></param>
        /// <param name="includeOrigin"></param>
        /// <returns></returns>
        public static List<CubeCoordinate> GetBall(CubeCoordinate origin, int radius, bool includeOrigin)
        {
            List<CubeCoordinate> selection = new();

            for (int i = -radius; i <= radius; i++)
            {
                for (int j = -radius; j <= radius; j++)
                {
                    CubeCoordinate coord = new CubeCoordinate(i, j, -(i + j));
                    if (ManhattanDistance(origin, coord) <= 2 * radius && (includeOrigin || !coord.Equals(origin)))
                    {
                        selection.Add(coord);
                    }
                }
            }

            return selection;
        }

        /// <summary>
        /// Returns Manhattan distance between the this coordinate
        /// and the specified coordinates (Aka: Edit Distance).
        /// </summary>
        /// <param name="coord1"></param>
        /// <param name="coord2"></param>
        /// <returns></returns>
        public static int ManhattanDistance(CubeCoordinate coord1, CubeCoordinate coord2)
        {
            var diff = coord2.Components - coord1.Components;
            return Math.Abs(diff.x) + Math.Abs(diff.y) + Math.Abs(diff.z);
        }
    }
}
