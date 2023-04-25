using Grim.Zones.Coordinates;
using System.Collections.Generic;

namespace Grim.Zones
{
    /// <summary>
    /// A compact zone leaves no empty coords between
    /// coords that contain items, and populates its
    /// coordinates starting from the first, using the
    /// order defined in the layout during initialization.
    /// </summary>
    /// <typeparam name="Coordinate"></typeparam>
    /// <typeparam name="Item"></typeparam>
    public class CompactZone<Coordinate, Item> : Zone<Coordinate, Item>
        where Item : IItem
        where Coordinate : ICoordinate
    {
        public CompactZone(string id, string type, List<Coordinate> layout) : base(id, type, layout)
        {
            ItemPut += e => Compact();
            OneOrMoreItemsPut += e => Compact();
            ItemRemoved += e => Compact();
            OneOrMoreItemsRemoved += e => Compact();
            ItemsShuffled += () => Compact();
        }

        private void Compact()
        {
            EventsEnabled = false;
            var items = GetAll();
            RemoveAll();
            items.ForEach(item => Put(item));
            EventsEnabled = true;
        }
    }
}
