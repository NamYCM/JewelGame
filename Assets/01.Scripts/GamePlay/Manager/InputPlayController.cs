using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputPlayController : MonoBehaviour
{
    PlayerInput inputActions;
    GamePiece pressPiece, enterPiece;

    bool isEnable = true;

    private void OnDestroy() {
        if (inputActions == null) return;

        #if (UNITY_STANDALONE_WIN || UNITY_WEBGL)
            inputActions.GamePlay.MouseLeftClick.started -= OnStarted;
            inputActions.GamePlay.MouseLeftClick.canceled -= OnCanceled;
        #elif (UNITY_IOS || UNITY_ANDROID)
            inputActions.GamePlay.TouchPress.started -= OnStarted;
            inputActions.GamePlay.TouchPress.canceled -= OnCanceled;
        #else
            throw new System.Exception("the platform is not valid");
        #endif

        inputActions.Disable();
    }

    private GamePiece GetPiece ()
    {
        Vector2 ray;
        #if (UNITY_STANDALONE_WIN || UNITY_WEBGL)
            ray = Camera.main.ScreenToWorldPoint(inputActions.GamePlay.MousePosition.ReadValue<Vector2>());
        #elif (UNITY_IOS || UNITY_ANDROID)
            ray = Camera.main.ScreenToWorldPoint(inputActions.GamePlay.TouchPosition.ReadValue<Vector2>());
        #else
            throw new System.Exception("the platform is not valid");
        #endif

        // Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hitSelection = Physics2D.Raycast(ray, Vector2.zero);
        if (hitSelection.collider != null && hitSelection.collider.TryGetComponent<GamePiece>(out var piece))
        {
            return piece;
        }

        return null;
    }

    public bool IsEnable () => isEnable == true;

    private void OnCanceled(InputAction.CallbackContext ct)
    {
        if (!IsEnable()) return;

        //swap piece
        enterPiece = GetPiece();

        if (pressPiece && pressPiece.IsChoosable())
        {
            pressPiece.ChoosePieceComponent.TurnOffChoose();
        }

        if (enterPiece)
        {
            //if user clicked the item...
            if (_selectedItem && !enterPiece.IsSpecialPiece())
            {
                //upgrade the piece
                enterPiece.UpgradeSkill(_selectedItem.SpecialType);

                //update data into database
                Data.UseItemOfUser(_selectedItem.SpecialType);
                //update data at local
                Data.ReduceItemInLocal(_selectedItem.SpecialType);
                _selectedItem.UpdateAmount();

                ResetSeletedItem();
                return;
            }

            enterPiece.GridRef.EnterPiece(enterPiece);
            enterPiece.GridRef.ReleasePiece();
            if (enterPiece.IsChoosable())
                enterPiece.ChoosePieceComponent.TurnOffChoose();
        }
    }

    private void OnStarted(InputAction.CallbackContext ct)
    {
        if (!IsEnable()) return;

        //set first piece
        pressPiece = GetPiece();
        if (pressPiece)
        {
            pressPiece.GridRef.PressPiece(pressPiece);
            if (pressPiece.IsChoosable())
            {
                pressPiece.ChoosePieceComponent.TurnOnChoose();
            }
        }
    }

    public void Init() {
        inputActions = new PlayerInput();

        #if UNITY_STANDALONE_WIN
            inputActions.GamePlay.MouseLeftClick.started += OnStarted;
            inputActions.GamePlay.MouseLeftClick.canceled += OnCanceled;
        #elif (UNITY_IOS || UNITY_ANDROID)
            inputActions.GamePlay.TouchPress.started += OnStarted;
            inputActions.GamePlay.TouchPress.canceled += OnCanceled;
        #else
            throw new System.Exception("the platform is not valid");
        #endif

        inputActions.GamePlay.MouseRightClick.started += OnRightClick;
        inputActions.Enable();
    }

    void OnRightClick (InputAction.CallbackContext context)
    {
        if (!IsEnable()) return;

        //set first piece
        pressPiece = GetPiece();
        if (pressPiece && !pressPiece.IsSpecialPiece())
        {
            pressPiece.UpgradeSkill(PieceType.COLUMN_CLEAR);
        }
    }

    public void DisableInput()
    {
        isEnable = false;
    }

    public void EnableInput()
    {
        isEnable = true;
    }

    UIUserItem _selectedItem = null;
    private void ResetSeletedItem ()
    {
        _selectedItem = null;
    }

    public void ChooseSkill (UIUserItem item)
    {
        _selectedItem = item;
    }
}
