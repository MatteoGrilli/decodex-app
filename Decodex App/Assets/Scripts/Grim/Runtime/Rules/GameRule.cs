using Grim.Events;
using Grim.Rules;
using System;
using System.Collections.Generic;

namespace Grim
{
    public class GameRule<T>
    {
        private List<GameTrigger> _triggers;
        private GameAction<T> _action;

        public string Id { get; private set; }
        public bool Enabled { get; set; }

        public GameRule(string id, List<GameTrigger> triggers, GameAction<T> action)
        {
            // TODO: null checks

            _triggers = triggers;
            _action = action;
            Id = id;
            Enabled = true;
        }

        public void Register()
        {
            Action<GameEventArgs<T>> action = payload => Execute(ref payload);
            _triggers.ForEach(trigger =>
            {
                GameEvents.Current.Subscribe(trigger.Context, trigger.Id, action);
            });
        }

        // TODO: add unregister

        protected virtual void Execute(ref GameEventArgs<T> args) => _action.Execute(ref args);
    }
}
