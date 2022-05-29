using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ChoosePiece : MonoBehaviour
{
    SpriteRenderer sprite;
    Color choose = new Color(1, 1, 1, 1), unChoose = new Color(1, 1, 1, 0);

    private void Awake() {
        sprite = GetComponent<SpriteRenderer>();

        TurnOffChoose();
    }

    public void TurnOnChoose () 
    {
        sprite.color = choose;
    }

    public void TurnOffChoose ()
    {
        sprite.color = unChoose;
    }
}
