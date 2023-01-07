//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/LevelBuilder/Input/LevelBuilder_InputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @LevelBuilder_InputActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @LevelBuilder_InputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""LevelBuilder_InputActions"",
    ""maps"": [
        {
            ""name"": ""LevelBuilder"",
            ""id"": ""dd75c545-f8e6-4716-ad32-b935cf9bd550"",
            ""actions"": [
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""Value"",
                    ""id"": ""fca05264-9a0f-4fc3-9c7a-e19b91c69780"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Delete"",
                    ""type"": ""Value"",
                    ""id"": ""e0170499-ca39-44fc-acc2-381b603ce28c"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Build"",
                    ""type"": ""Value"",
                    ""id"": ""adc5b017-daf6-4a23-9df3-31a66b830158"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""Value"",
                    ""id"": ""bdf14dc2-e5fa-4420-a8e9-d7f06c288dc3"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""1f1eeb88-5755-4dad-a409-d1bdec0ba9c0"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Save"",
                    ""type"": ""Button"",
                    ""id"": ""8a297560-f588-45e7-9553-9b32b30f4e16"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Undo"",
                    ""type"": ""Button"",
                    ""id"": ""07c2be81-c822-41fd-95b9-0bfbc2ad9710"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Redo"",
                    ""type"": ""Button"",
                    ""id"": ""1c6896a3-0b0e-406c-9c1c-a8ca3961ed2a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Test"",
                    ""type"": ""Button"",
                    ""id"": ""5596464f-4739-4144-a42f-92fbe151cabe"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""376ec939-d6ce-4469-99f1-69041ec73756"",
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
                    ""id"": ""f5b5e965-5471-4042-bd23-0c3ea46ad901"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Delete"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""123c105b-453b-4c40-9d8d-0143c23db346"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Build"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aed2bbe5-427b-4b60-8dcd-762dc3857183"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e3346c33-b245-49d8-91ad-d0c2cc04f405"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Ctrl + S"",
                    ""id"": ""08be82e0-ba53-4919-a41f-e6b1ce9c4263"",
                    ""path"": ""OneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Save"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""080dbc7b-69d6-4a40-a547-25aa0b64e495"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Save"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""binding"",
                    ""id"": ""14f088b8-8eae-479e-b9f6-956105f2d876"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Save"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Ctrl + Z"",
                    ""id"": ""79b03e75-0624-4090-a6f7-2c05edaa820a"",
                    ""path"": ""OneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Undo"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""f40df433-af39-4317-bb38-b8f15b1e2d29"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Undo"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""binding"",
                    ""id"": ""c5f64533-5e25-43eb-9572-bf5c52f88dad"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Undo"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Ctrl + Y"",
                    ""id"": ""e413a121-7a48-4bc7-b1eb-3c891332b36b"",
                    ""path"": ""OneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Redo"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""3c946471-241f-49a5-ae19-cb4f2470d4e2"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Redo"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""binding"",
                    ""id"": ""720cc2ca-7985-4858-b217-6f673d574e9e"",
                    ""path"": ""<Keyboard>/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Redo"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c24eee4b-b767-4cb3-9e08-9f21fb5c91e3"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Test"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // LevelBuilder
        m_LevelBuilder = asset.FindActionMap("LevelBuilder", throwIfNotFound: true);
        m_LevelBuilder_MousePosition = m_LevelBuilder.FindAction("MousePosition", throwIfNotFound: true);
        m_LevelBuilder_Delete = m_LevelBuilder.FindAction("Delete", throwIfNotFound: true);
        m_LevelBuilder_Build = m_LevelBuilder.FindAction("Build", throwIfNotFound: true);
        m_LevelBuilder_Zoom = m_LevelBuilder.FindAction("Zoom", throwIfNotFound: true);
        m_LevelBuilder_Move = m_LevelBuilder.FindAction("Move", throwIfNotFound: true);
        m_LevelBuilder_Save = m_LevelBuilder.FindAction("Save", throwIfNotFound: true);
        m_LevelBuilder_Undo = m_LevelBuilder.FindAction("Undo", throwIfNotFound: true);
        m_LevelBuilder_Redo = m_LevelBuilder.FindAction("Redo", throwIfNotFound: true);
        m_LevelBuilder_Test = m_LevelBuilder.FindAction("Test", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // LevelBuilder
    private readonly InputActionMap m_LevelBuilder;
    private ILevelBuilderActions m_LevelBuilderActionsCallbackInterface;
    private readonly InputAction m_LevelBuilder_MousePosition;
    private readonly InputAction m_LevelBuilder_Delete;
    private readonly InputAction m_LevelBuilder_Build;
    private readonly InputAction m_LevelBuilder_Zoom;
    private readonly InputAction m_LevelBuilder_Move;
    private readonly InputAction m_LevelBuilder_Save;
    private readonly InputAction m_LevelBuilder_Undo;
    private readonly InputAction m_LevelBuilder_Redo;
    private readonly InputAction m_LevelBuilder_Test;
    public struct LevelBuilderActions
    {
        private @LevelBuilder_InputActions m_Wrapper;
        public LevelBuilderActions(@LevelBuilder_InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @MousePosition => m_Wrapper.m_LevelBuilder_MousePosition;
        public InputAction @Delete => m_Wrapper.m_LevelBuilder_Delete;
        public InputAction @Build => m_Wrapper.m_LevelBuilder_Build;
        public InputAction @Zoom => m_Wrapper.m_LevelBuilder_Zoom;
        public InputAction @Move => m_Wrapper.m_LevelBuilder_Move;
        public InputAction @Save => m_Wrapper.m_LevelBuilder_Save;
        public InputAction @Undo => m_Wrapper.m_LevelBuilder_Undo;
        public InputAction @Redo => m_Wrapper.m_LevelBuilder_Redo;
        public InputAction @Test => m_Wrapper.m_LevelBuilder_Test;
        public InputActionMap Get() { return m_Wrapper.m_LevelBuilder; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(LevelBuilderActions set) { return set.Get(); }
        public void SetCallbacks(ILevelBuilderActions instance)
        {
            if (m_Wrapper.m_LevelBuilderActionsCallbackInterface != null)
            {
                @MousePosition.started -= m_Wrapper.m_LevelBuilderActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_LevelBuilderActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_LevelBuilderActionsCallbackInterface.OnMousePosition;
                @Delete.started -= m_Wrapper.m_LevelBuilderActionsCallbackInterface.OnDelete;
                @Delete.performed -= m_Wrapper.m_LevelBuilderActionsCallbackInterface.OnDelete;
                @Delete.canceled -= m_Wrapper.m_LevelBuilderActionsCallbackInterface.OnDelete;
                @Build.started -= m_Wrapper.m_LevelBuilderActionsCallbackInterface.OnBuild;
                @Build.performed -= m_Wrapper.m_LevelBuilderActionsCallbackInterface.OnBuild;
                @Build.canceled -= m_Wrapper.m_LevelBuilderActionsCallbackInterface.OnBuild;
                @Zoom.started -= m_Wrapper.m_LevelBuilderActionsCallbackInterface.OnZoom;
                @Zoom.performed -= m_Wrapper.m_LevelBuilderActionsCallbackInterface.OnZoom;
                @Zoom.canceled -= m_Wrapper.m_LevelBuilderActionsCallbackInterface.OnZoom;
                @Move.started -= m_Wrapper.m_LevelBuilderActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_LevelBuilderActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_LevelBuilderActionsCallbackInterface.OnMove;
                @Save.started -= m_Wrapper.m_LevelBuilderActionsCallbackInterface.OnSave;
                @Save.performed -= m_Wrapper.m_LevelBuilderActionsCallbackInterface.OnSave;
                @Save.canceled -= m_Wrapper.m_LevelBuilderActionsCallbackInterface.OnSave;
                @Undo.started -= m_Wrapper.m_LevelBuilderActionsCallbackInterface.OnUndo;
                @Undo.performed -= m_Wrapper.m_LevelBuilderActionsCallbackInterface.OnUndo;
                @Undo.canceled -= m_Wrapper.m_LevelBuilderActionsCallbackInterface.OnUndo;
                @Redo.started -= m_Wrapper.m_LevelBuilderActionsCallbackInterface.OnRedo;
                @Redo.performed -= m_Wrapper.m_LevelBuilderActionsCallbackInterface.OnRedo;
                @Redo.canceled -= m_Wrapper.m_LevelBuilderActionsCallbackInterface.OnRedo;
                @Test.started -= m_Wrapper.m_LevelBuilderActionsCallbackInterface.OnTest;
                @Test.performed -= m_Wrapper.m_LevelBuilderActionsCallbackInterface.OnTest;
                @Test.canceled -= m_Wrapper.m_LevelBuilderActionsCallbackInterface.OnTest;
            }
            m_Wrapper.m_LevelBuilderActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
                @Delete.started += instance.OnDelete;
                @Delete.performed += instance.OnDelete;
                @Delete.canceled += instance.OnDelete;
                @Build.started += instance.OnBuild;
                @Build.performed += instance.OnBuild;
                @Build.canceled += instance.OnBuild;
                @Zoom.started += instance.OnZoom;
                @Zoom.performed += instance.OnZoom;
                @Zoom.canceled += instance.OnZoom;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Save.started += instance.OnSave;
                @Save.performed += instance.OnSave;
                @Save.canceled += instance.OnSave;
                @Undo.started += instance.OnUndo;
                @Undo.performed += instance.OnUndo;
                @Undo.canceled += instance.OnUndo;
                @Redo.started += instance.OnRedo;
                @Redo.performed += instance.OnRedo;
                @Redo.canceled += instance.OnRedo;
                @Test.started += instance.OnTest;
                @Test.performed += instance.OnTest;
                @Test.canceled += instance.OnTest;
            }
        }
    }
    public LevelBuilderActions @LevelBuilder => new LevelBuilderActions(this);
    public interface ILevelBuilderActions
    {
        void OnMousePosition(InputAction.CallbackContext context);
        void OnDelete(InputAction.CallbackContext context);
        void OnBuild(InputAction.CallbackContext context);
        void OnZoom(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnSave(InputAction.CallbackContext context);
        void OnUndo(InputAction.CallbackContext context);
        void OnRedo(InputAction.CallbackContext context);
        void OnTest(InputAction.CallbackContext context);
    }
}
