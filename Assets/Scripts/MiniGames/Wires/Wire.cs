using System;
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
        private WireType _type;
        public (Rotation, Rotation) rotation;
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

        public void RotateWire(float duration)
        {
            rotation = GetNewDirection(GetNextRotation(rotation.Item1));

            WireGUI.ChangeRotation(duration);
        }
        
        public void SetRandomRotation()
        {
            var random = new System.Random();
            Rotation[] rotations = (Rotation[])Enum.GetValues(typeof(Rotation));
            var rotationLocal = rotations[random.Next(0, rotations.Length)];
            
            rotation = GetNewDirection(rotationLocal); // Gets random rotation
            
            MakeWireRotation(); // Подгоняет интерфейс по этот рандом
        }
        
        private void MakeWireRotation()
        {
            switch (rotation.Item1)
            {
                case Rotation.Degree90:
                    WireGUI.ChangeRotation(0);
                    break;
                case Rotation.Degree180:
                    for (var i = 0; i < 2; i++)
                        WireGUI.ChangeRotation(0);
                    break;
                case Rotation.Degree270:
                    for (var i = 0; i < 3; i++)
                        WireGUI.ChangeRotation(0);
                    break;
            }
        }

        private Rotation GetNextRotation(Rotation currentRotation)
        {
            return currentRotation switch
            {
                Rotation.Normal => Rotation.Degree90,
                Rotation.Degree90 => Rotation.Degree180,
                Rotation.Degree180 => Rotation.Degree270,
                Rotation.Degree270 => Rotation.Normal,
                _ => throw new ArgumentException()
            };
        }

        private (Rotation, Rotation) GetNewDirection(Rotation firstRotation) // Восстановление второго аргумента у rotation
        {
            return _type switch
            {
                WireType.Bridge or WireType.Long or WireType.Default => (firstRotation,
                    GetNextRotation(GetNextRotation(firstRotation))),
                WireType.Corner or WireType.LongCorner => (firstRotation, GetNextRotationCorner(firstRotation)),
                _ => throw new ArgumentException()
            };
        }

        private Rotation GetNextRotationCorner(Rotation rotation)
        {
            return rotation switch
            {
                Rotation.Normal => Rotation.Degree270,
                Rotation.Degree90 => Rotation.Normal,
                Rotation.Degree180 => Rotation.Degree90,
                Rotation.Degree270 => Rotation.Degree180,
                _ => throw new ArgumentException()
            };
        }

        public void ChangeDirection()
        {
            if (_type == WireType.Bridge || _type == WireType.Long || _type == WireType.Default) return;
            rotation = (rotation.Item2, rotation.Item1);
            WireGUI.ChangeRotationSides();
        }
    }
}