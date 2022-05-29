using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(GridDisplay))]
public abstract class ControlGrid : MonoBehaviour
{
    new Camera camera;

    Vector2Int currentGridPosition = new Vector2Int(-1, -1);
    GridDisplay gridDisplay;
    PlayerInput playerInput;

    protected Vector2Int CurrentGridPosition => currentGridPosition;
    protected GridDisplay GridDisplay => gridDisplay;
    
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        camera = Camera.main;
        gridDisplay = GetComponent<GridDisplay>();
    }

    protected virtual void OnDestroy() {
        CancleControl();
    }

    protected bool GetGridPosition (Vector2 mousePosition)
    {
        var worldPosition = camera.ScreenToWorldPoint(mousePosition);
        var gridPosition = gridDisplay.GetGridPosition(worldPosition);

        if (currentGridPosition == gridPosition) return false;

        // lastGridPosition = currentGridPosition;
        currentGridPosition = gridPosition;

        return true;
    }
    
    protected abstract void OnLeftClick (InputAction.CallbackContext ct);
    protected abstract void OnRightClick (InputAction.CallbackContext ct);
    protected abstract void OnMouseMove (InputAction.CallbackContext ct);

    public virtual void InitControl (PlayerInput playerInput)
    {
        if (playerInput == null) return;

        this.playerInput = playerInput;

        playerInput.GameEditor.MousePosition.performed += OnMouseMove;
        playerInput.GameEditor.MouseLeftClick.performed += OnLeftClick;
        playerInput.GameEditor.MouseRightClick.performed += OnRightClick;
    }

    public virtual void CancleControl ()
    {
        if (playerInput == null) return;
        playerInput.GameEditor.MousePosition.performed -= OnMouseMove;
        playerInput.GameEditor.MouseLeftClick.performed -= OnLeftClick;
        playerInput.GameEditor.MouseRightClick.performed -= OnRightClick;

        playerInput = null;
    }
}
