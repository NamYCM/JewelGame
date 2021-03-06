// GENERATED AUTOMATICALLY FROM 'Assets/01.Scripts/Input/PlayerInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""GameEditor"",
            ""id"": ""639f846d-0237-4dc8-8010-971f8ff83ddc"",
            ""actions"": [
                {
                    ""name"": ""MouseLeftClick"",
                    ""type"": ""Button"",
                    ""id"": ""77ef7976-001c-4b3b-a519-d7d8f4039b53"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseRightClick"",
                    ""type"": ""Button"",
                    ""id"": ""9ebd324b-11d7-482d-9c4c-e7227e2e05c3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""Value"",
                    ""id"": ""b1e9cb66-50d7-4c4f-9ca8-093e7ebb046b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MButtonClick"",
                    ""type"": ""Button"",
                    ""id"": ""3acee537-5f8a-47d8-a45c-741c524af93c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""UndoClick"",
                    ""type"": ""Button"",
                    ""id"": ""62f7d64e-7d03-492b-a95d-e4c500459af8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RedoClick"",
                    ""type"": ""Button"",
                    ""id"": ""dbf4df0f-a6ce-4755-80ab-b7c44f8fb2e7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""FButtonClick"",
                    ""type"": ""Button"",
                    ""id"": ""4de43f75-212e-4cd8-b341-cc79de94c5fe"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""5c58b68b-6f7b-4050-8309-f5e8193c48c1"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseLeftClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cadeaf6a-6ebf-4cde-a567-773abd5e7770"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseRightClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9154664b-6256-4519-82b0-e010ccf2288e"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d0f103a0-1624-4dff-8260-9e8d484a9aa4"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MButtonClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7a131bda-4b2c-48c4-a2f0-d71d802e2cae"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UndoClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f5b55730-fcd5-4cfd-b444-2b9ed4df1644"",
                    ""path"": ""<Keyboard>/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RedoClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bf4bb52e-2a30-4e45-b975-aec03d6d0469"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FButtonClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""GamePlay"",
            ""id"": ""c6b8229e-89b6-4ba4-b60c-f220ed59e327"",
            ""actions"": [
                {
                    ""name"": ""MouseLeftClick"",
                    ""type"": ""Button"",
                    ""id"": ""6481997d-76ba-45d9-afba-d1a6bf96220b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""Value"",
                    ""id"": ""4f364bc6-5a3a-4821-a340-841b19c2e6d8"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseRightClick"",
                    ""type"": ""Button"",
                    ""id"": ""f06a2d92-05ff-4750-9482-b3a560b123bc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""TouchPress"",
                    ""type"": ""Button"",
                    ""id"": ""6095e670-aa52-48be-8808-570a1ccbf4a0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""TouchPosition"",
                    ""type"": ""Value"",
                    ""id"": ""5ac5a13f-1908-4304-8862-056492fee346"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""114f1763-3be5-4942-9803-c77316515f39"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseLeftClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""633047da-14d0-49d2-89a1-283b969ad285"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e567f69f-2682-4623-836a-cc950b51a1c3"",
                    ""path"": ""<Touchscreen>/touch0/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TouchPress"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ea3b9db8-80e4-45ab-acf7-ff98852c0936"",
                    ""path"": ""<Touchscreen>/primaryTouch/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TouchPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ff8d05e2-5953-4f47-a808-8615f80f1087"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseRightClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // GameEditor
        m_GameEditor = asset.FindActionMap("GameEditor", throwIfNotFound: true);
        m_GameEditor_MouseLeftClick = m_GameEditor.FindAction("MouseLeftClick", throwIfNotFound: true);
        m_GameEditor_MouseRightClick = m_GameEditor.FindAction("MouseRightClick", throwIfNotFound: true);
        m_GameEditor_MousePosition = m_GameEditor.FindAction("MousePosition", throwIfNotFound: true);
        m_GameEditor_MButtonClick = m_GameEditor.FindAction("MButtonClick", throwIfNotFound: true);
        m_GameEditor_UndoClick = m_GameEditor.FindAction("UndoClick", throwIfNotFound: true);
        m_GameEditor_RedoClick = m_GameEditor.FindAction("RedoClick", throwIfNotFound: true);
        m_GameEditor_FButtonClick = m_GameEditor.FindAction("FButtonClick", throwIfNotFound: true);
        // GamePlay
        m_GamePlay = asset.FindActionMap("GamePlay", throwIfNotFound: true);
        m_GamePlay_MouseLeftClick = m_GamePlay.FindAction("MouseLeftClick", throwIfNotFound: true);
        m_GamePlay_MousePosition = m_GamePlay.FindAction("MousePosition", throwIfNotFound: true);
        m_GamePlay_MouseRightClick = m_GamePlay.FindAction("MouseRightClick", throwIfNotFound: true);
        m_GamePlay_TouchPress = m_GamePlay.FindAction("TouchPress", throwIfNotFound: true);
        m_GamePlay_TouchPosition = m_GamePlay.FindAction("TouchPosition", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // GameEditor
    private readonly InputActionMap m_GameEditor;
    private IGameEditorActions m_GameEditorActionsCallbackInterface;
    private readonly InputAction m_GameEditor_MouseLeftClick;
    private readonly InputAction m_GameEditor_MouseRightClick;
    private readonly InputAction m_GameEditor_MousePosition;
    private readonly InputAction m_GameEditor_MButtonClick;
    private readonly InputAction m_GameEditor_UndoClick;
    private readonly InputAction m_GameEditor_RedoClick;
    private readonly InputAction m_GameEditor_FButtonClick;
    public struct GameEditorActions
    {
        private @PlayerInput m_Wrapper;
        public GameEditorActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @MouseLeftClick => m_Wrapper.m_GameEditor_MouseLeftClick;
        public InputAction @MouseRightClick => m_Wrapper.m_GameEditor_MouseRightClick;
        public InputAction @MousePosition => m_Wrapper.m_GameEditor_MousePosition;
        public InputAction @MButtonClick => m_Wrapper.m_GameEditor_MButtonClick;
        public InputAction @UndoClick => m_Wrapper.m_GameEditor_UndoClick;
        public InputAction @RedoClick => m_Wrapper.m_GameEditor_RedoClick;
        public InputAction @FButtonClick => m_Wrapper.m_GameEditor_FButtonClick;
        public InputActionMap Get() { return m_Wrapper.m_GameEditor; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameEditorActions set) { return set.Get(); }
        public void SetCallbacks(IGameEditorActions instance)
        {
            if (m_Wrapper.m_GameEditorActionsCallbackInterface != null)
            {
                @MouseLeftClick.started -= m_Wrapper.m_GameEditorActionsCallbackInterface.OnMouseLeftClick;
                @MouseLeftClick.performed -= m_Wrapper.m_GameEditorActionsCallbackInterface.OnMouseLeftClick;
                @MouseLeftClick.canceled -= m_Wrapper.m_GameEditorActionsCallbackInterface.OnMouseLeftClick;
                @MouseRightClick.started -= m_Wrapper.m_GameEditorActionsCallbackInterface.OnMouseRightClick;
                @MouseRightClick.performed -= m_Wrapper.m_GameEditorActionsCallbackInterface.OnMouseRightClick;
                @MouseRightClick.canceled -= m_Wrapper.m_GameEditorActionsCallbackInterface.OnMouseRightClick;
                @MousePosition.started -= m_Wrapper.m_GameEditorActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_GameEditorActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_GameEditorActionsCallbackInterface.OnMousePosition;
                @MButtonClick.started -= m_Wrapper.m_GameEditorActionsCallbackInterface.OnMButtonClick;
                @MButtonClick.performed -= m_Wrapper.m_GameEditorActionsCallbackInterface.OnMButtonClick;
                @MButtonClick.canceled -= m_Wrapper.m_GameEditorActionsCallbackInterface.OnMButtonClick;
                @UndoClick.started -= m_Wrapper.m_GameEditorActionsCallbackInterface.OnUndoClick;
                @UndoClick.performed -= m_Wrapper.m_GameEditorActionsCallbackInterface.OnUndoClick;
                @UndoClick.canceled -= m_Wrapper.m_GameEditorActionsCallbackInterface.OnUndoClick;
                @RedoClick.started -= m_Wrapper.m_GameEditorActionsCallbackInterface.OnRedoClick;
                @RedoClick.performed -= m_Wrapper.m_GameEditorActionsCallbackInterface.OnRedoClick;
                @RedoClick.canceled -= m_Wrapper.m_GameEditorActionsCallbackInterface.OnRedoClick;
                @FButtonClick.started -= m_Wrapper.m_GameEditorActionsCallbackInterface.OnFButtonClick;
                @FButtonClick.performed -= m_Wrapper.m_GameEditorActionsCallbackInterface.OnFButtonClick;
                @FButtonClick.canceled -= m_Wrapper.m_GameEditorActionsCallbackInterface.OnFButtonClick;
            }
            m_Wrapper.m_GameEditorActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MouseLeftClick.started += instance.OnMouseLeftClick;
                @MouseLeftClick.performed += instance.OnMouseLeftClick;
                @MouseLeftClick.canceled += instance.OnMouseLeftClick;
                @MouseRightClick.started += instance.OnMouseRightClick;
                @MouseRightClick.performed += instance.OnMouseRightClick;
                @MouseRightClick.canceled += instance.OnMouseRightClick;
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
                @MButtonClick.started += instance.OnMButtonClick;
                @MButtonClick.performed += instance.OnMButtonClick;
                @MButtonClick.canceled += instance.OnMButtonClick;
                @UndoClick.started += instance.OnUndoClick;
                @UndoClick.performed += instance.OnUndoClick;
                @UndoClick.canceled += instance.OnUndoClick;
                @RedoClick.started += instance.OnRedoClick;
                @RedoClick.performed += instance.OnRedoClick;
                @RedoClick.canceled += instance.OnRedoClick;
                @FButtonClick.started += instance.OnFButtonClick;
                @FButtonClick.performed += instance.OnFButtonClick;
                @FButtonClick.canceled += instance.OnFButtonClick;
            }
        }
    }
    public GameEditorActions @GameEditor => new GameEditorActions(this);

    // GamePlay
    private readonly InputActionMap m_GamePlay;
    private IGamePlayActions m_GamePlayActionsCallbackInterface;
    private readonly InputAction m_GamePlay_MouseLeftClick;
    private readonly InputAction m_GamePlay_MousePosition;
    private readonly InputAction m_GamePlay_MouseRightClick;
    private readonly InputAction m_GamePlay_TouchPress;
    private readonly InputAction m_GamePlay_TouchPosition;
    public struct GamePlayActions
    {
        private @PlayerInput m_Wrapper;
        public GamePlayActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @MouseLeftClick => m_Wrapper.m_GamePlay_MouseLeftClick;
        public InputAction @MousePosition => m_Wrapper.m_GamePlay_MousePosition;
        public InputAction @MouseRightClick => m_Wrapper.m_GamePlay_MouseRightClick;
        public InputAction @TouchPress => m_Wrapper.m_GamePlay_TouchPress;
        public InputAction @TouchPosition => m_Wrapper.m_GamePlay_TouchPosition;
        public InputActionMap Get() { return m_Wrapper.m_GamePlay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GamePlayActions set) { return set.Get(); }
        public void SetCallbacks(IGamePlayActions instance)
        {
            if (m_Wrapper.m_GamePlayActionsCallbackInterface != null)
            {
                @MouseLeftClick.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMouseLeftClick;
                @MouseLeftClick.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMouseLeftClick;
                @MouseLeftClick.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMouseLeftClick;
                @MousePosition.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMousePosition;
                @MouseRightClick.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMouseRightClick;
                @MouseRightClick.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMouseRightClick;
                @MouseRightClick.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMouseRightClick;
                @TouchPress.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnTouchPress;
                @TouchPress.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnTouchPress;
                @TouchPress.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnTouchPress;
                @TouchPosition.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnTouchPosition;
                @TouchPosition.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnTouchPosition;
                @TouchPosition.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnTouchPosition;
            }
            m_Wrapper.m_GamePlayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MouseLeftClick.started += instance.OnMouseLeftClick;
                @MouseLeftClick.performed += instance.OnMouseLeftClick;
                @MouseLeftClick.canceled += instance.OnMouseLeftClick;
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
                @MouseRightClick.started += instance.OnMouseRightClick;
                @MouseRightClick.performed += instance.OnMouseRightClick;
                @MouseRightClick.canceled += instance.OnMouseRightClick;
                @TouchPress.started += instance.OnTouchPress;
                @TouchPress.performed += instance.OnTouchPress;
                @TouchPress.canceled += instance.OnTouchPress;
                @TouchPosition.started += instance.OnTouchPosition;
                @TouchPosition.performed += instance.OnTouchPosition;
                @TouchPosition.canceled += instance.OnTouchPosition;
            }
        }
    }
    public GamePlayActions @GamePlay => new GamePlayActions(this);
    public interface IGameEditorActions
    {
        void OnMouseLeftClick(InputAction.CallbackContext context);
        void OnMouseRightClick(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
        void OnMButtonClick(InputAction.CallbackContext context);
        void OnUndoClick(InputAction.CallbackContext context);
        void OnRedoClick(InputAction.CallbackContext context);
        void OnFButtonClick(InputAction.CallbackContext context);
    }
    public interface IGamePlayActions
    {
        void OnMouseLeftClick(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
        void OnMouseRightClick(InputAction.CallbackContext context);
        void OnTouchPress(InputAction.CallbackContext context);
        void OnTouchPosition(InputAction.CallbackContext context);
    }
}
