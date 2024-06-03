using System;
using System.Collections.Generic;
using MiniGames;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MazeMiniGame
{
    public class WireHandler : CurrentMiniGameHandler
    {
        [SerializeField] private WiresGameObject wireGamePrefab;
        [SerializeField] private WiresGameObject game;
        [SerializeField] private WireGUIPref _wireGUI;
        [SerializeField] private FieldGUIPref _fieldGUI;
        [SerializeField] private GameObject StartPoint;
        [SerializeField] private GameObject EndPoint;
        [SerializeField] private GameObject LinePref;

        public override void StartMiniGame()
        {
            game = gameObject.AddComponent<WiresGameObject>();
            miniGame = game;
            var level = 2;
            game.SetGameState(_wireGUI, _fieldGUI, StartPoint, EndPoint, LinePref, level);
            base.StartMiniGame();
        }
    }
}