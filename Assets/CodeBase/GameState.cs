using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;
using Code.UI;

namespace Code
{
    public enum GameStates
    {
        None,
        Menu,
        Game,
        Lose,
        Finish
    }

    public class GameState : MonoBehaviour
    {
        [SerializeField] private int _menuIndex, _finishIndex, _loseIndex;
        [SerializeField] private GameObject _blockerImg;

        private PanelManager _panelManager;
        public GameStates CurrentState = GameStates.None;

        [Inject]
        private void Construct(PanelManager panelManager)
        {
            _panelManager = panelManager;
        }

        private void Start()
        {
            IsBlockerActive(false);
            ChangeState(GameStates.Menu);
        }

        public void ChangeState(GameStates newState)
        {
            CurrentState = newState;

            switch (CurrentState)
            {
                case GameStates.Menu:
                    _panelManager.OpenPanelByIndex(_menuIndex);
                    break;

                case GameStates.Game:
                    IsBlockerActive(false);
                    _panelManager.CloseAllPanels();
                    break;

                case GameStates.Lose:
                    _panelManager.OpenPanelByIndex(_loseIndex);
                    break;

                case GameStates.Finish:
                    _panelManager.OpenPanelByIndex(_finishIndex);
                    break;
            }
        }

        private void IsBlockerActive(bool isActive) => _blockerImg.SetActive(isActive);
    }
}
