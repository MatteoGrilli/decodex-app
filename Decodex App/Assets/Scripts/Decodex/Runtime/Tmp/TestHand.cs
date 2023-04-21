using Decodex.Cards;
using Decodex.Zones;
using Grim.Zones;
using Grim.Zones.Coordinates;
using Grim.Zones.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Decodex
{
    public class TestHand : MonoBehaviour
    {
        [SerializeField]
        private int _nCards = 10;
        // Start is called before the first frame update
        void Start()
        {
            var layout = LinearCoordinateSpace.GetSegment(new LinearCoordinate(0), LinearCoordinateSpace.Right, 9, true);
            var model = new CompactZone<LinearCoordinate, CardInstance>("hand", layout);
            var handController = GetComponent<HandController>();
            handController.Init(model);
            StartCoroutine(PopulateHand(model));
           
        }

        private IEnumerator PopulateHand(Zone<LinearCoordinate, CardInstance> model)
        {
            var addCardInterval = new WaitForSeconds(.3f);
            for (int i = 0; i < _nCards; i++)
            {
                model.Put(new CardInstance($"{i}", "TST"));
                yield return addCardInterval;
            }
            GetComponent<HandController>().Render();
            yield break;
        }
    }
}
