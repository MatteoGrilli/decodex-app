using System;
using System.Collections.Generic;

namespace Grim.Events
{
    public class EventArgs<T>
    {
        public T Args;
        
        public bool Continue { get; set; }
        
        private List<string> _previouslyExecutedActions;

        public DateTime Timestamp { get; private set; }

        public EventArgs(ref T args)
        {
            Timestamp = DateTime.Now;
            Continue = true;
            Args = args;
            _previouslyExecutedActions = new();
        }

        // TODO: EW THE NAMES
        public void RegisterReplacementRule(string id) => _previouslyExecutedActions.Add(id);
        public bool HasAlreadyExecutedReplacementRule(string id) => _previouslyExecutedActions.Contains(id);
    }
}
