using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MazeMiniGame
{
    public enum WireType
    {
        Default,
        Long,
        Corner,
        LongCorner,
        Bridge
    }
    
    public class Wire : MonoBehaviour
    {
        public Vector2 Entry;
        public Vector2 Exit;
        private WireType _type;
        public Rotation rotation;
        public WireGUI WireGUI;

        public WireType Type
        {
            get => _type;
            set
            {
                _type = value;
                WireGUI.type = value;
            }
        }

        public Vector2 Position
        {
            get => Entry;
            set
            {
                Entry = value;
                Exit = value + Rotate(rotation, _type);
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
            WireGUI.ChangeRotation(rotation); 
        }

        private static Vector2 Rotate(Rotation rotation, WireType type)
        {
            return rotation switch
            {
                Rotation.Normal => new Vector2(WiresStateFormer.sizes[type].Width, -WiresStateFormer.sizes[type].Height),
                Rotation.Degree90 => new Vector2(WiresStateFormer.sizes[type].Height, -WiresStateFormer.sizes[type].Width),
                Rotation.Degree180 => new Vector2(WiresStateFormer.sizes[type].Width, -WiresStateFormer.sizes[type].Height),
                Rotation.Degree270 => new Vector2(WiresStateFormer.sizes[type].Height, -WiresStateFormer.sizes[type].Width),
                _ => new Vector2()
            };
        }
        
        public void SetRandomRotation()
        {
            var random = new System.Random();
            Rotation[] rotations = (Rotation[])Enum.GetValues(typeof(Rotation));
            var rotationLocal = rotations[random.Next(0, rotations.Length)];
            rotation = rotationLocal;
            WireGUI.rotation = rotationLocal;
            
            DrawWireRotation();
        }
        
        private void DrawWireRotation()
        {
            if (rotation == Rotation.Degree90)
                RotateWire();
            else if (rotation == Rotation.Degree180)
                for (var i = 0; i < 2; i++)
                    RotateWire();
            else if (rotation == Rotation.Degree270)
                for (var i = 0; i < 2; i++)
                    RotateWire();
        }
    }
}