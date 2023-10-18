using Decodex.Cards;
using Grim;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Decodex
{
    public class CardController : ItemController<CardInstance>
    {
        [SerializeField]
        private InputActionAsset _inputActions;

        private Vector3 _currentScreenPosition;
        private Camera _camera;
        private bool _isDragging;

        private Vector3 WorldPosition
        {
            get
            {
                // TODO: rename this lil shit
                float z = _camera.WorldToScreenPoint(transform.position).z;
                return _camera.ScreenToWorldPoint(_currentScreenPosition + new Vector3(0, 0, z));
            }
        }

        private bool IsBeingClickedOn
        {
            get
            {
                Ray ray = _camera.ScreenPointToRay(_currentScreenPosition);
                Debug.Log(_currentScreenPosition);
                Debug.Log(LayerMask.GetMask("Cards"));
                Debug.Log(Physics.Raycast(ray, out var hit, 1000, LayerMask.GetMask("Cards")));
                return Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Cards")) ? hit.transform == transform : false;
            }
        }

        private void OnEnable()
        {
            _inputActions["Click"].Enable();
            _inputActions["Point"].Enable();
            _inputActions["Point"].performed += UpdateCurrentPosition;
            _inputActions["Click"].performed += StartDrag;
            _inputActions["Click"].canceled += StopDrag;
        }

        private void OnDisable()
        {
            _inputActions["Point"].performed -= UpdateCurrentPosition;
            _inputActions["Click"].performed -= StartDrag;
            _inputActions["Click"].performed -= StopDrag;
            _inputActions["Click"].Disable();
            _inputActions["Point"].Disable();
        }

        private void Awake()
        {
            _camera = Camera.main; // Instead should be player camera probably
        }

        private void StartDrag(InputAction.CallbackContext ctx)
        {
            if (IsBeingClickedOn)
            {
                StartCoroutine(Drag());
            }
        }

        private void StopDrag(InputAction.CallbackContext ctx) => _isDragging = false;

        private IEnumerator Drag()
        {
            _isDragging = true;
            Vector3 offset = transform.position - WorldPosition;
            while (_isDragging)
            {
                transform.position = WorldPosition + offset;
                yield break; // maybe yield return null
            }
        }

        private void UpdateCurrentPosition(InputAction.CallbackContext ctx) => _currentScreenPosition = ctx.ReadValue<Vector2>();
    }
}
