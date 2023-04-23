using Grim.Zones.Items;
using System;

namespace Decodex.Tiles
{
    public class TileInstance : IItem
    {
        public string Id { get; private set; }

        public string ZoneId { get; set; }

        public TileInstance(string id)
        {
            Id = id;
        }

        public bool Equals(IItem other) => other is TileInstance && Id == other.Id;

        public override bool Equals(object other) => other is TileInstance && Equals((TileInstance)other);

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, ZoneId);
        }

      
    }
}
