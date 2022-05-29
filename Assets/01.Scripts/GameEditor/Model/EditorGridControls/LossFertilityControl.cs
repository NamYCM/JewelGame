using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class LossFertilityControl : ControlGrid
{
    public GameObject xPrefab;
    GameObject previewX;

    private void PreviewX ()
    {
        if (GridDisplay.OutOfGrid(CurrentGridPosition.x, CurrentGridPosition.y)) return;

        Destroy(previewX);

        var gridPositionOntoGrid = GridDisplay.GetGridPositionOntoGrid(CurrentGridPosition.x);

        var targetPosition = GridDisplay.GetWorldPosition(gridPositionOntoGrid.x, gridPositionOntoGrid.y);
        previewX = Instantiate(xPrefab, targetPosition, transform.rotation, transform);
    }

    protected override void OnLeftClick(UnityEngine.InputSystem.InputAction.CallbackContext ct)
    {
        if (!EventSystem.current.IsPointerOverGameObject() && !GridDisplay.OutOfGrid(CurrentGridPosition.x, CurrentGridPosition.y))
        {
            //build piece
            // gridDisplay.InitPiece(selectedPiece, currentGridPosition.x, currentGridPosition.y);
            var command = new LossFertilityCommand(GridDisplay, CurrentGridPosition.x);
            command.Execute();
            InputController.Instance.AddCommand(command);
        }
    }

    protected override void OnMouseMove(InputAction.CallbackContext ct)
    {
        if (!GetGridPosition(ct.ReadValue<Vector2>())) return;

        PreviewX();
    }

    protected override void OnRightClick(UnityEngine.InputSystem.InputAction.CallbackContext ct)
    {
        if (!EventSystem.current.IsPointerOverGameObject() 
        && !GridDisplay.OutOfGrid(CurrentGridPosition.x, CurrentGridPosition.y)
        && GridDisplay.IsLossFertilityAt(CurrentGridPosition.x))
        {
            //build piece
            // gridDisplay.RemovePiece(currentGridPosition.x, currentGridPosition.y);
            var command = new RemoveLossFertilityCommand(GridDisplay, CurrentGridPosition.x);
            command.Execute();
            InputController.Instance.AddCommand(command);
        }
    }

    public override void CancleControl()
    {
        base.CancleControl();

        Destroy(previewX);
    }
}
