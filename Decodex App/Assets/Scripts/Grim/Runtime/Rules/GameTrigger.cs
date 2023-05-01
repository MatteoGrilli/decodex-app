using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grim.Rules
{
    // Not a fan of the name. Could probably be a struct
    public class GameTrigger : MonoBehaviour
    {
        public string Context { get; private set; }
        public string Id { get; private set; }
        
        public GameTrigger(string context, string id)
        {
            Context = context;
            Id = id;
        }
    }
}
