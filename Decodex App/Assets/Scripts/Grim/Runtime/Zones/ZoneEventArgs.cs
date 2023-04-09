using Grim.Zones.Coordinates;
using Grim.Zones.Items;
using System;

namespace Grim.Zones
{
    public class ZoneEventArgs<Coordinate, Item>
        where Coordinate: ICoordinate
        where Item: IItem
    {
        public Coordinate _coordinate;
        public Item _item;
        public DateTime Timestamp { get; private set; }

        public ZoneEventArgs(Coordinate coord, Item item)
        {
            _coordinate = coord;
            _item = item;
            Timestamp = DateTime.Now;
        }
    }

}