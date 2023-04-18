using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Decodex
{
    public class HandSlotController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
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
