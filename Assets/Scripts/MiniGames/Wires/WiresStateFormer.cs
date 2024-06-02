using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        public static WireField GetLevel(int number, GameObject gameObject, WireGUIPref wirePef, FieldGUIPref fieldPref,
            Transform cameraTransform, GameObject start, GameObject end, GameObject line)
        {
            switch (number)
            {
                case 1:
                    return MakeWireField1(gameObject, wirePef, fieldPref, cameraTransform, start, end, line);
            }

            return null;
        }

        private static WireField MakeWireField1(GameObject gameObject, WireGUIPref wirePef, FieldGUIPref fieldPref,
            Transform camera, GameObject start, GameObject end, GameObject line)
        {
            var field = gameObject.AddComponent<WireField>();
            field.transform.position = camera.position;
            field.cameraTransform = camera;
            field.WirePrefab = wirePef;
            field.fieldGUIPrefab = fieldPref;
            field.linePref = line;

            var stringField = new[]
            {
                "-/-/",
                "----",
                "///-"
            };
            var scale = 6.25f;
            var shiftX = (stringField[0].Length - 1) / 2.0f;
            var shiftY = (stringField.Length - 1) / 2.0f;
            var shift = new Vector2(shiftX, -shiftY);
            field.startPosition1 = (0, 0);
            field.startPosition2 = (0, 2);
            field.currentPosition2 = field.startPosition2;
            field.currentPosition1 = field.startPosition1;
            field.endPosition = (1, 3);
            field.StartPointPref = start;
            field.EndPointPref = end;
            field.SetField(stringField, scale, shift);
            return field;
        }
    }
}