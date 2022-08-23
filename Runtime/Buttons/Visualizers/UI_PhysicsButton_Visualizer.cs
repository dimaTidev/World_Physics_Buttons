using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimaTi.PhysicsButtons
{
    [RequireComponent(typeof(UI_PhysicsButton))]
    public class UI_PhysicsButton_Visualizer : UI_PointerButton_Visualizer//, IPhysicsPointer
    {
        Vector3 startScale;

        private UI_PhysicsButton button;
        float tempDistance;

        protected override void Start()
        {
            base.Start();
            button = GetComponent<UI_PhysicsButton>();
            button.OnInteractors_Update += OnUpdate_Interactors;
            startScale = VisualButton.transform.localScale;
            startScale.z = Data.PressDepth;
            VisualButton.transform.localScale = startScale;

            BoxCollider boxCollider = GetComponent<BoxCollider>();
            if (boxCollider)
            {
                float offset = startScale.z * 1.2f - startScale.z;
                boxCollider.center = new Vector3(0, 0, startScale.z / 2 - offset / 2);
                boxCollider.size = new Vector3(startScale.x, startScale.y, startScale.z + offset);
            }
        }

        private void Update()
        {
            if (!button.IsPressed && !button.IsPhysicalHover && tempDistance < startScale.z)
                SetScale(Mathf.MoveTowards(tempDistance, startScale.z, Time.deltaTime * Data.SpringSpeed));
        }

        void OnUpdate_Interactors() => SetScale(button.MinDistToInteractor < startScale.z ? button.MinDistToInteractor : startScale.z);

        protected override void OnStateChanged(bool isActive, bool isEntered, bool isPressed)
        {
            base.OnStateChanged(isActive, isEntered, isPressed);
            if (isPressed)
                SetScale(startScale.z * 0.1f);
        }

        void SetScale(float dist)
        {
            Vector3 localScale = new Vector3(startScale.x, startScale.y, dist);
            VisualButton.transform.localScale = localScale;
            tempDistance = dist;
        }
    }
}
