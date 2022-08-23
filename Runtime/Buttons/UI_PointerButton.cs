//#define USE_UNITY_XRI_TOOLKIT
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DimaTi.PhysicsButtons
{
    public class UI_PointerButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] bool m_isInteractible = true;

        [SerializeField] bool isToggleMode = false;
        public bool IsActive { get; private set; }

        public UnityEvent
            onEnter,
            onExit;
        public UnityEvent_bool
            onDown,
            onUp;

        [System.Serializable] public class UnityEvent_bool : UnityEvent<bool> { }


        bool m_isPressed;
        public virtual bool IsPressed => m_isPressed;

        bool m_isEntered;
        public virtual bool IsEntered => m_isEntered;

        protected Vector3 interactionPoint;
        public bool IsInteractible
        {
            get { return m_isInteractible; }
            set { m_isInteractible = value; }
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (!m_isInteractible)
                return;
            m_isEntered = true;
            onEnter?.Invoke();
        }
        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (!m_isInteractible)
                return;
            m_isEntered = false;
            onExit?.Invoke();
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (!m_isInteractible)
                return;

            m_isPressed = true;
            IsActive = !IsActive;

            onDown?.Invoke(IsActive);


#if USE_UNITY_XRI_TOOLKIT
        Util_XRI_Support.XRI_InteractionPoint(eventData, transform.position, transform.forward); //Calculate interaction point, Import this from samples
#endif
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (!m_isInteractible)
                return;

            if(!isToggleMode)
                IsActive = false;
            m_isPressed = false;

            onUp?.Invoke(IsActive);
        }

        //  protected virtual void OnDrawGizmosSelected()
        //  {
        //      if (Application.isPlaying)
        //      {
        //          Gizmos.color = Color.yellow;
        //          Gizmos.DrawLine(transform.position, interactionPoint);
        //          Gizmos.DrawSphere(interactionPoint, 0.05f);
        //      }
        //  }
    }
}
