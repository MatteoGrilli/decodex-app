using Decodex.Cards;
using Decodex.Tiles;
using Decodex.Zones;
using Grim.Zones;
using Grim.Zones.Coordinates;
using System.Collections;
using UnityEngine;

namespace Decodex
{
    public class Test : MonoBehaviour
    {
        [SerializeField]
        private int _nCards = 10;

        void Start()
        {
            CreateHand("Hand 1", 0);
            CreateHand("Hand 2", 10);
            CreateBoard("Board");
            CreateMemory("Memory 1");
            CreateMemory("Memory 2");
        }

        private void CreateHand(string name, int startingCoord)
        {
            var layout = LinearCoordinateSpace.GetSegment(new LinearCoordinate(0), LinearCoordinateSpace.Right, 9, true);
            var model = new CompactZone<LinearCoordinate, CardInstance>(name, "HAND", layout);
            var handController = GameObject.Find(name).GetComponent<HandController>();
            handController.Init(model);
            StartCoroutine(PopulateHand(name, startingCoord, model));
        }

        private void CreateBoard(string name)
        {
            var layout = CubeCoordinateSpace.GetBall(new CubeCoordinate(0, 0, 0), 2, true);
            var model = new Zone<CubeCoordinate, TileInstance>(name, "BOARD", layout);
            for(int i = 0; i < layout.Count; i++)
            {
                model.Put(layout[i], new TileInstance($"{i}"));
            }
            var boardController = GameObject.Find(name).GetComponent<BoardController>();
            boardController.Init(model);
            boardController.Render();
        }

        private void CreateMemory(string name)
        {
            var layout = LinearCoordinateSpace.GetSegment(new LinearCoordinate(0), LinearCoordinateSpace.Right, 6, true);
            var model = new Zone<LinearCoordinate, CardInstance>(name, "MEMORY", layout);
            var memoryController = GameObject.Find(name).GetComponent<MemoryController>();
            memoryController.Init(model);
        }

        private IEnumerator PopulateHand(string name, int startingCoord, Zone<LinearCoordinate, CardInstance> model)
        {
            var addCardInterval = new WaitForSeconds(.3f);
            for (int i = startingCoord; i < _nCards + startingCoord; i++)
            {
                model.Put(new CardInstance($"{i}", "TST"));
                yield return addCardInterval;
            }
            GameObject.Find(name).GetComponent<HandController>().Render();
            yield break;
        }
    }
}
