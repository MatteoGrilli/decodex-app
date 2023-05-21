using Decodex.Cards;
using Grim.Zones;
using Grim.Zones.Coordinates;
namespace Decodex
{
    // Just a box for now
    public class DeckController : ZoneController<LinearCoordinate, CardInstance>
    {
        protected override void OnItemsPut(ZoneEventArgs<LinearCoordinate, CardInstance> e)
        {

        }

        protected override void OnItemsRemoved(ZoneEventArgs<LinearCoordinate, CardInstance> e)
        {

        }

        protected override void OnItemsShuffled()
        {

        }
    }
}
