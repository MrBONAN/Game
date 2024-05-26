using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using MiniGames;
using UnityEditor;

namespace MazeMiniGame
{
    public class WiresGameObject : MiniGame
    {
        private WireField wireField;
        
        private static Dictionary<MoveDirection, KeyCode> player1Control = new()
        {
            { MoveDirection.Up, KeyCode.UpArrow },
            { MoveDirection.Down, KeyCode.DownArrow },
            { MoveDirection.Left, KeyCode.LeftArrow },
            { MoveDirection.Right, KeyCode.RightArrow }
        };

        private static Dictionary<MoveDirection, KeyCode> player2Control = new()
        {
            { MoveDirection.Up, KeyCode.W },
            { MoveDirection.Down, KeyCode.S },
            { MoveDirection.Left, KeyCode.A },
            { MoveDirection.Right, KeyCode.D }
        };
        public override void StartMiniGame()
        {
            Debug.Log("WireMiniGame started");
        }
        
        public override MiniGameResult UpdateMiniGame()
        {
            foreach (var (keyCode, pressed) in player1Control)
            {
                if (Input.GetKeyDown(pressed))
                    MovePosition1(keyCode);
            }
            
            foreach (var (keyCode, pressed) in player2Control)
            {
                if (Input.GetKeyDown(pressed))
                    MovePosition2(keyCode);
            }

            if (Input.GetKeyDown(KeyCode.KeypadEnter))
                wireField.RotateWire1();

            if (Input.GetKeyDown(KeyCode.E))
                wireField.RotateWire2();
                
            if (wireField.CheckWin())
                return MiniGameResult.Win;
            
            return MiniGameResult.ContinuePlay;
        }

        public override void OnDestroy()
        {
            //ToDo destroy objects created
        }

        public void SetGameState()
        {
            
        }

        private void MovePosition1(MoveDirection direction)
        {
            switch (direction)
            {
                case MoveDirection.Up:
                    if (IsInBorders1(wireField.currentPosition1.xPos, wireField.currentPosition1.yPos+1))
                        wireField.currentPosition1.yPos++;
                    break;
                case MoveDirection.Down:
                    if (IsInBorders1(wireField.currentPosition1.xPos, wireField.currentPosition1.yPos-1))
                        wireField.currentPosition1.yPos--;
                    break;
                case MoveDirection.Right:
                    if (IsInBorders1(wireField.currentPosition1.xPos+1, wireField.currentPosition1.yPos))
                        wireField.currentPosition1.xPos++;
                    break;
                case MoveDirection.Left:
                    if (IsInBorders1(wireField.currentPosition1.xPos-1, wireField.currentPosition1.yPos))
                        wireField.currentPosition1.xPos--;
                    break;
            }
        }
        
        private void MovePosition2(MoveDirection direction)
        {
            switch (direction)
            {
                case MoveDirection.Up:
                    if (IsInBorders2(wireField.currentPosition2.xPos, wireField.currentPosition2.yPos+1))
                        wireField.currentPosition2.yPos++;
                    break;
                case MoveDirection.Down:
                    if (IsInBorders2(wireField.currentPosition2.xPos, wireField.currentPosition2.yPos-1))
                        wireField.currentPosition2.yPos--;
                    break;
                case MoveDirection.Right:
                    if (IsInBorders2(wireField.currentPosition2.xPos+1, wireField.currentPosition2.yPos))
                        wireField.currentPosition2.xPos++;
                    break;
                case MoveDirection.Left:
                    if (IsInBorders2(wireField.currentPosition2.xPos-1, wireField.currentPosition2.yPos))
                        wireField.currentPosition2.xPos--;
                    break;
            }
        }

        private bool IsInBorders1(int xPos, int yPos)
        {
            return xPos >= 0 && yPos >= 0 && xPos < wireField.Width/2 && yPos < wireField.Height/2;
        }
        
        private bool IsInBorders2(int xPos, int yPos)
        {
            return xPos >= wireField.Width/2 && yPos >= wireField.Width/2 && xPos < wireField.Width && yPos < wireField.Height;
        }
    }
}