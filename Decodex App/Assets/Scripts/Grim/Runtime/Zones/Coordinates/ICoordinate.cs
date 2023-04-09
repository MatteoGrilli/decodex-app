using UnityEngine;

// So, I'm basically reinventing the n-dimensional point.
// I'm sure someone already solved this problem. Look it up on google.
namespace Grim.Zones.Coordinates
{
    public interface ICoordinate
    {
        /// <summary>
        /// Returns true if the two coordinates fully overlap.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ICoordinate other);

        /// <summary>
        /// Returns a cartesian representation of the coordinate.
        /// Remember that Unity uses the left-hand rule.
        /// </summary>
        /// <returns></returns>
        public Vector3 ToCartesian();
    }
}
