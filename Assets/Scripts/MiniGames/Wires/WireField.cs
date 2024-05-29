using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace MazeMiniGame
{
    public enum Player
    {
        First,
        Second
    }

    public enum Direction
    {
        Right,
        Left,
        Up,
        Down
    }

    public class WireField : MonoBehaviour
    {
        public Wire[,] wireField;
        public (int yPos, int xPos) currentPosition1;
        public (int yPos, int xPos) currentPosition2;
        public (int yPos, int xPos) startPosition1;
        public (int yPos, int xPos) startPosition2;
        public (int yPos, int xPos) endPosition;
        public FieldGUIPref fieldGUIPrefab;
        public WireGUIPref WirePrefab;
        public WireFieldGUI field;
        public Transform cameraTransform;

        public int Width => wireField.GetLength(1);
        public int Height => wireField.GetLength(0);

        public bool CheckWin()
        {
            return RecFind(startPosition1, startPosition2, new HashSet<(int xPos, int yPos)>(), Player.First) &&
                   RecFind(startPosition2, endPosition, new HashSet<(int xPos, int yPos)>(), Player.Second);
        }

        private bool RecFind((int posY, int posX) currentPos, (int posY, int posX) toFind,
            HashSet<(int xPos, int yPos)> visited, Player player)
        {
            if (IsInBounds(currentPos.posX, currentPos.posY, player))
                return false;

            visited.Add(currentPos);

            if (currentPos.posX == toFind.posX && currentPos.posY == toFind.posY)
                return true;

            var goneRight =
                !visited.Contains((currentPos.posX + 1, currentPos.posY)) &&
                IsInBounds(currentPos.posX + 1, currentPos.posY, player) &&
                wireField[currentPos.posY, currentPos.posX].Exit ==
                wireField[currentPos.posY, currentPos.posX + 1].Entry &&
                RecFind((currentPos.posY, currentPos.posX + 1), toFind, visited, player);
            var goneUp =
                !visited.Contains((currentPos.posX, currentPos.posY + 1)) &&
                IsInBounds(currentPos.posX, currentPos.posY + 1, player) &&
                wireField[currentPos.posY, currentPos.posX].Exit ==
                wireField[currentPos.posY + 1, currentPos.posX].Entry &&
                RecFind((currentPos.posY + 1, currentPos.posX), toFind, visited, player);
            var goneLeft =
                !visited.Contains((currentPos.posX - 1, currentPos.posY)) &&
                IsInBounds(currentPos.posX - 1, currentPos.posY, player) &&
                wireField[currentPos.posY, currentPos.posX].Exit ==
                wireField[currentPos.posY, currentPos.posX - 1].Entry &&
                RecFind((currentPos.posY, currentPos.posX - 1), toFind, visited, player);
            var goneDown =
                !visited.Contains((currentPos.posX, currentPos.posY - 1)) &&
                IsInBounds(currentPos.posX, currentPos.posY - 1, player) &&
                wireField[currentPos.posY, currentPos.posX].Exit ==
                wireField[currentPos.posY - 1, currentPos.posX].Entry &&
                RecFind((currentPos.posY - 1, currentPos.posX), toFind, visited, player);

            return goneLeft || goneRight || goneUp || goneDown;
        }

        public void RotateWire1()
        {
            wireField[currentPosition1.yPos, currentPosition1.xPos].RotateWire();
        }

        public void RotateWire2()
        {
            wireField[currentPosition2.yPos, currentPosition2.xPos].RotateWire();
        }

        public void DestroyWires()
        {
            for (var i = 0; i < Height; i++)
            for (var j = 0; j < Width; j++)
                Destroy(wireField[i, j].WireGUI);
        }

        private bool IsInBounds(int x, int y, Player player)
        {
            if (y < 0 || y >= Height
                      || x < (player == Player.First ? 0 : Width / 2)
                      || x >= (player == Player.First ? Width / 2 : Width))
                return false;
            return true;
        }

        public void SetField(string[] fieldInfo, string[] positionsReal, string[] positionalsGUI, float[] scales)
        {
            wireField = new Wire[fieldInfo.Length, fieldInfo[0].Length];
            WirePrefab.SetScales(scales[0], scales[1], scales[2], scales[3], scales[4]);

            field = gameObject.AddComponent<WireFieldGUI>();
            field.fieldPrefab = fieldGUIPrefab.fieldPrefab;
            field.position = fieldGUIPrefab.position;
            for (var i = 0; i < fieldInfo.Length; i++)
            {
                var posReal = positionsReal[i].Split("|").Select(x => x.Split(' ').ToArray()).ToArray();
                var posGUI = positionalsGUI[i].Split("|").Select(x => x.Split(' ').ToArray()).ToArray();
                for (var j = 0; j < fieldInfo[0].Length; j++)
                {
                    AddNewWire(fieldInfo[i][j], i, j, float.Parse(posReal[j][0]), float.Parse(posReal[j][1]),
                        float.Parse(posGUI[j][0], CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(posGUI[j][1], CultureInfo.InvariantCulture.NumberFormat));
                }
            }

            wireField[startPosition1.yPos, startPosition1.xPos].WireGUI.Visualize();
            wireField[startPosition2.yPos, startPosition2.xPos].WireGUI.Visualize();
        }

        private void AddNewWire(char wireType, int i, int j, float realPosX, float realPosY, float GUIPosX,
            float GUIPosY)
        {
            var wire = gameObject.AddComponent<Wire>();
            wire.WireGUI = gameObject.AddComponent<WireGUI>();
            wire.WireGUI.objectSizes = WirePrefab.objectsRenderer;
            SetWireGUIPref(wire.WireGUI);
            wire.Type = WiresStateFormer.translateDict[wireType]; // set type to wire
            wire.WireGUI.SetGUIPosition(new Vector2(GUIPosX, GUIPosY));
            wire.WireGUI.DrawWire(field.transform);
            wire.SetRandomRotation(); // DrawWire дает возможность вращать обьект
            wire.Position = new Vector2(realPosX, realPosY); // set entry and exit to wire
            wireField[i, j] = wire;
        }

        private void SetWireGUIPref(WireGUI wire)
        {
            wire.defaultPrefab = WirePrefab.defaultPrefab;
            wire.longPrefab = WirePrefab.longPrefab;
            wire.cornerPrefab = WirePrefab.cornerPrefab;
            wire.longCornerPrefab = WirePrefab.longCornerPrefab;
            wire.bridgePrefab = WirePrefab.bridgePrefab;
        }

        public void GoToFirst(Direction direction)
        {
            UnVisualize(wireField[currentPosition1.yPos, currentPosition1.xPos]);
            switch (direction)
            {
                case Direction.Right:
                    currentPosition1.xPos++;
                    break;
                case Direction.Left:
                    currentPosition1.xPos--;
                    break;
                case Direction.Up:
                    currentPosition1.yPos--;
                    break;
                case Direction.Down:
                    currentPosition1.yPos++;
                    break;
            }

            Visualize(wireField[currentPosition1.yPos, currentPosition1.xPos]);
        }

        public void GoToSecond(Direction direction)
        {
            UnVisualize(wireField[currentPosition2.yPos, currentPosition2.xPos]);
            switch (direction)
            {
                case Direction.Right:
                    currentPosition2.xPos++;
                    break;
                case Direction.Left:
                    currentPosition2.xPos--;
                    break;
                case Direction.Up:
                    currentPosition2.yPos--;
                    break;
                case Direction.Down:
                    currentPosition2.yPos++;
                    break;
            }

            Visualize(wireField[currentPosition2.yPos, currentPosition2.xPos]);
        }

        private void Visualize(Wire wire)
        {
            wire.WireGUI.Visualize();
        }

        private void UnVisualize(Wire wire)
        {
            wire.WireGUI.UnVisualize();
        }
    }
}