using System;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

namespace LampsMiniGame
{
    public enum ButtonContent
    {
        Default,
        Empty,
        
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
    }

    public enum Result
    {
        Success,
        Correct,
        Failure,
        Neutral,
        
        LetsStart,
        NextLevel,
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
                return currentLevelRepeat + 1 == CurrentLevel ?
                    Result.Success :
                    Result.Correct;
            if (content != expectedSequence.Item1 && content != expectedSequence.Item2 ||
                currentSequenceRepeat.Count(x => x == content) > 1)
                return Result.Failure;
            return Result.Neutral;
        }

        public (ButtonContent, ButtonContent) GetSelectedContent()
        {
            var txt = currentSequenceRepeat.FirstOrDefault(x => x is >= ButtonContent.One and <= ButtonContent.Eight);
            if (txt is ButtonContent.Default) txt = ButtonContent.Empty;
            var bg = currentSequenceRepeat.FirstOrDefault(x => x is >= ButtonContent.Red and <= ButtonContent.White);
            return (txt, bg);
        }
            

        private List<(ButtonContent, ButtonContent)> GenerateNewSequence()
        {
            var newSequence = new List<(ButtonContent, ButtonContent)>(levelsCount);
            var r = new Random();
            for (var i = 0; i < levelsCount; i++)
                newSequence.Add((
                    (ButtonContent)r.Next((int)ButtonContent.One, (int)ButtonContent.Eight),
                    (ButtonContent)r.Next((int)ButtonContent.Red, (int)ButtonContent.White)
                    ));

            return newSequence;
        }

        public void NextLevel()
        {
            currentSequenceRepeat.Clear();
            CurrentLevel++;
            currentLevelRepeat = 0;
        }

        public void NextLevelRepeat()
        {
            currentSequenceRepeat.Clear();
            currentLevelRepeat++;
        }

        public void Restart()
        {
            correctSequence = GenerateNewSequence();
            currentSequenceRepeat.Clear();
            currentLevelRepeat = 0;
            CurrentLevel = 1;
        }
    }
}