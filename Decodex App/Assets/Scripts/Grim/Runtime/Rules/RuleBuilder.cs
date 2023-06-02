using Grim.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grim.Rules
{
    public class RuleBuilder
    {
        private string _id;
        private List<string> _path;
        private Func<GameEventData, Task<bool>> _action;
        private Func<GameEventData, bool> _condition;
        private int? _maxExecutions;

        public RuleBuilder()
        {
            _path = new();
        }

        public RuleBuilder WithId(string id)
        {
            _id = id;
            return this;
        }

        public RuleBuilder WithPath(params string[] path)
        {
            _path = new(path);
            return this;
        }

        public RuleBuilder WithCondition(Func<GameEventData, bool> condition)
        {
            _condition = condition;
            return this;
        }

        public RuleBuilder WithAction(Func<GameEventData, Task<bool>> action)
        {
            _action = action;
            return this;
        }

        public RuleBuilder WithAction(Func<GameEventData, Task> action)
        {
            _action = async data => { await action.Invoke(data); return true; };
            return this;
        }

        public RuleBuilder WithMaxExecutions(int maxExecutions)
        {
            _maxExecutions = maxExecutions;
            return this;
        }

        public Rule Build() => new(_id, _path, _condition, _action, _maxExecutions);
    }
}
