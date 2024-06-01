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

        public void RotateWire()
        {
            rotation = GetNewDirection(GetNextRotation(rotation.Item1));

            WireGUI.ChangeRotation();
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
                case Rotation.Degree270:
                    WireGUI.ChangeRotation();
                    break;
                case Rotation.Normal:
                    for (var i = 0; i < 2; i++)
                        WireGUI.ChangeRotation();
                    break;
                case Rotation.Degree90:
                    for (var i = 0; i < 3; i++)
                        WireGUI.ChangeRotation();
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
                WireType.Corner or WireType.LongCorner => (firstRotation, GetNextRotation(firstRotation)),
                _ => throw new ArgumentException()
            };
        }
    }
}