using System.Collections.Generic;

namespace Grim.Items // ?????
{
    public interface IModifiable<Type>
    {
        public List<IModifier<Type>> Modifiers { get; }
        public void AddModifier(IModifier<Type> modifier);
        public void RemoveModifier(IModifier<Type> modifier);
        public Type GetModified(bool force = false);
    }
}
