using Decodex.Utils;
using Grim.Zones.Coordinates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Grim.Zones
{
    public class Zone<Coordinate, Item> : IItem, IZone
        where Item : IItem
        where Coordinate : ICoordinate
    {
        public string Id { get; private set; }

        public string Type { get; private set; }

        public IZone ParentZone { get; set; }

        private Dictionary<Coordinate, Item> _items;

        public int NumSlots => _items.Count;

        public Zone(string id, string type, List<Coordinate> layout)
        {
            Id = id;
            Type = type;
            _items = new();
            layout.ForEach(coord => _items[coord] = default(Item));
            EventsEnabled = true;
        }

        /* ---------- EVENT HANDLING ---------- */

        public event Action<ZoneEventArgs<Coordinate, Item>> ItemPut;
        public event Action<ZoneEventArgs<Coordinate, Item>> OneOrMoreItemsPut;
        public event Action<ZoneEventArgs<Coordinate, Item>> ItemRemoved;
        public event Action<ZoneEventArgs<Coordinate, Item>> OneOrMoreItemsRemoved;
        public event Action ItemsShuffled;

        protected bool EventsEnabled { get; set; }

        protected void OnItemPut(ZoneEventArgs<Coordinate, Item> e)
        {
            ItemPut?.Invoke(e);
            //GameEvents.Current.Trigger<string>("abc", "abc");
        }
        protected void OnOneOrMoreItemsPut(ZoneEventArgs<Coordinate, Item> e) => OneOrMoreItemsPut?.Invoke(e);
        protected void OnItemRemoved(ZoneEventArgs<Coordinate, Item> e) => ItemRemoved?.Invoke(e);
        protected void OnOneOrMoreItemsRemoved(ZoneEventArgs<Coordinate, Item> e) => OneOrMoreItemsRemoved?.Invoke(e);
        protected void OnItemsShuffled() => ItemsShuffled?.Invoke();

        /* ---------- UTILITY ----------*/
        public Coordinate GetCoordinateForItem(Item item) => _items.FirstOrDefault(entry => entry.Value.Equals(item)).Key;
        public int ItemsCount() => _items.Keys.Where(coord => !IsCoordinateEmpty(coord)).Count();
        public bool IsFull() => ItemsCount() == NumSlots;
        private bool IsCoordinateInLayout(Coordinate coord) => _items.ContainsKey(coord);
        public bool IsCoordinateEmpty(Coordinate coord) => _items[coord] == null;
        private Coordinate FirstEmptyCoord() => _items.Keys.First(coord => IsCoordinateEmpty(coord));
        private List<Coordinate> NonEmptyCoords() => _items.Keys.Where(coord => !IsCoordinateEmpty(coord)).ToList();

        /* ---------- METHODS ---------- */

        private bool InnerPut(Coordinate coord, Item item)
        {
            // Check if coord is occupied
            if (IsCoordinateEmpty(coord))
            {
                // Insert new item
                _items[coord] = item;
                item.ParentZone = this;

                // Trigger item put event
                if (EventsEnabled)
                    OnItemPut(new ZoneEventArgs<Coordinate, Item>(coord, item));

                return true;
            }
            else
            {
                return false;
            }
        }

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
            if (InnerPut(coord, item))
            {
                // Trigger items put event
                if (EventsEnabled)
                    OnOneOrMoreItemsPut(new ZoneEventArgs<Coordinate, Item>(coord, item));

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
        public bool Put(Item item) => !IsFull() ? Put(FirstEmptyCoord(), item) : false;

        /// <summary>
        /// Puts the items in the specified coordinate for each.
        /// </summary>
        /// <param name="entries"></param>
        public void Put(List<CoordinateItem<Coordinate, Item>> entries)
        {
            List<CoordinateItem<Coordinate, Item>> inserted = new();
            entries.ForEach(entry =>
            {
                if (InnerPut(entry.Coordinate, entry.Item))
                    inserted.Add(entry);
            });

            if (EventsEnabled)
            {
                OnOneOrMoreItemsPut(new ZoneEventArgs<Coordinate, Item>(inserted));
            }
        }

        /// <summary>
        /// Puts the items each in the next open coordinate,
        /// as defined in the order of the layout during
        /// initialization.
        /// </summary>
        /// <param name="items"></param>
        public void Put(List<Item> items)
        {
            List<CoordinateItem<Coordinate, Item>> inserted = new();
            items.ForEach(item =>
            {
                Coordinate coord = FirstEmptyCoord();
                if (InnerPut(coord, item))
                {
                    inserted.Add(new CoordinateItem<Coordinate, Item>(coord, item));
                }
            });

            if (EventsEnabled)
            {
                OneOrMoreItemsPut(new ZoneEventArgs<Coordinate, Item>(inserted));
            }
        }

        private bool InnerRemove(Coordinate coord)
        {
            if (!IsCoordinateEmpty(coord))
            {
                // Remove item
                Item itemToRemove = _items[coord];
                _items[coord] = default(Item);
                itemToRemove.ParentZone = null;

                // Trigger item removed event
                if (EventsEnabled)
                    OnItemRemoved(new ZoneEventArgs<Coordinate, Item>(coord, itemToRemove));

                return true;
            }
            else
            {
                return false;
            }
        }

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

            if (InnerRemove(coord))
            {
                Item itemToRemove = Get(coord);
                // Trigger items removed event
                if (EventsEnabled)
                    OnOneOrMoreItemsRemoved(new ZoneEventArgs<Coordinate, Item>(coord, itemToRemove));

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
        public void RemoveAll()
        {
            List<CoordinateItem<Coordinate, Item>> removed = new();
            NonEmptyCoords().ForEach(coord =>
            {
                Item item = Get(coord);
                if (InnerRemove(coord)) removed.Add(new CoordinateItem<Coordinate, Item>(coord, item));
            });

            if (EventsEnabled)
            {
                OnOneOrMoreItemsRemoved(new ZoneEventArgs<Coordinate, Item>(removed));
            }
        }

        /// <summary>
        /// Removes the item from the specified coordinates.
        /// </summary>
        /// <param name="coords"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void Remove(List<Coordinate> coords)
        {
            // Check arguments
            if (coords == null)
            {
                throw new ArgumentNullException($"[{Id}.Remove] null or empty list of coords");
            }

            List<CoordinateItem<Coordinate, Item>> removed = new();
            coords.ForEach(coord =>
            {
                Item item = Get(coord);
                if (InnerRemove(coord)) removed.Add(new CoordinateItem<Coordinate, Item>(coord, item));
            });

            if (EventsEnabled)
            {
                OnOneOrMoreItemsRemoved(new ZoneEventArgs<Coordinate, Item>(removed));
            }
        }

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

        public override bool Equals(object other) => other is Zone<Coordinate, Item> && Equals((Zone<Coordinate,Item>)other);
        public override int GetHashCode() => HashCode.Combine(Id);
        public bool Equals(IItem other) => GetHashCode() == other.GetHashCode();
    }
}
