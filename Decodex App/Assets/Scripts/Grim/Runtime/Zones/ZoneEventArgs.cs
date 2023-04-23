using Grim.Zones.Coordinates;
using Grim.Zones.Items;
using System;
using System.Collections.Generic;

namespace Grim.Zones
{
    public class ZoneEventArgs<Coordinate, Item>
        where Coordinate: ICoordinate
        where Item: IItem
    {
        public List<CoordinateItem<Coordinate, Item>> _entries;
        public DateTime Timestamp { get; private set; }

        public ZoneEventArgs(Coordinate coord, Item item)
        {
            _entries = new();
            _entries.Add(new CoordinateItem<Coordinate, Item>(coord, item));
            Timestamp = DateTime.Now;
        }

        public ZoneEventArgs(List<CoordinateItem<Coordinate, Item>> list)
        {
            _entries = list;
            Timestamp = DateTime.Now;
        }
    }

}