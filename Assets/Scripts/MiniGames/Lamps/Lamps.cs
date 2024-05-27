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
        
        Default,
        Empty
    }

    public enum Result
    {
        Success,
        Correct,
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

        public List<(ButtonContent, ButtonContent)> correctSequence;
        
        private List<ButtonContent> currentSequenceRepeat = new();
        private int currentLevelRepeat;
        public int CurrentLevel { get; private set; } = 1;
        
        private readonly int levelsCount;

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
            var expectedSequence = correctSequence[currentLevelRepeat];
            var content = first ? control1[cursor1.Y, cursor1.X] : control2[cursor2.Y, cursor2.X];
            
            currentSequenceRepeat.Add(content);
            if (currentSequenceRepeat.Count == 2 &&
                currentSequenceRepeat.Contains(expectedSequence.Item1) &&
                currentSequenceRepeat.Contains(expectedSequence.Item2))
                return CorrectGuess();
            if (content != expectedSequence.Item1 && content != expectedSequence.Item2 ||
                currentSequenceRepeat.Count(x => x == content) > 1)
                return WrongGuess();
            return Result.Neutral;
        }

        private Result CorrectGuess()
        {
            currentSequenceRepeat.Clear();
            currentLevelRepeat++;
            if (currentLevelRepeat == CurrentLevel)
            {
                CurrentLevel = currentLevelRepeat + 1;
                currentLevelRepeat = 0;
                return Result.Success;
            }
            return Result.Correct;
        }

        private Result WrongGuess()
        {
            currentSequenceRepeat.Clear();
            currentLevelRepeat = 0;
            CurrentLevel = 1;
            return Result.Failure;
        }

        private List<(ButtonContent, ButtonContent)> GenerateNewSequence()
        {
            var newSequence = new List<(ButtonContent, ButtonContent)>(levelsCount);
            var r = new Random();
            for (var i = 0; i < levelsCount; i++)
                newSequence.Add(((ButtonContent)r.Next(0, 7), (ButtonContent)r.Next(8, 15)));

            return newSequence;
        }

        public void Restart()
        {
            correctSequence = GenerateNewSequence();
            currentSequenceRepeat.Clear();
            currentLevelRepeat = 0;
        }
    }
}