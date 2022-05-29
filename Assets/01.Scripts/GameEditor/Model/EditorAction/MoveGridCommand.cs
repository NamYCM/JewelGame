using UnityEngine;

public class MoveGridCommand : Command 
{
    GridDisplay gridDisplay;
    Vector3 position, positionBefore;
    int x, y;
    int xBefore, yBefore;

    public MoveGridCommand (GridDisplay gridDisplay, Vector3 position)
    {
        this.gridDisplay = gridDisplay;
        this.position = position;
        positionBefore = Vector3.zero;
    }

    public override void Execute()
    {
        positionBefore = position;

        gridDisplay.transform.position = position;
        gridDisplay.RedrawConnectLine();
    }

    public override void Undo()
    {
        gridDisplay.transform.position = positionBefore;
        gridDisplay.RedrawConnectLine();
    }
}
