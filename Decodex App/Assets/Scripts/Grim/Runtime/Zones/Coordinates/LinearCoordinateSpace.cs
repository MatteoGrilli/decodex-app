using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grim.Zones.Coordinates
{
    public class LinearCoordinateSpace
    {
        public static int Right => 1;
        public static int Left => -1;

        /// <summary>
        /// Returns true if the coordinate is a valid coordinate.
        /// </summary>
        /// <returns></returns>
        public static bool IsValid(LinearCoordinate coord) => true;

        /// <summary>
        /// Returns a line of coordinates starting from this coordinate in the
        /// chosen direction.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        /// <param name="length"></param>
        /// <param name="includeOrigin"></param>
        /// <returns></returns>
        public static List<LinearCoordinate> GetSegment(LinearCoordinate origin, int direction, int length, bool includeOrigin)
        {
            List<LinearCoordinate> selection = new();

            if (includeOrigin)
            {
                selection.Add(origin);
            }

            for (int i = 1; i <= length; i++)
            {
                selection.Add(new LinearCoordinate(origin.Value + direction * i));
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
        public static List<LinearCoordinate> GetBall(LinearCoordinate origin, int radius, bool includeOrigin)
        {
            List<LinearCoordinate> selection = new();

            for (int i = -radius; i <= radius; i++)
            {
                LinearCoordinate coord = new LinearCoordinate(i);
                if (ManhattanDistance(origin, coord) <= radius && (includeOrigin || !coord.Equals(origin)))
                {
                    selection.Add(coord);
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
        public static int ManhattanDistance(LinearCoordinate coord1, LinearCoordinate coord2) => Mathf.Abs(coord2.Value - coord1.Value);
    }
}
