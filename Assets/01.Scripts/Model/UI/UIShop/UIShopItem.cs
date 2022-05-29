using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UIShopWindow;

public class UIShopItem : SubjectMono
{
    [SerializeField] Image _icon;
    [SerializeField] TextMeshProUGUI _price;

    [SerializeField] PieceType _specialType;

    UIShopWindow shopWindow;

    public PieceType SpecialType => _specialType;
    public Image Icon => _icon;
    public uint Price => uint.Parse(_price.text);

    private void Awake() {
        shopWindow = GetComponentInParent<UIShopWindow>();
    }

    void UpdateUserMoney ()
    {
        shopWindow.ChangeUserMoney((uint)Data.GetMoneyOfUser());
    }

    void UpdateUserItem ()
    {
        shopWindow.AddItemAmount(this);
    }

    public void OnBuyClicked ()
    {
        if (Price > Data.GetMoneyOfUser())
        {
            LevelSelectManager.Instance.ModelWindow.StartBuild.SetTitle("Unfortunately!!").SetMessage("Your money is not enough").SetIcon(_icon.sprite).Show();
            return;
        }

        Data.BuyItemForUser(this);
        Data.ReduceMoneyInLocal(Price);
        Data.AddItemInLocal(this);

        UpdateUserMoney();
        UpdateUserItem();
    }

    public void SetItemData (ShopItem itemData)
    {
        //set ratio
        var size = itemData.Icon.bounds.size;
        var heigh = transform.localScale.y < size.y ? transform.localScale.y : size.y;
        _icon.transform.localScale = new Vector2(heigh * size.x / size.y, heigh);

        _icon.sprite = itemData.Icon;
        _price.text = itemData.Price.ToString();

        //to save properties
        // Undo.RecordObject(gameObject, "set speical type");
        // PrefabUtility.RecordPrefabInstancePropertyModifications(gameObject);

        _specialType = itemData.SpecialType;
    }
}
