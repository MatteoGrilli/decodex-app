using Grim.Zones.Coordinates;
using Grim.Zones.Items;

namespace Grim.Zones
{
    public class CoordinateItem<CoordinateType, ItemType>
        where CoordinateType: ICoordinate
        where ItemType: IItem
    {
        public CoordinateType Coordinate { get; private set; }
        public ItemType Item { get; private set; }

        public CoordinateItem(CoordinateType coordinate, ItemType item)
        {
            Coordinate = coordinate;
            Item = item;
        }
    }
}
