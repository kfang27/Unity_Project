using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidLandController : MonoBehaviour
{
    Rigidbody _rigidbody = null;
    [SerializeField] HumanoidLandInput _input;

    Vector3 _playerMoveInput = Vector3.zero;

    // This heading will show in the inspector
    [Header("Movement Heading")]
    [SerializeField] float _movementMultiplier = 30.0f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _playerMoveInput = GetMoveInput();
        PlayerMove();

        _rigidbody.AddRelativeForce(_playerMoveInput, ForceMode.Force);
    }

    private Vector3 GetMoveInput()
    {
        // MoveInput was a 2D vector but is now being converted into a 3D vector
        // As the 0.0f is for the jump/gravity and the third argument is for the forward/back movement
        return new Vector3(x: _input.MoveInput.x, y: 0.0f, z: _input.MoveInput.y);
    }

    private void PlayerMove()
    {
        _playerMoveInput = (new Vector3(_playerMoveInput.x * _movementMultiplier * _rigidbody.mass,
                                        _playerMoveInput.y,
                                        _playerMoveInput.z * _movementMultiplier * _rigidbody.mass));
    }
}
