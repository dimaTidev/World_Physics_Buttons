# World button system

### 3 types of buttons:
- UI_PointerButton.cs
  - Only standard UI TriggerEvents `onPointerEnter, onPointerExit, onPointerUp, onPointerDown`
  - Toggle mode
- UI_PhysicsButton.cs
  - Standard UI TriggerEvents `onPointerEnter, onPointerExit, onPointerUp, onPointerDown`
  - Press by interactor
  - Toggle mode
- UI_TriggerButton.cs
  - Support standard UI TriggerEvents `onPointerEnter, onPointerExit, onPointerUp, onPointerDown`
  - Triggering by interactor
  - Triggering when OnTriggerEnter, but you can disable `isTriggerWhenOnEnter`
    - If you want to make Triggering by OnTriggerEnter + Button press, disable `isTriggerWhenOnEnter` and provide button press logic
  - Toggle mode

## How to setup
- Import prefabs from samples
  - For support ray interactor from `Unity XR Intergaration Toolkit` import XRI_Support from samples and add define `USE_UNITY_XRI_TOOLKIT` to project
- Put `buttons` and `interactor` to the scene

## How it works
- When `interactor` OnTriggerEnter or OnTriggerExit to button, it adds or removes button from `List<>`
- Interactor say to button about OnTriggerEnter or OnTriggerExit, and invoke Update in buttons
- Button determines whether is it pressed or not
- Visualizers visualize button state by color or press scale
- For Visuzl parameters use ScriptableObject `Create/ScriptableObjects/PhysicsButton/ButtonInteractionData`
- Buttons can receive standard UI TriggerEvents `onPointerEnter, onPointerExit, onPointerUp, onPointerDown`
