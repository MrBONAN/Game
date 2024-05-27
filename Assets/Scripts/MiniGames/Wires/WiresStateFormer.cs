using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MazeMiniGame
{
    public class WiresStateFormer : MonoBehaviour
    {
        public static Dictionary<char, WireType> translateDict = new Dictionary<char, WireType>()
        {
            { '-', WireType.Default },
            { '_', WireType.Long },
            { '/', WireType.Corner },
            { '|', WireType.LongCorner },
            { '?', WireType.Bridge }
        };
        public static WireField GetLevel(int number, GameObject gameObject)
        {
            switch (number)
            {
                case 1:
                    return MakeWireField1(gameObject);
            }

            return null;
        }

        private static WireField MakeWireField1(GameObject gameObject)
        {
            var field = gameObject.AddComponent<WireField>();
            var stringField = new string[]
            {
                "/|?-",
                "_-?/"
            };
            var positions = new string[] 
            {
                "(5, 5)|(15, 9)|(25, 40)|(56, 70)",
                "(5, 85)|(15, 29)|(25, 70)|(56, 99)"
            };
            field.startPosition1 = (0, 0);
            field.startPosition2 = (2, 0);
            field.endPoosition = (3, 1);
            field.SetField(stringField, positions);
            return field;
        }
    }
}