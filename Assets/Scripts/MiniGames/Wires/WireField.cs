using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MiniGames;
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
            if (RecFind(startPosition1, startPosition2, new HashSet<(int posX, int posY)>()) &&
                RecFind(startPosition2, endPosition, new HashSet<(int posX, int posY)>()))
                return true;

            return false;
        }

        private bool RecFind((int posX, int posY) currPos, (int posY, int posX) toFind,
            HashSet<(int posX, int posY)> visited)
        {
            visited.Add(currPos);
            
            if (currPos.posX == toFind.posX && currPos.posY == toFind.posY)
                return true;

            switch (wireField[currPos.posY, currPos.posX].rotation.Item2)
            {
                case Rotation.Degree90:
                    if (!visited.Contains((currPos.posY + 1, currPos.posX)) &&
                        IsInBounds(currPos.posY + 1, currPos.posX) &&
                        AreConnected(
                        wireField[currPos.posY + 1, currPos.posX].rotation.Item1,
                        wireField[currPos.posY, currPos.posX].rotation.Item2))
                    {
                        currPos.posY++;
                        return RecFind(currPos, toFind, visited);
                    }

                    return false;
                case Rotation.Degree180:
                    if (!visited.Contains((currPos.posY, currPos.posX - 1)) &&
                        IsInBounds(currPos.posY, currPos.posX - 1) &&
                        AreConnected(
                        wireField[currPos.posY, currPos.posX - 1].rotation.Item1,
                        wireField[currPos.posY, currPos.posX].rotation.Item2))
                    {
                        currPos.posX--;
                        return RecFind(currPos, toFind, visited);
                    }

                    return false;
                case Rotation.Normal:
                    if (!visited.Contains((currPos.posY, currPos.posX + 1)) &&
                        IsInBounds(currPos.posY, currPos.posX + 1) &&
                        AreConnected(
                        wireField[currPos.posY, currPos.posX + 1].rotation.Item1,
                        wireField[currPos.posY, currPos.posX].rotation.Item2))
                    {
                        currPos.posX++;
                        return RecFind(currPos, toFind, visited);
                    }

                    return false;
                case Rotation.Degree270:
                    if (!visited.Contains((currPos.posY - 1, currPos.posX)) &&
                        IsInBounds(currPos.posY - 1, currPos.posX) &&
                        AreConnected(
                        wireField[currPos.posY - 1, currPos.posX].rotation.Item1,
                        wireField[currPos.posY, currPos.posX].rotation.Item2))
                    {
                        currPos.posY--;
                        return RecFind(currPos, toFind, visited);
                    }

                    return false;
            }

            throw new ArgumentException();
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

        public void SetField(string[] fieldInfo, float scale, Vector2 shift)
        {
            wireField = new Wire[fieldInfo.Length, fieldInfo[0].Length];
            WirePrefab.SetScales(scale);

            field = gameObject.AddComponent<WireFieldGUI>();
            field.fieldPrefab = fieldGUIPrefab.fieldPrefab;
            field.position = fieldGUIPrefab.position;
            for (var i = 0; i < Height; i++)
            {
                for (var j = 0; j < Width; j++)
                {
                    AddNewWire(fieldInfo[i][j], i, j, 3, shift);
                }
            }

            wireField[startPosition1.yPos, startPosition1.xPos].WireGUI.Visualize();
            wireField[startPosition2.yPos, startPosition2.xPos].WireGUI.Visualize();
        }

        private void AddNewWire(char wireType, int i, int j, float scale, Vector2 shift)
        {
            var wire = gameObject.AddComponent<Wire>();
            wire.WireGUI = gameObject.AddComponent<WireGUI>();
            wire.WireGUI.objectSizes = WirePrefab.objectsRenderer;
            SetWireGUIPref(wire.WireGUI);
            wire.Type = WiresStateFormer.translateDict[wireType]; // set type to wire
            wire.WireGUI.SetGUIPosition((new Vector2(j, -i) - shift) * scale ); // тут вместо i и j надо да
            wire.WireGUI.DrawWire(field.transform);
            wire.SetRandomRotation(); // DrawWire дает возможность вращать обьект
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

        private bool IsInBounds(int posY, int posX)
        {
            return (posY >= 0 && posX >= 0 && posY < Height && posX < Width);
        }

        private bool AreConnected(Rotation rotation1, Rotation rotation2)
        {
            if (rotation1 == Rotation.Normal)
                return rotation2 == Rotation.Degree180;
            if (rotation1 == Rotation.Degree90)
                return rotation2 == Rotation.Degree270;
            if (rotation1 == Rotation.Degree180)
                return rotation2 == Rotation.Normal;
            if (rotation1 == Rotation.Degree270)
                return rotation2 == Rotation.Degree90;
            
            throw new ArgumentException();
        }
    }
}