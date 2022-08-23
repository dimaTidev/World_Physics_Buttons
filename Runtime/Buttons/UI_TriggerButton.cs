using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimaTi.PhysicsButtons
{
    //TODO: move interactor.OnInteractorDown(); to the OnPointerDown(null); 
    public class UI_TriggerButton : AUI_PhysicsInteractible_Base
    {
        [SerializeField] bool isTriggerWhenOnEnter = false;

        protected override void OnAdd_PhysicalInteractor(IUI_Interactor interactor) //enter trigger
        {
            base.OnAdd_PhysicalInteractor(interactor);
            if (isTriggerWhenOnEnter)
                OnPointerDown(interactor);
        }

        protected override void OnRemove_PhysicalInteractor(IUI_Interactor interactor) //exit trigger
        {
            base.OnRemove_PhysicalInteractor(interactor);
            if (isTriggerWhenOnEnter)
                OnPointerUp(interactor);
        }

        public override void OnSelect_button(IUI_Interactor interactor, bool isActive)
        {
            base.OnSelect_button(interactor, isActive);
            if (!isTriggerWhenOnEnter)
            {
                if (isActive)
                {
                    OnPointerDown(interactor);
                    interactor.OnInteractorDown();
                }
                else
                {
                    OnPointerUp(interactor);
                    interactor.OnInteractorUp();
                }
            }
        }
    }
}
