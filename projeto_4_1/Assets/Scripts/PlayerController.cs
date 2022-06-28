using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public int coins = 0;
    private Controls _controls;
    private PlayerInput _playerinput;
    private Camera _mainCamera;
    private Vector2 _moveInput;
    private Rigidbody _rigidbody;
    private bool _isGrounded;
     public float moveMultiplier;

     public float maxVelocity;

     public float rayDistance;

     public LayerMask layerMask;
     public float JumpForce;

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

        if (obj.action.name.CompareTo(_controls.Gameplay.Jump.name) == 0)
        {
            if(obj.performed) Jump();
        }
    }

    private void Move()
    {
        Vector3 camForward = _mainCamera.transform.forward;
        Vector3 camRight = _mainCamera.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        
        _rigidbody.AddForce(
            (camForward * _moveInput.y +
                 camRight* _moveInput.x  )
            * moveMultiplier * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        Move();
        LimitVelocity();
    }

    private void LimitVelocity()
    {
        Vector3 velocity = _rigidbody.velocity;
        if (Mathf.Abs(velocity.x) > maxVelocity) velocity.x = Mathf.Sign(velocity.x) + maxVelocity;
       // if (Mathf.Abs(velocity.z) > maxVelocity) velocity.z = Mathf.Sign(velocity.z) + maxVelocity;

        _rigidbody.velocity = velocity;
    }
    

    private void CheckGround()
    {
        RaycastHit collison;

        if (Physics.Raycast(transform.position, Vector3.down, out collison, rayDistance, layerMask))
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }
    }

    private void Jump()
    {
        if (_isGrounded)
        {
             _rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
    }

    private void Update()
    {
        CheckGround();
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(start:transform.position, dir:Vector3.down * rayDistance, Color.yellow);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Coin"))
        {
            coins++;
            Destroy(other.gameObject);
        }
    
    }
}   
