using Decodex.Cards;
using Decodex.Tiles;
using Grim;
using Grim.Zones;
using Grim.Zones.Coordinates;
using System;
using UnityEngine;

namespace Decodex.Zones

{
    public class BoardController : ZoneController<CubeCoordinate, TileInstance>
    {
        [SerializeField]
        [Tooltip("Center to center distance between tiles.")]
        private float _distance;

        [SerializeField]
        private GameObject _tilePrefab;

        private GameObject _tiles;

        public override void Init(Zone<CubeCoordinate, TileInstance> model)
        {
            base.Init(model);
            InitTiles();
        }

        private void InitTiles()
        {
            _tiles = new();
            CreateTiles();
        }

        private void CreateTiles()
        {
            zone.GetAll().ForEach(item =>
            {
                Debug.Log(item.Id + " : " + zone.GetCoordinateForItem(item));
                var tile = Instantiate(_tilePrefab);
                tile.transform.SetParent(transform);
                tile.transform.localPosition = zone.GetCoordinateForItem(item).ToCartesian();
                _tiles = tile;
            });
        }

        public override void Render()
        {
            base.Render();

        }

        protected override void OnItemsPut(ZoneEventArgs<CubeCoordinate, TileInstance> e)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnItemsRemoved(ZoneEventArgs<CubeCoordinate, TileInstance> e)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnItemsShuffled()
        {
            throw new System.NotImplementedException();
        }
    }
}
