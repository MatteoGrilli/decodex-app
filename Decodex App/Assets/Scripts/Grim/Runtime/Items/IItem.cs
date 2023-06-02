namespace Grim.Zones
{
    public interface IItem
    {
        public string Id { get; }
        public IZone ParentZone { get; set; }
        public bool Equals(IItem other);
    }
}
