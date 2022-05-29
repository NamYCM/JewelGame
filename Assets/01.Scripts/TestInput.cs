using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class TestInput : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;
    PlayerInput _playerInput;

    private void Awake() {
        _playerInput = new PlayerInput();
    }

    private void Start() {
        _playerInput.GamePlay.TouchPress.started += StartTouch;
        _playerInput.GamePlay.TouchPress.canceled += EndTouch;
    }

    private void OnEnable() {
        _playerInput.Enable();
        TouchSimulation.Enable();

        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable() {
        _playerInput.Disable();
        TouchSimulation.Disable();

        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= FingerDown;

    }

    private void Update() {
        Debug.Log(UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count);
    }

    void StartTouch (InputAction.CallbackContext context)
    {
        _text.text = (int.Parse(_text.text) + 1).ToString();
        Debug.Log("start at: " + _playerInput.GamePlay.TouchPosition.ReadValue<Vector2>() +  " " + Input.mousePosition);
    }

    void EndTouch (InputAction.CallbackContext context)
    {
        Debug.Log("end at: " + _playerInput.GamePlay.TouchPosition.ReadValue<Vector2>() +  " " + Input.mousePosition);
    }

    void FingerDown (Finger finger)
    {
        Debug.Log(finger.screenPosition);
    }
}
