using Decodex.Tiles;
using Grim.Zones;
using Grim.Zones.Coordinates;
using NaughtyAttributes;
using System.Collections.Generic;
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

        private List<GameObject> _tiles;

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
            Model.GetAll().ForEach(item =>
            {
                var tile = Instantiate(_tilePrefab);
                tile.transform.SetParent(transform);
                tile.transform.localPosition = Model.GetCoordinateForItem(item).ToCartesian() * _distance;
                _tiles.Add(tile);
            });
        }

        private void CleanTiles()
        {
            _tiles.ForEach(tile => Destroy(tile));
            _tiles.Clear();
        }

        [Button]
        public override void Render()
        {
            base.Render();
            CleanTiles();
            CreateTiles();
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
