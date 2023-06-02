using Grim.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grim.Rules
{
    public class RuleEngine
    {
        private static RuleEngine _instance;
        public static RuleEngine Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RuleEngine();
                }
                return _instance;
            }
        }

        private Node<string, Rule> _rulesTree;

        /// <summary>
        /// Nodes in which rules can be inserted. Corresponds to the
        /// leaves of the tree after registering all the necessary paths
        /// with RegisterPath.
        /// </summary>
        private List<Node<string, Rule>> _ruleInsertionNodes;

        private Stack<RuleExecutor> _activeExecutors;

        private RuleEngine()
        {
            Reset();
        }

        public void Reset()
        {
            _rulesTree = new();
            _ruleInsertionNodes = new();
            _ruleInsertionNodes.Add(_rulesTree);
            _activeExecutors = new();
        }

        public void RegisterPath(params string[] steps) => RegisterPath(new List<string>(steps));

        public void RegisterPath(List<string> path)
        {
            Node<string, Rule> current = _rulesTree;
            path.ForEach(step =>
            {
                if (!current.Children.ContainsKey(step))
                {
                    _ruleInsertionNodes.Remove(current);
                    current.AddChild(step, new());
                }
                current = current.Children[step];
            });
            _ruleInsertionNodes.Add(current);
        }

        public void Unregister(Rule rule)
        {
            Node<string, Rule> current = _rulesTree;
            rule.Path.ForEach(step =>
            {
                if (!current.Children.ContainsKey(step))
                    throw new System.Exception($"[RULES ENGINE] Unregister rule: invalid step {step}");
                current = current.Children[step];
            });
            current.RemoveChild(rule.Id);
        }

        public void Register(Rule rule)
        {
            Node<string, Rule> current = _rulesTree;
            rule.Path.ForEach(step =>
            {
                if (!current.Children.ContainsKey(step))
                    throw new System.Exception($"[RULES ENGINE] Register rule: invalid step {step}");
                current = current.Children[step];
            });
            if (!_ruleInsertionNodes.Contains(current))
            {
                throw new System.Exception($"[RULES ENGINE] Register path {rule.Path}");
            }
            current.AddChild(rule.Id, new(rule));
        }

        public async Task Process(GameEventData gameEvent)
        {
            var executor = new RuleExecutor(_rulesTree, _activeExecutors.Count > 0 ? _activeExecutors.Peek().ExecutionCount : new());
            _activeExecutors.Push(executor);
            await executor.Process(gameEvent);
            _activeExecutors.Pop();
        }
    }
}
