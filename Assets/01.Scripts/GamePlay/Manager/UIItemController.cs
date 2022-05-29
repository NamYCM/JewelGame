using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UIItemController : MonoBehaviour
{
    [SerializeField] GameObject _userItemPrefab;
    [SerializeField] Transform _userItemsArea;

    public void Init() {
        foreach (var item in Data.GetItemsInShop())
        {
            var userItem = Instantiate(_userItemPrefab, _userItemsArea).GetComponent<UIUserItem>();
            userItem.SetItemData(new UIShopWindow.ShopItem(){
                SpecialType = item.Key,
                //TODO move all icon into resources folder and use Resources.Load instead
                Icon = AssetDatabase.LoadAssetAtPath<Sprite>(item.Value.IconPath)
            });
            userItem.UpdateAmount();

            var buttonItem = userItem.GetComponentInChildren<CustomButton>();
            buttonItem.OnDown.AddListener(() => {
                GameManager.Instance.InputPlayController.ChooseSkill(userItem);
            });
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
