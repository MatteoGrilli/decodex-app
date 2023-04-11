using Decodex.Cards;
using Grim;
using Grim.Zones;
using Grim.Zones.Coordinates;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using RangeAttribute = UnityEngine.RangeAttribute;

namespace Decodex.Zones
{
    public class HandController : ZoneController<LinearCoordinate, CardInstance>, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private bool _debug;

        [Tooltip("Curvature of the hand. 0 would be a straight line.")]
        [SerializeField]
        [Min(0f)]
        private float _curvature;

        [Tooltip("On hover, the hand extends into the view by this distance.")]
        [SerializeField]
        private float _extensionOffset;

        [Tooltip("Offset used to have the bottom side of the card align with the bottom of the screen while the card is in close-up")]
        [SerializeField]
        private float _closeUpOffset;

        [Tooltip("Tilt angle in degrees to prevent overlapping of the sides of cards.")]
        [SerializeField]
        private float _tiltAngle;

        [Tooltip("Total angle of fanning of the hand.")]
        [SerializeField]
        [Range(0f, 360f)]
        private float _fanningAngle;

        [SerializeField]
        [Tooltip("Left to right means the left card is on top.")]
        private FanningDirection _fanningDirection;

        [SerializeField]
        private GameObject _slotPrefab;

        private Pose _centerOfCurvature;
        private List<(Pose, Pose)> _slotPoses;

        private float _radius;

        public override void Init(Zone<LinearCoordinate, CardInstance> model)
        {
            base.Init(model);
            _radius = 1 / _curvature;
            _centerOfCurvature = CalculateCenterOfCurvature();
            _slotPoses = CalculateSlotPoses();
        }

        public override void Render()
        {
            base.Render();
            if (_debug)
            {
                RenderDebug();
            }

            ArrangeSlots();
            ArrangeCardControllers();
        }

        private void RenderDebug()
        {
            _slotPoses.ForEach(pose => RenderPoseGizmo(pose.Item1));
            RenderPoseGizmo(_centerOfCurvature);
        }

        private void ArrangeSlots()
        {
            // Clear previous slots
            foreach(Transform child in transform)
            {
                GameObject.Destroy(child);
            }
            // Create new slots
            var startingIndex = zone.NumSlots - zone.ItemsCount();
            for (int i = 0; i < zone.ItemsCount(); i++)
            {
                var pose = _slotPoses[startingIndex + 2 * i];
                var slot = GameObject.Instantiate(_slotPrefab, pose.Item1.position, pose.Item1.rotation);
                slot.name = $"{i}";
                slot.transform.SetParent(transform);
            }
        }

        private void ArrangeCardControllers()
        {
            for(var i = 0; i < zone.ItemsCount(); i++)
            {
                var slot = transform.GetChild(i);
                var cardInstance = zone.GetAll()[i];
                var cardInstanceGameObject = GameObject.Find(cardInstance.Id);
                cardInstanceGameObject.transform.position = slot.position;
                cardInstanceGameObject.transform.rotation = slot.rotation;
            }
        }

        private GameObject RenderPoseGizmo(Pose pose)
        {
            var gameObject = new GameObject();
            gameObject.name = "Hand Debug";
            gameObject.transform.position = pose.position;
            gameObject.transform.rotation = pose.rotation;
            EditorGUIUtility.SetIconForObject(gameObject, (Texture2D)EditorGUIUtility.IconContent("sv_icon_dot4_sml").image);
            return gameObject;
        }

        private List<(Pose, Pose)> CalculateSlotPoses()
        {
            List<(Pose, Pose)> poses = new();
            int nPoses = 2 * zone.NumSlots - 1;
            float fanningAngleStep = _fanningAngle / (nPoses - 1);
            for (var i = 0; i < nPoses; i++)
            {
                Pose restingPose = CalculateRestingPose(fanningAngleStep * i);
                Pose closeupPose = CalculateRestingPose(fanningAngleStep * i);
                poses.Add((restingPose, closeupPose));
            }
            return poses;
        }

        private Pose CalculateRestingPose(float angle)
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
        /// </summary>
        /// <param name="radius"></param>
        /// <returns></returns>
        private Pose CalculateCenterOfCurvature()
        {
            var centerOfCurvature = new Pose();
            centerOfCurvature.rotation = transform.rotation;
            centerOfCurvature.position = transform.position - centerOfCurvature.up * _radius;
            return centerOfCurvature;
        }

        protected override void OnItemsPut(ZoneEventArgs<LinearCoordinate, CardInstance> e)
        {
            //throw new System.NotImplementedException();
        }

        protected override void OnItemsRemoved(ZoneEventArgs<LinearCoordinate, CardInstance> e)
        {
            //throw new System.NotImplementedException();
        }

        protected override void OnItemsShuffled()
        {
            //throw new System.NotImplementedException();
        }

        /* -------------------- EXTENSION AND RETRACTION --------------------*/

        public void OnPointerEnter(PointerEventData eventData)
        {
            Extend();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Retract();
        }

        public void Extend()
        {
            transform.position += transform.up * _extensionOffset;
            ArrangeCardControllers();
        }

        public void Retract()
        {
            transform.position -= transform.up * _extensionOffset;
            ArrangeCardControllers();
        }

        /* -------------------- CLOSE UP --------------------*/

        private void OnMouseOver()
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            Physics.Raycast(ray, out var hit);
            var name = hit.collider.name;
            Debug.Log(name);
        }


    }
}
