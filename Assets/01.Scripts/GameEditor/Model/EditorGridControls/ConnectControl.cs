using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ConnectControl : MonoBehaviour
{
    public GameObject previewConnectPrefab;
    public bool debug = false;

    new Camera camera;

    GridDisplay gridDisplay;
    GameEditorSystem gameEditorSystem;

    Vector2 mousePosition;
    PlayerInput playerInput;
    Vector2Int currentGridPosition = new Vector2Int(-1, -1);
    bool isOrigin;
    GameObject previewConnect;

    void Awake()
    {
        camera = Camera.main;
        gridDisplay = GetComponent<GridDisplay>();
    }

    private void Start() {
        gameEditorSystem = GameEditorSystem.Instance;
    }

    private void OnDestroy() {
        CancleControl();
    }

    private bool GetGridPosition ()
    {
        var worldPosition = camera.ScreenToWorldPoint(mousePosition);
        var gridPosition = gridDisplay.GetGridPosition(worldPosition);

        if (currentGridPosition == gridPosition) return false;

        currentGridPosition = gridPosition;

        return true;
    }

    private Vector2Int GetUpperOrBottomGridPosition ()
    {
        return !isOrigin ? gridDisplay.GetUpperColumnPosition(currentGridPosition.x) : gridDisplay.GetBottomColumnPosition(currentGridPosition.x);
    }

    private void PreviewPiece ()
    {
        if (!gameEditorSystem.CanChooseColumn(isOrigin)) return;

        if(previewConnect) Destroy(previewConnect);

        if(gridDisplay.OutOfGrid(currentGridPosition.x, currentGridPosition.y)
        || (!isOrigin && gridDisplay.IsTargetOfAnotherGrid(currentGridPosition.x))) 
        {
            return;
        }

        var upperBottomPosition = GetUpperOrBottomGridPosition();

        var targetPosition = gridDisplay.GetWorldPosition(upperBottomPosition.x, upperBottomPosition.y);
        previewConnect = Instantiate(previewConnectPrefab, targetPosition, transform.rotation, transform);
    }

    private void OnMouseMove(InputAction.CallbackContext ct)
    {
        mousePosition = ct.ReadValue<Vector2>();
        //preview piece
        //Get current position of bottom/upper grid
        if (!GetGridPosition()) return;
        //Destroy preview at last grid position
        //Generate preview at current position
        PreviewPiece();
    }

    private void OnLeftClick (InputAction.CallbackContext ct)
    {
        if (!gameEditorSystem.CanChooseColumn(isOrigin)) return;

        if (!EventSystem.current.IsPointerOverGameObject() 
            && !gridDisplay.OutOfGrid(currentGridPosition.x, currentGridPosition.y))
        {
            if (!isOrigin && gridDisplay.IsTargetOfAnotherGrid(currentGridPosition.x)) return;
            //send origin/target column
            gameEditorSystem.ChooseColumn(gridDisplay, currentGridPosition.x, isOrigin);
        }
    }

    private void OnRightClick (InputAction.CallbackContext ct)
    {
        //send message cancle connect
        gameEditorSystem.ResetChooseColumn();

        if (!isOrigin 
        || (!EventSystem.current.IsPointerOverGameObject() && gridDisplay.OutOfGrid(currentGridPosition.x, currentGridPosition.y))
        || !gridDisplay.BeOriginOfAnotherGrid(currentGridPosition.x))
            return;

        var command = new UnconnectGridCommand(gridDisplay, currentGridPosition.x);
        command.Execute();
        InputController.Instance.AddCommand(command);
    }

    public void InitControl (PlayerInput playerInput, bool isOrigin)
    {
        if (playerInput == null) return;

        if (this.playerInput != null && isOrigin != this.isOrigin) CancleControl();
        else if (this.playerInput != null && isOrigin == this.isOrigin) return;

        this.playerInput = playerInput;
        this.isOrigin = isOrigin;

        playerInput.GameEditor.MousePosition.performed += OnMouseMove;
        playerInput.GameEditor.MouseLeftClick.performed += OnLeftClick;
        playerInput.GameEditor.MouseRightClick.performed += OnRightClick;

        if(debug) Debug.Log(name + " init connect control " + isOrigin);
    }

    public void CancleControl ()
    {
        if (playerInput == null) return;

        playerInput.GameEditor.MousePosition.performed -= OnMouseMove;
        playerInput.GameEditor.MouseLeftClick.performed -= OnLeftClick;
        playerInput.GameEditor.MouseRightClick.performed -= OnRightClick;

        playerInput = null;
        Destroy(previewConnect);

        if(debug) Debug.Log(name + " cancle connect control " + isOrigin);
    }
}
