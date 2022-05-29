using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveGrid : IState
{
    MoveableControl moveableControl;
    PlayerInput playerInput;

    public MoveGrid (MoveableControl moveableControl, PlayerInput playerInput)
    {
        this.moveableControl = moveableControl;
        this.playerInput = playerInput;
    }

    public void Enter() 
    { 
        moveableControl.InitControl(playerInput);
    }

    public void Exit() 
    { 
        moveableControl.CancleControl();
    }

    public void Update() { }

    public void Test() {}
}
