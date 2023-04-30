using System;

namespace Grim.Rules
{
    public class Condition<T>
    {
        public string Id { get; private set; }
        public Func<T, bool> Evaluate { get; private set; }

        public Condition(string id, Func<T, bool> evaluate)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (evaluate == null) throw new ArgumentNullException(nameof(evaluate));
            Id = id;
            Evaluate = evaluate;
        }
    }
}
