// GENERATED AUTOMATICALLY FROM 'Assets/Input/Controls.inputactions'

using System;
using UnityEngine;
using UnityEngine.Experimental.Input;


[Serializable]
public class Controls : InputActionAssetReference
{
    public Controls()
    {
    }
    public Controls(InputActionAsset asset)
        : base(asset)
    {
    }
    private bool m_Initialized;
    private void Initialize()
    {
        // BasicCtrl
        m_BasicCtrl = asset.GetActionMap("BasicCtrl");
        m_BasicCtrl_A = m_BasicCtrl.GetAction("A");
        m_Initialized = true;
    }
    private void Uninitialize()
    {
        m_BasicCtrl = null;
        m_BasicCtrl_A = null;
        m_Initialized = false;
    }
    public void SetAsset(InputActionAsset newAsset)
    {
        if (newAsset == asset) return;
        if (m_Initialized) Uninitialize();
        asset = newAsset;
    }
    public override void MakePrivateCopyOfActions()
    {
        SetAsset(ScriptableObject.Instantiate(asset));
    }
    // BasicCtrl
    private InputActionMap m_BasicCtrl;
    private InputAction m_BasicCtrl_A;
    public struct BasicCtrlActions
    {
        private Controls m_Wrapper;
        public BasicCtrlActions(Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @A { get { return m_Wrapper.m_BasicCtrl_A; } }
        public InputActionMap Get() { return m_Wrapper.m_BasicCtrl; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled { get { return Get().enabled; } }
        public InputActionMap Clone() { return Get().Clone(); }
        public static implicit operator InputActionMap(BasicCtrlActions set) { return set.Get(); }
    }
    public BasicCtrlActions @BasicCtrl
    {
        get
        {
            if (!m_Initialized) Initialize();
            return new BasicCtrlActions(this);
        }
    }
}
