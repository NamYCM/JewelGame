using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DanielLochner.Assets.SimpleScrollSnap;

public class LevelButtonCreater : MonoBehaviour
{
    public const int BUTTON_AMOUNT_A_PAGE = 18;

    [SerializeField] GameObject levelButtonPrefab;
    LevelSelectManager levelSelectManager;

    Dictionary<int, List<UILevelButton>> levelButtons = new Dictionary<int, List<UILevelButton>>();

    private void Awake() {
        levelSelectManager = LevelSelectManager.Instance;
    }

    private int GetMaxLevel ()
    {
        return Data.MaxLevel();
    }

    private int GetCurrentLevel ()
    {
        return Data.GetCurrentLevel();
    }

    public int GetPageConstanceCurrentLevel ()
    {
        return (GetCurrentLevel() - 1) / BUTTON_AMOUNT_A_PAGE;
    }

    private bool IsEnoughPage ()
    {
        return levelSelectManager.SnapController.NumberOfPage() * 1.0f >= GetMaxLevel() * 1.0f / BUTTON_AMOUNT_A_PAGE;
    }

    private void InstantButtonPreviousPage (int number)
    {
        if (number <= 0) return;

        InstantButtonInPage(number - 1);
    }

    private void InstantButtonNextPage (int number)
    {
        if (number + 1 >= levelSelectManager.SnapController.NumberOfPage()) return;

        InstantButtonInPage(number + 1);
    }

    /// <param name="number">number of page, start by 0</param>
    private void InstantButtonInPage (int number)
    {
        if (number < 0)
            throw new System.IndexOutOfRangeException($"page {number} is invalute");

        //this page was loaded already
        if (levelButtons.ContainsKey(number)) return;

        //caculate min level in that page
        int minButtonLevel = BUTTON_AMOUNT_A_PAGE * number;

        int level;
        int maxLevel = GetMaxLevel();

        //enshrine level buttons
        levelButtons.Add(number, new List<UILevelButton>());

        //load level button
        for (var buttonLevel = 0; buttonLevel < BUTTON_AMOUNT_A_PAGE; buttonLevel++)
        {
            level = buttonLevel + minButtonLevel + 1;
            if (level > maxLevel)
                break;

            UILevelButton button = Instantiate(levelButtonPrefab, levelSelectManager.SnapController.GetPageObject(number).transform).GetComponent<UILevelButton>();
            button.Init(level);
            levelButtons[number].Add(button);
            // Instantiate(levelButtonPrefab, levelSelectManager.SnapController.GetPageObject(number).transform)
            //     .GetComponent<UILevelButton>().Init(level);
        }
    }

    private void CreatePages ()
    {
        while (!IsEnoughPage())
        {
            //create page
            levelSelectManager.SnapController.AddBackPage();
        }

        // int currentPage = GetCurrentPage();

        // LoadPreviousPage(currentPage);
        // LoadPage(currentPage);
        // LoadNextPage(currentPage);

        // levelSelectManager.SnapController.SelectPage(currentPage);
    }

    public void Init ()
    {
        CreatePages();
    }

    /// <summary>use to init information of one page</summary>
    private void InitButtonAnimationInPage (int number)
    {
        if (!levelButtons.ContainsKey(number))
            throw new System.NullReferenceException($"buttons in page {number} was not instanted already");

        foreach (var button in levelButtons[number])
        {
            button.StartChainAnimation();
        }
    }

    public void LoadPage (int number)
    {
        //instance all button
        InstantButtonInPage(number);
        InstantButtonNextPage(number);
        InstantButtonPreviousPage(number);

        //init information
        InitButtonAnimationInPage(number);
    }
}
