using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridUtility
{
    public static Vector2Int GetGridPosition (this Grid grid, Vector2 position)
    {
        //translate to coordinate angle
        position.x -= grid.transform.position.x;
        position.y -= grid.transform.position.y;

        //rotate arround O
        // float angleInRadian = -grid.transform.rotation.eulerAngles.z * Mathf.PI / 180;
        float angleInRadian = -grid.Angle * Mathf.PI / 180;
        float rotateX = (position.x * Mathf.Cos(angleInRadian) - position.y * Mathf.Sin(angleInRadian));
        float rotateY = (position.x * Mathf.Sin(angleInRadian) + position.y * Mathf.Cos(angleInRadian));
        Vector2 afterRotate = new Vector2(rotateX, rotateY);

        //translate to origin position
        Vector2 result = new Vector2(afterRotate.x + grid.transform.position.x, afterRotate.y + grid.transform.position.y);

        return new Vector2Int(System.Convert.ToInt16(result.x - grid.transform.position.x + grid.XDim / 2.0f - 0.5f),
                            System.Convert.ToInt16(-result.y + grid.transform.position.y + grid.YDim / 2.0f -0.5f));
    }

    public static Vector2 GetWorldPosition(this Grid grid, int column, int row)
    {
        Vector2 originPosition = new Vector2(grid.transform.position.x - grid.XDim / 2.0f + (column + 0.5f),
                                             grid.transform.position.y + grid.YDim / 2.0f - (row + 0.5f));

        //translate to coordinate angle
        originPosition.x -= grid.transform.position.x;
        originPosition.y -= grid.transform.position.y;

        //rotate arround O
        // float angleInRadian = grid.transform.rotation.eulerAngles.z * Mathf.PI / 180;
        float angleInRadian = grid.Angle * Mathf.PI / 180;
        float rotateX = (originPosition.x * Mathf.Cos(angleInRadian) - originPosition.y * Mathf.Sin(angleInRadian));
        float rotateY = (originPosition.x * Mathf.Sin(angleInRadian) + originPosition.y * Mathf.Cos(angleInRadian));
        Vector2 afterRotate = new Vector2(rotateX, rotateY);

        //translate to origin position
        Vector2 result = new Vector2(afterRotate.x + grid.transform.position.x, afterRotate.y + grid.transform.position.y);

        return result;
    }
 
    public static string ChangeGridName (string name, int targetIndex)
    {
        var saveName = name.Split(' ');
        
        try
        {
            return saveName[0] + " " + (int.Parse(saveName[1]) - 1);
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning(ex.Message + "\n" + ex.StackTrace);
            return "";
        }
    }
}
