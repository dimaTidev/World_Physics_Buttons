using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

[System.Obsolete]
public class XRD_ButtonPhysicsUI_touchable_OLD : UI_PointerButton, IPhysicsPointer
{
    List<Transform> interactors = new List<Transform>();
    float minDist;

    public float MinDistToInteractor => minDist;

    public event Action OnInteractors_Update;
    bool isPhysicalHover;
    public bool IsPhysicalHover => isPhysicalHover;

    void Update()
    {
        if (interactors.Count > 0)
        {
            Get_ClosestInteractor(out minDist);
            OnInteractors_Update?.Invoke();
        }
    }

    Transform Get_ClosestInteractor(out float minDist)
    {
        minDist = float.MaxValue;

        if (interactors.Count > 0)
        {
            int idClosest = 0;
            for (int i = 0; i < interactors.Count; i++)
            {
                Vector3 intPoint = Get_IntersectionPoint(interactors[i]);
                float dist = Vector3.Distance(intPoint, interactors[i].position);
                if(dist < minDist)
                {
                    minDist = dist;
                    idClosest = i;
                }
            }
            return interactors[idClosest];
        }
        return null;
    }

    Vector3 Get_IntersectionPoint(Transform target) => Vector3.ProjectOnPlane(target.position - transform.position, transform.forward) + transform.position;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (eventData == null && interactors.Count > 0) //if interaction comes not from UI system and we have physical interactors
            interactionPoint = Get_IntersectionPoint(Get_ClosestInteractor(out _));
    }

    #region Interface
    //----------------------------------------------------------------------------------------------------------
    void IPhysicsPointer.OnPointerEnter(GameObject interactor)
    {
        if (!interactor)
            return;
        if (!interactors.Contains(interactor.transform))
            interactors.Add(interactor.transform);
    }

    void IPhysicsPointer.OnPointerExit(GameObject interactor)
    {
        if (!interactor)
            return;
        interactors.Remove(interactor.transform);
        isPhysicalHover = interactors.Count > 0;
    }
    //----------------------------------------------------------------------------------------------------------
    #endregion
}
