namespace Grim.Rules
{
    public interface IAction<T>
    {
        public void Execute(ref T args);
    }
}
