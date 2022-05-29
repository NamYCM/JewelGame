using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APIAccessObject : SingletonMono<APIAccessObject>
{
    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }
}
