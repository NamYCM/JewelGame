using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridPlay))]
public class ConnectGridComponent : MonoBehaviour
{
    [System.Serializable]
    public struct TargetData
    {
        public GridPlay grid;
        public int x;

        public TargetData (GridPlay grid, int column) 
        {
            this.grid = grid;
            this.x = column;
        }
    }

    GridPlay originGrid;

    private Dictionary<int, TargetData> targets;
    public Dictionary<int, TargetData> Targets => targets;

    private void Awake() {
        originGrid = GetComponent<GridPlay>();

        targets = new Dictionary<int, TargetData>();
    }

    public void InitTargets (List<BuildingMap.ConnectData> connectDatas)
    {
        foreach (var data in connectDatas)
        {
            var grid = GameObject.Find(data.TargetGridName).GetComponent<GridPlay>();
            if (grid == null) throw new System.NullReferenceException($"missing grid play in {data.TargetGridName}");

            foreach (var item in data.ConnectColumns)
            {
                if (!grid.Spawner)
                    throw new System.NullReferenceException($"missing spawner in {data.TargetGridName}");
                
                grid.Spawner.SetConnectColumn(originGrid, item.originColumn, item.targetColumn);

                targets.Add(item.originColumn, new TargetData(grid, item.targetColumn));
            }
        }
    }

    public bool IsConnect (int x)
    {
        return targets != null && targets.ContainsKey(x);
    }
}
