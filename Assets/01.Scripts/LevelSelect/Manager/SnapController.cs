using System.Collections;
using System.Collections.Generic;
using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine;

public class SnapController : MonoBehaviour
{
    [SerializeField] GameObject levelGridPrefab;
    [SerializeField] GameObject paginationPrefab;

    LevelSelectManager levelSelectManager;
    SimpleScrollSnap snap;
    
    private void Awake() {
        levelSelectManager = LevelSelectManager.Instance;
        snap = GetComponentInChildren<SimpleScrollSnap>(); 
    }

    private void Start() {
        snap.onPanelChanged.AddListener(() => {
            levelSelectManager.LevelButtonCreater.LoadPage(snap.CurrentPanel);
        });
    }

    private void AddPagination ()
    {
        Instantiate(paginationPrefab, snap.pagination.transform);
    }

    public void AddFrontPage ()
    {
        AddPagination();
        snap.AddToFront(levelGridPrefab);
    }

    public void AddBackPage ()
    {
        AddPagination();
        snap.AddToBack(levelGridPrefab);
    }

    public int NumberOfPage ()
    {
        return snap.NumberOfPanels;
    }

    /// <summary>get panel</summary>
    /// <param name="number">number of panel in array</param>
    public GameObject GetPageObject (int number) 
    {
        return snap.Panels[number];
    }

    public void SelectPage (int number) 
    {
        if (number == snap.CurrentPanel)
        {
            //when enter the game and current level is in the first page
            //we need to load this page because it'll not go into onPanelChanged event
            levelSelectManager.LevelButtonCreater.LoadPage(snap.CurrentPanel);
            return;
        }
        
        snap.GoToPanel(number);
    }
}
