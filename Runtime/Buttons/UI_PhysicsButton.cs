using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.EventSystems;

namespace DimaTi.PhysicsButtons
{
    public class UI_PhysicsButton : AUI_PhysicsInteractible_Base
    {
        [SerializeField] float clickThreshold = 0.1f;
        HashSet<IUI_Interactor> clickedSet = new HashSet<IUI_Interactor>();
        List<Vector3> lastInteractorsPotitions = new List<Vector3>();

        float minDist;
        public float MinDistToInteractor => minDist;
        public event Action OnInteractors_Update;

        public override bool IsPressed => clickedSet.Count > 0 | base.IsPressed;

        protected override void OnDisable()
        {
            foreach (var inter in interactors)
                Try_OnPointerUp(inter);
            base.OnDisable();
        }

        protected override void OnAdd_PhysicalInteractor(IUI_Interactor interactor)
        {
            base.OnAdd_PhysicalInteractor(interactor);
            lastInteractorsPotitions.Add(interactors[interactors.Count - 1].transform.position); //for fixing bug when interactor move to fast and button don't triggered
        }

        protected override void OnRemove_PhysicalInteractor(IUI_Interactor interactor)
        {
            Try_OnPointerUp(interactor);
            int id = interactors.IndexOf(interactor);
            if (id >= 0)
                lastInteractorsPotitions.RemoveAt(id); //for fixing bug when interactor move to fast and button don't triggered
            base.OnRemove_PhysicalInteractor(interactor);
        }

        public override void OnUpdate_Interactible(IUI_Interactor interactor)
        {
            int idInteractor = interactors.IndexOf(interactor); //if we have tis interactor in list

            if (idInteractor >= 0)
            {
                if (interactors.Count != lastInteractorsPotitions.Count)
                {
                    Debug.LogError("interactors.Count != lastInteractorsPotitions.Count at: " + name);
                    return;
                }

                Vector3 tempPos = lastInteractorsPotitions[idInteractor];
                while (tempPos != interactors[idInteractor].transform.position)
                {
                    tempPos = Vector3.MoveTowards(tempPos, interactors[idInteractor].transform.position, clickThreshold * 0.5f);

                    if (CheckIntersection(transform, tempPos))
                        Try_OnPointerDown(interactors[idInteractor], tempPos, lastInteractorsPotitions[idInteractor]);
                    else
                        Try_OnPointerUp(interactors[idInteractor]);
                }
                lastInteractorsPotitions[idInteractor] = interactors[idInteractor].transform.position;
            }

            if (interactors.Count > 0)
            {
                IUI_Interactor closestInteractor = Get_ClosestInteractor(out minDist);
                OnInteractors_Update?.Invoke();

                if (closestInteractor != null)
                    interactionPoint = Get_IntersectionPoint(transform, closestInteractor.transform.position);
            }
        }

        bool CheckIntersection(Transform button, Vector3 pointerPos) => Vector3.Distance(Get_IntersectionPoint(button, pointerPos), pointerPos) < clickThreshold;
        Vector3 Get_IntersectionPoint(Transform button, Vector3 pointerPos) => Vector3.ProjectOnPlane(pointerPos - button.position, button.forward) + button.position;

        bool IsValidHit(Transform button, Vector3 pointerPos, Vector3 lastPos) //need to fix repeat hit when go up from below button
        {
            Vector3 myDir = (pointerPos - lastPos).normalized;
            return Vector3.Dot(myDir, button.forward) < 0;
        }

        void Try_OnPointerUp(IUI_Interactor interactor)
        {
            if (clickedSet.Contains(interactor))
            {
                clickedSet.Remove(interactor);
                OnPointerUp(interactor);
            }
        }
        void Try_OnPointerDown(IUI_Interactor interactor, Vector3 pointerPos, Vector3 lastPointerPos)
        {
            if (!clickedSet.Contains(interactor))
            {
                if (!IsPressed && IsValidHit(transform, pointerPos, lastPointerPos))//need to fix repeat hit when go up from below button
                {
                    OnPointerDown(interactor);
                }
                clickedSet.Add(interactor);
            }
        }

        //---------------
        IUI_Interactor Get_ClosestInteractor(out float minDist)
        {
            minDist = float.MaxValue;

            if (interactors.Count > 0)
            {
                int idClosest = 0;
                for (int i = 0; i < interactors.Count; i++)
                {
                    Vector3 intPoint = Get_IntersectionPoint(transform, interactors[i].transform.position);
                    float dist;
                    if (Vector3.Dot((interactors[i].transform.position - intPoint).normalized, transform.forward) < 0) //behind button
                        dist = 0;
                    else
                        dist = Vector3.Distance(intPoint, interactors[i].transform.position);

                    if (dist < minDist)
                    {
                        minDist = dist;
                        idClosest = i;
                    }
                }
                return interactors[idClosest];
            }
            return null;
        }
        //-----------------

        protected void OnDrawGizmosSelected() //override
        {
            //base.OnDrawGizmosSelected();
            Gizmos.color = Color.green; //click threshld
            Gizmos.DrawRay(transform.position, transform.forward * clickThreshold);

            // Gizmos.color = Color.yellow;
            //
            // if (interactors.Count > 0)
            //     Gizmos.DrawWireSphere(transform.position, 0.02f);
            //
            // foreach (var inter in interactors)
            // {
            //     Vector3 interPoint = Get_IntersectionPoint(transform, inter.transform.position);
            //
            //     Gizmos.DrawLine(interPoint, inter.transform.position);
            // }
        }
    }
}
