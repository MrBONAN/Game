using System.Numerics;
using UnityEngine;

namespace MazeMiniGame
{
    public enum Rotation
    {
        Normal,
        Degree90,
        Degree180,
        Degree270
    }
    public class WireGUI
    {
        public WireType Type;
        public Vector2Int Position;
        public Rotation Rotation;

        public void Draw()
        {
            
        }
    }
}