using Grim.Zones.Coordinates;
using UnityEngine;

namespace Grim.Zones
{
    public abstract class ZoneController<Coordinate, Item> : MonoBehaviour, IRenderable
        where Coordinate: ICoordinate
        where Item: IItem
    {
        public bool Initialized { get; private set; }
        public Zone<Coordinate, Item> Model { get; protected set; }

        public virtual void Init(Zone<Coordinate, Item> model)
        {
            this.Model = model;
            this.Model.ItemPut += OnItemsPut;
            this.Model.OneOrMoreItemsPut += OnItemsPut;
            this.Model.ItemRemoved += OnItemsRemoved;
            this.Model.OneOrMoreItemsRemoved += OnItemsRemoved;
            this.Model.ItemsShuffled += OnItemsShuffled;
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
