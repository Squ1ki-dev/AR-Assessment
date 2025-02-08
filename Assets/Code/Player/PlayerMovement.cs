using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Code;
using Code.Player;
using Code.Services.Input;

namespace CodeBase.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] private PlayerStatsSO playerConfig;
        private Rigidbody _rigidbody;
        private IInputService _inputService;
        private Vector3 _move;
        private bool _walking;

        private void Awake()
        {
            _inputService = new MobileInputService();
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        private void FixedUpdate()
        {
            _move = new Vector3(_inputService.Axis.x, 0f, _inputService.Axis.y).normalized;

            if (_move.sqrMagnitude > Constants.Epsilon)
            {
                if (!_walking)
                {
                    _walking = true;
                    StartCoroutine(PlayerMovement());
                }
            }
            else
            {
                _walking = false;
                StopCoroutine(PlayerMovement());
            }
        }

        private IEnumerator PlayerMovement()
        {
            while (_walking)
            {
                _rigidbody.MovePosition(_rigidbody.position + _move * playerConfig.Speed * Time.deltaTime);
                if (_move != Vector3.zero)
                    _rigidbody.rotation = Quaternion.Slerp(_rigidbody.rotation, Quaternion.LookRotation(_move), Time.deltaTime * 5.0f);
                yield return null;
            }
        }
    }
}