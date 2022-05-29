﻿using UnityEngine;

public class GameManager : SingleSubject<GameManager>
{
    [SerializeField] BuildingMap map;

    [SerializeField] UIModelWindow modelWindow;
    public UIModelWindow ModelWindow => modelWindow;

    GameState stateBefore;
    [SerializeField] GameState currentState = GameState.WaitInput;

    //map creater
    GridCreater gridCreater;
    LevelCreater levelCreater;

    InputPlayController inputPlayController;
    UIItemController m_UIItemController;
    // Level level;
    HUD hud;

    public GridPlay[] Grids => gridCreater.Grids;

    public InputPlayController InputPlayController => inputPlayController;
    public Level Level => levelCreater.Level;

    public GameState CurrentState
    {
        get { return currentState; }
        // If new state different to old state, change gameStateBefore
        private set {
            if (value != currentState)
                stateBefore = currentState;
            currentState = value;
        }
    }

    public HUD HUD => hud;

    private void Awake() {
        CurrentState = GameState.Loading;

        map = Data.GetMapInfor(Data.GetCurrentLevel());

        gridCreater = GetComponentInChildren<GridCreater>();
        levelCreater = GetComponentInChildren<LevelCreater>();
        inputPlayController = GetComponent<InputPlayController>();
        m_UIItemController = FindObjectOfType<UIItemController>();
        hud = FindObjectOfType<HUD>();

        inputPlayController.Init();
        m_UIItemController.Init();

        //init level before init grid, because grid will refer to level
        levelCreater.InitLevel(map);
        gridCreater.InitGrids(map.Grids);

        hud.Init();
    }

    private void Start() {
        //TODO caculate the cell of grids
        PiecePool.Instance.InitReadyPiece(30);

        StartGame();
    }

    // start game
    private void StartGame ()
    {
        CurrentState = GameState.Filling;

        //start level before start fill grids
        levelCreater.StartLevel();

        foreach (var gridElement in Grids)
        {
            gridElement.StartFill();
        }
    }

    private void ChangeToWaitInput ()
    {
        foreach (var grid in Grids)
        {
            if (grid.IsFilling)
                return;
        }

        CurrentState = GameState.WaitInput;
        inputPlayController.EnableInput();

        SendMessage(GameState.WaitInput);
    }
    private void ChangeToFilling ()
    {
        if (CurrentState == GameState.Filling) return;

        CurrentState = GameState.Filling;
        inputPlayController.DisableInput();
    }

    private void ChangeToEndGame ()
    {
        CurrentState = GameState.EndGame;

        if(inputPlayController.IsEnable()) inputPlayController.DisableInput();

        SendMessage(GameState.EndGame);
    }

    public void ChangeGameState (GameState state)
    {
        switch (state)
        {
            case GameState.WaitInput:
                ChangeToWaitInput();
                break;
            case GameState.Filling:
                ChangeToFilling();
                break;
            case GameState.EndGame:
                ChangeToEndGame();
                break;
            default:
                break;
        }
    }

    public enum GameState
    {
        Loading,
        Filling,
        WaitInput,
        EndGame
    }
}
