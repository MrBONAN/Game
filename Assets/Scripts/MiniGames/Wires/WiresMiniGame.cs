using UnityEngine;
using MiniGames;

namespace MazeMiniGame
{
    public class WiresGameObject : MiniGame
    {
        private WireField wireField;
        private bool gameWon = false;
        private bool noGameActions = false;
        
        public override void StartMiniGame()
        {
            Debug.Log("WireMiniGame started");
        }
        
        public override MiniGameResult UpdateMiniGame()
        {
            CheckNoActions();
            
            foreach (var (keyCode, pressed) in PlayerControl.ControlFirst)
            {
                CheckNoActions();
                if (Input.GetKeyDown(pressed) && keyCode is Control.Up or Control.Down or Control.Left or Control.Right)
                    MovePosition1(keyCode);
                if (Input.GetKeyDown(pressed) && keyCode is Control.Use)
                {
                    wireField.RotateWire1();
                    if (wireField.CheckWin())
                        gameWon = true;
                }
            }
            
            foreach (var (keyCode, pressed) in PlayerControl.ControlSecond)
            {
                CheckNoActions();
                if (Input.GetKeyDown(pressed) && keyCode is Control.Up or Control.Down or Control.Left or Control.Right)
                    MovePosition2(keyCode);
                if (Input.GetKeyDown(pressed) && keyCode is Control.Use)
                {
                    wireField.RotateWire2();
                    if (wireField.CheckWin())
                        gameWon = true;
                }
            }
            CheckNoActions();
            
            if (gameWon && noGameActions)
                return MiniGameResult.Win;
            
            return MiniGameResult.ContinuePlay;
        }

        public override void OnDestroy()
        {
            wireField.DestroyWires();
            Destroy(wireField.field);
        }

        public void SetGameState(WireGUIPref wirePref, FieldGUIPref fieldPref, GameObject start, GameObject end)
        {
            wireField = WiresStateFormer.GetLevel(1, gameObject, wirePref, fieldPref, transform, start, end);
        }

        private void MovePosition1(Control direction)
        {
            switch (direction)
            {
                case Control.Up:
                    if (IsInBorders1(wireField.currentPosition1.xPos, wireField.currentPosition1.yPos-1))
                        wireField.GoToFirst(Direction.Up);
                    break;
                case Control.Down:
                    if (IsInBorders1(wireField.currentPosition1.xPos, wireField.currentPosition1.yPos+1))
                        wireField.GoToFirst(Direction.Down);
                    break;
                case Control.Right:
                    if (IsInBorders1(wireField.currentPosition1.xPos+1, wireField.currentPosition1.yPos))
                        wireField.GoToFirst(Direction.Right);
                    break;
                case Control.Left:
                    if (IsInBorders1(wireField.currentPosition1.xPos-1, wireField.currentPosition1.yPos))
                        wireField.GoToFirst(Direction.Left);
                    break;
            }
        }
        
        private void MovePosition2(Control direction)
        {
            switch (direction)
            {
                case Control.Up:
                    if (IsInBorders2(wireField.currentPosition2.xPos, wireField.currentPosition2.yPos-1))
                        wireField.GoToSecond(Direction.Up);
                    break;
                case Control.Down:
                    if (IsInBorders2(wireField.currentPosition2.xPos, wireField.currentPosition2.yPos+1))
                        wireField.GoToSecond(Direction.Down);
                    break;
                case Control.Right:
                    if (IsInBorders2(wireField.currentPosition2.xPos+1, wireField.currentPosition2.yPos))
                        wireField.GoToSecond(Direction.Right);
                    break;
                case Control.Left:
                    if (IsInBorders2(wireField.currentPosition2.xPos-1, wireField.currentPosition2.yPos))
                        wireField.GoToSecond(Direction.Left);
                    break;
            }
        }

        private bool IsInBorders1(int xPos, int yPos)
        {
            return xPos >= 0 && yPos >= 0 && xPos < wireField.Width/2 && yPos < wireField.Height;
        }
        
        private bool IsInBorders2(int xPos, int yPos)
        {
            return xPos >= wireField.Width/2 && yPos >= 0 && xPos < wireField.Width && yPos < wireField.Height;
        }

        private void CheckNoActions()
        {
            if (gameWon)
                if (wireField.NoGameActions())
                    noGameActions = true;
        }
    }
}