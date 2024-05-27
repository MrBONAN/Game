using System;
using System.Collections;
using UnityEngine;
using MiniGames;
using UnityEngine.U2D;

namespace LampsMiniGame
{
    /* Класс LampsObject будет графической интерпретацией обычного Lamps */
    public class LampsObject : MiniGame
    {
        private Lamps game;
        [SerializeField] private int levelsCount = 5;
        [SerializeField] public int levelsDifficulty = 1;
        private float difficultyTime;

        [SerializeField] private Button ButtonPrefab;
        [SerializeField] private GameObject SelectorPrefab;
        [SerializeField] private SpriteAtlas ResultSpriteAtlas;

        private GameObject firstPlayerButtons;
        private GameObject secondPlayerButtons;
        private Button[,] firstPlayer = new Button[2, 4];
        private Button[,] secondPlayer = new Button[2, 4];

        private Button mainButton;
        private GameObject resultSheet;
        private SpriteRenderer resultSheetRenderer;

        private GameObject select1, select2;
        private float gameScale = 2f;
        private float buttonScale = 1.5f;

        private bool controlIsAvailable = true;
        private int animationsQueue;

        public override void StartMiniGame()
        {
            game = new Lamps(levelsCount, levelsDifficulty);
            CreateButtons();
            CreateResultSheet();
            select1 = Instantiate(SelectorPrefab, firstPlayerButtons.transform);
            select2 = Instantiate(SelectorPrefab, secondPlayerButtons.transform);
            select1.transform.localScale = new Vector3(buttonScale, buttonScale);
            select2.transform.localScale = new Vector3(buttonScale, buttonScale);
            UpdateSelectorsPosition();
            PlayAnimation(PlayResultAnimation(Result.LetsStart, 2f));
            PlayAnimation(PlayCurrentSequenceAnimation());
            difficultyTime = Math.Max(0.4f, 1f - (levelsDifficulty - 1f) * 0.1f);
            Debug.Log("MemorizingLampsMiniGame started");
        }

        public override MiniGameResult UpdateMiniGame()
        {
            if (animationsQueue > 0) return MiniGameResult.ContinuePlay;
            foreach (var (direction, keyCode) in PlayerControl.ControlFirst)
                if (Input.GetKeyDown(keyCode) &&
                    direction is Control.Up or Control.Down or Control.Left or Control.Right)
                {
                    game.MoveCursor(ref game.cursor1, direction);
                    UpdateSelectorsPosition();
                }

            foreach (var (direction, keyCode) in PlayerControl.ControlSecond)
                if (Input.GetKeyDown(keyCode) &&
                    direction is Control.Up or Control.Down or Control.Left or Control.Right)
                {
                    game.MoveCursor(ref game.cursor2, direction);
                    UpdateSelectorsPosition();
                }

            var result = CheckPlayerAction(first: true);
            if (result != MiniGameResult.ContinuePlay) return result;
            result = CheckPlayerAction(first: false);
            return result;
        }

        private MiniGameResult CheckPlayerAction(bool first)
        {
            if (!Input.GetKeyDown(first
                    ? PlayerControl.ControlFirst[Control.Use]
                    : PlayerControl.ControlSecond[Control.Use]))
                return MiniGameResult.ContinuePlay;
            return game.PlayerClicked(first) switch
            {
                Result.Success => SuccessAction(),
                Result.Correct => CorrectAction(),
                Result.Failure => FailureAction(),
                Result.Neutral => NeutralAction(),
                _ => throw new ArgumentException()
            };
        }

        private MiniGameResult SuccessAction()
        {
            Debug.Log("Success action");
            UpdateMainButton();
            PlayAnimation(PlayResultAnimation(Result.NextLevel, 2f));

            if (game.CurrentLevel == levelsCount)
            {
                OnDestroy();
                return MiniGameResult.Win;
            }

            game.NextLevel();
            PlayAnimation(PlayCurrentSequenceAnimation());
            return MiniGameResult.ContinuePlay;
        }

        private MiniGameResult CorrectAction()
        {
            Debug.Log("Correct action");
            UpdateMainButton();
            PlayAnimation(PlayResultAnimation(Result.Correct));
            game.NextLevelRepeat();
            return MiniGameResult.ContinuePlay;
        }

        private MiniGameResult FailureAction()
        {
            Debug.Log("Failure action");
            PlayAnimation(PlayResultAnimation(Result.Failure));
            game.Restart();
            UpdateMainButton();
            PlayAnimation(PlayCurrentSequenceAnimation());
            return MiniGameResult.ContinuePlay;
        }

        private MiniGameResult NeutralAction()
        {
            Debug.Log("Neutral action");
            UpdateMainButton();

            return MiniGameResult.ContinuePlay;
        }

        private void CreateButtons()
        {
            var go = new GameObject();
            var shift = new Vector3(4.3f, 0, 0.2f);
            var centerShift = new Vector3(1.5f, 0.75f);
            firstPlayerButtons = Instantiate(go, transform);
            firstPlayerButtons.transform.localScale = new Vector3(gameScale, gameScale, 1);
            firstPlayerButtons.transform.localPosition = -shift;
            for (var i = 0; i < 8; i++)
            {
                var b = Instantiate(ButtonPrefab, firstPlayerButtons.transform);
                b.SetButton((ButtonContent)((int)ButtonContent.One + i), ButtonContent.Default);
                b.Position = new Vector3(i % 4, -i / 4) - centerShift;
                b.transform.localScale = new Vector3(buttonScale, buttonScale, 1);
                firstPlayer[i / 4, i % 4] = b;
            }

            secondPlayerButtons = Instantiate(go, transform);
            secondPlayerButtons.transform.localScale = new Vector3(gameScale, gameScale, 1);
            secondPlayerButtons.transform.localPosition = shift;
            for (var i = 0; i < 8; i++)
            {
                var b = Instantiate(ButtonPrefab, secondPlayerButtons.transform);
                b.ButtonBackground = ButtonContent.Red + i;
                b.SetButtonTextColor(ButtonContent.Red + i);
                b.Position = new Vector3(i % 4, -i / 4) - centerShift;
                b.transform.localScale = new Vector3(buttonScale, buttonScale, 1);
                secondPlayer[i / 4, i % 4] = b;
            }

            Destroy(go);

            mainButton = Instantiate(ButtonPrefab, transform);
            mainButton.transform.localScale = new Vector3(5, 5, 1);
            mainButton.transform.localPosition = new Vector3(0, 2.5f, 0.1f);
            mainButton.SetButton(ButtonContent.Empty, ButtonContent.Default);
        }

        private void CreateResultSheet()
        {
            var go = new GameObject();
            resultSheet = Instantiate(go, transform);
            resultSheet.AddComponent<SpriteRenderer>();
            resultSheet.transform.localScale = new Vector3(5, 5, 1);
            resultSheet.transform.localPosition = new Vector3(0, 2.5f, 0);
            resultSheetRenderer = resultSheet.GetComponent<SpriteRenderer>();
            SetResultSheetState(Result.Correct);
            resultSheetRenderer.enabled = false;
            Destroy(go);
        }

        private void SetResultSheetState(Result result)
            => resultSheetRenderer.sprite = ResultSpriteAtlas.GetSprite(result.ToString());

        private void PlayAnimation(IEnumerator enumerator)
        {
            animationsQueue++;
            StartCoroutine(enumerator);
            animationsQueue--;
        }

        private IEnumerator PlayCurrentSequenceAnimation()
        {
            while (!controlIsAvailable) yield return new WaitForSeconds(0.1f);
            controlIsAvailable = false;
            for (var i = 0; i < game.CurrentLevel; i++)
            {
                var (number, color) = game.correctSequence[i];
                mainButton.SetButton(number, color);
                yield return new WaitForSeconds(difficultyTime);
            }

            mainButton.SetButton(ButtonContent.Empty, ButtonContent.Default);
            controlIsAvailable = true;
        }

        private IEnumerator PlayResultAnimation(Result result, float time = 1f)
        {
            while (!controlIsAvailable) yield return new WaitForSeconds(0.1f);
            controlIsAvailable = false;
            SetResultSheetState(result);
            resultSheetRenderer.enabled = true;
            yield return new WaitForSeconds(time);
            resultSheetRenderer.enabled = false;
            controlIsAvailable = true;
        }

        private void UpdateSelectorsPosition()
        {
            var (cursor1, cursor2) = (game.cursor1, game.cursor2);
            select1.transform.localPosition = firstPlayer[cursor1.Y, cursor1.X].Position + new Vector3(0, 0, 0.5f);
            select2.transform.localPosition = secondPlayer[cursor2.Y, cursor2.X].Position + new Vector3(0, 0, 0.5f);
        }

        private void UpdateMainButton()
            => (mainButton.ButtonText, mainButton.ButtonBackground) = game.GetSelectedContent();

        public override void OnDestroy()
        {
            foreach (var button in firstPlayer)
                Destroy(button);
            foreach (var button in secondPlayer)
                Destroy(button);
            Destroy(firstPlayerButtons);
            Destroy(secondPlayerButtons);
            Destroy(select1);
            Destroy(select2);
            Destroy(resultSheet);

            Debug.Log("MemorizingLampsMiniGame destroyed");
        }
    }
}