namespace Grim.Rules
{
    public interface IAction<T>
    {
        public void Execute(ref GameEventArgs<T> args); // TODO: maybe change name to reinforce the idea you are continuing an execution chain
        public void Execute(ref T args); // TODO: change name to reinforce the idea you are starting a new execution chain
    }
}
