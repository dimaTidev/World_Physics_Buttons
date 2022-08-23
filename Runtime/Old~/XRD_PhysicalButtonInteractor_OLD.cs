using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public interface IPhysicsPointer
{
    void OnPointerEnter(GameObject interactor);
    void OnPointerExit(GameObject interactor);
   //void OnPointerDown(GameObject interactor);
   //void OnPointerUp(GameObject interactor);
}

[RequireComponent(typeof(Rigidbody))]
[System.Obsolete]
public class XRD_PhysicalButtonInteractor_OLD : AUI_PhysicsInteractor_Base
{
    [SerializeField] string triggerTag;
    [SerializeField] float clickThreshold = 0.1f;

    Vector3 lastPos; //need to fix repeat hit when go up from below button

    List<Transform> buttons = new List<Transform>();
    HashSet<Transform> clickedSet = new HashSet<Transform>();

    public void Update()
    {
        LineCheck();
    }

    void LineCheck()
    {
        Vector3 tempPos = lastPos;
        while (tempPos != transform.position)
        {
            tempPos = Vector3.MoveTowards(tempPos, transform.position, clickThreshold * 0.5f);
            foreach (var button in buttons)
            {
                if (CheckIntersection(button, tempPos))
                    Try_OnPointerDown(button);
                else
                    Try_OnPointerUp(button);
            }
        }
        lastPos = transform.position;
    }

    bool CheckIntersection(Transform button, Vector3 pointerPos) => Vector3.Distance(Get_IntersectionPoint(button, pointerPos), pointerPos) < clickThreshold;
    Vector3 Get_IntersectionPoint(Transform button, Vector3 pointerPos) => Vector3.ProjectOnPlane(pointerPos - button.position, button.forward) + button.position;

    bool IsValidHit(Transform button) //need to fix repeat hit when go up from below button
    {
        Vector3 myDir = (transform.position - lastPos).normalized;
        return Vector3.Dot(myDir, button.forward) < 0;
    }

    void Try_OnPointerUp(Transform button)
    {
        if (clickedSet.Contains(button))
        {
            clickedSet.Remove(button);
            OnPointerUp(button);
        }
    }
    void Try_OnPointerDown(Transform button)
    {
        if (!clickedSet.Contains(button))
        {
            clickedSet.Add(button);
            if(IsValidHit(button)) //need to fix repeat hit when go up from below button
                OnPointerDown(button);
        }
    }



    #region Pointers
    void OnPointerEnter(Transform button)
    {
        //Debug.Log("OnPointerEnter");
        IPointerEnterHandler iPointerEvent = button.GetComponent<IPointerEnterHandler>();
        if (iPointerEvent != null) iPointerEvent.OnPointerEnter(null);
        IPhysicsPointer iPhysicsPointerEvent = button.GetComponent<IPhysicsPointer>();
        if (iPhysicsPointerEvent != null) iPhysicsPointerEvent.OnPointerEnter(gameObject);
        onEnter?.Invoke();
    }

    void OnPointerExit(Transform button)
    {
        //Debug.Log("OnPointerExit");
        IPointerExitHandler iPointerEvent = button.GetComponent<IPointerExitHandler>();
        if (iPointerEvent != null) iPointerEvent.OnPointerExit(null);
        IPhysicsPointer iPhysicsPointerEvent = button.GetComponent<IPhysicsPointer>();
        if (iPhysicsPointerEvent != null) iPhysicsPointerEvent.OnPointerExit(gameObject);
        onExit?.Invoke();
    }

    void OnPointerDown(Transform button)
    {
        Debug.Log("OnPointerDown");
        IPointerDownHandler iPointerEvent = button.GetComponent<IPointerDownHandler>();
        if (iPointerEvent != null) iPointerEvent.OnPointerDown(null);
       // IPhysicsPointer iPhysicsPointerEvent = button.GetComponent<IPhysicsPointer>();
       // if (iPhysicsPointerEvent != null) iPhysicsPointerEvent.OnPointerDown(gameObject);
        onDown?.Invoke();
    }

    void OnPointerUp(Transform button)
    {
        Debug.Log("OnPointerUp");
        IPointerUpHandler iPointerEvent = button.GetComponent<IPointerUpHandler>();
        if (iPointerEvent != null) iPointerEvent.OnPointerUp(null);
       // IPhysicsPointer iPhysicsPointerEvent = button.GetComponent<IPhysicsPointer>();
       // if (iPhysicsPointerEvent != null) iPhysicsPointerEvent.OnPointerUp(gameObject);
        onUp?.Invoke();
    }
    #endregion

    #region Trigger
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(triggerTag))
            return;
        if (!buttons.Contains(other.transform))
        {
            buttons.Add(other.transform);
            OnPointerEnter(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag(triggerTag))
            return;
        buttons.Remove(other.transform);
        OnPointerExit(other.transform);
        Try_OnPointerUp(other.transform);
    }
    #endregion


    private void OnDrawGizmos()
    {
        foreach (var button in buttons)
        {
            if (CheckIntersection(button, transform.position))
                Gizmos.color = Color.red;
            else
                Gizmos.color = Color.green;
       
            Gizmos.DrawLine(Get_IntersectionPoint(button, transform.position), transform.position);
        }
    }
}
