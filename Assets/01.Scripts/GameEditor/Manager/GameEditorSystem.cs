using UnityEngine;

public class GameEditorSystem : MonoBehaviour
{
    public static GameEditorSystem Instance;
    GridDisplayCreator gridDisplayCreator;
    UIEditorController uiEditor;
    InputController inputController;

    GridDisplay currentGrid, originGrid, targetGrid;
    BuildingPiece currentPiece;

    FileManager fileManager;

    public GridDisplay CurrentGrid => currentGrid;
    public GridDisplay OriginGrid => originGrid;
    public GridDisplay TargetGrid => targetGrid;
    public BuildingPiece CurrentPiece => currentPiece;
    public UIEditorController UIEditor => uiEditor;
    public FileManager FileManager => fileManager;
    public GridDisplayCreator GridDisplayCreator => gridDisplayCreator;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        gridDisplayCreator = GetComponent<GridDisplayCreator>();
        uiEditor = GetComponent<UIEditorController>();
        inputController = GetComponent<InputController>();
        fileManager = GetComponent<FileManager>();

        //load level data
        uiEditor.ModelWindow.StartBuild.SetLoadingWindow(true).SetTitle("Loading data level").Show();
        // StartCoroutine(Data.InitLevelData(() => {
        //     uiEditor.ModelWindow.Close();
        // }, (message) => {
        //     uiEditor.ModelWindow.StartBuild.SetLoadingWindow(false).SetTitle("Error").SetMessage(message).Show();
        // }));
        Data.InitLevelData(() => {
            uiEditor.ModelWindow.Close();
        }, (message) => {
            uiEditor.ModelWindow.StartBuild.SetLoadingWindow(false).SetTitle("Error").SetMessage(message).Show();
        });
    }

    private bool ChangeCurrentGrid (GridDisplay targetGrid)
    {
        if (currentGrid == targetGrid) return false;

        inputController.ChangeGrid(targetGrid);

        currentGrid = targetGrid;
        uiEditor.UpdateGridInfor(currentGrid);
        return true;
    }

    public void CreateNewGrid ()
    {
        GridDisplay tempGrid = gridDisplayCreator.CreateGridDisplay();
        uiEditor.AddGrid(tempGrid);
    }
    private void CreateNewGrid (BuildingMap.GridData gridData)
    {
        GridDisplay tempGrid = gridDisplayCreator.CreateGridDisplay(gridData);
        uiEditor.AddGrid(tempGrid);
    }

    public void DeleteCurrentGrid ()
    {
        if (currentGrid == null) return;

        string tempName = currentGrid.name;
        gridDisplayCreator.DestroyGridDisplay(currentGrid);
        uiEditor.DeleteGrid(tempName);
    }

    public void ChangeGridDropDown (int index)
    {
        GridDisplay tempGrid = gridDisplayCreator.GetGridByName(uiEditor.GetOption(index));
        ChangeCurrentGrid(tempGrid);
    }

    public void ChangeOriginGridDropDown (int index)
    {
        if (originGrid && uiEditor.OriginGridList.options[index].text == originGrid.name)
            return;

        originGrid = gridDisplayCreator.GetGridByName(uiEditor.GetOriginOption(index));
        inputController.ChangeEditorMode(InputController.EditorMode.Connect);
        uiEditor.UpdateTargetGridListOption();
    }

    public void ChangeTargetGridDropDown (int index)
    {
        if (targetGrid && uiEditor.TargetGridList.options[index].text == targetGrid.name)
            return;

        targetGrid = gridDisplayCreator.GetGridByName(uiEditor.GetTargetOption(index));
        inputController.ChangeEditorMode(InputController.EditorMode.Connect);
        uiEditor.UpdateOriginGridListOption();
    }

    public void ChangeColumn (string columnText)
    {
        if (!currentGrid) return;

        int column;
        try
        {
            column = int.Parse(columnText);
        }
        catch (System.Exception)
        {
            return;
        }

        currentGrid.XDim = column;
    }

    public void ChangeRow (string rowText)
    {
        if (!currentGrid) return;

        int row;
        try
        {
            row = int.Parse(rowText);
        }
        catch (System.Exception)
        {
            return;
        }

        currentGrid.YDim = row;
    }

    public void ChangeAngle (string angleText)
    {
        if (!currentGrid) return;

        int angle;
        try
        {
            angle = int.Parse(angleText);
        }
        catch (System.Exception)
        {
            return;
        }

        currentGrid.Angle = angle;
    }

    public void SetBuildingPiece (BuildingPiece piece)
    {
        currentPiece = piece;

        inputController.ChangeEditorMode(InputController.EditorMode.Build);
    }

    public void GenerateScritableLevel ()
    {
        if (fileManager.Map != null)
        {
            uiEditor.ModelWindow.StartBuild.SetMessage("Are you sure?").ChangeAlternateButtonName("Save as a new map")
                .OnAlternateAction(() => {
                    //show loading window after the current window closed
                    uiEditor.ModelWindow.StartBuild.OnEndCloseAction(() => {
                        uiEditor.ModelWindow.StartBuild.SetTitle("Loading").SetLoadingWindow(true).Show();
                    });

                    gridDisplayCreator.GenerateLevel(() => {
                        //get new map data
                        Data.InitLevelDataFromFirebase(() => {
                            //close loading window
                            ResetEditor();
                            uiEditor.ModelWindow.Close();
                        }, (err) => {
                            //show error message
                            uiEditor.ModelWindow.OnEndCloseAction(() => {
                                uiEditor.ModelWindow.StartBuild.SetTitle("error").SetMessage(err).Show();
                            });
                            uiEditor.ModelWindow.Close();
                        });
                    }, (err) => {
                        //show error message
                        uiEditor.ModelWindow.OnEndCloseAction(() => {
                            uiEditor.ModelWindow.StartBuild.SetTitle("error").SetMessage(err).Show();
                        });
                        uiEditor.ModelWindow.Close();
                    });
                })
                .OnConfirmAction(() => {
                    //show loading window after the current window closed
                    uiEditor.ModelWindow.StartBuild.OnEndCloseAction(() => {
                        uiEditor.ModelWindow.StartBuild.SetTitle("Loading").SetLoadingWindow(true).Show();
                    });

                    gridDisplayCreator.SaveLevel(() => {
                        Debug.Log("1");
                        //get new map data
                        Data.InitLevelDataFromFirebase(() => {
                        Debug.Log("2");
                            //close loading window
                            ResetEditor();
                            uiEditor.ModelWindow.Close();
                        }, (err) => {
                        Debug.Log("3");
                            //show error message
                            uiEditor.ModelWindow.OnEndCloseAction(() => {
                                uiEditor.ModelWindow.StartBuild.SetTitle("error").SetMessage(err).Show();
                            });
                            uiEditor.ModelWindow.Close();
                        });
                    }, (err) => {
                        //show error message
                        uiEditor.ModelWindow.OnEndCloseAction(() => {
                            uiEditor.ModelWindow.StartBuild.SetTitle("error").SetMessage(err).Show();
                        });
                        uiEditor.ModelWindow.Close();
                    });
                })
                .OnDeclineAction(() => { return; }).Show();
        }
        else
        {
            uiEditor.ModelWindow.StartBuild.SetMessage("Are you sure?")
                .OnConfirmAction(() => {
                    gridDisplayCreator.GenerateLevel();
                    ResetEditor();
                }).OnDeclineAction(() => { return; }).Show();
        }
    }

    private void ResetEditor ()
    {
        gridDisplayCreator.ResetGrids();
        uiEditor.ResetUI();
        fileManager.ResetData();
    }

    public void ChangeConnectMode (bool isConnectMode)
    {
        uiEditor.ChangeConnectMode();

        if(isConnectMode)
        {
            inputController.ChangeEditorMode(InputController.EditorMode.Connect);
        }
        else
        {
            inputController.ChangeEditorMode(InputController.EditorMode.None);
        }
    }

    public void ChooseColumn (GridDisplay grid, int column, bool isOrigin)
    {
        gridDisplayCreator.ChooseColumnConnect(grid, column, isOrigin);
    }

    public bool CanChooseColumn (bool isOrigin)
    {
        if (isOrigin) return gridDisplayCreator.CanChooseOrigin();
        else return gridDisplayCreator.CanChooseTarget();
    }

    public void ResetChooseColumn ()
    {
        gridDisplayCreator.ResetChooseColumnConnect();
    }

    public void GenerateMap (BuildingMap mapData)
    {
        gridDisplayCreator.ResetGrids();
        uiEditor.ResetUI();

        foreach (var grid in mapData.Grids)
        {
            CreateNewGrid(grid);
        }

        //we have to create all grid display first
        gridDisplayCreator.ConnectGridDisplays(mapData.Grids);

        uiEditor.UILevelEditor.SetLevel(mapData);
    }
}
