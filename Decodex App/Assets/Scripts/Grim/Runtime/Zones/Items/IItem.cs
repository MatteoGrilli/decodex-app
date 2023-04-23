namespace Grim.Zones.Items
{
    public interface IItem
    {
        public string Id { get; }
        public string ZoneId { get; set; }
    }
}
