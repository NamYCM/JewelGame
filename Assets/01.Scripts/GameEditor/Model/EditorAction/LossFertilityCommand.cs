public class LossFertilityCommand : Command
{
    GridDisplay gridDisplay;
    int column;

    public LossFertilityCommand(GridDisplay gridDisplay, int column)
    {
        this.gridDisplay = gridDisplay;
        this.column = column;
    }

    public override void Execute()
    {
        gridDisplay.InitLossFertilityColumn(column);
    }

    public override void Undo()
    {
        gridDisplay.RemoveLossFertilityColumn(column);
    }
}

public class RemoveLossFertilityCommand : Command
{
    GridDisplay gridDisplay;
    int column;

    public RemoveLossFertilityCommand(GridDisplay gridDisplay, int column)
    {
        this.gridDisplay = gridDisplay;
        this.column = column;
    }

    public override void Execute()
    {
        gridDisplay.RemoveLossFertilityColumn(column);
    }

    public override void Undo()
    {
        gridDisplay.InitLossFertilityColumn(column);
    }
}
