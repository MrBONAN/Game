using System;
using System.Collections.Generic;
using System.Linq;
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
                "?--?",
                "////"
            };
            var bridgesRotations = new Dictionary<(int, int), (Rotation, Rotation)>
            {
                { (1, 0), (Rotation.Degree270, Rotation.Degree90) },
                { (1, 3), (Rotation.Degree270, Rotation.Degree90) }
            };
            var scale = 6.2f;
            var bridgePositions = stringField
                .SelectMany((row, y) => row.Select((c, x) => new { Char = c, Coord = (y, x) }))
                .Where(item => item.Char == '?')
                .Select(item => item.Coord)
                .ToArray();
            var shiftX = (stringField[0].Length - 1) / 2.0f;
            var shiftY = (stringField.Length - 1) / 2.0f;
            var shift = new Vector2(shiftX, -shiftY);
            field.startPosition1 = (0, 0);
            field.startPosition2 = (0, 2);
            field.currentPosition2 = field.startPosition2;
            field.currentPosition1 = field.startPosition1;
            field.endPosition = (2, 3);
            field.StartPointPref = start;
            field.EndPointPref = end;
            field.SetField(stringField, scale, shift, bridgePositions, bridgesRotations);
            return field;
        }

        private static WireField MakeField2(GameObject gameObject, WireGUIPref wirePef, FieldGUIPref fieldPref,
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
                "--/--/--",
                "-?//-/?-",
                "-/--/-/-",
                "-////-/",
                "-/-?///-",
                "///-////",
                "-/--/-/-",
                "----?-/-",
            };
            var bridgesRotations = new Dictionary<(int, int), (Rotation, Rotation)>
            {
                { (1, 1), (Rotation.Degree270, Rotation.Degree90) },
                { (4, 3), (Rotation.Degree180, Rotation.Normal) },
                { (8, 5), (Rotation.Degree180, Rotation.Normal)},
                { (1, 7), (Rotation.Degree180, Rotation.Normal)}
            };
            var scale = 3.5f;
            var bridgePositions = stringField
                .SelectMany((row, y) => row.Select((c, x) => new { Char = c, Coord = (y, x) }))
                .Where(item => item.Char == '?')
                .Select(item => item.Coord)
                .ToArray();
            var shiftX = (stringField[0].Length - 1) / 2.0f;
            var shiftY = (stringField.Length - 1) / 2.0f;
            var shift = new Vector2(shiftX, -shiftY);
            field.startPosition1 = (0, 0);
            field.startPosition2 = (8, 5);
            field.currentPosition2 = field.startPosition2;
            field.currentPosition1 = field.startPosition1;
            field.endPosition = (3, 8);
            field.StartPointPref = start;
            field.EndPointPref = end;
            field.SetField(stringField, scale, shift, bridgePositions, bridgesRotations);
            return field;
        }
    }
}