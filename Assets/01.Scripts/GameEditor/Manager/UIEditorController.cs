using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIEditorController : MonoBehaviour {
    [SerializeField] Dropdown gridList;
    [SerializeField] InputField columnInput;
    [SerializeField] InputField rowInput;
    [SerializeField] InputField angleInput;
    [SerializeField] Toggle isConnectToggle;
    [SerializeField] Dropdown originGridList;
    [SerializeField] Dropdown targetGridList;
    [SerializeField] Text editorMode;
    [SerializeField] UIModelWindow modelWindow;
    [SerializeField] private GameObject _helpContainer;
    
    GameEditorSystem gameEditorSystem;
    UILevelEditor levelEditor;

    public Dropdown OriginGridList => originGridList;
    public Dropdown TargetGridList => targetGridList;
    public UIModelWindow ModelWindow => modelWindow;
    public UILevelEditor UILevelEditor => levelEditor;

    private void Awake() {
        levelEditor = FindObjectOfType<UILevelEditor>();
    }

    private void Start() {
        gameEditorSystem = GameEditorSystem.Instance;
    }

    public void HandleOnHelpButtonClick()
    {
        if (_helpContainer == null)
            return;
        
        _helpContainer.SetActive(!_helpContainer.activeInHierarchy);
    }

    private void ResetGridInfor ()
    {
        columnInput.text = null;
        rowInput.text = null;
        angleInput.text = null;
        isConnectToggle.isOn = false;
    }

    private void ResetGridList ()
    {
        gridList.ClearOptions();
    }

    public void UpdateGridInfor (GridDisplay currentGrid)
    {
        
        if (!currentGrid)
        {
            levelEditor.ResetLevelInfor();
            ResetGridInfor();
            return;
        }
        columnInput.text = currentGrid.XDim.ToString();
        rowInput.text = currentGrid.YDim.ToString();
        angleInput.text = currentGrid.Angle.ToString();
    }

    public void ResetUI ()
    {
        levelEditor.ResetLevelInfor();
        ResetGridList();
        ResetGridInfor();
        isConnectToggle.isOn = false;
    }

    public void AddGrid (GridDisplay grid)
    {
        gridList.options.Add(new Dropdown.OptionData(grid.name));
        gridList.value = -1;
        gridList.value = gridList.options.Count - 1;

    }

    public void DeleteGrid (string name)
    {
        bool isFind = false;
        for (int gridCount = 0; gridCount < gridList.options.Count; gridCount ++)
        {
            if (gridList.options[gridCount].text == name)
            {
                gridList.value = gridCount - 1;
                gridList.options.RemoveAt(gridCount);
                isFind = true;
                gridCount --;
            }
            else if (isFind)
            {
                gridList.options[gridCount].text = GridUtility.ChangeGridName(gridList.options[gridCount].text, gridCount);
            }
        }

        if (gridList.options.Count == 0)
            gridList.ClearOptions();
    }

    public string GetOption (int index)
    {
        return gridList.options[index].text;
    }

    public string GetOriginOption (int index) 
    {
        return originGridList.options[index].text;
    }

    public string GetTargetOption (int index) 
    {
        return targetGridList.options[index].text;
    }

    public string GetCurrentOption ()
    {
        return gridList.options.Count != 0 ? gridList.options[gridList.value].text : null;
    }

    public void UpdateOriginGridListOption()
    {
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

        foreach (var optionElement in gridList.options)
        {
            // if (targetGridList.options.Count > 0 && optionElement == targetGridList.options[targetGridList.value]) continue;
            options.Add(optionElement);
        }

        originGridList.options = options;
        
            // originGridList.value = -1;
        if(gameEditorSystem.OriginGrid == null) 
        {
            originGridList.value = -1;
        }
        else
        {
            for (var i = 0; i < originGridList.options.Count; i++)
            {
                if (originGridList.options[i].text == gameEditorSystem.OriginGrid.name)
                {
                    if(originGridList.value != i) originGridList.value = (i != 0) ? i : -1;
                    break;
                }
            }
        }
    }

    public void UpdateTargetGridListOption ()
    {
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

        foreach (var optionElement in gridList.options)
        {
            if (originGridList.options.Count > 0 && optionElement == originGridList.options[originGridList.value]) continue;
            options.Add(optionElement);
        }

        targetGridList.options = options;

        // targetGridList.value = -1;
        if(gameEditorSystem.TargetGrid == null)
        {
            targetGridList.value = -1;
        } 
        else
        {
            for (var i = 0; i < targetGridList.options.Count; i++)
            {
                if (targetGridList.options[i].text == gameEditorSystem.TargetGrid.name)
                {
                    if(targetGridList.value != i) targetGridList.value = (i != 0) ? i : -1;
                    break;
                }
                else if (i == targetGridList.options.Count - 1)
                {
                    targetGridList.value = -1;
                }
            }
        }
    }

    public void ChangeConnectMode ()
    {
        originGridList.interactable = targetGridList.interactable = isConnectToggle.isOn;

        if (isConnectToggle.isOn) 
        {
            UpdateOriginGridListOption();
            
            UpdateTargetGridListOption();
        }
    }

    public void TurnOffConnectToggle ()
    {
        isConnectToggle.isOn = false;
    }

    public void ChangeEditorMode (string mode) 
    {
        editorMode.text = mode;
    }
}