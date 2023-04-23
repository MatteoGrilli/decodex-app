using System.Collections;
using System.Collections.Generic;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Decodex
{
    public class HandSlotController : MonoBehaviour
    {
        public int Index { get; private set; }
        public void Init(int index)
        {
            name = $"Slot {index}";
            Index = index;
        }
    }
}
