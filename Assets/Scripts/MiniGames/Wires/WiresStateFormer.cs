using System;
using System.Collections.Generic;
using UnityEngine;

namespace MazeMiniGame
{
    public class WiresStateFormer : MonoBehaviour
    {
        public static readonly Dictionary<char, WireType> translateDict = new()
        {
            { '-', WireType.Default },
            { '_', WireType.Long },
            { '/', WireType.Corner },
            { '|', WireType.LongCorner },
            { '?', WireType.Bridge }
        };
        public static readonly Dictionary<WireType, (float Width, float Height)> sizes = new()
        {
            { WireType.Default, (2, 0) },
            { WireType.Long, (3, 0) },
            { WireType.Corner, (2, 2) },
            { WireType.LongCorner, (3, 2) },
            { WireType.Bridge, (2, 0) }
        };
        public static WireField GetLevel(int number, GameObject gameObject, WireGUIPref wirePef, FieldGUIPref fieldPref, Transform cameraTransform)
        {
            switch (number)
            {
                case 1:
                    return MakeWireField1(gameObject, wirePef, fieldPref, cameraTransform);
            }

            return null;
        }

        private static WireField MakeWireField1(GameObject gameObject, WireGUIPref wirePef, FieldGUIPref fieldPref, Transform camera)
        {
            var field = gameObject.AddComponent<WireField>();
            field.transform.position = camera.position;
            field.cameraTransform = camera;
            field.WirePrefab = wirePef;
            field.fieldGUIPrefab = fieldPref;
            
            var stringField = new[]
            {
                "---/",
                "----"
            };
            var positionsReal = new[] 
            {
                "0 0|2 2|4 0|6 0",
                "0 2|2 2|4 2|8 2"
            };
            var positionsGUI = new[]
            {
                "-4 2|0 2|4 2|6.5 2",
                "-4 -2|0 -2|4 -2|8 -2"
            };
            var scales = new float[]
            {
                8.1f, 8.1f, 8.1f, 8.1f, 8.1f
            };
            field.startPosition1 = (0, 0);
            field.startPosition2 = (0, 2);
            field.currentPosition2 = field.startPosition2;
            field.currentPosition1 = field.startPosition1;
            field.endPosition = (2, 0);
            field.SetField(stringField, positionsReal, positionsGUI, scales);
            return field;
        }
    }
}