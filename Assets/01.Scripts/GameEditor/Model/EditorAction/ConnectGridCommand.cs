public class ConnectGridCommand : Command
{
    GameEditorSystem gameEditorSystem;
    GridDisplayCreator.ConnectGrid origin, target;

    public ConnectGridCommand(GridDisplayCreator.ConnectGrid origin, GridDisplayCreator.ConnectGrid target)
    {
        gameEditorSystem = GameEditorSystem.Instance;
        this.origin = origin;
        this.target = target;
    }

    public override void Execute()
    {
        origin.grid.ConnectGrid(target.grid, origin.column, target.column);
    }

    public override void Undo()
    {
        origin.grid.DisconnectGridAtColumn(origin.column);
    }
}

public class UnconnectGridCommand : Command
{
    GameEditorSystem gameEditorSystem;
    GridDisplayCreator.ConnectGrid origin, target;

    public UnconnectGridCommand(GridDisplay originGrid, int originColumn)
    {
        gameEditorSystem = GameEditorSystem.Instance;
        this.origin.grid = originGrid;
        this.origin.column = originColumn;
    }

    public override void Execute()
    {
        target = origin.grid.DisconnectGridAtColumn(origin.column);
    }

    public override void Undo()
    {
        origin.grid.ConnectGrid(target.grid, origin.column, target.column);
    }
}
