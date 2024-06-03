using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MiniGames;
using UnityEditor;
using UnityEditor.Compilation;
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
        public GameObject linePref;
        public WireFieldGUI field;
        public Transform cameraTransform;
        public GameObject StartPointPref;
        public GameObject EndPointPref;
        public HashSet<GameObject> points = new HashSet<GameObject>();
        private GameObject line;
        private (int yPos, int xPos)[] bridgesPos;
        private (int yPos, int xPos)[] way;

        public int Width => wireField.GetLength(1);
        public int Height => wireField.GetLength(0);

        public void CheckWin(WiresGameObject obj)
        {
            var firstPath = RecFind(startPosition1, startPosition2, new List<(int posY, int posX)>());
            var secondPath = RecFind(startPosition2, endPosition, new List<(int posX, int posY)>());
            if (firstPath.Item1 && secondPath.Item1 && CheckIsOutOfMap(endPosition.yPos, endPosition.xPos))
            {
                var win = true;
                var allWay = firstPath.Item2.Concat(secondPath.Item2).ToArray();
                way = allWay;
                foreach (var pos in bridgesPos)
                    if (!allWay.Contains(pos))
                        win = false;

                if (win)
                    obj.gameWon = true;
            }
        }

        private (bool, List<(int, int)>) RecFind((int posY, int posX) currPos, (int posY, int posX) toFind,
            List<(int posY, int posX)> visited)
        {
            visited.Add(currPos);

            if (currPos.posX == toFind.posX && currPos.posY == toFind.posY)
                return (true, visited);

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

                    return (false, visited);
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

                    return (false, visited);
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

                    return (false, visited);
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

                    return (false, visited);
            }

            throw new ArgumentException();
        }

        public void RotateWire1()
        {
            if (!wireField[currentPosition1.yPos, currentPosition1.xPos].WireGUI.rotating &&
                wireField[currentPosition1.yPos, currentPosition1.xPos].Type != WireType.Bridge)
                wireField[currentPosition1.yPos, currentPosition1.xPos].RotateWire(0.4f);
        }

        public void RotateWire2()
        {
            if (!wireField[currentPosition2.yPos, currentPosition2.xPos].WireGUI.rotating &&
                wireField[currentPosition2.yPos, currentPosition2.xPos].Type != WireType.Bridge)
                wireField[currentPosition2.yPos, currentPosition2.xPos].RotateWire(0.4f);
        }

        public void DestroyWires()
        {
            for (var i = 0; i < Height; i++)
            for (var j = 0; j < Width; j++)
                Destroy(wireField[i, j].WireGUI.gameObj);
            WirePrefab.objectsRenderer.Clear();
        }

        public void SetField(string[] fieldInfo, float scale, Vector2 shift, (int yPos, int xPos)[] bridges,
            Dictionary<(int, int), (Rotation, Rotation)> bridgeRotations, float scaleDist)
        {
            bridgesPos = bridges;
            wireField = new Wire[fieldInfo.Length, fieldInfo[0].Length];
            WirePrefab.SetScales(scale);
            SetPointScales(new Vector2(scale, scale));

            field = gameObject.AddComponent<WireFieldGUI>();
            field.fieldPrefab = fieldGUIPrefab.fieldPrefab;
            for (var i = 0; i < Height; i++)
            {
                for (var j = 0; j < Width; j++)
                {
                    AddNewWire(fieldInfo[i][j], i, j, scaleDist, shift);
                }
            }

            RotateBridges(bridgeRotations);
            DrawPoints(scaleDist, shift);
            DrawLine(scaleDist, shift);
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
            wire.WireGUI.SetGUIPosition((new Vector2(j, -i) - shift) * scale); // тут вместо i и j надо да
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
            wire.backCornerPref = WirePrefab.backCornerPref;
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

        private void DrawPoints(float dist, Vector2 shift)
        {
            var start1 = Instantiate(StartPointPref, field.transform);
            start1.transform.localPosition = (new Vector2(startPosition1.xPos, -startPosition1.yPos) - shift) * dist;
            var start2 = Instantiate(StartPointPref, field.transform);
            start2.transform.localPosition = (new Vector2(startPosition2.xPos, -startPosition2.yPos) - shift) * dist;
            var end = Instantiate(EndPointPref, field.transform);
            end.transform.localPosition = (new Vector2(endPosition.xPos, -endPosition.yPos) - shift) * dist;
            points.Add(start1);
            points.Add(start2);
            points.Add(end);
        }

        private void SetPointScales(Vector2 scale)
        {
            StartPointPref.transform.localScale = scale;
            EndPointPref.transform.localScale = scale;
        }

        private void DrawLine(float dist, Vector2 shift)
        {
            line = Instantiate(linePref, field.transform);
            line.transform.localPosition = new Vector2();
        }

        public void ChangeRotation1()
        {
            wireField[currentPosition1.yPos, currentPosition1.xPos].ChangeDirection();
        }

        public void ChangeRotation2()
        {
            wireField[currentPosition2.yPos, currentPosition2.xPos].ChangeDirection();
        }

        private void RotateBridges(Dictionary<(int, int), (Rotation, Rotation)> bridges)
        {
            foreach (var pair in bridges)
            {
                var coordinates = pair.Key;
                var rotations = pair.Value;
                wireField[coordinates.Item1, coordinates.Item2].SetRotationToBridge(rotations);
            }
        }

        public IEnumerator WinAnimation(WiresGameObject obj)
        {
            wireField[currentPosition2.yPos, currentPosition2.xPos].WireGUI.UnVisualize();
            wireField[currentPosition1.yPos, currentPosition1.xPos].WireGUI.UnVisualize();
            foreach (var point in points)
                Destroy(point);
            Destroy(line);

            foreach (var (y, x) in way)
            {
                wireField[y, x].HighLight();
                yield return new WaitForSeconds(0.2f);
            }

            
            obj.animated = true;
        }
        

        private bool CheckIsOutOfMap(int posY, int posX)
        {
            switch (wireField[posY, posX].rotation.Item2)
            {
                case Rotation.Degree90:
                    return !IsInBounds(posY + 1, posX);
                case Rotation.Degree180:
                    return !IsInBounds(posY, posX - 1);
                case Rotation.Normal:
                    return !IsInBounds(posY, posX + 1);
                case Rotation.Degree270:
                    return IsInBounds(posY - 1, posX);
            }

            throw new AggregateException();
        }
    }
}