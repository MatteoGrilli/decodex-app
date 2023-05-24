using Grim.Utils;
using System;
using System.Collections.Generic;

namespace Grim.Rules
{
    public class RuleBuilder
    {
        private string _id;
        private List<string> _path;
        private Func<GameEventData, bool> _action;
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

        public RuleBuilder WithPath(string[] path)
        {
            _path = new(path);
            return this;
        }

        public RuleBuilder WithCondition(Func<GameEventData, bool> condition)
        {
            _condition = condition;
            return this;
        }

        public RuleBuilder WithAction(Func<GameEventData, bool> action)
        {
            _action = action;
            return this;
        }

        public RuleBuilder WithAction(Action<GameEventData> action)
        {
            _action = data => { action.Invoke(data); return true; };
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