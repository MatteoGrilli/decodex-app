using Grim.Items;
using Grim.Zones;
using UnityEngine;

namespace Grim
{
    public class ItemController<Item> : MonoBehaviour, IRenderable
        where Item : IItem
    {
        public bool Initialized { get; private set; }
        public Item Model { get; protected set; }

        public virtual void Init(Item model)
        {
            this.Model = model;
            Initialized = true;
        }
        public void Render()
        {
            if (!Initialized)
            {
                throw new System.Exception("ZoneController cannot render before the zone has been initialized!");
            }
        }
    }
}
