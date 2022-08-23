# World button system

## How to setup
- Import prefabs from samples
  - For support ray interactor from `Unity XR Intergaration Toolkit` import XRI_Support from samples and add define `USE_UNITY_XRI_TOOLKIT` to project
- Put `buttons` and `interactor` to the scene

## How it works
- When `interactor` OnTriggerEnter or OnTriggerExit to button, it adds or removes button from `List<>`
- Interactor say to button about OnTriggerEnter or OnTriggerExit, and invoke Update in buttons
- Button determines whether is it pressed or not
- Visualizers visualize button state by color or press scale
- Buttons can receive standard UI TriggerEvents `onPointerEnter, onPointerExit, onPointerUp, onPointerDown`
