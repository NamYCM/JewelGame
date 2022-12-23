﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridCreater : MonoBehaviour
{
    [SerializeField] GameObject gridPrefab;    
    List<BuildingMap.GridData> gridsData;
    GridPlay[] grids;

    public GridPlay[] Grids => grids;

    public void InitGrids (List<BuildingMap.GridData> gridsData)
    {
        this.gridsData = gridsData;

        InstanteGrids();
        ConnectAndInitGrids();
    }

    private GridPlay[] InstanteGrids ()
    {
        grids = new GridPlay[gridsData.Count];

        //Instantiate grids
        for (var countGrid = 0; countGrid < gridsData.Count; countGrid++)
        {
            GridPlay tempGrid = Instantiate(gridPrefab, Vector3.zero, transform.rotation, transform).GetComponent<GridPlay>();
            BuildingMap.GridData data = gridsData[countGrid];
            
            tempGrid.name = data.name;
            tempGrid.transform.position = data.position;
            tempGrid.transform.rotation = data.rotation;

            tempGrid.GetComponent<Spawner>().SetLossFertilityColumnData(data.lossFertilityColumns);
            //set static for grid
            tempGrid.gameObject.isStatic = true;

            grids[countGrid] = tempGrid;
        }

        return grids;
    }

    //Add connect grid component and init grid
    private void ConnectAndInitGrids ()
    {
        foreach (var gridElement in gridsData)
        {
            // GridPlay currentGrid = GameObject.Find(name + "/" + gridElement.Name).GetComponent<GridPlay>();
            GridPlay currentGrid = grids.Where(grid => grid.name == gridElement.name).ToArray()[0];

            if (gridElement.targetGrids.Count > 0)
            {
                var connectGridComponent = currentGrid.gameObject.AddComponent<ConnectGridComponent>();
                connectGridComponent.InitTargets(gridElement.targetGrids);
            }

            currentGrid.Init(gridElement.x, gridElement.y, gridElement.initialPieces);
        }
    }
}
