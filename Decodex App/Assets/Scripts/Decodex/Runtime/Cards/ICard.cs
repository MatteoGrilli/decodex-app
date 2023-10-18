using System;
using System.Collections.Generic;

namespace Decodex.Cards
{
    public interface ICard
    {
        public string SetId { get; }
        public List<PrimeAttribute> MemoryCells { get; }
        public List<PrimeAttribute> MemoryRequirement { get; }
    }
}