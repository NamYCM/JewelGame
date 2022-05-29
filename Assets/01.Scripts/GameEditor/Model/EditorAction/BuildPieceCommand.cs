using UnityEngine;

public class BuildPieceCommand : Command
{
    GridDisplay gridDisplay;
    BuildingPiece piece;
    int x, y;

    public BuildPieceCommand(GridDisplay gridDisplay, BuildingPiece piece, int x, int y)
    {
        this.gridDisplay = gridDisplay;
        this.piece = piece;
        this.x = x;
        this.y = y;
    }

    public override void Execute()
    {
        gridDisplay.InitPiece(piece, x, y);
    }

    public override void Undo()
    {
        gridDisplay.RemovePiece(x, y);
    }
}

public class RemovePieceCommand : Command
{
    GridDisplay gridDisplay;
    BuildingPiece piece;
    int x, y;

    public RemovePieceCommand(GridDisplay gridDisplay, int x, int y)
    {
        this.gridDisplay = gridDisplay;
        this.x = x;
        this.y = y;
    }

    public override void Execute()
    {
        piece = gridDisplay.RemovePiece(x, y);
    }

    public override void Undo()
    {
        if (piece == null) 
        {
            Debug.Log(piece);
            return;
        }
        gridDisplay.InitPiece(piece, x, y);
    }
}
