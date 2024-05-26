namespace MazeMiniGame
{
    public class WireField
    {
        public Wire[,] wireField;
        public (int xPos, int yPos) currentPosition1;
        public (int xPos, int yPos) currentPosition2;

        public int Width => wireField.GetLength(1);
        public int Height => wireField.GetLength(0);

        public bool CheckWin()
        {
            return false;
        }

        public void RotateWire1()
        {
            wireField[currentPosition1.xPos, currentPosition1.yPos].RotateWire();
        }
        
        public void RotateWire2()
        {
            wireField[currentPosition2.xPos, currentPosition2.yPos].RotateWire();
        }
    }
}