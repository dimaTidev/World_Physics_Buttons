public interface IUI_interactible
{
    void OnEnter(IUI_Interactor interactor);
    void OnExit(IUI_Interactor interactor);
    void OnUpdate_Interactible(IUI_Interactor interactor);
    void OnSelect_button(IUI_Interactor interactor, bool isActive);
}
