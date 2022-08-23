using UnityEngine;
using UnityEngine.Events;

public abstract class AUI_PhysicsInteractor_Base : MonoBehaviour, IUI_Interactor
{
    public UnityEvent
      onEnter,
      onExit,
      onDown,
      onUp;

    void IUI_Interactor.OnInteractorEnter() => onEnter?.Invoke();
    void IUI_Interactor.OnInteractorExit() => onExit?.Invoke();
    void IUI_Interactor.OnInteractorDown() => onDown?.Invoke();
    void IUI_Interactor.OnInteractorUp() => onUp?.Invoke();

    
}
