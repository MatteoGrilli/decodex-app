using Grim.Utils;
using System.Collections.Generic;

namespace Grim.Rules
{
    public class RuleExecutor
    {
        private Node<string, Rule> _root;
        private Dictionary<string, int> _executionCount;
        public Dictionary<string, int> ExecutionCount { get { return new(_executionCount); } }

        public RuleExecutor(Node<string, Rule> root) : this(root, new()) { }

        public RuleExecutor(Node<string, Rule> root, Dictionary<string, int> executionCount)
        {
            _root = root;
            _executionCount = executionCount;
        }

        public void Process(GameEventData data)
        {
            RecursiveExecute(_root, ref data);
        }

        private bool RecursiveExecute(Node<string, Rule> node, ref GameEventData data)
        {
            // Execute children in order
            var continueExecution = true;
            var enumerator = node.Children.Values.GetEnumerator();
            while (continueExecution && enumerator.MoveNext())
            {
                continueExecution = RecursiveExecute(enumerator.Current, ref data);
            }
            // Then execute this node
            Rule rule = node.Value;
            if (continueExecution && ShouldExecute(rule, data))
            {
                if (!_executionCount.ContainsKey(rule.Id))
                {
                    _executionCount[rule.Id] = 0;
                }
                _executionCount[rule.Id]++;
                continueExecution = node.Value.Action(data);
            }
            return continueExecution;
        }

        private bool ShouldExecute(Rule rule, GameEventData data) =>
            rule != null &&
            !MaxExecutionsReached(rule) &&
            rule.Condition(data);

        private bool MaxExecutionsReached(Rule rule) =>
            _executionCount.ContainsKey(rule.Id) &&
            rule.MaxExecutions != null &&
            _executionCount[rule.Id] >= rule.MaxExecutions;
    }
}
