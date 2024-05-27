using System.Collections.Generic;
using UnityEngine;

namespace MazeMiniGame
{
    public enum Player
    {
        First,
        Second
    }

    public class WireField : MonoBehaviour
    {
        public Wire[,] wireField;
        public (int xPos, int yPos) currentPosition1;
        public (int xPos, int yPos) currentPosition2;
        public (int xPos, int yPos) startPosition1;
        public (int xPos, int yPos) startPosition2;
        public (int xPos, int yPos) endPosition;
        public WireFieldGUI fieldGUI;

        public int Width => wireField.GetLength(1);
        public int Height => wireField.GetLength(0);

        public bool CheckWin()
        {
            return RecFind(startPosition1, startPosition2, new HashSet<(int xPos, int yPos)>(), Player.First) &&
                   RecFind(startPosition2, endPosition, new HashSet<(int xPos, int yPos)>(), Player.Second);
        }

        private bool RecFind((int posX, int posY) currentPos, (int posX, int posY) toFind,
            HashSet<(int xPos, int yPos)> visited, Player player)
        {
            if (currentPos.posY < 0 || currentPos.posY >= Height
                                    || currentPos.posX < (player==Player.First? 0 : Width/2)
                                    || currentPos.posX >= (player==Player.First? Width/2 : Width))
                return false;

            visited.Add(currentPos);

            if (currentPos.posX == toFind.posX && currentPos.posY == toFind.posY)
                return true;
            
            var goneRight =
                !visited.Contains((currentPos.posX + 1, currentPos.posY)) &&
                wireField[currentPos.posX, currentPos.posY] == wireField[currentPos.posX + 1, currentPos.posY] &&
                RecFind((currentPos.posX + 1, currentPos.posY), toFind, visited, player);
            var goneUp =
                !visited.Contains((currentPos.posX, currentPos.posY + 1)) &&
                wireField[currentPos.posX, currentPos.posY] == wireField[currentPos.posX, currentPos.posY + 1] &&
                RecFind((currentPos.posX, currentPos.posY + 1), toFind, visited, player);
            var goneLeft =
                !visited.Contains((currentPos.posX - 1, currentPos.posY)) &&
                wireField[currentPos.posX, currentPos.posY] == wireField[currentPos.posX - 1, currentPos.posY] &&
                RecFind((currentPos.posX - 1, currentPos.posY), toFind, visited, player);
            var goneDown =
                !visited.Contains((currentPos.posX, currentPos.posY - 1)) &&
                wireField[currentPos.posX, currentPos.posY] == wireField[currentPos.posX, currentPos.posY - 1] &&
                RecFind((currentPos.posX, currentPos.posY - 1), toFind, visited, player);

            return goneLeft || goneRight || goneUp || goneDown;
        }

        public void RotateWire1()
        {
            wireField[currentPosition1.xPos, currentPosition1.yPos].RotateWire();
        }

        public void RotateWire2()
        {
            wireField[currentPosition2.xPos, currentPosition2.yPos].RotateWire();
        }

        public void DestroyWires()
        {
            for (var i = 0; i < this.Width; i++)
            for (var j = 0; j < this.Height; j++)
                Destroy(wireField[i, j].WireGUI);
        }

        public void SetField(string[] fieldInfo, string[] positions)
        {
            wireField = new Wire[fieldInfo.Length,fieldInfo[0].Length];
            fieldGUI = gameObject.AddComponent<WireFieldGUI>();
            fieldGUI.DrawField();
            for (var i = 0; i < fieldInfo.Length; i++)
            {
                var pos = positions[i].Split("|");
                for (var j = 0; j < fieldInfo[0].Length; j++)
                {
                    var wire = new Wire
                    {
                        Type = WiresStateFormer.translateDict[fieldInfo[j][i]], // set type to wire
                        Position = new Vector2Int(pos[j][0], pos[j][3]) // set entry and exit to wire
                    };
                    wireField[i, j] = wire;
                    wire.WireGUI.DrawWire(fieldGUI.transform);
                }
            }
        }
    }
}