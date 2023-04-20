using System.Collections;
using System.Collections.Generic;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Decodex
{
    public class HandSlotController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public int Index { get; private set; }
        public void Init(int index)
        {
            name = $"Slot {index}";
            Index = index;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Slot on enter " + name);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("Slot on exit " + name);
        }
    }
}
