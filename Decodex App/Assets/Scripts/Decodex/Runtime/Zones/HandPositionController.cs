using UnityEngine;
using UnityEngine.EventSystems;

namespace Decodex.Zones
{
    // Require component in parent: HandController
    public class HandPositionController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private bool _enabled;
        public float ExtensionOffset { get; private set; }
        private HandController _handController;

        public void Init(float extensionOffset, bool enabled)
        {
            _enabled = enabled;
            ExtensionOffset = extensionOffset;
            _handController = GetComponentInParent<HandController>();
            if (_enabled) Retract();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_enabled) Extend();
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if (_enabled) Retract();
        }

        private void Extend()
        {
            _handController.transform.position += _handController.transform.up * ExtensionOffset;
            _handController.Render();
        }

        private void Retract()
        {
            _handController.transform.position -= _handController.transform.up * ExtensionOffset;
            _handController.Render();
        }
    }
}
