using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataShop
{
    [System.Serializable]
    public struct ShopItem
    {
        public string IconPath;
        public uint Price;
        // public PieceType SpecialType;
    }

    public Dictionary<PieceType, ShopItem> shopItems = new Dictionary<PieceType, ShopItem>();

    public DataShop () {}
}
