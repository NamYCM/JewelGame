using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UIItemController : MonoBehaviour
{
    [SerializeField] GameObject _userItemPrefab;
    [SerializeField] Transform _userItemsArea;

    public void Init() {
        try
        {
            foreach (var item in Data.GetItemsInShop())
            {
                var userItem = Instantiate(_userItemPrefab, _userItemsArea).GetComponent<UIUserItem>();
                userItem.SetItemData(new UIShopWindow.ShopItem(){
                    SpecialType = item.Key,
                    Icon = Resources.Load<Sprite>(item.Value.IconPath)
                });
                userItem.Init();

                // var buttonItem = userItem.GetComponentInChildren<CustomButton>();
                // buttonItem.OnDown.AddListener(() => {
                //     GameManager.Instance.InputPlayController.ChooseSkill(userItem);
                // });
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("data didn't init already\n" + ex);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
