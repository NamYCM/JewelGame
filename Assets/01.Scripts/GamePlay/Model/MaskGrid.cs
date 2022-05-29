using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskGrid : MonoBehaviour
{
    public GameObject MaskPrefab;

    private void SpawnMask (Vector3 position, Quaternion quaternion)
    {
        Instantiate(MaskPrefab, position, quaternion, this.transform);
    }

    public void Init(int x, int y, Quaternion rotate)
    {
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localScale = new Vector3(x, y, 1);
        transform.rotation = rotate;
    }
    public void Init(GridPlay grid, Quaternion rotate)
    {
        for (int x = 0; x < grid.XDim; x++)
        {
            for (int y = 0; y < grid.YDim; y++)
            {
                if (grid.Pieces[x, y].Type != PieceType.NULL)
                {
                    SpawnMask(grid.GetWorldPosition(x, y), rotate);
                }                
            }
        }
    }
}
