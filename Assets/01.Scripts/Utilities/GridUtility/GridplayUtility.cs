using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridplayUtility
{
    public static bool CanMoveDirectDown (this GridPlay grid, int x, int y)
    {
        return (!grid.IsOutOfGrid(x, y + 1) && grid.Pieces[x, y + 1].Type == PieceType.EMPTY);
    }
    public static bool CanMoveDirectDown (this GridPlay grid, int x, int y, out int desY) 
    {
        desY = 0;
        int lowestRow = grid.GetLowestRow(x);

        if (!grid.IsOutOfGrid(x, y + 1) && grid.Pieces[x, y + 1].Type == PieceType.EMPTY)
        {
            for (int underY = y + 1; underY <= lowestRow; underY ++)
            {
                if (grid.Pieces[x, underY].Type != PieceType.EMPTY)
                {
                    desY = underY - 1;
                    break;
                }
                else if (underY == lowestRow)
                {
                    desY = underY;
                }
            }
            return true;
        }

        return false;
    }

    public static int CountEmptyPieceAtColumn (this GridPlay grid, int x)
    {
        int count = 0;
        int highestRow = grid.GetHighestRow(x);
        int lowestRow = grid.GetLowestRow(x);

        for (int loopY = highestRow; loopY <= lowestRow; loopY ++)
        {
            if (grid.Pieces[x, loopY].Type == PieceType.EMPTY) count ++;
        }

        return count;
    }
    public static int GetPieceAmountThatCanBeSpawnedAtColumn (this GridPlay grid, int x)
    {
        int count = 0;
        int highestRow = grid.GetHighestRow(x);
        int lowestRow = grid.GetLowestRow(x);

        for (int loopY = highestRow; loopY <= lowestRow; loopY ++)
        {
            if (grid.Pieces[x, loopY].Type == PieceType.EMPTY) count ++;
            else if (!grid.Pieces[x, loopY].IsMovable() || grid.Pieces[x, loopY].IsIgnored())
            {
                //because loop from upper to bottom
                break;
            }
        }

        return count;
    }
    public static int CountEmptyPieceCanBeFallDown (this GridPlay grid, int x, int y)
    {
        if (grid.IsOutOfGrid(x, y)) return 0;

        int count = 0;
        // int highestRow = grid.GetHighestRow(x);
        int lowestRow = grid.GetLowestRow(x);

        for (int loopY = lowestRow; loopY >= y; loopY --)
        {
            if (grid.Pieces[x, loopY].Type == PieceType.EMPTY) count ++;
            else if (!grid.Pieces[x, loopY].IsMovable() || grid.Pieces[x, loopY].IsIgnored())
            {
                count = 0;
            }
        }

        return count;
    }

    public static bool CanMoveDiagonalDown (this GridPlay grid, int x, int y, out int diagX, out int diagY, bool inverse = false)
    {
        diagX = -1;
        diagY = y + 1;

        for (int diag = -1; diag <= 1; diag++)
        {
            if (diag == 0) continue;
            
            diagX = x + diag;

            if (inverse)
            {
                diagX = x - diag;
            }

            if (diagX < 0 || diagX >= grid.XDim) continue;
            
            GamePiece diagonalPiece = grid.Pieces[diagX, y + 1];

            // if (diagonalPiece.Type != PieceType.EMPTY && diagonalPiece.Type != PieceType.IGNORE) continue;
            if (diagonalPiece.Type != PieceType.EMPTY) continue;
            
            bool hasMovablePieceAbove = true;

            for (int aboveY = y; aboveY >= grid.GetHighestRow(diagX); aboveY--)
            {
                GamePiece pieceAbove = grid.Pieces[diagX, aboveY];
                
                if (pieceAbove.IsMovable())
                {
                    break;
                }
                else if (pieceAbove.Type != PieceType.EMPTY && pieceAbove.Type != PieceType.IGNORE)
                {
                    hasMovablePieceAbove = false;
                }
                else if (aboveY == grid.GetHighestRow(diagX))// && grid.Spawner.IsLossFertilityColumn(diagX))
                {
                    hasMovablePieceAbove = false;
                }
            }

            bool hasMovingPieceUnder = false;
            for (int underY = y; underY <= grid.GetLowestRow(diagX); underY++)
            {
                GamePiece pieceUnder = grid.Pieces[diagX, underY];

                if (pieceUnder.IsMovable() && pieceUnder.MovableComponent.IsRunning())
                {
                    hasMovingPieceUnder = true;
                    break;
                }
                // else if (pieceUnder.Type != PieceType.EMPTY && pieceUnder.Type != PieceType.IGNORE)
                // {
                //     hasMovablePieceAbove = false;
                // }
            }

            if (hasMovablePieceAbove || hasMovingPieceUnder) continue;
            
            return true;
        }

        return false;
    }

    public static List<GamePiece> GetMatch(this GridPlay grid, GamePiece piece, int newX, int newY)
    {
        if (!piece.IsColored()) return null;
        var color = piece.ColorComponent.Color;
        var matchingPieces = new List<GamePiece>();
        var horizontalPieces = new List<GamePiece>();
        var verticalPieces = new List<GamePiece>();

        // First check horizontal
        horizontalPieces.Add(piece);

        for (int dir = 0; dir <= 1; dir++)
        {
            for (int xOffset = 1; xOffset < grid.XDim; xOffset++)
            {
                int x;

                if (dir == 0)
                { // Left
                    x = newX - xOffset;
                }
                else
                { // right
                    x = newX + xOffset;                        
                }

                // out-of-bounds
                if (x < 0 || x >= grid.XDim) { break; }


                // piece is the same color?
                if (grid.Pieces[x, newY].IsColored() && grid.Pieces[x, newY].ColorComponent.Color == color)
                {
                    //to make sure does not get miss any match just because the upper piece is moving 
                    if (grid.Pieces[x, newY].IsMovable() && grid.Pieces[x, newY].MovableComponent.IsRunning()) return null;
                    if (grid.Pieces[x, newY].IsClearable() && grid.Pieces[x, newY].ClearableComponent.IsBeingCleared) return null;

                    horizontalPieces.Add(grid.Pieces[x, newY]);
                }
                else
                {
                    break;
                }
            }
        }

        if (horizontalPieces.Count >= 3)
        {
            for (int i = 0; i < horizontalPieces.Count; i++)
            {
                matchingPieces.Add(horizontalPieces[i]);
            }
        }

        // Traverse vertically if we found a match (for L and T shape)
        if (horizontalPieces.Count >= 3)
        {
            for (int i = 0; i < horizontalPieces.Count; i++ )
            {
                for (int dir = 0; dir <= 1; dir++)
                {
                    for (int yOffset = 1; yOffset < grid.YDim; yOffset++)                        
                    {
                        int y;
                            
                        if (dir == 0)
                        { // Up
                            y = newY - yOffset;
                        }
                        else
                        { // Down
                            y = newY + yOffset;
                        }

                        if (y < 0 || y >= grid.YDim)
                        {
                            break;
                        }


                        if (grid.Pieces[horizontalPieces[i].X, y].IsColored() && grid.Pieces[horizontalPieces[i].X, y].ColorComponent.Color == color)
                        {
                            if (grid.Pieces[horizontalPieces[i].X, y].IsMovable() && grid.Pieces[horizontalPieces[i].X, y].MovableComponent.IsRunning()) return null;
                            if (grid.Pieces[horizontalPieces[i].X, y].IsClearable() && grid.Pieces[horizontalPieces[i].X, y].ClearableComponent.IsBeingCleared) return null;

                            verticalPieces.Add(grid.Pieces[horizontalPieces[i].X, y]);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (verticalPieces.Count < 2)
                {
                    verticalPieces.Clear();
                }
                else
                {
                    for (int j = 0; j < verticalPieces.Count; j++)
                    {
                        matchingPieces.Add(verticalPieces[j]);
                    }
                    break;
                }
            }
        }

        if (matchingPieces.Count >= 3)
        {
            return matchingPieces;
        }

        // Didn't find anything going horizontally first,
        // so now check vertically
        horizontalPieces.Clear();
        verticalPieces.Clear();
        verticalPieces.Add(piece);

        for (int dir = 0; dir <= 1; dir++)
        {
            for (int yOffset = 1; yOffset < grid.YDim; yOffset++)
            {
                int y;

                if (dir == 0)
                { // Up
                    y = newY - yOffset;
                }
                else
                { // Down
                    y = newY + yOffset;                        
                }

                GamePiece checkPiece;
                // out-of-bounds
                if (y < 0 || y >= grid.YDim) 
                {
                    break; 
                }
                else
                {
                    checkPiece = grid.Pieces[newX, y];
                }
                

                if (checkPiece.IsColored() && checkPiece.ColorComponent.Color == color)
                {
                    if (checkPiece.IsMovable() && checkPiece.MovableComponent.IsRunning()) return null;
                    if (checkPiece.IsClearable() && checkPiece.ClearableComponent.IsBeingCleared) return null;

                    verticalPieces.Add(checkPiece);
                }
                else
                {
                    break;
                }
            }
        }

        if (verticalPieces.Count >= 3)
        {
            for (int i = 0; i < verticalPieces.Count; i++)
            {
                matchingPieces.Add(verticalPieces[i]);
            }
        }

        // Traverse horizontally if we found a match (for L and T shape)
        if (verticalPieces.Count >= 3)
        {
            for (int i = 0; i < verticalPieces.Count; i++)
            {
                for (int dir = 0; dir <= 1; dir++)
                {
                    for (int xOffset = 1; xOffset < grid.YDim; xOffset++)
                    {
                        int x;

                        if (dir == 0)
                        { // Left
                            x = newX - xOffset;
                        }
                        else
                        { // Right
                            x = newX + xOffset;
                        }

                        // if (x < 0 || x >= xDim)
                        GamePiece checkPiece;
                        if (grid.IsOutOfGrid(x, verticalPieces[i].Y))
                        {
                            break;
                        }
                        else
                        {
                            checkPiece = grid.Pieces[x, verticalPieces[i].Y];
                        }


                        if (checkPiece.IsColored() && checkPiece.ColorComponent.Color == color)
                        {
                            if (checkPiece.IsMovable() && checkPiece.MovableComponent.IsRunning()) return null;
                            if (checkPiece.IsClearable() && checkPiece.ClearableComponent.IsBeingCleared) return null;

                            horizontalPieces.Add(checkPiece);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (horizontalPieces.Count < 2)
                {
                    horizontalPieces.Clear();
                }
                else
                {
                    for (int j = 0; j < horizontalPieces.Count; j++)
                    {
                        matchingPieces.Add(horizontalPieces[j]);
                    }
                    break;
                }
            }
        }

        if (matchingPieces.Count >= 3)
        {
            return matchingPieces;
        }

        return null;
    }
    /// <returns>horizontal and vertial piece list</returns>
    public static List<GamePiece> GetMatch(this GridPlay grid, GamePiece piece, int newX, int newY, out List<GamePiece> horizontalPieces, out List<GamePiece> verticalPieces)
    {
        verticalPieces = null;
        horizontalPieces = null;
        if (!piece.IsColored()) return null;
        var color = piece.ColorComponent.Color;
        var matchingPieces = new List<GamePiece>();
        horizontalPieces = new List<GamePiece>();
        verticalPieces = new List<GamePiece>();

        // First check horizontal
        horizontalPieces.Add(piece);

        for (int dir = 0; dir <= 1; dir++)
        {
            for (int xOffset = 1; xOffset < grid.XDim; xOffset++)
            {
                int x;

                if (dir == 0)
                { // Left
                    x = newX - xOffset;
                }
                else
                { // right
                    x = newX + xOffset;                        
                }

                // out-of-bounds
                if (x < 0 || x >= grid.XDim) { break; }


                // piece is the same color?
                if (grid.Pieces[x, newY].IsColored() && grid.Pieces[x, newY].ColorComponent.Color == color)
                {
                    //to make sure does not get miss any match just because the upper piece is moving 
                    if (grid.Pieces[x, newY].IsMovable() && grid.Pieces[x, newY].MovableComponent.IsRunning()) return null;
                    if (grid.Pieces[x, newY].IsClearable() && grid.Pieces[x, newY].ClearableComponent.IsBeingCleared) return null;

                    horizontalPieces.Add(grid.Pieces[x, newY]);
                }
                else
                {
                    break;
                }
            }
        }

        if (horizontalPieces.Count >= 3)
        {
            for (int i = 0; i < horizontalPieces.Count; i++)
            {
                matchingPieces.Add(horizontalPieces[i]);
            }
        }

        // Traverse vertically if we found a match (for L and T shape)
        if (horizontalPieces.Count >= 3)
        {
            for (int i = 0; i < horizontalPieces.Count; i++ )
            {
                for (int dir = 0; dir <= 1; dir++)
                {
                    for (int yOffset = 1; yOffset < grid.YDim; yOffset++)                        
                    {
                        int y;
                            
                        if (dir == 0)
                        { // Up
                            y = newY - yOffset;
                        }
                        else
                        { // Down
                            y = newY + yOffset;
                        }

                        if (y < 0 || y >= grid.YDim)
                        {
                            break;
                        }


                        if (grid.Pieces[horizontalPieces[i].X, y].IsColored() && grid.Pieces[horizontalPieces[i].X, y].ColorComponent.Color == color)
                        {
                            if (grid.Pieces[horizontalPieces[i].X, y].IsMovable() && grid.Pieces[horizontalPieces[i].X, y].MovableComponent.IsRunning()) return null;
                            if (grid.Pieces[horizontalPieces[i].X, y].IsClearable() && grid.Pieces[horizontalPieces[i].X, y].ClearableComponent.IsBeingCleared) return null;

                            verticalPieces.Add(grid.Pieces[horizontalPieces[i].X, y]);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (verticalPieces.Count < 2)
                {
                    verticalPieces.Clear();
                }
                else
                {
                    for (int j = 0; j < verticalPieces.Count; j++)
                    {
                        matchingPieces.Add(verticalPieces[j]);
                    }
                    break;
                }
            }
        }

        if (matchingPieces.Count >= 3)
        {
            return matchingPieces;
        }

        // Didn't find anything going horizontally first,
        // so now check vertically
        horizontalPieces.Clear();
        verticalPieces.Clear();
        verticalPieces.Add(piece);

        for (int dir = 0; dir <= 1; dir++)
        {
            for (int yOffset = 1; yOffset < grid.YDim; yOffset++)
            {
                int y;

                if (dir == 0)
                { // Up
                    y = newY - yOffset;
                }
                else
                { // Down
                    y = newY + yOffset;                        
                }

                GamePiece checkPiece;
                // out-of-bounds
                if (y < 0 || y >= grid.YDim) 
                {
                    break; 
                }
                else
                {
                    checkPiece = grid.Pieces[newX, y];
                }
                

                if (checkPiece.IsColored() && checkPiece.ColorComponent.Color == color)
                {
                    if (checkPiece.IsMovable() && checkPiece.MovableComponent.IsRunning()) return null;
                    if (checkPiece.IsClearable() && checkPiece.ClearableComponent.IsBeingCleared) return null;

                    verticalPieces.Add(checkPiece);
                }
                else
                {
                    break;
                }
            }
        }

        if (verticalPieces.Count >= 3)
        {
            for (int i = 0; i < verticalPieces.Count; i++)
            {
                matchingPieces.Add(verticalPieces[i]);
            }
        }

        // Traverse horizontally if we found a match (for L and T shape)
        if (verticalPieces.Count >= 3)
        {
            for (int i = 0; i < verticalPieces.Count; i++)
            {
                for (int dir = 0; dir <= 1; dir++)
                {
                    for (int xOffset = 1; xOffset < grid.YDim; xOffset++)
                    {
                        int x;

                        if (dir == 0)
                        { // Left
                            x = newX - xOffset;
                        }
                        else
                        { // Right
                            x = newX + xOffset;
                        }

                        // if (x < 0 || x >= xDim)
                        GamePiece checkPiece;
                        if (grid.IsOutOfGrid(x, verticalPieces[i].Y))
                        {
                            break;
                        }
                        else
                        {
                            checkPiece = grid.Pieces[x, verticalPieces[i].Y];
                        }


                        if (checkPiece.IsColored() && checkPiece.ColorComponent.Color == color)
                        {
                            if (checkPiece.IsMovable() && checkPiece.MovableComponent.IsRunning()) return null;
                            if (checkPiece.IsClearable() && checkPiece.ClearableComponent.IsBeingCleared) return null;

                            horizontalPieces.Add(checkPiece);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (horizontalPieces.Count < 2)
                {
                    horizontalPieces.Clear();
                }
                else
                {
                    for (int j = 0; j < horizontalPieces.Count; j++)
                    {
                        matchingPieces.Add(horizontalPieces[j]);
                    }
                    break;
                }
            }
        }

        if (matchingPieces.Count >= 3)
        {
            return matchingPieces;
        }

        return null;
    }

    public static bool CanMoveDown (this GridPlay grid, int x, int y)
    {
        if (grid.IsOutOfGrid(x, y + 1)) return false;
        for (var newY = y; newY < grid.YDim - 1; newY++)
        {
            //move down directly
            if (grid.CanMoveDirectDown(x, newY)) return true;

            if (!grid.Pieces[x, newY + 1].IsMovable()) break;
        }

        return false;
    }

    public static bool IsOutOfGrid(this GridPlay grid, int x = 0, int y = 0)
    {
        return x >= grid.XDim || x < 0 || y > grid.GetLowestRow(x) || y < grid.GetHighestRow(x);
        // return column >= grid.XDim || column < 0 || row >= grid.YDim || row < 0;
    }

    public static List<GamePiece> GetColor (this GridPlay grid, ColorType color) 
    {
        var pieces = new List<GamePiece>();

        for (int x = 0; x < grid.XDim; x++)
        {
            for (int y = 0; y < grid.YDim; y++)
            {                
                Vector2Int xy = grid.GetGridPosition(grid.Pieces[x, y].transform.position);
                if (grid.IsOutOfGrid(xy.x, xy.y))  continue;

                if (color == ColorType.ANY)
                {
                    //do not get piece has any color
                    if (grid.Pieces[x, y].IsColored() && grid.Pieces[x, y].ColorComponent.Color == ColorType.ANY)
                        continue;
                    pieces.Add(grid.Pieces[x, y]);
                }
                else if (((grid.Pieces[x, y].IsColored() && grid.Pieces[x, y].ColorComponent.Color == color))// || (color == ColorType.ANY)) 
                    && grid.CanClear(x, y, true))
                {
                    pieces.Add(grid.Pieces[x, y]);
                }
            }
        }

        return pieces;
    }

    public static bool CanClear (this GridPlay grid, int x, int y, bool isIgnoreRunning = false)
    {
        if (!grid.Pieces[x, y].IsClearable() || grid.Pieces[x, y].ClearableComponent.IsBeingCleared || !grid.Pieces[x, y].isActiveAndEnabled) return false;
        if (!isIgnoreRunning && grid.Pieces[x, y].IsMovable() && grid.Pieces[x, y].MovableComponent.IsRunning()) return false;
        
        //can not clear piece if it's out of grid
        Vector2Int xy = grid.GetGridPosition(grid.Pieces[x, y].transform.position);
        if (grid.IsOutOfGrid(xy.x, xy.y)) return false;

        return true;
    }

    public static int GetHighestRow (this GridPlay grid, int x)
    {
        try
        {
            return grid.Columns[x].highest;
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning($"column {x} is not exist in {grid.name}\n" + ex.StackTrace);
            return 0;
        }
    }

    public static int GetLowestRow (this GridPlay grid, int x)
    {
        try
        {
            return grid.Columns[x].lowest;
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning($"column {x} is not exist in {grid.name}\n" + ex.StackTrace);
            return grid.YDim - 1;
        }
    }

    public static List<GamePiece> GetPiecesOfType(this GridPlay grid, PieceType type)
    {
        var piecesOfType = new List<GamePiece>();

        for (int x = 0; x < grid.XDim; x++)
        {
            for (int y = 0; y < grid.YDim; y++)
            {
                if (grid.Pieces[x, y].Type == type)
                {
                    piecesOfType.Add(grid.Pieces[x, y]);
                }
            }
        }

        return piecesOfType;
    }
}
