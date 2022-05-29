using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(GridDisplay))]
public class BuildingControl : MonoBehaviour
{
    new Camera camera;
    
    GridDisplay gridDisplay;

    Vector2 mousePosition;
    PlayerInput playerInput;
    Vector2Int currentGridPosition = new Vector2Int(-1, -1);

    BuildingPiece selectedPiece;
    GameObject previewPiece;

    public BuildingPiece SelectedPiece
    {
        get { return selectedPiece; }
        set { selectedPiece = value; }
    }
    

    // Start is called before the first frame update
    void Awake()
    {
        camera = Camera.main;
        gridDisplay = GetComponent<GridDisplay>();
    }

    private void OnDestroy() {
        CancleControl();
    }

    private void OnMouseMove(InputAction.CallbackContext ct)
    {
        mousePosition = ct.ReadValue<Vector2>();
        //preview piece
        //Get current, last grid position
        if (!GetGridPosition()) return;
        //Destroy temporary piece at last grid position
        //Generate temporary piece at current position
        PreviewPiece();
    }

    private void OnLeftClick (InputAction.CallbackContext ct)
    {
        if (!EventSystem.current.IsPointerOverGameObject() && !gridDisplay.OutOfGrid(currentGridPosition.x, currentGridPosition.y))
        {
            //build piece
            // gridDisplay.InitPiece(selectedPiece, currentGridPosition.x, currentGridPosition.y);
            var command = new BuildPieceCommand(gridDisplay, selectedPiece, currentGridPosition.x, currentGridPosition.y);
            command.Execute();
            InputController.Instance.AddCommand(command);
        }
    }

    private void OnRightClick (InputAction.CallbackContext ct)
    {
        //cancle build
        // CancleControl();

        if (!EventSystem.current.IsPointerOverGameObject() 
        && !gridDisplay.OutOfGrid(currentGridPosition.x, currentGridPosition.y)
        && gridDisplay.HasInitPeace(currentGridPosition.x, currentGridPosition.y))
        {
            //build piece
            // gridDisplay.RemovePiece(currentGridPosition.x, currentGridPosition.y);
            var command = new RemovePieceCommand(gridDisplay, currentGridPosition.x, currentGridPosition.y);
            command.Execute();
            InputController.Instance.AddCommand(command);
        }
    }

    private void PreviewPiece ()
    {
        Destroy(previewPiece);
        if(gridDisplay.OutOfGrid(currentGridPosition.x, currentGridPosition.y)) 
        {
            return;
        }

        var targetPosition = gridDisplay.GetWorldPosition(currentGridPosition.x, currentGridPosition.y);
        previewPiece = Instantiate(selectedPiece.Prefab, targetPosition, transform.rotation, transform);
    }

    /// <returns>return true if have change of grid position</returns>
    private bool GetGridPosition ()
    {
        var worldPosition = camera.ScreenToWorldPoint(mousePosition);
        var gridPosition = gridDisplay.GetGridPosition(worldPosition);

        if (currentGridPosition == gridPosition) return false;

        // lastGridPosition = currentGridPosition;
        currentGridPosition = gridPosition;

        return true;
    }

    public void InitControl (PlayerInput playerInput, BuildingPiece piece)
    {
        if (playerInput == null || piece == null) return;

        this.playerInput = playerInput;
        this.selectedPiece = piece;

        playerInput.GameEditor.MousePosition.performed += OnMouseMove;
        playerInput.GameEditor.MouseLeftClick.performed += OnLeftClick;
        playerInput.GameEditor.MouseRightClick.performed += OnRightClick;
    }

    public void CancleControl ()
    {
        if (playerInput == null) return;
        playerInput.GameEditor.MousePosition.performed -= OnMouseMove;
        playerInput.GameEditor.MouseLeftClick.performed -= OnLeftClick;
        playerInput.GameEditor.MouseRightClick.performed -= OnRightClick;

        playerInput = null;
        selectedPiece = null;
        Destroy(previewPiece);
    }
}
