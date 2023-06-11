using System;
using System.Collections.Generic;

namespace Decodex.Cards
{
    public class Card : ICard
    {
        public string SetId { get; private set; }

        public List<PrimeAttribute> MemoryCells { get; }
        public List<PrimeAttribute> MemoryRequirement { get; }

        public Card(string setId, List<PrimeAttribute> memoryCells, List<PrimeAttribute> memoryRequirement)
        {
            if (memoryCells is null) { throw new ArgumentNullException(nameof(memoryCells)); }
            if (memoryRequirement is null) { throw new ArgumentNullException(nameof(memoryRequirement)); }
            SetId = setId;
            MemoryCells = memoryCells;
            MemoryRequirement = memoryRequirement;
        }
    }
}
