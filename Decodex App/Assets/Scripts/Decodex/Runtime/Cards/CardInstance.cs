using Grim.Items;
using Grim.Zones;
using System.Collections.Generic;
using System.Linq;

namespace Decodex.Cards
{
    public class CardInstance : IItem, ICard, IModifiable<ICard>
    {
        // --------- FROM IITEM ---------
        public string Id { get; private set; }

        public IZone ParentZone { get; set; }

        // --------- FROM ICARD ---------
        public string SetId => GetModified().SetId;
        public List<PrimeAttribute> MemoryCells => GetModified().MemoryCells;
        public List<PrimeAttribute> MemoryRequirement => GetModified().MemoryRequirement;

        // --------- FROM IMODIFIABLE ---------
        public ICard Blueprint { get; private set; }
        private ICard _modifiedCard;
        private bool _shouldUpdateModifiedCard = false;

        private Dictionary<string, List<IModifier<ICard>>> _modifiers;
        public List<IModifier<ICard>> Modifiers { get { return _modifiers.Values.SelectMany(x => x).ToList(); } }

        public CardInstance(string id, ICard blueprint)
        {
            Id = id;
            Blueprint = blueprint;
            _modifiers = new();
            InitModifiers();
            GetModified(true);
        }

        private void InitModifiers()
        {
            // TODO: decide names for layers
            _modifiers = new();
            _modifiers.Add("LAYER_1", new());
            _modifiers.Add("LAYER_2", new());
            _modifiers.Add("LAYER_3", new());
            _modifiers.Add("LAYER_4", new());
            _modifiers.Add("LAYER_5", new());
            _modifiers.Add("LAYER_6", new());
            _modifiers.Add("LAYER_7", new());
        }

        public bool Equals(IItem other)
        {
            return other is CardInstance && Id == other.Id;
        }

        public void AddModifier(IModifier<ICard> modifier)
        {
            this._modifiers[modifier.Layer].Add(modifier);
            this._shouldUpdateModifiedCard = true;
        }

        public void RemoveModifier(IModifier<ICard> modifier)
        {
            this._modifiers[modifier.Layer].Remove(modifier);
            this._shouldUpdateModifiedCard = true;
        }

        public ICard GetModified(bool force = false)
        {
            if (force || this._shouldUpdateModifiedCard)
            {
                // Then actually do it
                var result = Blueprint;
                this._modifiers.Keys.ToList().ForEach(layer =>
                {
                    this._modifiers[layer].ForEach(modifier =>
                    {
                        result = modifier.Apply(result);
                    });
                });
                this._shouldUpdateModifiedCard = false;
                this._modifiedCard = result;
            }
            return _modifiedCard;
        }
    }
}