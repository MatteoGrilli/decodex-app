using Grim.Events;
using System;

namespace Grim.Rules
{
    public class GameAction<T> : IAction<T>
    {
        public string Context { get; set; }
        public string Id { get; private set; }
        private Action<T> _action;

        public GameAction(string context, string id, Action<T> action)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (action == null) throw new ArgumentNullException(nameof(action));

            Context = context;
            Id = id;
            _action = action;
        }

        public virtual void Execute(ref T args)
        {
            GameEventArgs<T> payload = new(ref args);

            GameEvents.Current.Trigger(Context, $"Pre_{Id}", payload);
            if (!payload.Continue) return;
            
            if (!payload.HasAlreadyExecuted(this))
            {
                payload.RegisterAction(Id);
                _action.Invoke(args);
            }

            GameEvents.Current.Trigger(Context, $"Post_{Id}", payload);
        }
    }
}
