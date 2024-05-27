using UnityEngine;
using MiniGames;

namespace MazeMiniGame
{
    public class WiresGameObject : MiniGame
    {
        private WireField wireField;
        
        public override void StartMiniGame()
        {
            Debug.Log("WireMiniGame started");
        }
        
        public override MiniGameResult UpdateMiniGame()
        {
            foreach (var (keyCode, pressed) in PlayerControl.ControlFirst)
            {
                if (Input.GetKeyDown(pressed) && keyCode is Control.Up or Control.Down or Control.Left or Control.Right)
                    MovePosition1(keyCode);
                if (Input.GetKeyDown(pressed) && keyCode is Control.Use)
                    wireField.RotateWire1();
            }
            
            foreach (var (keyCode, pressed) in PlayerControl.ControlSecond)
            {
                if (Input.GetKeyDown(pressed) && keyCode is Control.Up or Control.Down or Control.Left or Control.Right)
                    MovePosition2(keyCode);
                if (Input.GetKeyDown(pressed) && keyCode is Control.Use)
                    wireField.RotateWire2();
            }
                
            if (wireField.CheckWin())
                return MiniGameResult.Win;
            
            return MiniGameResult.ContinuePlay;
        }

        public override void OnDestroy()
        {
            wireField.DestroyWires();
        }

        public void SetGameState()
        {
            wireField = WiresStateFormer.GetLevel(1, gameObject);
        }

        private void MovePosition1(Control direction)
        {
            switch (direction)
            {
                case Control.Up:
                    if (IsInBorders1(wireField.currentPosition1.xPos, wireField.currentPosition1.yPos+1))
                        wireField.currentPosition1.yPos++;
                    break;
                case Control.Down:
                    if (IsInBorders1(wireField.currentPosition1.xPos, wireField.currentPosition1.yPos-1))
                        wireField.currentPosition1.yPos--;
                    break;
                case Control.Right:
                    if (IsInBorders1(wireField.currentPosition1.xPos+1, wireField.currentPosition1.yPos))
                        wireField.currentPosition1.xPos++;
                    break;
                case Control.Left:
                    if (IsInBorders1(wireField.currentPosition1.xPos-1, wireField.currentPosition1.yPos))
                        wireField.currentPosition1.xPos--;
                    break;
            }
        }
        
        private void MovePosition2(Control direction)
        {
            switch (direction)
            {
                case Control.Up:
                    if (IsInBorders2(wireField.currentPosition2.xPos, wireField.currentPosition2.yPos+1))
                        wireField.currentPosition2.yPos++;
                    break;
                case Control.Down:
                    if (IsInBorders2(wireField.currentPosition2.xPos, wireField.currentPosition2.yPos-1))
                        wireField.currentPosition2.yPos--;
                    break;
                case Control.Right:
                    if (IsInBorders2(wireField.currentPosition2.xPos+1, wireField.currentPosition2.yPos))
                        wireField.currentPosition2.xPos++;
                    break;
                case Control.Left:
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