using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI_PhysicsInteractor : AUI_PhysicsInteractor_Base
{
    // [SerializeField] string triggerTag;

    List<IUI_interactible> interactibles = new List<IUI_interactible>();

    private void Update()
    {
        foreach (var inter in interactibles)
            inter.OnUpdate_Interactible(this);
    }

    private void OnTriggerEnter(Collider other)
    {
       // if (!other.gameObject.CompareTag(triggerTag))
       //     return;
        IUI_interactible[] iInteractible = other.GetComponents<IUI_interactible>();
        foreach (var item in iInteractible)
        {
            if (!interactibles.Contains(item))
            {
                item.OnEnter(this);
                interactibles.Add(item);
            }
        }    
    }

    private void OnTriggerExit(Collider other)
    {
       // if (!other.gameObject.CompareTag(triggerTag))
       //     return;
        IUI_interactible[] iInteractible = other.GetComponents<IUI_interactible>();
        foreach (var item in iInteractible)
        {
            interactibles.Remove(item);
            item.OnExit(this);
        }     
    }

    public void OnPress_SelectButton(bool isPressed)
    {
        foreach (var inter in interactibles)
            if (inter != null)
                inter.OnSelect_button(this, isPressed);
    }

#if UNITY_EDITOR
    [ContextMenu("Log interactibles count")]
    void Debug_InteractiblesCount()
    {
        Debug.Log($"interactibles.Count:{interactibles.Count} at object {name}");
    }
#endif

}
