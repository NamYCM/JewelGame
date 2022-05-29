using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Grid : MonoBehaviour
{
    protected int xDim = 0;
    protected int yDim = 0;
    protected int angle = 0;

    public abstract int XDim
    {
        get; 
        set;
    }
    public abstract int YDim
    {
        get;
        set;
    }
    public abstract int Angle
    {
        get;
        set;
    }
}
