using Grim.Players;
using UnityEngine;

namespace Grim
{
    public class PlayerController : MonoBehaviour
    {
        public Player Model { get; private set; }

        public void Init(Player model)
        {
            Model = model;
        }
    }
}
