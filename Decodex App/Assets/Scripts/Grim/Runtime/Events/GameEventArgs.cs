using Grim.Rules;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grim
{
    public class GameEventArgs<T>
    {
        public T Args;
        public bool Continue { get; set; }
        private List<string> _previouslyExecutedActions;
        public GameEventArgs(ref T args)
        {
            Continue = true;
            Args = args;
            _previouslyExecutedActions = new();
        }

        public void RegisterAction(string id) => _previouslyExecutedActions.Add(id);
        public bool HasAlreadyExecuted(GameAction<T> action) => _previouslyExecutedActions.Contains(action.Id);
    }
}
