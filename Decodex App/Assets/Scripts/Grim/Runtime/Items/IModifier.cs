namespace Grim.Items
{
    public interface IModifier<Object>
    {
        public string Id { get; }
        public string Layer { get; }
        public Object Apply(Object item);
    }
}
