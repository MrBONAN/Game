using Unity.VisualScripting;
using UnityEngine;

namespace MazeMiniGame
{
    public class WireField : MonoBehaviour
    {
        public Wire[,] wireField;
        public (int xPos, int yPos) currentPosition1;
        public (int xPos, int yPos) currentPosition2;
        public int scaleX;
        public int scaleY;

        public int Width => wireField.GetLength(1);
        public int Height => wireField.GetLength(0);

        public bool CheckWin()
        {
            return false;
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
            for (var i = 0; i < fieldInfo.Length; i++)
            {
                var pos = positions[i].Split("|");
                for (var j = 0; j < fieldInfo[0].Length; j++)
                {
                    var wire = new Wire();
                    wire.Type = WiresStateFormer.translateDict[fieldInfo[j][i]];
                    wire.Entry = new Vector2Int(j * pos[j][0], i * pos[j][3]);

                }
            }
        }
    }
}