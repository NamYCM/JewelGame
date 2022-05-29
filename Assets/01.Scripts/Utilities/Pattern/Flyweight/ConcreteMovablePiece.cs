public class ConcreteMovablePiece
{
    public string StartMoveAnimationTrigger { get; private set; }
    public string TouchGroundAnimationTrigger { get; private set; }
    public string StartMoveAnimationName { get; private set; }

    public ConcreteMovablePiece()
    {
        StartMoveAnimationTrigger = "StartMove";
        TouchGroundAnimationTrigger = "EndMove"; 
        StartMoveAnimationName = "BeginMove";
    }
}

static class MovablePieceFactory 
{
    private static ConcreteMovablePiece movablePieceInfor;

    public static ConcreteMovablePiece GetMovablePieceInfor ()
    {
        if (movablePieceInfor == null) return new ConcreteMovablePiece();
        
        return movablePieceInfor;
    }
}