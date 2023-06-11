using Grim.Zones;
using System.Collections.Generic;

namespace Grim.Items
{
    public interface IItem
    {
        public string Id { get; }
        public IZone ParentZone { get; set; }
        public bool Equals(IItem other);
    }
}
