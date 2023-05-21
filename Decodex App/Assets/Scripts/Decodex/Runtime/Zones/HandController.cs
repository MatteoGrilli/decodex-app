using Decodex.Cards;
using Grim.Zones;
using Grim.Zones.Coordinates;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.EventSystems;
using RangeAttribute = UnityEngine.RangeAttribute;

namespace Decodex.Zones
{
    // TODO: _slots might not be necessary afterall
    // TODO: the component for the hand extension/retraction is also not necessary
    public class HandController : ZoneController<LinearCoordinate, CardInstance>, IPointerMoveHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [BoxGroup("Appearence")]
        [Tooltip("Curvature of the hand. 0 would be a straight line.")]
        [SerializeField]
        [Min(0f)]
        private float _curvature;

        [BoxGroup("Appearence")]
        [Tooltip("Tilt angle in degrees to prevent overlapping of the sides of cards.")]
        [SerializeField]
        [Range(0f, 360f)]
        private float _tiltAngle;

        [BoxGroup("Appearence")]
        [Tooltip("Total angle of fanning of the hand.")]
        [SerializeField]
        [Range(0f, 360f)]
        private float _fanningAngle;

        [BoxGroup("Appearence")]
        [SerializeField]
        [Tooltip("Left to right means the left card is on top.")]
        private FanningDirection _fanningDirection;

        [BoxGroup("Extend/Retract")]
        [Label("Enabled")]
        [SerializeField]
        private bool _enableRetract;

        [BoxGroup("Extend/Retract")]
        [Tooltip("On hover, the hand extends into the view by this distance.")]
        [SerializeField]
        private float _extensionOffset;

        [BoxGroup("Inspection")]
        [Label("Enabled")]
        [SerializeField]
        private bool _enableInspection;

        [BoxGroup("Inspection")]
        [Label("Vertical Offset")]
        [Tooltip("Offset used to have the bottom side of the card align with the bottom of the screen while the card is in close-up")]
        [SerializeField]
        private float _inspectionUpOffset;

        [BoxGroup("Inspection")]
        [Label("Distance from Camera")]
        [Tooltip("Distance from the camera")]
        [SerializeField]
        private float _inspectionDistanceFromCamera = 1f;

        [BoxGroup("Debug")]
        [Label("Center of curvature")]
        [SerializeField]
        private bool _showCenterOfCurvature;

        [BoxGroup("Debug")]
        [Label("Anchors")]
        [SerializeField]
        private bool _showAnchors;

        [BoxGroup("Debug")]
        [SerializeField]
        private Transform _debugRoot;

        [SerializeField]
        private GameObject _slotPrefab;

        private Pose _centerOfCurvature;
        private List<Pose> _anchors;
        private List<GameObject> _slots;

        private int _currentlyInspectedSlotIndex;
        private float _radius;

        public override void Init(Zone<LinearCoordinate, CardInstance> model)
        {
            base.Init(model);
            _radius = 1 / _curvature;
            _centerOfCurvature = CalculateCenterOfCurvature();
            InitAnchors();
            InitSlots();
            InitInspectedSlot();
            InitPositionController();
        }

        private void InitPositionController() => GetComponentInChildren<HandPositionController>().Init(_extensionOffset, _enableRetract);

        private void InitAnchors() => _anchors = CalculateAnchors();

        private void InitSlots()
        {
            _slots = new();
            CreateSlots();
        }

        private void InitInspectedSlot() => _currentlyInspectedSlotIndex = -1;

        [Button]
        public override void Render()
        {
            base.Render();
            if (_showAnchors || _showCenterOfCurvature) CleanDebugGizmos();
            if (_showAnchors) RenderAnchorGizmo();
            if (_showCenterOfCurvature) RenderCenterOfCurvatureGizmo();
            ArrangeCardControllers();
        }

        private void CleanDebugGizmos()
        {
            foreach (Transform child in _debugRoot)
            {
                Destroy(child.gameObject);
            }
        }

        private void RenderAnchorGizmo() => _anchors.ForEach(anchor => RenderPoseGizmo("Anchor", anchor));

        private void RenderCenterOfCurvatureGizmo() => RenderPoseGizmo("Center of Curvature", _centerOfCurvature);

        private void CreateSlots()
        {
            // Destroy previous slots
            _slots.ForEach(slot => Destroy(slot.gameObject));
            _slots.Clear();

            // Create new slots
            var startingIndex = Model.NumSlots - Model.ItemsCount();
            for (int i = 0; i < Model.ItemsCount(); i++)
            {
                var pose = _anchors[startingIndex + 2 * i];
                var slot = Instantiate(_slotPrefab, Vector3.zero, Quaternion.identity);
                slot.transform.SetParent(transform);
                slot.transform.localPosition = pose.position;
                slot.transform.localRotation = pose.rotation;
                slot.GetComponent<HandSlotController>().Init(i);
                _slots.Add(slot);
            }
        }

        private void ArrangeCardControllers()
        {
            for (var i = 0; i < Model.ItemsCount(); i++)
            {
                if (i != _currentlyInspectedSlotIndex) ArrangeCardController(i);
            }
        }

        private void ArrangeCardController(int index)
        {
            var slot = _slots[index];
            var cardInstance = Model.GetAll()[index];
            var cardInstanceGameObject = GameObject.Find(cardInstance.Id);
            cardInstanceGameObject.transform.position = slot.transform.position;
            cardInstanceGameObject.transform.rotation = slot.transform.rotation;
        }

        private GameObject RenderPoseGizmo(string name, Pose pose)
        {
            var gameObject = new GameObject();
            gameObject.name = name;
            gameObject.transform.SetParent(_debugRoot);
            gameObject.transform.localPosition = pose.position;
            gameObject.transform.localRotation = pose.rotation;
            EditorGUIUtility.SetIconForObject(gameObject, (Texture2D)EditorGUIUtility.IconContent("sv_icon_dot4_sml").image);
            return gameObject;
        }

        private List<Pose> CalculateAnchors()
        {
            List<Pose> anchors = new();
            int nPoses = 2 * Model.NumSlots - 1;
            float fanningAngleStep = _fanningAngle / (nPoses - 1);
            for (var i = 0; i < nPoses; i++)
            {
                anchors.Add(CalculateAnchor(fanningAngleStep * i));
            }
            return anchors;
        }

        private Pose CalculateAnchor(float angle)
        {
            var sign = _fanningDirection == FanningDirection.RIGHT_TO_LEFT ? -1 : 1;
            Pose pose = _centerOfCurvature;
            pose.rotation = Quaternion.AngleAxis(0.5f * _fanningAngle, pose.forward) * pose.rotation;
            pose.rotation = Quaternion.AngleAxis(-angle, pose.forward) * pose.rotation;
            pose.rotation = Quaternion.AngleAxis(_tiltAngle * sign, pose.up) * pose.rotation;
            pose.position += pose.up * _radius;
            return pose;
        }

        /// <summary>
        /// Generates the center of the curvature of the hand.
        /// The position is local with respect to the hand root.
        /// </summary>
        /// <param name="radius"></param>
        /// <returns></returns>
        private Pose CalculateCenterOfCurvature()
        {
            var centerOfCurvature = new Pose();
            centerOfCurvature.rotation = Quaternion.identity;
            centerOfCurvature.position = Vector3.zero - centerOfCurvature.up * _radius;
            return centerOfCurvature;
        }

        protected override void OnItemsPut(ZoneEventArgs<LinearCoordinate, CardInstance> e)
        {
            CreateSlots();
            Render();
        }

        protected override void OnItemsRemoved(ZoneEventArgs<LinearCoordinate, CardInstance> e)
        {
            CreateSlots();
            Render();
        }

        protected override void OnItemsShuffled()
        {
            throw new System.NotImplementedException();
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (_enableInspection) UpdateInspectedSlot(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_enableInspection) UpdateInspectedSlot(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_enableInspection) UpdateInspectedSlot(eventData);
        }

        private void UpdateInspectedSlot(PointerEventData eventData)
        {
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            Physics.Raycast(ray, 100f, LayerMask.GetMask("Hand Slots"));
            if (Physics.Raycast(ray, out var hit, 10f, LayerMask.GetMask("Hand Slots")))
            {
                InspectSlot(hit.collider.GetComponent<HandSlotController>().Index);
            } else
            {
                _currentlyInspectedSlotIndex = -1;
            }
        }

        private void InspectSlot(int index)
        {
            if (index == _currentlyInspectedSlotIndex) return;

            // Rearrange previously inspected card
            if (_currentlyInspectedSlotIndex >= 0)
                ArrangeCardController(_currentlyInspectedSlotIndex);

            // Calculate inspection position
            var slot = _slots[index];
            var slotCenterOnScreen = Camera.main.WorldToScreenPoint(slot.transform.position);
            Vector3 inspectionPosition = Camera.main.ScreenToWorldPoint(new Vector3(slotCenterOnScreen.x, 0f, _inspectionDistanceFromCamera));

            // Arrange inspected card
            var cardInstance = Model.GetAll()[index];
            var cardInstanceGameObject = GameObject.Find(cardInstance.Id);
            cardInstanceGameObject.transform.position = inspectionPosition;
            cardInstanceGameObject.transform.position += cardInstanceGameObject.transform.up * _inspectionUpOffset;
            var upwards = Camera.main.transform.up;
            var forward = -Camera.main.transform.forward;
            cardInstanceGameObject.transform.rotation = Quaternion.LookRotation(forward, upwards);

            _currentlyInspectedSlotIndex = index;
        }
    }
}
