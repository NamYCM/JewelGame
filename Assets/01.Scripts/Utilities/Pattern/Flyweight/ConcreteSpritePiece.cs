using UnityEngine;

public class ConcreteSpritePiece
{
    public string HiddenLayer { get; private set; }
    public string NormalLayer { get; private set; }
    
    public Color DefaultColor { get; private set; }

    public ConcreteSpritePiece()
    {
        HiddenLayer = "HidenJewel";
        NormalLayer = "Jewel"; 
        DefaultColor = new Color(1,1,1,1);
    }
}

static class SpritePieceFactory 
{
    private static ConcreteSpritePiece spritePiece;

    public static ConcreteSpritePiece GetSpritePiece ()
    {
        if (spritePiece == null) return new ConcreteSpritePiece();
        
        return spritePiece;
    }
}