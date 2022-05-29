using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectGrid : IState
{
    ConnectControl originConnect, targetConnect;
    PlayerInput playerInput;

    public ConnectGrid(ConnectControl originConnect, ConnectControl targetConnect, PlayerInput playerInput)
    {
        this.originConnect = originConnect;
        this.targetConnect = targetConnect;
        this.playerInput = playerInput;
    }

    public void Enter()
    {
        //TODO change to choose bottom for origin and choose top for target (invert to old algorithm)
        originConnect.InitControl(playerInput, true);
        targetConnect.InitControl(playerInput, false);
    }

    public void Exit()
    {
        originConnect.CancleControl();
        targetConnect.CancleControl();
    }

    public void Update()
    {
    }

    public void CancleOriginControl ()
    {
        originConnect.CancleControl();
    }

    public void CancleTargetControl ()
    {
        targetConnect.CancleControl();
    }
}
