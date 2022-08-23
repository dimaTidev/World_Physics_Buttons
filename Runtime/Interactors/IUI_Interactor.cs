using UnityEngine;

public interface IUI_Interactor
{
    void OnInteractorEnter();
    void OnInteractorExit();
    void OnInteractorDown();
    void OnInteractorUp();
    Transform transform { get; }
}
