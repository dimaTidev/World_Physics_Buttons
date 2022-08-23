using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimaTi.PhysicsButtons
{
    [RequireComponent(typeof(UI_PointerButton))]
    public class UI_PointerButton_Visualizer : MonoBehaviour
    {
        private UI_PointerButton m_button;

        [SerializeField] UI_PhysicsButtonInteractionData m_data;
        public UI_PhysicsButtonInteractionData Data => m_data;

        [SerializeField] Renderer m_visualButton = null;
        protected Renderer VisualButton => m_visualButton;

        MaterialPropertyBlock m_PropertyBlock;

        protected virtual void Start()
        {
            m_button = GetComponent<UI_PointerButton>();
            m_button.onDown.AddListener((x) => OnStateChanged(m_button.IsActive, m_button.IsEntered, m_button.IsPressed));
            m_button.onUp.AddListener((x) => OnStateChanged(m_button.IsActive, m_button.IsEntered, m_button.IsPressed));
            m_button.onEnter.AddListener(() => OnStateChanged(m_button.IsActive, m_button.IsEntered, m_button.IsPressed));
            m_button.onExit.AddListener(() => OnStateChanged(m_button.IsActive, m_button.IsEntered, m_button.IsPressed));
        }

        protected virtual void OnStateChanged(bool isActive, bool isEntered, bool isPressed)
        {
            if(isActive && isPressed)
                Set_color(Data.ColorPressed);
            else if (isActive && !isPressed)
                Set_color(Data.ColorSelected);
            else if (!isActive && !isPressed && isEntered)
                Set_color(Data.ColorHovered);
            else
                Set_color(Data.ColorDefault);

            //Set_color(m_button.IsActive ? Data.ColorPressed : m_button.IsEntered ? Data.ColorHovered : Data.ColorDefault);
        }

        void Set_color(Color color)
        {
            if (m_PropertyBlock == null) m_PropertyBlock = new MaterialPropertyBlock();
            m_PropertyBlock.SetColor("_Color", color);
            VisualButton.SetPropertyBlock(m_PropertyBlock);
        }
    }
}
