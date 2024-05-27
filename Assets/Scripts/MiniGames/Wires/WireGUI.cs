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
    public class WireGUI : MonoBehaviour
    {
        public WireType type;
        public Vector2Int position;
        public Rotation rotation;
        public GameObject gameObj;


        public void ChangeRotation(Rotation rotation)
        {
            this.rotation = rotation;
        }
    }
}