using Decodex.Cards;
using Decodex.Tiles;
using Decodex.Zones;
using Grim.Zones;
using Grim.Zones.Coordinates;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Decodex
{
    public class Test : MonoBehaviour
    {
        [SerializeField]
        private int _nCards = 10;

        void Start()
        {
            CreateHand();
            CreateBoard();
        }

        private void CreateHand()
        {
            var layout = LinearCoordinateSpace.GetSegment(new LinearCoordinate(0), LinearCoordinateSpace.Right, 9, true);
            var model = new CompactZone<LinearCoordinate, CardInstance>("hand", layout);
            var handController = GameObject.Find("Hand").GetComponent<HandController>();
            handController.Init(model);
            StartCoroutine(PopulateHand(model));
        }

        private void CreateBoard()
        {
            var layout = CubeCoordinateSpace.GetBall(new CubeCoordinate(0, 0, 0), 2, true);
            var model = new Zone<CubeCoordinate, TileInstance>("board", layout);
            for(int i = 0; i < layout.Count; i++)
            {
                model.Put(layout[i], new TileInstance($"{i}"));
            }
            var boardController = GameObject.Find("Board").GetComponent<BoardController>();
            boardController.Init(model);
            boardController.Render();
        }

        private IEnumerator PopulateHand(Zone<LinearCoordinate, CardInstance> model)
        {
            var addCardInterval = new WaitForSeconds(.3f);
            for (int i = 0; i < _nCards; i++)
            {
                model.Put(new CardInstance($"{i}", "TST"));
                yield return addCardInterval;
            }
            GameObject.Find("Hand").GetComponent<HandController>().Render();
            yield break;
        }
    }
}
