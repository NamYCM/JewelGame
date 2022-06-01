using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShopWindow : MonoBehaviour
{
    [System.Serializable]
    public struct ShopItem
    {
        public Sprite Icon;
        public uint Price;
        public PieceType SpecialType;
    }

    [SerializeField] TextMeshProUGUI _money;

    [SerializeField] List<ShopItem> _shopItems = new List<ShopItem>();

    [SerializeField] Transform _userItemArea;
    [SerializeField] UIUserItem _userItemPrefab;
    [SerializeField] Transform _shopItemArea;
    [SerializeField] UIShopItem _shopItemPrefab;

    //To instantate effect when user buy a item
    [SerializeField] GameObject _iconItemPrefab;

    UIAnimateCompoment _animCompoment;

    //because the dictionary can not save the data in editor, we need to use array to save
    [SerializeField] UIUserItem[] _userItemsArray;
    [SerializeField] UIShopItem[] _shopItemsArray;
    Dictionary<PieceType, UIUserItem> _userItemsDictionary;
    Dictionary<PieceType, UIShopItem> _shopItemsDictionary;

#region Properties
    public List<ShopItem> ShopItems => _shopItems;
    public Transform UserItemArea => _userItemArea;
    public UIUserItem UserItemPrefab => _userItemPrefab;
    public Transform ShopItemArea => _shopItemArea;
    public UIShopItem ShopItemPrefab => _shopItemPrefab;
#endregion

    public void Init (Action onSucessfulLoad = null, Action onFailedLoad = null)
    {
        LoadItemDataFromFirestore((dataShop) => {
            Data.InitShopData(dataShop);

            InitItems(_shopItems);

            _animCompoment = GetComponentInChildren<UIAnimateCompoment>(true);
            _animCompoment.OnCompleteActionDisable.AddListener(() => {
                gameObject.SetActive(false);
            });

            //init dictionaries
            _userItemsDictionary = new Dictionary<PieceType, UIUserItem>();
            foreach (var item in _userItemsArray)
            {
                _userItemsDictionary.Add(item.SpecialType, item);
            }

            _shopItemsDictionary = new Dictionary<PieceType, UIShopItem>();
            foreach (var item in _shopItemsArray)
            {
                _shopItemsDictionary.Add(item.SpecialType, item);
            }

            UpdateUserData();
            onSucessfulLoad?.Invoke();
        }, onFailedLoad);
    }

    void UpdateUserMoney ()
    {
        _money.text = Data.GetMoneyOfUser().ToString();
    }

    void UpdateUserItem ()
    {
        foreach (var userItem in _userItemsDictionary)
        {
            userItem.Value.UpdateAmount();
        }
    }

    private void UpdateUserData ()
    {
        UpdateUserMoney();
        UpdateUserItem();
    }

    public void Open ()
    {
        UpdateUserData();
        gameObject.SetActive(true);
        // StartCoroutine(Test());
        _animCompoment.Show();
    }

    // IEnumerator Test ()
    // {
    //     yield return new WaitForEndOfFrame();
    //     LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    // }

    public void Close ()
    {
        _animCompoment.Disable();

        //ensure does not any hack while buy
        Data.ReloadUserDataFromFirestore();
    }


    void SetUserItems (UIUserItem[] userItems)
    {
        _userItemsArray = userItems;
    }

    void SetShopItems (UIShopItem[] shopItems)
    {
        _shopItemsArray = shopItems;
    }

    /// <summary>Change apprearance of user's money in shop by using dotween</summary>
    public void ChangeUserMoney (uint money)
    {
        DOTween.To(() => uint.Parse(_money.text),
        (targetMoney) => {
            _money.text = targetMoney.ToString();
        }, money, 0.5f);
    }

    public void AddItemAmount (UIShopItem item)
    {
        //instate tempItem and set paramaters
        var tempItem = Instantiate(_iconItemPrefab, item.Icon.transform.position, Quaternion.identity, transform);
        tempItem.GetComponent<Image>().sprite = item.Icon.sprite;
        tempItem.transform.localScale = item.Icon.transform.localScale;

        //animate tempItem
        tempItem.transform.DOMove(_userItemsDictionary[item.SpecialType].Icon.transform.position, 0.5f);
        var rect = tempItem.GetComponent<RectTransform>();
        var beginSize = rect.sizeDelta;
        var endSize = _userItemsDictionary[item.SpecialType].Icon.GetComponent<RectTransform>().sizeDelta;
        DOTween.To(() => beginSize,
        (targetSize) => {
            rect.sizeDelta = targetSize;
        }, endSize, 0.5f).OnComplete(() => {
            Destroy(tempItem);
            _userItemsDictionary[item.SpecialType].IncreteAmount();
        });
    }

    /// <summary>init the items in shop</summary>
    public void InitItems (List<UIShopWindow.ShopItem> initItems)
    {
        //instatiate shop items
        var shopItems = _shopItemArea.GetComponentsInChildren<UIShopItem>();

        if (shopItems.Length <= initItems.Count)
        {
            for (var itemCount = 0; itemCount < initItems.Count; itemCount++)
            {
                if (itemCount < shopItems.Length)
                {
                    shopItems[itemCount].SetItemData(initItems[itemCount]);
                }
                else
                {
                    Instantiate<UIShopItem>(_shopItemPrefab, _shopItemArea).SetItemData(initItems[itemCount]);
                }
            }
        }
        else
        {
            for (var itemCount = 0; itemCount < shopItems.Length; itemCount++)
            {
                if (itemCount < initItems.Count)
                {
                    // if (!items[itemCount].IsSame(initItems[itemCount]))
                        shopItems[itemCount].SetItemData(initItems[itemCount]);
                }
                else
                {
                    DestroyImmediate(shopItems[itemCount]);
                }
            }
        }

        //instatiate user items
        var userItems = _userItemArea.GetComponentsInChildren<UIUserItem>();

        if (userItems.Length <= initItems.Count)
        {
            for (var itemCount = 0; itemCount < initItems.Count; itemCount++)
            {
                if (itemCount < userItems.Length)
                {
                    userItems[itemCount].SetItemData(initItems[itemCount]);
                }
                else
                {
                    Instantiate(_userItemPrefab, _userItemArea).SetItemData(initItems[itemCount]);
                }
            }
        }
        else
        {
            for (var itemCount = 0; itemCount < userItems.Length; itemCount++)
            {
                if (itemCount < initItems.Count)
                {
                    userItems[itemCount].SetItemData(initItems[itemCount]);
                }
                else
                {
                    DestroyImmediate(userItems[itemCount]);
                }
            }
        }

        SetShopItems(_shopItemArea.GetComponentsInChildren<UIShopItem>());
        SetUserItems(_userItemArea.GetComponentsInChildren<UIUserItem>());
    }

    /// <summary>load item data and save into ShopItems param</summary>
    public void LoadItemDataFromFirestore (Action<DataShop> onSucessfulLoad, Action onFailedLoad)
    {
        APIAccessObject.Instance.StartCoroutine(APIAccesser.GetAllItemsCoroutine((dataShop) => {
            _shopItems.Clear();
            foreach (var item in dataShop.shopItems)
            {
                _shopItems.Add(new UIShopWindow.ShopItem(){
                    Price = item.Value.Price,
                    SpecialType = item.Key,
                    Icon = Resources.Load<Sprite>(item.Value.IconPath)
                });
            }

            onSucessfulLoad?.Invoke(dataShop);
            // shopWindow.InitItems(shopWindow.ShopItems);
            //to save changed objects
            // EditorUtility.SetDirty(shopWindow);
        }, (message) => {
            onFailedLoad?.Invoke();
            throw new Exception(message);
        }));
    }
}
