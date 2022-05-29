public class LevelObstacles : Level
{
    public int numMoves;
    public PieceType[] obstacleTypes;

    private const int ScorePerPieceCleared = 1000;
    
    private int _movesUsed = 0;
    private int _numObstaclesLeft;
    
    private void Awake() {
        Type = LevelType.OBSTACLE;
    }

    public override void Init ()
	{
        base.Init();

	    for (int i = 0; i < obstacleTypes.Length; i++)
	    {
            foreach (var grid in gameManager.Grids)
            {
	            _numObstaclesLeft += grid.GetPiecesOfType(obstacleTypes[i]).Count;
            }
	    }
	}

    public override void StartLevel()
    {
        base.StartLevel();

        hud.SetLevelType(Type);
        hud.SetTarget(_numObstaclesLeft);
        hud.SetRemaining(numMoves);
    }

    public override void OnMove()
    {
        base.OnMove();
        
        _movesUsed++;

        hud.SetRemaining(numMoves - _movesUsed);

        if (numMoves - _movesUsed == 0 && _numObstaclesLeft > 0)
        {
            EndGame();
            // GameLose();
        }
    }

    public override void OnPieceCleared(GamePiece piece)
    {
        base.OnPieceCleared(piece);

        for (int i = 0; i < obstacleTypes.Length; i++)
        {
            if (obstacleTypes[i] != piece.Type) continue;
            
            _numObstaclesLeft--;
            hud.SetTarget(_numObstaclesLeft);
            if (_numObstaclesLeft != 0) continue;
            
            currentScore += ScorePerPieceCleared * (numMoves - _movesUsed);
            hud.SetScore(currentScore);
            
            EndGame();
        }
    }

    protected override bool IsWinGame()
    {
        if (_numObstaclesLeft == 0)
            return true;
        else    
            return false;
    }
}
