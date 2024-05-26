using System;
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
    
    public class Wire
    {
        public Vector2Int Entry;
        public Vector2Int Exit;
        private WireType _type;
        public Rotation rotation;
        private WireGUI WireGUI = new WireGUI();

        public WireType Type
        {
            get => _type;
            set
            {
                _type = value;
                WireGUI.type = value;
            }
        }

        public Rotation Rotation
        {
            get => rotation;
            set
            {
                rotation = value;
                WireGUI.rotation = value;
            }
        }

        public Vector2Int Position
        {
            get => Entry;
            set
            {
                Entry = value;
                Exit = value + Rotate(rotation, _type);
                WireGUI.position = Entry;
            }
        }

        public void RotateWire()
        {
            switch (rotation)
            {
                case Rotation.Normal:
                    rotation = Rotation.Degree90;
                    break;
                case Rotation.Degree90:
                    rotation = Rotation.Degree180;
                    break;
                case Rotation.Degree180:
                    rotation = Rotation.Degree270;
                    break;
                case Rotation.Degree270:
                    rotation = Rotation.Normal;
                    break;
            }
            Exit = Entry + Rotate(rotation, _type);
            WireGUI.rotation = rotation;
        }

        private static Vector2Int Rotate(Rotation rotation, WireType type)
        {
            var sizes = new Dictionary<WireType, (int Width, int Height)>()
            {
                { WireType.Default, (5, 0) },
                { WireType.Long, (7, 0) },
                { WireType.Corner, (3, 3) },
                { WireType.LongCorner, (8, 5) },
                { WireType.Bridge, (4, 0) }
            };
            return rotation switch
            {
                Rotation.Normal => new Vector2Int(sizes[type].Width, sizes[type].Height),
                Rotation.Degree90 => new Vector2Int(sizes[type].Height, sizes[type].Width),
                Rotation.Degree180 => new Vector2Int(-sizes[type].Width, sizes[type].Height),
                Rotation.Degree270 => new Vector2Int(sizes[type].Height, -sizes[type].Width),
                _ => new Vector2Int()
            };
        }
    }
}