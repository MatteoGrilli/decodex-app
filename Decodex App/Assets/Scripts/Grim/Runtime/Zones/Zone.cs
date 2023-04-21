using Decodex.Utils;
using Grim.Zones.Coordinates;
using Grim.Zones.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Grim.Zones
{
    public class Zone<Coordinate, Item>
        where Item : IItem
        where Coordinate : ICoordinate
    {
        public string Id { get; private set; }
        private Dictionary<Coordinate, Item> _items;
        public int NumSlots => _items.Count;

        public Zone(string id, List<Coordinate> layout)
        {
            Id = id;
            _items = new();
            layout.ForEach(coord => _items[coord] = default(Item));
            EventsEnabled = true;
        }

        /* ---------- EVENT HANDLING ---------- */

        public event Action<ZoneEventArgs<Coordinate, Item>> ItemsPut;
        public event Action<ZoneEventArgs<Coordinate, Item>> ItemsRemoved;
        public event Action ItemsShuffled;

        protected bool EventsEnabled { get; set; }

        protected void OnItemsPut(ZoneEventArgs<Coordinate, Item> e) => ItemsPut?.Invoke(e);
        protected void OnItemsRemoved(ZoneEventArgs<Coordinate, Item> e) => ItemsRemoved?.Invoke(e);
        protected void OnItemsShuffled() => ItemsShuffled?.Invoke();

        /* ---------- UTILITY ----------*/
        public Coordinate GetCoordinateForItem(Item item) => _items.FirstOrDefault(i => item.Equals(i)).Key;
        public int ItemsCount() => _items.Keys.Where(coord => !IsCoordinateEmpty(coord)).Count();
        public bool IsFull() => ItemsCount() == NumSlots;
        private bool IsCoordinateInLayout(Coordinate coord) => _items.ContainsKey(coord);
        public bool IsCoordinateEmpty(Coordinate coord) => _items[coord] == null;
        private Coordinate FirstEmptyCoord() => _items.Keys.First(coord => IsCoordinateEmpty(coord));
        private List<Coordinate> NonEmptyCoords() => _items.Keys.Where(coord => !IsCoordinateEmpty(coord)).ToList();

        /* ---------- METHODS ---------- */

        /// <summary>
        /// Puts the item in the specified coordinate.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="coord"></param>
        /// <returns>Returns true if the item was inserted, false otherwise.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public bool Put(Coordinate coord, Item item)
        {
            // Check arguments
            if (item == null)
            {
                throw new ArgumentNullException($"[{Id}.Put] null or empty item");
            }
            if (!IsCoordinateInLayout(coord))
            {
                throw new ArgumentException($"[{Id}.Put] Coordinate {coord} not included in zone layout");
            }

            // Check if coord is occupied
            if (IsCoordinateEmpty(coord))
            {
                // Insert new item
                _items[coord] = item;

                // Trigger items put event
                if (EventsEnabled)
                    OnItemsPut(new ZoneEventArgs<Coordinate, Item>(coord, item));

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Puts the item in the next open coordinate,
        /// as defined in the order of the layout during
        /// initialization.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Returns true if the item was inserted, false otherwise</returns>
        public bool Put(Item item) => !IsFull() ? this.Put(FirstEmptyCoord(), item) : false;

        
        /// <summary>
        /// Removes the item from the specified coordinate.
        /// </summary>
        /// <param name="coord"></param>
        /// <returns>Returns true if the item was successfully removed, false otherwise.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public bool Remove(Coordinate coord)
        {
            // Check arguments
            if (coord == null)
            {
                throw new ArgumentNullException($"[{Id}.Remove] null or empty coord");
            }
            if (!IsCoordinateInLayout(coord))
            {
                throw new ArgumentException($"[{Id}.Remove] Invalid coordinate " + coord);
            }

            if (!IsCoordinateEmpty(coord))
            {
                // Remove item
                Item itemToRemove = _items[coord];
                _items[coord] = default(Item);

                // Trigger items removed event
                if (EventsEnabled)
                    OnItemsRemoved(new ZoneEventArgs<Coordinate, Item>(coord, itemToRemove));

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Removes all items from the zone.
        /// </summary>
        public void RemoveAll() => NonEmptyCoords().ForEach(coord => Remove(coord));

        /// <summary>
        /// Gets the item associated with the specified coord.
        /// Will not remove the item from the zone.
        /// </summary>
        /// <param name="coord"></param>
        /// <returns>Returns the item if present, null otherwise</returns>
        public Item Get(Coordinate coord)
        {
            // Check arguments
            if (coord.Equals(default(Coordinate)))
            {
                throw new ArgumentNullException($"[{Id}.Get] null or empty coord");
            }
            if (!IsCoordinateInLayout(coord))
            {
                throw new ArgumentException($"[{Id}.Get] Invalid coordinate " + coord);
            }

            return _items[coord];
        }

        /// <summary>
        /// Gets all items in the zone.
        /// Will not remove the item from the zone.
        /// </summary>
        /// <returns>Returns all non null items in the zone</returns>
        public List<Item> GetAll() => NonEmptyCoords().Select(coord => _items[coord]).ToList();

        /// <summary>
        /// 
        /// </summary>
        public void Shuffle()
        {
            var keys = _items.Keys.ToArray();
            var values = _items.Values.ToArray();
            ArrayUtils.Shuffle(values);
            for (int i = 0; i < _items.Count(); i++)
            {
                _items[keys[i]] = values[i];
            }

            // Trigger items put event
            if (EventsEnabled)
                OnItemsShuffled();
        }
    }
}
