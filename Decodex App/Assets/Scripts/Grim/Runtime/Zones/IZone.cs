namespace Grim.Zones
{
    public interface IZone
    {
        public string Id { get; }
        public string Type { get; }
        public int NumSlots { get; }
    }
}
