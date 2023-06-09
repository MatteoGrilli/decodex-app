using Grim.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grim.Rules
{
    public class Rule
    {
        public string Id { get; private set; }
        private List<string> _path;
        public List<string> Path { get { return new(_path); } }
        public Func<GameEventData, bool> Condition;
        public Func<GameEventData, Task<bool>> Action;
        public int? MaxExecutions { get; private set; }

        public Rule(string id, List<string> path, Func<GameEventData, bool> condition, Func<GameEventData, Task<bool>> action, int? maxExecutions)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (path.Count == 0) throw new ArgumentException("Path must contain at least one step");
            if (action == null) throw new ArgumentNullException(nameof(action));
            Id = id;
            _path = path;
            Condition = condition ?? (args => true);
            Action = action;
            MaxExecutions = maxExecutions;
        }

        public static RuleBuilder New() => new RuleBuilder();
    }
}
