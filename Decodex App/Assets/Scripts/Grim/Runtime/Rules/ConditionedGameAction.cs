using System;

namespace Grim.Rules
{
    public class ConditionedGameAction<T> : GameAction<T>
    {
        private Condition<T> _condition;
        
        public ConditionedGameAction(string context, string id, Condition<T> condition, Action<T> action) : base(context, id, action)
        {
            if (condition == null) throw new ArgumentNullException(nameof(condition));
            _condition = condition;
        }

        public ConditionedGameAction(string context, string id, Func<T, bool> condition, Action<T> action) : base(context, id, action)
        {
            if (condition == null) throw new ArgumentNullException(nameof(condition));
            _condition = new Condition<T>($"{id}_condition", condition);
        }

        public override void Execute(ref T args)
        {
            if (_condition.Evaluate(args))
                base.Execute(ref args);
        }
    }
}
