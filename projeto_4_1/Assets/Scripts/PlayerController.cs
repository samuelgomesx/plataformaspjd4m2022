using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Controls _controls;
    private PlayerInput _playerinput;
    private Camera _mainCamera;
    private Vector2 _moveInput;
    private Rigidbody _rigidbody;
     public float moveMultiplier;

    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        _controls = new Controls();
        
        _playerinput = GetComponent<PlayerInput>();
        
        _mainCamera = Camera.main;
        
        _playerinput.onActionTriggered += OnActionTriggered;
        
    }

    private void OnDisable()
    {
        _playerinput.onActionTriggered -= OnActionTriggered;
        
    }

    private void OnActionTriggered(InputAction.CallbackContext obj)
    {
        if (obj.action.name.CompareTo(_controls.Gameplay.Move.name) == 0)
        {
            _moveInput = obj.ReadValue<Vector2>();

        }
    }

    private void Move()
    {
        _rigidbody.AddForce((_mainCamera.transform.forward * _moveInput.y + _mainCamera.transform.right * _moveInput.x) * moveMultiplier * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        Move();
    }
}   
