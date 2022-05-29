using System.Collections.Generic;
using UnityEngine;

public sealed class PiecePool : SingletonMono<PiecePool>
{
    [System.Serializable]
    public struct PiecePrefab
    {
        public PieceType type;
        public GameObject prefab;
    };

    [SerializeField] private PiecePrefab[] piecePrefabs;

    Dictionary<PieceType, GamePiece> piecePrefabDict;
    Dictionary<PieceType, Queue<GamePiece>> pieceQueueDict;

    private void Awake() {
        pieceQueueDict = new Dictionary<PieceType, Queue<GamePiece>>();

        for (int i = 0; i < piecePrefabs.Length; i++)
        {
            if (!pieceQueueDict.ContainsKey(piecePrefabs[i].type))
            {
                pieceQueueDict.Add(piecePrefabs[i].type, new Queue<GamePiece>());
            }
        }

        piecePrefabDict = new Dictionary<PieceType, GamePiece>();

        for (int i = 0; i < piecePrefabs.Length; i++)
        {
            if (!piecePrefabDict.ContainsKey(piecePrefabs[i].type))
            {
                piecePrefabDict.Add(piecePrefabs[i].type, piecePrefabs[i].prefab.GetComponent<GamePiece>());
            }
        }
    }

    private void GeneratePiece(PieceType type, int amount)
    {
        for (int count = 0; count < amount; count ++)
        {
            GamePiece gamePiece = Instantiate(piecePrefabDict[type], this.transform);

            if(type != PieceType.EMPTY) gamePiece.gameObject.SetActive(false);
            
            pieceQueueDict[type].Enqueue(gamePiece);
        }
    }

    /// <summary>init normal and empty piece rely on cell amount</summary>
    public void InitReadyPiece (int cell)
    {
        if (cell <= 0) return;
        GeneratePiece(PieceType.NORMAL, cell);
    }

    public GamePiece GetPiece(PieceType type, Vector3 position, Quaternion rotate)
    {
        try
        {
            if (pieceQueueDict[type].Count == 0)
            {
                GeneratePiece(type, 1);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("missing prefab in piece pool \n" +ex.StackTrace);
        }

        GamePiece piece = pieceQueueDict[type].Dequeue();

        piece.transform.position = position;
        
        if(type != PieceType.EMPTY) piece.gameObject.SetActive(true);

        return piece;
    }

    public void ReturnToPool(GamePiece gamePiece)
    {
        if (gamePiece.IsIgnored()) gamePiece.ResetIgnore();


        //empty piece doesn't need to disactive, this thing will optimize performance
        if(gamePiece.Type != PieceType.EMPTY) gamePiece.gameObject.SetActive(false);

        if (gamePiece.IsMovable() && gamePiece.MovableComponent.IsRunning())
        {
            gamePiece.MovableComponent.Destroy();
        }
        pieceQueueDict[gamePiece.Type].Enqueue(gamePiece);
    }
}
