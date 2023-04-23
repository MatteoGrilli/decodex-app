using Grim.Zones.Items;

namespace Decodex.Cards
{
    public class CardInstance : IItem, ICard
    {
        public string Id { get; private set; }

        public string SetId { get; private set; }
        public string ZoneId { get; set; }

        public CardInstance(string id, string setId)
        {
            Id = id;
            SetId = setId;
        }

        public bool Equals(IItem other)
        {
            return other is CardInstance && Id == other.Id;
        }
    }
}