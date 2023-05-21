using Decodex.Cards;
using Grim.Zones;
using Grim.Zones.Coordinates;
using System.Collections.Generic;
using UnityEngine;

namespace Decodex.Zones
{
    public class MemoryController : ZoneController<LinearCoordinate, CardInstance>
    {
        [SerializeField]
        [Tooltip("Center to center distance between slots.")]
        private float _distance;

        [SerializeField]
        private GameObject _slotPrefab;

        private List<GameObject> _slots;

        public override void Init(Zone<LinearCoordinate, CardInstance> model)
        {
            base.Init(model);
            InitSlots();
        }

        private void InitSlots()
        {
            _slots = new();
            CreateSlots();
        }

        private void CreateSlots()
        {
            // Destroy previous slots
            _slots.ForEach(slot => Destroy(slot.gameObject));
            _slots.Clear();

            // Create new slots
            var startingIndex = Model.NumSlots - Model.ItemsCount();
            var totalWidth = _distance * (Model.NumSlots - 1);
            for (int i = 0; i < Model.NumSlots; i++)
            {
                var slot = Instantiate(_slotPrefab);
                slot.transform.SetParent(transform);
                slot.transform.localPosition = Vector3.zero;
                slot.transform.localPosition += slot.transform.right * 0.5f * totalWidth;
                slot.transform.localPosition -= slot.transform.right * _distance * i;
                slot.transform.localRotation = Quaternion.identity;
                _slots.Add(slot);
            }
        }

        public override void Render()
        {
            base.Render();
            ArrangeCardControllers();
        }

        private void ArrangeCardControllers()
        {
            for (var i = 0; i < Model.ItemsCount(); i++)
            {
                ArrangeCardController(i);
            }
        }

        private void ArrangeCardController(int index)
        {
            var slot = _slots[index];
            var cardInstance = Model.GetAll()[index];
            var cardInstanceGameObject = GameObject.Find(cardInstance.Id);
            cardInstanceGameObject.transform.localPosition = slot.transform.position;
            cardInstanceGameObject.transform.localRotation = slot.transform.rotation;
        }

        protected override void OnItemsPut(ZoneEventArgs<LinearCoordinate, CardInstance> e)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnItemsRemoved(ZoneEventArgs<LinearCoordinate, CardInstance> e)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnItemsShuffled()
        {
            throw new System.NotImplementedException();
        }
    }
}
