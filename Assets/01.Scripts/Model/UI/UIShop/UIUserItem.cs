using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UIShopWindow;

public class UIUserItem : MonoBehaviour
{
    [SerializeField] Image _icon;
    [SerializeField] TextMeshProUGUI _amount;

    [SerializeField] PieceType _specialType;

    public PieceType SpecialType => _specialType;
    public Image Icon => _icon;

    public void UpdateAmount ()
    {
        _amount.text = Data.GetAmountOfSpecialPiece(_specialType).ToString();
    }

    public void IncreteAmount ()
    {
        _amount.text = (uint.Parse(_amount.text) + 1).ToString();
    }

    public void SetItemData (ShopItem itemData)
    {
        //set ratio
        var size = itemData.Icon.bounds.size;
        var heigh = transform.localScale.y < size.y ? transform.localScale.y : size.y;
        _icon.transform.localScale = new Vector2(heigh * size.x / size.y, heigh);

        _icon.sprite = itemData.Icon;

        _amount.text = "0";

        _specialType = itemData.SpecialType;
    }
}
