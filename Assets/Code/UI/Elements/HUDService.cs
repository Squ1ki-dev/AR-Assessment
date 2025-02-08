using System.Collections.Generic;
using Code;
using Code.Wave;
using TMPro;
using UnityEngine;
using Zenject;

namespace Code.UI.Elements
{
    public class HUDService : MonoBehaviour
    {
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private GameObject _joystick;
        [SerializeField] private WaveSetupSO _waveConfig;
        private GameState _gameState;

        [Inject]
        public void Construct(GameState gameState)
        {
            _gameState = gameState;
        }

        private void Start()
        {
            _levelText.text = "Level: " + _waveConfig.CurrentWave.ToString();
            CheckForValidState();
        }

        private void Update() => CheckForValidState();

        private void CheckForValidState()
        {
            if(_gameState.CurrentState == GameStates.Game)
            {
                _levelText.gameObject.SetActive(true);
                _joystick.SetActive(true);
            }
            else
            {
                _levelText.gameObject.SetActive(false);
                _joystick.SetActive(false);
            }
        }
    }
}
