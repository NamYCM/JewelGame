using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public static InputController Instance;

    PlayerInput playerInput;

    GameEditorSystem gameEditorSystem;

    UIEditorController UIEditor;

    Stack<Command> commands = new Stack<Command>();
    Stack<Command> undoCommands = new Stack<Command>();

    IState currentState;

    DoNothing emptyState = new DoNothing();

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        UIEditor = GetComponent<UIEditorController>();

        playerInput = new PlayerInput();
        playerInput.Enable();
        playerInput.GameEditor.MButtonClick.performed += OnMButtonClick;
        playerInput.GameEditor.UndoClick.performed += OnUndo;
        playerInput.GameEditor.RedoClick.performed += OnRedo;
        playerInput.GameEditor.FButtonClick.performed += OnFButtonClick;

        currentState = emptyState;
    }

    private void Start() {
        gameEditorSystem = GameEditorSystem.Instance;

        currentState.Enter();
    }

    private void OnDestroy() {
        currentState.Exit();
        
        playerInput.Disable();
        playerInput.GameEditor.MButtonClick.performed -= OnMButtonClick;
        playerInput.GameEditor.UndoClick.performed -= OnUndo;
        playerInput.GameEditor.RedoClick.performed -= OnRedo;
        playerInput.GameEditor.FButtonClick.performed -= OnFButtonClick;
    }
    
    private bool IsMoveGridState (IState state)
    {
        return state != null && state.GetType() == typeof(MoveGrid);
    }

    private bool IsConnectGridState (IState state)
    {
        return state != null && state.GetType() == typeof(ConnectGrid);
    }

    private bool IsBuildPieceState (IState state)
    {
        return state != null && state.GetType() == typeof(BuildPiece);
    }
    
    private Command GetCommand ()
    {
        return commands.Pop();
    }

    private Command GetUndoCommand ()
    {
        return undoCommands.Pop();
    }

    private void OnUndo (InputAction.CallbackContext ct)
    {
        if (commands.Count == 0) return;

        var command = GetCommand();
        command.Undo();

        undoCommands.Push(command);
    }

    private void OnRedo (InputAction.CallbackContext ct)
    {
        if (undoCommands.Count == 0) return;

        var command = GetUndoCommand();
        command.Execute();

        commands.Push(command);
    }

    private void ChangeEditorState(IState targetState)
    {
        if (targetState == null) return;

        currentState.Exit();

        if (IsConnectGridState(currentState) && !IsConnectGridState(targetState))
            UIEditor.TurnOffConnectToggle();

        if (IsMoveGridState(targetState))
        {
            if (IsMoveGridState(currentState))
            {
                currentState = emptyState;
            }
            else if (GameEditorSystem.Instance.CurrentGrid.IsMoveableControl())
            {
                currentState = targetState;
            }
        }
        else
            currentState = targetState;
        
        currentState.Enter();

        UIEditor.ChangeEditorMode(currentState.GetType().ToString());
    }

    private void OnMButtonClick(InputAction.CallbackContext ct)
    {
        ChangeEditorMode(EditorMode.Move);
    }

    private void OnFButtonClick(InputAction.CallbackContext ct)
    {
        ChangeEditorMode(EditorMode.Fertility);
    }

    public void ChangeGrid (GridDisplay gridAfter)
    {
        IState targetState = null;
        
        if (!gridAfter) 
        {
            targetState = new DoNothing();
        }
        else if (IsMoveGridState(currentState)) 
        {
            targetState = new MoveGrid(gridAfter.MoveableControl, playerInput);
            //turn off move
            ChangeEditorState(targetState);
        }
        else if (IsBuildPieceState(currentState))
        {
            targetState = new BuildPiece(gridAfter.BuildingControl, GameEditorSystem.Instance.CurrentPiece, playerInput);
        }
        else 
        {
            targetState = new DoNothing();
        }

        ChangeEditorState(targetState);
    }

    public void ChangeEditorMode (EditorMode targetMode)
    {
        if (targetMode == EditorMode.Move && GameEditorSystem.Instance.CurrentGrid.IsMoveableControl())
        {
            ChangeEditorState(new MoveGrid(GameEditorSystem.Instance.CurrentGrid.MoveableControl, playerInput));
        }
        else if (targetMode == EditorMode.Build && GameEditorSystem.Instance.CurrentGrid.IsBuildingControl())
        {
            ChangeEditorState(new BuildPiece(GameEditorSystem.Instance.CurrentGrid.BuildingControl,
                                            GameEditorSystem.Instance.CurrentPiece, playerInput));
        }
        else if (targetMode == EditorMode.Connect &&
        GameEditorSystem.Instance.OriginGrid && GameEditorSystem.Instance.OriginGrid.IsConnectControl() &&
        GameEditorSystem.Instance.TargetGrid && GameEditorSystem.Instance.TargetGrid.IsConnectControl())
        {
            ChangeEditorState(new ConnectGrid(GameEditorSystem.Instance.OriginGrid.ConnectControl,
                                                GameEditorSystem.Instance.TargetGrid.ConnectControl, playerInput));
        }
        else if (targetMode == EditorMode.None)
        {
            ChangeEditorState(new DoNothing());
        }
        else if (targetMode == EditorMode.Fertility && GameEditorSystem.Instance.CurrentGrid.IsLossFertilityControl())
        {
            ChangeEditorState(new LossFertility(GameEditorSystem.Instance.CurrentGrid.LossFertilityControl, playerInput));
        }
    }

    public void AddCommand (Command command)
    {
        commands.Push(command);
        undoCommands.Clear();
    }
    
    public enum EditorMode
    {
        None,
        Move,
        Build,
        Connect,
        Fertility
    }
}
