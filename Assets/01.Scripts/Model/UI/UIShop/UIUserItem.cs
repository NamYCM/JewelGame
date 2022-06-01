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

    CustomButton _button;

    public void Init ()
    {
        _button = GetComponentInChildren<CustomButton>();
        _button.OnDown.AddListener(() => {
            GameManager.Instance.InputPlayController.ChooseSkill(this);
        });

        UpdateAmount();
    }

    public void UpdateAmount ()
    {
        var newAmount = Data.GetAmountOfSpecialPiece(_specialType);
        _amount.text = newAmount.ToString();
        if (newAmount == 0 && _button)
        {
            _button.Disable();
        }
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
