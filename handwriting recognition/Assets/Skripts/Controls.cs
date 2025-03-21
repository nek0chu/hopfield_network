//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.11.2
//     from Assets/Skripts/Controls.inputactions
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

public partial class @Controls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""painting"",
            ""id"": ""bab22d54-f2d5-4f4c-916e-a55c9bb33318"",
            ""actions"": [
                {
                    ""name"": ""Press"",
                    ""type"": ""Value"",
                    ""id"": ""4504c4ad-cc9d-442f-a2f3-ac6cf9213a6f"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": "" Position"",
                    ""type"": ""Value"",
                    ""id"": ""c4f5409f-93ea-4740-bf5a-23cd57978957"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d964323a-2196-4d83-8048-64d0a9ad0094"",
                    ""path"": ""<Touchscreen>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": "" Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0c007edc-2544-41f9-80ee-aea1a298f38f"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": "" Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""09da024e-c999-49f8-966e-247c44a302d1"",
                    ""path"": ""<Mouse>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Press"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""69c7db0f-fbb4-4440-8cb5-33f88dfef69e"",
                    ""path"": ""<Touchscreen>/primaryTouch"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Press"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // painting
        m_painting = asset.FindActionMap("painting", throwIfNotFound: true);
        m_painting_Press = m_painting.FindAction("Press", throwIfNotFound: true);
        m_painting_Position = m_painting.FindAction(" Position", throwIfNotFound: true);
    }

    ~@Controls()
    {
        UnityEngine.Debug.Assert(!m_painting.enabled, "This will cause a leak and performance issues, Controls.painting.Disable() has not been called.");
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

    // painting
    private readonly InputActionMap m_painting;
    private List<IPaintingActions> m_PaintingActionsCallbackInterfaces = new List<IPaintingActions>();
    private readonly InputAction m_painting_Press;
    private readonly InputAction m_painting_Position;
    public struct PaintingActions
    {
        private @Controls m_Wrapper;
        public PaintingActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Press => m_Wrapper.m_painting_Press;
        public InputAction @Position => m_Wrapper.m_painting_Position;
        public InputActionMap Get() { return m_Wrapper.m_painting; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PaintingActions set) { return set.Get(); }
        public void AddCallbacks(IPaintingActions instance)
        {
            if (instance == null || m_Wrapper.m_PaintingActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PaintingActionsCallbackInterfaces.Add(instance);
            @Press.started += instance.OnPress;
            @Press.performed += instance.OnPress;
            @Press.canceled += instance.OnPress;
            @Position.started += instance.OnPosition;
            @Position.performed += instance.OnPosition;
            @Position.canceled += instance.OnPosition;
        }

        private void UnregisterCallbacks(IPaintingActions instance)
        {
            @Press.started -= instance.OnPress;
            @Press.performed -= instance.OnPress;
            @Press.canceled -= instance.OnPress;
            @Position.started -= instance.OnPosition;
            @Position.performed -= instance.OnPosition;
            @Position.canceled -= instance.OnPosition;
        }

        public void RemoveCallbacks(IPaintingActions instance)
        {
            if (m_Wrapper.m_PaintingActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPaintingActions instance)
        {
            foreach (var item in m_Wrapper.m_PaintingActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PaintingActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PaintingActions @painting => new PaintingActions(this);
    public interface IPaintingActions
    {
        void OnPress(InputAction.CallbackContext context);
        void OnPosition(InputAction.CallbackContext context);
    }
}
