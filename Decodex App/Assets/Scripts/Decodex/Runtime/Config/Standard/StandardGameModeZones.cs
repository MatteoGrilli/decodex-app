using Decodex.Tiles;
using Decodex.Zones;
using Grim.Zones.Coordinates;
using Grim.Zones;
using UnityEngine;
using Decodex.Cards;
using UnityEditor;

namespace Decodex
{
    /// <summary>
    /// WARNING! This is just to make it quick. In the future, we should
    /// create a configuration system to load up different game modes.
    /// 
    /// ONLY WORKS IN EDITOR!
    /// </summary>
    public static class StandardGameModeZones
    {
        public static GameObject CreateInspector(string id, Transform parent, Vector3 localPosition, Quaternion localRotation)
        {
            // Create gameobject
            var zoneObj = new GameObject(id);
            zoneObj.name = id;
            zoneObj.transform.SetParent(parent);
            zoneObj.transform.localPosition = localPosition;
            zoneObj.transform.localRotation = localRotation;
            return zoneObj;
        }

        public static GameObject CreateBoard(string id, Transform parent, Vector3 localPosition, Quaternion localRotation)
        {
            // Create model
            var layout = CubeCoordinateSpace.GetBall(new CubeCoordinate(0, 0, 0), 2, true);
            var model = new Zone<CubeCoordinate, TileInstance>(id, ZoneTypes.Board, layout);
            for (int i = 0; i < layout.Count; i++)
            {
                model.Put(layout[i], new TileInstance($"{i}"));
            }

            // Create gameobject
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Zones/Board.prefab");
            var zoneObj = GameObject.Instantiate(prefab);
            zoneObj.name = id;
            zoneObj.transform.SetParent(parent);
            zoneObj.transform.localPosition = localPosition;
            zoneObj.transform.localRotation = localRotation;
            zoneObj.GetComponent<BoardController>().Init(model);
            return zoneObj;
        }

        public static GameObject CreateHand(string id, Transform parent, Vector3 localPosition, Quaternion localRotation)
        {
            // Create model
            var layout = LinearCoordinateSpace.GetSegment(new LinearCoordinate(0), LinearCoordinateSpace.Right, 9, true);
            var model = new CompactZone<LinearCoordinate, CardInstance>(id, ZoneTypes.Hand, layout);

            // Create gameobject
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Zones/Hand.prefab");
            var zoneObj = GameObject.Instantiate(prefab);
            zoneObj.name = id;
            zoneObj.transform.SetParent(parent);
            zoneObj.transform.localPosition = localPosition;
            zoneObj.transform.localRotation = localRotation;
            zoneObj.GetComponent<HandController>().Init(model);
            return zoneObj;
        }

        public static GameObject CreateMemory(string id, Transform parent, Vector3 localPosition, Quaternion localRotation)
        {
            // Create model
            var layout = LinearCoordinateSpace.GetSegment(new LinearCoordinate(0), LinearCoordinateSpace.Right, 6, true);
            var model = new Zone<LinearCoordinate, CardInstance>(id, ZoneTypes.Memory, layout);

            // Create gameobject
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Zones/Memory.prefab");
            var zoneObj = GameObject.Instantiate(prefab);
            zoneObj.name = id;
            zoneObj.transform.SetParent(parent);
            zoneObj.transform.localPosition = localPosition;
            zoneObj.transform.localRotation = localRotation;
            zoneObj.GetComponent<MemoryController>().Init(model);
            return zoneObj;
        }

        public static GameObject CreateDeck(string id, Transform parent, Vector3 localPosition, Quaternion localRotation)
        {
            // Create model
            var size = 30;
            var layout = LinearCoordinateSpace.GetSegment(new LinearCoordinate(0), LinearCoordinateSpace.Right, size - 1, true);
            var model = new CompactZone<LinearCoordinate, CardInstance>(id, ZoneTypes.Deck, layout);

            // Create gameobject
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Zones/Deck.prefab");
            var zoneObj = GameObject.Instantiate(prefab);
            zoneObj.name = id;
            zoneObj.transform.SetParent(parent);
            zoneObj.transform.localPosition = localPosition;
            zoneObj.transform.localRotation = localRotation;
            zoneObj.GetComponent<DeckController>().Init(model);

            // Populate with mock cards
            var cardsRootObj = GameObject.Find("Cards");
            var cardPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Card.prefab");
            for(int i = 0; i < size; i++)
            {
                var card = GameObject.Instantiate(cardPrefab);
                card.transform.SetParent(cardsRootObj.transform);
                // Should be position on "top" of the deck, but whatev
                card.transform.localPosition = zoneObj.transform.position;
                card.transform.localRotation = zoneObj.transform.rotation;
                var cardId = $"{id}_{i}";
                card.name = cardId;
                var cardModel = new CardInstance(cardId, "PRT_00");
                model.Put(cardModel);
            }

            return zoneObj;
        }
    }
}
