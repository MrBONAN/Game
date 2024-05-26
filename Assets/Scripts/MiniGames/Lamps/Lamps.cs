using System;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

namespace LampsMiniGame
{
    public enum ButtonContent
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,

        Red,
        Green,
        Blue,
        Violet,
        Yellow,
        Cyan,
        Orange,
        White,
        
        Default
    }

    public enum Result
    {
        Success,
        Failure,
        Neutral
    }

    public class Lamps
    {
        private ButtonContent[,] control1 = new ButtonContent[2, 4]
        {
            { ButtonContent.One, ButtonContent.Two, ButtonContent.Three, ButtonContent.Four, },
            { ButtonContent.Five, ButtonContent.Six, ButtonContent.Seven, ButtonContent.Eight, }
        };

        private ButtonContent[,] control2 = new ButtonContent[2, 4]
        {
            { ButtonContent.Red, ButtonContent.Green, ButtonContent.Blue, ButtonContent.Violet, },
            { ButtonContent.Yellow, ButtonContent.Cyan, ButtonContent.Orange, ButtonContent.White, }
        };

        public List<(List<ButtonContent>, List<ButtonContent>)> correctSequence;
        private (List<ButtonContent>, List<ButtonContent>) currentSequenceRepeat = (new(), new());
        private int currentLevelRepeat;
        private readonly int levelsCount;
        public int CurrentLevel { get; set; }

        private readonly int levelsDifficulty;
        public (int X, int Y) cursor1 = (0, 0);
        public (int X, int Y) cursor2 = (0, 0);


        public Lamps(int levelsCount = 5, int levelsDifficulty = 1)
        {
            this.levelsCount = Math.Max(1, levelsCount);
            this.levelsDifficulty = Math.Max(1, levelsDifficulty);
            Restart();
        }

        public void MoveCursor(ref (int X, int Y) cursor, Control direction)
        {
            if (direction is Control.Down) cursor = (cursor.X, 1);
            else if (direction is Control.Up) cursor = (cursor.X, 0);
            else if (direction is Control.Left) cursor = (Math.Max(cursor.X - 1, 0), cursor.Y);
            else if (direction is Control.Right) cursor = (Math.Min(cursor.X + 1, 3), cursor.Y);
        }

        public Result PlayerClicked(bool first)
        {
            var control = control1;
            var cursor = cursor1;
            var expectedSequence = correctSequence[currentLevelRepeat].Item1;
            var actualSequence = currentSequenceRepeat.Item1;
            if (!first)
            {
                control = control2;
                cursor = cursor2;
                expectedSequence = correctSequence[currentLevelRepeat].Item2;
                actualSequence = currentSequenceRepeat.Item2;
            }
            var content = control[cursor.Y, cursor.X];
            actualSequence.Add(content);
            if (actualSequence.SequenceEqual(expectedSequence))
                return Result.Success;
            if (actualSequence.Except(expectedSequence).Any())
                return Result.Failure;
            return Result.Neutral;
        }

        private List<(List<ButtonContent>, List<ButtonContent>)> GenerateNewSequence()
        {
            var newSequence = new List<(List<ButtonContent>, List<ButtonContent>)>(levelsCount);
            var r = new Random();
            for (var i = 0; i < levelsCount; i++)
            {
                newSequence.Add(new(new(), new()));
                for (var c = 0; c < levelsDifficulty; c++)
                {
                    newSequence[i].Item1.Add((ButtonContent)r.Next(0, 7)); // генерация цифр
                    newSequence[i].Item2.Add((ButtonContent)r.Next(8, 15)); // генерация цветов
                }
            }

            return newSequence;
        }

        public void Restart()
        {
            correctSequence = GenerateNewSequence();
            currentSequenceRepeat = (new(), new());
            currentLevelRepeat = 0;
        }
    }
}