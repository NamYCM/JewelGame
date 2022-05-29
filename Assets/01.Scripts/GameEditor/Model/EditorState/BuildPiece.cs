using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPiece : IState
{
    BuildingControl buildingControl;
    BuildingPiece buildingPiece;
    PlayerInput playerInput;

    public BuildPiece (BuildingControl buildingControl, BuildingPiece buildingPiece, PlayerInput playerInput)
    {
        this.buildingControl = buildingControl;
        this.buildingPiece = buildingPiece;
        this.playerInput = playerInput;
    }

    public void Enter()
    {
        buildingControl.InitControl(playerInput, buildingPiece);
    }

    public void Exit()
    {
        buildingControl.CancleControl();
    }

    public void Update()
    {
    }
}
