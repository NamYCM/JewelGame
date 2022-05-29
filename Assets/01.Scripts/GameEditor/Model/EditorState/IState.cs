using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IState
{
    void Update ();
    void Enter ();
    void Exit ();
}
