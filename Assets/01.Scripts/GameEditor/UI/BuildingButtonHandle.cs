using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButtonHandle : MonoBehaviour
{
    [SerializeField] BuildingPiece piece;
    public BuildingPiece Piece => piece;
    
    Button button;

    // Start is called before the first frame update
    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);
    }

    private void ButtonClicked ()
    {
        GameEditorSystem.Instance.SetBuildingPiece(piece);
    }
}
