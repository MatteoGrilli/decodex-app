using Grim.Zones;
using Grim.Zones.Coordinates;
using Grim.Zones.Items;
using UnityEngine;

namespace Grim
{
    public abstract class ZoneController<Coordinate, Item> : MonoBehaviour
        where Coordinate: ICoordinate
        where Item: IItem
    {
        public bool Initialized { get; private set; }
        protected Zone<Coordinate, Item> zone;

        public virtual void Init(Zone<Coordinate, Item> model)
        {
            zone = model;
            zone.ItemsPut += OnItemsPut;
            zone.ItemsRemoved += OnItemsRemoved;
            zone.ItemsShuffled += OnItemsShuffled;
            Initialized = true;
        }

        public virtual void Render()
        {
            if (!Initialized)
            {
                throw new System.Exception("ZoneController cannot render before the zone has been initialized!");
            }
        }

        protected abstract void OnItemsPut(ZoneEventArgs<Coordinate, Item> e);
        protected abstract void OnItemsRemoved(ZoneEventArgs<Coordinate, Item> e);
        protected abstract void OnItemsShuffled();
    }
}
