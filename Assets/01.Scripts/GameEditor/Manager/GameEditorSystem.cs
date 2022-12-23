using UnityEngine;
using UnityEngine.UI;

public class GameEditorSystem : MonoBehaviour
{
    public static GameEditorSystem Instance;
    
    [SerializeField] Button _deleteMapButton;
 
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

    string email, password, verifyPassword, code;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        gridDisplayCreator = GetComponent<GridDisplayCreator>();
        uiEditor = GetComponent<UIEditorController>();
        inputController = GetComponent<InputController>();
        fileManager = GetComponent<FileManager>();

        //load level data
        // uiEditor.ModelWindow.StartBuild.SetLoadingWindow(true).SetTitle("Loading data level").Show();
        // StartCoroutine(Data.InitLevelData(() => {
        //     uiEditor.ModelWindow.Close();
        // }, (message) => {
        //     uiEditor.ModelWindow.StartBuild.SetLoadingWindow(false).SetTitle("Error").SetMessage(message).Show();
        // }));
        Data.InitLevelData(() => {
            // uiEditor.ModelWindow.Close();
            UILoader.Instance.Close();
        }, (message) => {
            Debug.LogError("load level data failed \n" + message);
            UILoader.Instance.OnEndLoading.AddListener(() => {
                uiEditor.ModelWindow.StartBuild.SetLoadingWindow(false).SetTitle("Error").SetMessage(message).Show();
            });
            UILoader.Instance.Close();
            // uiEditor.ModelWindow.StartBuild.SetLoadingWindow(false).SetTitle("Error").SetMessage(message).Show();
        });

        DeactiveDeleteMapButton();
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

    public void LoadHomeScene ()
    {
        uiEditor.ModelWindow.StartBuild.SetTitle("Confirm").SetMessage("Are you sure to log out?").OnConfirmAction(() => {
            UILoader.Instance.LoadScene("LoginAdmin");
        }).OnDeclineAction(() => {}).Show();
    }

    void SignUpAdmin ()
    {
        // uiEditor.ModelWindow.OnEndCloseActionCoroutine(this, () => {
        uiEditor.ModelWindow.StartBuild.SetTitle("Sign Up Admin")
            .SetInputField1("Email...", email)
            .SetInputField2("Password...", password, TMPro.TMP_InputField.ContentType.Password)
            .SetInputField3("Verify Password...", verifyPassword, TMPro.TMP_InputField.ContentType.Password)
            .SetInputField4("Code...", code, TMPro.TMP_InputField.ContentType.IntegerNumber, 6)
            .OnConfirmAction(() => {
                email = uiEditor.ModelWindow.GetInputField1();
                password = uiEditor.ModelWindow.GetInputField2();
                verifyPassword = uiEditor.ModelWindow.GetInputField3();
                code = uiEditor.ModelWindow.GetInputField4();

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password)
                    || string.IsNullOrWhiteSpace(verifyPassword) || string.IsNullOrWhiteSpace(code))
                {
                    uiEditor.ModelWindow.OnEndCloseActionCoroutine(this, () => {
                        uiEditor.ModelWindow.StartBuild.SetTitle("Sign Up Failed")
                            .SetMessage("Don't let the input field is blank!!!")
                            .OnEndCloseActionCoroutine(this, SignUpAdmin).Show();
                    });
                    return;
                }

                if (password.Trim().CompareTo(verifyPassword.Trim()) != 0)
                {
                    uiEditor.ModelWindow.OnEndCloseActionCoroutine(this, () => {
                        uiEditor.ModelWindow.StartBuild.SetTitle("Sign Up Failed")
                            .SetMessage("password and verify password does not match")
                            .OnEndCloseActionCoroutine(this, SignUpAdmin).Show();
                    });
                    return;
                }

                // sign up admin
                DataAdmin admin = new DataAdmin(email, password, code);
                uiEditor.ModelWindow.OnEndCloseAction(() => {
                    uiEditor.ModelWindow.StartBuild.SetTitle("Siging Up").SetMessage("Please wait...").SetLoadingWindow(true).Show();

                    APIAccessObject.Instance.StartCoroutine(APIAccesser.SignUpAdminCoroutine(admin, () => {
                        email = password = verifyPassword = code = null;
                        uiEditor.ModelWindow.OnEndCloseAction(() => {
                            uiEditor.ModelWindow.StartBuild.SetTitle("Congratulations!!!").SetMessage("Sign up succesful").Show();
                        });
                        uiEditor.ModelWindow.Close();
                    }, (message) => {
                        uiEditor.ModelWindow.OnEndCloseActionCoroutine(this, () => {
                            uiEditor.ModelWindow.StartBuild.SetTitle("Sign Up Failed").SetMessage(message).OnEndCloseActionCoroutine(this, SignUpAdmin).Show();
                        });
                        uiEditor.ModelWindow.Close();
                    }));
                });
            }).OnDeclineAction(() => {
                email = uiEditor.ModelWindow.GetInputField1();
                password = uiEditor.ModelWindow.GetInputField2();
                verifyPassword = uiEditor.ModelWindow.GetInputField3();
                code = uiEditor.ModelWindow.GetInputField4();
            }).Show();
        // });
    }

    public void RegisterNewAdminAccount ()
    {
        uiEditor.ModelWindow.StartBuild.SetTitle("Are you sure to create new admin account?").SetMessage("You will be recieved a verify code in you email").OnConfirmAction(() => {
            //send verify code
            APIAccessObject.Instance.StartCoroutine(APIAccesser.SendVerifyGmail(null, (message) => {
                Debug.LogError(message);
            }));
            uiEditor.ModelWindow.OnEndCloseActionCoroutine(this, () => {
                SignUpAdmin();
            });
        }).OnDeclineAction(() => {}).Show();
    }

    public void ActiveDeleteMapButton ()
    {
        _deleteMapButton.interactable = true;
    }

    public void DeactiveDeleteMapButton ()
    {
        _deleteMapButton.interactable = false;
    }

    public void DeleteMap ()
    {
        if (fileManager.Map == null)
        {
            // show error window
        }
        else
        {
            uiEditor.ModelWindow.StartBuild.SetMessage("Are you sure to delete this map?")
            .OnDeclineAction(() => {})
            .OnConfirmAction(() => {
                if (!uint.TryParse(fileManager.Map.name, out uint levelNumber))
                {
                    uiEditor.ModelWindow.StartBuild.OnEndCloseAction(() => {
                        uiEditor.ModelWindow.StartBuild.SetTitle("Error")
                        .SetMessage($"level's name {fileManager.Map.name} is not valid").Show();
                    });
                    return;
                }

                //show deleting window after the current window closed
                uiEditor.ModelWindow.StartBuild.OnEndCloseAction(() => {
                    uiEditor.ModelWindow.StartBuild.SetTitle("Deleting").SetLoadingWindow(true).Show();
                });

                gridDisplayCreator.DeleteLevel(levelNumber, () => {
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
            }).Show();
        }
    }
}
