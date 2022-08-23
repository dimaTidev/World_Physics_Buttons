using System.Collections.Generic;
using UnityEngine;

namespace DimaTi.PhysicsButtons
{
    public abstract class AUI_PhysicsInteractible_Base : UI_PointerButton, IUI_interactible
    {
        protected List<IUI_Interactor> interactors = new List<IUI_Interactor>();
        bool isPhysicalHover;
        public bool IsPhysicalHover => isPhysicalHover;

        public override bool IsEntered => interactors.Count > 0 | base.IsEntered;

        protected virtual void OnDisable()
        {
            int removeCount = interactors.Count;
            for (int i = 0; i < removeCount; i++)
                OnRemove_PhysicalInteractor(interactors[0]);
            Debug.Log("interactors.Count: " + interactors);
        }

        protected virtual void OnAdd_PhysicalInteractor(IUI_Interactor interactor)
        {
            if (!interactors.Contains(interactor))
            {
                interactors.Add(interactor);
                interactor.OnInteractorEnter();
                OnPointerEnter(null);
            }
        }

        protected virtual void OnRemove_PhysicalInteractor(IUI_Interactor interactor)
        {
            interactors.Remove(interactor);
            interactor.OnInteractorExit();
            isPhysicalHover = interactors.Count > 0;
            OnPointerExit(null);
        }

        protected void OnPointerDown(IUI_Interactor interactor)
        {
            interactor.OnInteractorDown();
            base.OnPointerDown(null);
        }

        protected void OnPointerUp(IUI_Interactor interactor)
        {
            interactor.OnInteractorUp();
            base.OnPointerUp(null);
        }

        #region Interface_IUI_interactible
        //----------------------------------------------------------------------------------------------------------
        void IUI_interactible.OnEnter(IUI_Interactor interactor) => OnAdd_PhysicalInteractor(interactor);
        void IUI_interactible.OnExit(IUI_Interactor interactor) => OnRemove_PhysicalInteractor(interactor);
        public virtual void OnSelect_button(IUI_Interactor interactor, bool isActive) { }
        public virtual void OnUpdate_Interactible(IUI_Interactor interactor) { }
        //----------------------------------------------------------------------------------------------------------
        #endregion
    }
}
