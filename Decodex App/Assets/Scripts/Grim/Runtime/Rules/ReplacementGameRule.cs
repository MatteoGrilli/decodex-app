using System.Collections.Generic;

namespace Grim.Rules
{
    public class ReplacementGameRule<T> : GameRule<T>
    {
        public ReplacementGameRule(string id, List<GameTrigger> triggers, GameAction<T> action) : base(id, triggers, action)
        {
        }

        protected override void Execute(ref GameEventArgs<T> args)
        {
            if (!args.HasAlreadyExecutedReplacementRule(Id))
            {
                args.RegisterReplacementRule(Id);
                base.Execute(ref args);
                args.Continue = false;
            }
        }
    }
}
