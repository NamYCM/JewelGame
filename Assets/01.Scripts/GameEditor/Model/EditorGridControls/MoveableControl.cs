using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class MoveableControl : MonoBehaviour
{
    new Camera camera;

    Vector2 mousePosition;
    PlayerInput playerInput;
    Vector2 currentGridPosition = Vector2.zero;
    int unit = 1;

    bool isPressed = false;

    private void Awake() {
        camera = Camera.main;
    }

    private void OnDestroy() {
        CancleControl();
    }

    private void MoveFollowGrid(Vector2 newGridPosition)
    {
        float distance = (currentGridPosition - newGridPosition).magnitude;

        if (distance < unit * 1.0f / 2) return;

        // change this algorithm if change the unit
        float newX = Mathf.Round(newGridPosition.x * 2) / 2;
        float newY = Mathf.Round(newGridPosition.y * 2) / 2;

        //TODO limited space to move so that instance's grid does not exceed the boundary 
        currentGridPosition = new Vector2(newX, newY);

        MoveGridCommand move = new MoveGridCommand(GetComponent<GridDisplay>(), currentGridPosition);
        move.Execute();
        InputController.Instance.AddCommand(move);
    }

    public void OnMouseMove(InputAction.CallbackContext ct)
    {
        if(!isPressed) return;

        mousePosition = ct.ReadValue<Vector2>();
        MoveFollowGrid(camera.ScreenToWorldPoint(mousePosition));
    }

    public void OnLeftClick (InputAction.CallbackContext ct)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
            isPressed = true;
    }

    public void OnLeftEndClick (InputAction.CallbackContext ct)
    {
        isPressed = false;
    }

    public void InitControl (PlayerInput playerInput)
    {
        if (playerInput == null) return;

        this.playerInput = playerInput;

        playerInput.GameEditor.MousePosition.performed += OnMouseMove;
        playerInput.GameEditor.MouseLeftClick.started += OnLeftClick;
        playerInput.GameEditor.MouseLeftClick.canceled += OnLeftEndClick;
    }

    public void CancleControl ()
    {
        if (playerInput == null) return;
        playerInput.GameEditor.MousePosition.performed -= OnMouseMove;
        playerInput.GameEditor.MouseLeftClick.started -= OnLeftClick;
        playerInput.GameEditor.MouseLeftClick.canceled -= OnLeftEndClick;

        playerInput = null;
    }
}
