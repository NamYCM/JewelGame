using UnityEngine;

public class LevelSelectManager : SingleSubject<LevelSelectManager>
{
    LevelButtonCreater _levelButtonCreater;
    public LevelButtonCreater LevelButtonCreater => _levelButtonCreater;

    private SnapController _snapController;
    public SnapController SnapController => _snapController;

    [SerializeField] UIModelWindow _modelWindow;
    public UIModelWindow ModelWindow => _modelWindow;

    [SerializeField] UIShopWindow _shopWindow;
    public UIShopWindow ShopWindow => _shopWindow;

    private void Awake() {
        _snapController = FindObjectOfType<SnapController>();
        _levelButtonCreater = FindObjectOfType<LevelButtonCreater>();

        try
        {
            _shopWindow.Init();
        }
        catch (System.Exception)
        {
             // TODO
        }
    }

    private void Start()
    {
        _levelButtonCreater.Init();
        _snapController.SelectPage(_levelButtonCreater.GetPageConstanceCurrentLevel());
    }
}
