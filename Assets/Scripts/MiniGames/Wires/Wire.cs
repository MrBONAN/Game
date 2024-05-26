using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

namespace MazeMiniGame
{
    public enum VisitedState
    {
        Visited,
        Unvisited
    }

    public enum WireType
    {
        Default,
        Long,
        Corner,
        LongCorner,
        Bridge
    }
    
    public Dictionary<WireType, (int Width, int Height)> WireSize;
    
    public class Wire
    {
        public Vector2Int Entry;
        public Vector2Int Exit;
        private WireType _type;
        private WireGUI WireGUI;

        public WireType Type
        {
            get { return _type;}
            set
            {
                _type = value;
                WireGUI.Type = value;
            }
        }

        public void SetPosition(Vector2Int positionOfEntry, Rotation rotation, WireType type)
        {
            
        }
    }
}