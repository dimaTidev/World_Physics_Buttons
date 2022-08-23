using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debuger_UI_PhysicsInteractor : MonoBehaviour
{
    [SerializeField] AUI_PhysicsInteractor_Base interactor = null;
    [SerializeField] Image image_triggered;
    [SerializeField] Image image_entered;

    [SerializeField] Color color_active;
    [SerializeField] Color color_deactive;


    private void Start()
    {
        if (interactor)
        {
            interactor.onEnter.AddListener(() => { OnEnter(); });
            interactor.onExit.AddListener(() => { OnExit(); });
            interactor.onDown.AddListener(() => { OnDown(); });
            interactor.onUp.AddListener(() => { OnUp(); });
        }
    }


    void OnEnter()
    {
        if (image_entered)
            image_entered.color = color_active;
    }

    void OnExit()
    {
        if (image_entered)
            image_entered.color = color_deactive;
    }

    void OnDown()
    {
        if (image_triggered)
            image_triggered.color = color_active;
    }

    void OnUp()
    {
        if (image_triggered)
            image_triggered.color = color_deactive;
    }
}
