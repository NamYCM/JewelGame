using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(UIShopWindow))]
public class UIShopWindowEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if (GUILayout.Button("Load items to firestore"))
        {
            var shopWindow = target as UIShopWindow;
            var initItems = shopWindow.ShopItems;
            DataShop data = new DataShop();

            foreach (var item in initItems)
            {
                data.shopItems.Add(item.SpecialType, new DataShop.ShopItem(){
                    Price = item.Price,
                    IconPath =  AssetDatabase.GetAssetPath(item.Icon).Substring(17).Split('.')[0]
                });
            }

            APIAccessObject.Instance.StartCoroutine(APIAccesser.LoadAllItemsCoroutine(data));
        }

        if (GUILayout.Button("Init shop items from local"))
        {
            var shopWindow = target as UIShopWindow;
            var initItems = shopWindow.ShopItems;

            shopWindow.InitItems(initItems);
            //to save changed objects
            EditorUtility.SetDirty(shopWindow);
        }

        if (GUILayout.Button("Init shop items from firebase"))
        {
            var shopWindow = target as UIShopWindow;
            // APIAccessObject.Instance.StartCoroutine(APIAccesser.GetAllItemsCoroutine((dataShop) => {
            //     shopWindow.ShopItems.Clear();
            //     foreach (var item in dataShop.shopItems)
            //     {
            //         // shopWindow.ShopItems.Add(new UIShopWindow.ShopItem(){
            //         //     Price = item.Price,
            //         //     SpecialType = item.SpecialType,
            //         //     //TODO load all icon into resources folder and use Resources.Load instead
            //         //     Icon = AssetDatabase.LoadAssetAtPath<Sprite>(item.IconPath)
            //         // });
            //         shopWindow.ShopItems.Add(new UIShopWindow.ShopItem(){
            //             Price = item.Value.Price,
            //             SpecialType = item.Key,
            //             //TODO load all icon into resources folder and use Resources.Load instead
            //             Icon = AssetDatabase.LoadAssetAtPath<Sprite>(item.Value.IconPath)
            //         });
            //     }

            //     shopWindow.InitItems(shopWindow.ShopItems);
            //     //to save changed objects
            //     EditorUtility.SetDirty(shopWindow);
            // }, null));
            shopWindow.LoadItemDataFromFirestore((dataShop) => {
                shopWindow.InitItems(shopWindow.ShopItems);
                EditorUtility.SetDirty(shopWindow);
            }, null);
        }
    }

}