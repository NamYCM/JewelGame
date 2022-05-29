using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "BuildingPiece", menuName = "Build/BuildingPiece", order = 0)]
public class BuildingPiece : ScriptableObject {
    public PieceType Type;
    public GameObject Prefab;
}
