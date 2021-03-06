﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerControllerBehaviour : MonoBehaviour
{
    //fields
    [SerializeField] private Transform _modelTransform = default;
    [SerializeField] private Transform _cameraArmTransform = default;
    [SerializeField] private Transform _cameraTransform = default;
    [SerializeField] private Transform _cameraFocusTransform = default;
    [SerializeField] private float     _cameraRadius = 3f;

    [SerializeField] private float _speed = 10f;

    private CharacterController _characterController = default;
    private Vector2 _mouseMoved = Vector2.zero;
    private InputAction _onLookAction = null;

#if UNITY_EDITOR
    private readonly float _debugLineLength = 3f;
#endif
    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        var playerInput = GetComponent<PlayerInput>();
        _onLookAction = playerInput.actions["Look"];
    }

    // Update is called once per frame
    void Update()
    {
        #region Debug
#if UNITY_EDITOR
        //player
        Debug.DrawLine(transform.position, transform.position + transform.forward * _debugLineLength, Color.blue);
        Debug.DrawLine(transform.position, _characterController.velocity + transform.position, Color.red);

        //model
        Debug.DrawLine(_modelTransform.position, _modelTransform.position + _modelTransform.forward * _debugLineLength);

        //cam
        Debug.DrawLine(_cameraTransform.position, _cameraTransform.position + _cameraTransform.forward * _debugLineLength);
#endif
        #endregion

        //get input
        //Vector3 inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));


        //_mouseMoved += new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        //_cameraRadius += Input.GetAxis("Mouse ScrollWheel");
        _cameraArmTransform.rotation = Quaternion.Euler(0f, _mouseMoved.x, 0f);

        Vector3 camMove = new Vector3(
              0f
            , Mathf.Sin(Mathf.Deg2Rad * _mouseMoved.y) * _cameraRadius
            , Mathf.Cos(Mathf.Deg2Rad * _mouseMoved.y) * -_cameraRadius
            );

        //move
        _cameraTransform.localPosition = camMove;
        _cameraTransform.LookAt(_modelTransform);
        //Vector3 test = _cameraTransform.right;
        //_characterController.Move(test * Time.deltaTime * _speed);
    }

#pragma warning disable IDE0051
    private void OnLook(InputValue value)
#pragma warning restore IDE0051
    {
        _mouseMoved += value.Get<Vector2>() * Time.deltaTime * _speed;
    }

#pragma warning disable IDE0051
    private void OnCaptureMouse(InputValue _)
#pragma warning restore IDE0051
    {
        if (_onLookAction.enabled)
        {
            _onLookAction.Disable();
        }
        else
        {
            _onLookAction.Enable();
        }
    }
}
