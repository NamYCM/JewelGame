using UnityEngine;

internal class ClearLinePiece : ClearJewelPiece
{
    public bool isRow;

    protected override void BeforeClear()
    {
        base.BeforeClear();
        // ((LineClearAnimation)clearAnimation).ResetEffect();
        ((LineClearAnimation)clearAnimation).InitFire(isRow, piece.GridRef.GetWorldPosition(piece.X, piece.Y));

    }

    public override void Clear()
    {
        base.Clear();
        // if (isRow)
        // {            
        //     piece.GridRef.ClearRow(piece.Y);

        //     try
        //     {
        //         int clearRow;
        //         foreach (var grid in piece.Grids)
        //         {
        //             if (grid == piece.GridRef) continue;

        //             clearRow = (int)grid.GetGridPosition(piece.GridRef.GetWorldPosition(0, piece.Y)).y;
        //             if (clearRow >= 0 && clearRow < grid.YDim)
        //             {
        //                 grid.ClearRow(clearRow);
        //                 if (!grid.IsFilling)
        //                     grid.StartFill(true);
        //             }
        //         }
        //     }
        //     catch (System.Exception)
        //     {
        //             Debug.LogWarning(piece);
        //             Debug.LogWarning(piece.GridRef);
        //     }
        // }
        // else
        // {            
        //     piece.GridRef.ClearColumn(piece.X);

        //     try
        //     {
        //         int clearColumn;
        //         foreach (var grid in piece.Grids)
        //         {
        //             if (grid == piece.GridRef) continue;

        //             clearColumn = (int)grid.GetGridPosition(piece.GridRef.GetWorldPosition(piece.X, 0)).x;
        //             if (clearColumn >= 0 && clearColumn < grid.XDim)
        //             {
        //                 grid.ClearColumn(clearColumn);
        //                 if (!grid.IsFilling)
        //                 {
        //                     grid.StartFill(true);
        //                 }
        //             }
        //         }
        //     }
        //     catch (System.Exception ex)
        //     {
        //             Debug.LogWarning(piece + " " + ex.StackTrace);
        //             Debug.LogWarning(piece.GridRef);
        //     }
        // }

    }
}
