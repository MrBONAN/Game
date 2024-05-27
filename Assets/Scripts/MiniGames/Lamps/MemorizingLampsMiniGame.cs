using System;
using System.Collections;
using UnityEngine;
using MiniGames;
using UnityEngine.Serialization;
using UnityEngine.U2D;

namespace LampsMiniGame
{
    public enum IndicatorState
    {
        Passed,
        Current,
        Upcoming
    }

    /* Класс LampsObject будет графической интерпретацией обычного Lamps */
    public class LampsObject : MiniGame
    {
        private Lamps _game;
        [SerializeField] private int levelsCount = 5;
        [SerializeField] public int levelsDifficulty = 1;
        private float _difficultyTime;

        [SerializeField] private Button buttonPrefab;
        [SerializeField] private GameObject selectorPrefab;
        [SerializeField] private SpriteAtlas resultSpriteAtlas;
        [SerializeField] private SpriteAtlas indicatorSpriteAtlas;

        private GameObject _firstPlayerButtons;
        private GameObject _secondPlayerButtons;
        private Button[,] _firstPlayer = new Button[2, 4];
        private Button[,] _secondPlayer = new Button[2, 4];

        private Button _mainButton;
        private GameObject _resultSheet;
        private GameObject[] _indicators;
        private SpriteRenderer _resultSheetRenderer;

        private GameObject _select1, _select2;
        private const float GameScale = 2f;
        private const float ButtonScale = 1.5f;
        private const float IndicatorScale = 3f;

        private bool _controlIsAvailable = true;
        private int _animationsQueue;

        public override void StartMiniGame()
        {
            _game = new Lamps(levelsCount, levelsDifficulty);
            CreateButtons();
            CreateResultSheet();
            CreateIndicators();
            _select1 = Instantiate(selectorPrefab, _firstPlayerButtons.transform);
            _select2 = Instantiate(selectorPrefab, _secondPlayerButtons.transform);
            _select1.transform.localScale = new Vector3(ButtonScale, ButtonScale);
            _select2.transform.localScale = new Vector3(ButtonScale, ButtonScale);
            UpdateSelectorsPosition();
            PlayAnimation(PlayResultAnimation(Result.LetsStart, 2f));
            PlayAnimation(PlayCurrentSequenceAnimation());
            _difficultyTime = Math.Max(0.4f, 1f - (levelsDifficulty - 1f) * 0.1f);
            Debug.Log("MemorizingLampsMiniGame started");
        }

        public override MiniGameResult UpdateMiniGame()
        {
            if (_animationsQueue > 0) return MiniGameResult.ContinuePlay;
            foreach (var (direction, keyCode) in PlayerControl.ControlFirst)
                if (Input.GetKeyDown(keyCode) &&
                    direction is Control.Up or Control.Down or Control.Left or Control.Right)
                {
                    _game.MoveCursor(ref _game.cursor1, direction);
                    UpdateSelectorsPosition();
                }

            foreach (var (direction, keyCode) in PlayerControl.ControlSecond)
                if (Input.GetKeyDown(keyCode) &&
                    direction is Control.Up or Control.Down or Control.Left or Control.Right)
                {
                    _game.MoveCursor(ref _game.cursor2, direction);
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
            return _game.PlayerClicked(first) switch
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
            SetIndicator(_game.CurrentLevel - 1, IndicatorState.Passed);
            PlayAnimation(PlayResultAnimation(Result.NextLevel, 2f));

            if (_game.CurrentLevel == levelsCount)
            {
                OnDestroy();
                return MiniGameResult.Win;
            }

            _game.NextLevel();
            SetIndicator(_game.CurrentLevel - 1, IndicatorState.Current);
            PlayAnimation(PlayCurrentSequenceAnimation());
            return MiniGameResult.ContinuePlay;
        }

        private MiniGameResult CorrectAction()
        {
            Debug.Log("Correct action");
            UpdateMainButton();
            PlayAnimation(PlayResultAnimation(Result.Correct));
            _game.NextLevelRepeat();
            return MiniGameResult.ContinuePlay;
        }

        private MiniGameResult FailureAction()
        {
            Debug.Log("Failure action");
            PlayAnimation(PlayResultAnimation(Result.Failure, 2f));
            _game.Restart();
            UpdateMainButton();
            ResetIndicators();
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
            _firstPlayerButtons = Instantiate(go, transform);
            _firstPlayerButtons.transform.localScale = new Vector3(GameScale, GameScale, 1);
            _firstPlayerButtons.transform.localPosition = -shift;
            for (var i = 0; i < 8; i++)
            {
                var b = Instantiate(buttonPrefab, _firstPlayerButtons.transform);
                b.SetButton((ButtonContent)((int)ButtonContent.One + i), ButtonContent.Default);
                b.Position = new Vector3(i % 4, -i / 4) - centerShift;
                b.transform.localScale = new Vector3(ButtonScale, ButtonScale, 1);
                _firstPlayer[i / 4, i % 4] = b;
            }

            _secondPlayerButtons = Instantiate(go, transform);
            _secondPlayerButtons.transform.localScale = new Vector3(GameScale, GameScale, 1);
            _secondPlayerButtons.transform.localPosition = shift;
            for (var i = 0; i < 8; i++)
            {
                var b = Instantiate(buttonPrefab, _secondPlayerButtons.transform);
                b.ButtonBackground = ButtonContent.Red + i;
                b.SetButtonTextColor(ButtonContent.Red + i);
                b.Position = new Vector3(i % 4, -i / 4) - centerShift;
                b.transform.localScale = new Vector3(ButtonScale, ButtonScale, 1);
                _secondPlayer[i / 4, i % 4] = b;
            }

            Destroy(go);

            _mainButton = Instantiate(buttonPrefab, transform);
            _mainButton.transform.localScale = new Vector3(5, 5, 1);
            _mainButton.transform.localPosition = new Vector3(0, 2.5f, 0.1f);
            _mainButton.SetButton(ButtonContent.Empty, ButtonContent.Default);
        }

        private void CreateResultSheet()
        {
            var go = new GameObject("ResultSheet");
            _resultSheet = Instantiate(go, transform);
            _resultSheet.AddComponent<SpriteRenderer>();
            _resultSheet.transform.localScale = new Vector3(5, 5, 1);
            _resultSheet.transform.localPosition = new Vector3(0, 2.5f, 0);
            _resultSheetRenderer = _resultSheet.GetComponent<SpriteRenderer>();
            SetResultSheetState(Result.Correct);
            _resultSheetRenderer.enabled = false;
            Destroy(go);
        }

        private void CreateIndicators()
        {
            _indicators = new GameObject[levelsCount];
            var shift = new Vector3(-(float)(levelsCount - 1) / 2, 0);
            var go = new GameObject("Indicator");
            for (var i = 0; i < levelsCount; i++)
            {
                var ind = Instantiate(go, transform);
                ind.AddComponent<SpriteRenderer>();
                ind.transform.localPosition = (new Vector3(i, 0, 0) + shift) * 2;
                ind.transform.localScale = new Vector3(IndicatorScale, IndicatorScale);
                _indicators[i] = ind;
            }

            ResetIndicators();
            Destroy(go);
        }

        private void ResetIndicators()
        {
            foreach (var indicator in _indicators) // Вызываю не особо часто, так что не супер затратно по ресурсам
                indicator.GetComponent<SpriteRenderer>().sprite =
                    indicatorSpriteAtlas.GetSprite(IndicatorState.Upcoming.ToString());
            _indicators[0].GetComponent<SpriteRenderer>().sprite =
                indicatorSpriteAtlas.GetSprite(IndicatorState.Current.ToString());
        }

        private void SetIndicator(int index, IndicatorState state)
        {
            if (index < levelsCount)
                _indicators[index].GetComponent<SpriteRenderer>().sprite =
                    indicatorSpriteAtlas.GetSprite(state.ToString());
        }

        private void SetResultSheetState(Result result)
            => _resultSheetRenderer.sprite = resultSpriteAtlas.GetSprite(result.ToString());

        private void PlayAnimation(IEnumerator enumerator)
        {
            _animationsQueue++;
            StartCoroutine(enumerator);
            _animationsQueue--;
        }

        private IEnumerator PlayCurrentSequenceAnimation()
        {
            while (!_controlIsAvailable) yield return new WaitForSeconds(0.01f);
            _controlIsAvailable = false;
            for (var i = 0; i < _game.CurrentLevel; i++)
            {
                var (number, color) = _game.correctSequence[i];
                _mainButton.SetButton(number, color);
                yield return new WaitForSeconds(_difficultyTime);
            }

            _mainButton.SetButton(ButtonContent.Empty, ButtonContent.Default);
            _controlIsAvailable = true;
        }

        private IEnumerator PlayResultAnimation(Result result, float time = 1f)
        {
            while (!_controlIsAvailable) yield return new WaitForSeconds(0.1f);
            _controlIsAvailable = false;
            SetResultSheetState(result);
            _resultSheetRenderer.enabled = true;
            yield return new WaitForSeconds(time);
            _resultSheetRenderer.enabled = false;
            _controlIsAvailable = true;
        }

        private void UpdateSelectorsPosition()
        {
            var (cursor1, cursor2) = (_game.cursor1, _game.cursor2);
            _select1.transform.localPosition = _firstPlayer[cursor1.Y, cursor1.X].Position + new Vector3(0, 0, 0.5f);
            _select2.transform.localPosition = _secondPlayer[cursor2.Y, cursor2.X].Position + new Vector3(0, 0, 0.5f);
        }

        private void UpdateMainButton()
            => (_mainButton.ButtonText, _mainButton.ButtonBackground) = _game.GetSelectedContent();

        public override void OnDestroy()
        {
            foreach (var button in _firstPlayer)
                Destroy(button);
            foreach (var button in _secondPlayer)
                Destroy(button);
            Destroy(_firstPlayerButtons);
            Destroy(_secondPlayerButtons);
            Destroy(_select1);
            Destroy(_select2);
            Destroy(_resultSheet);

            Debug.Log("MemorizingLampsMiniGame destroyed");
        }
    }
}