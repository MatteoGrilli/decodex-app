using System.Collections.Generic;

namespace Grim.Players
{
    public class Player
    {
        public string Id { get; private set; }
        private Dictionary<string, string> _zoneIds;
        public Dictionary<string, string> ZoneIds
        {
            get
            {
                return new(_zoneIds);
            }
        }

        public object Properties { get; private set; }
        public object DeckList;
        public object Avatar;

        public Player(string id, Dictionary<string, string> zoneIds, object properties, object deckList, object avatar)
        {
            Id = id;
            _zoneIds = zoneIds;
            Properties = properties;
            DeckList = deckList;
            Avatar = avatar;
        }
    }
}
