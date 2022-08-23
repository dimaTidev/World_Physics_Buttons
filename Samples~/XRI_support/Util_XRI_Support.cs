using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace DimaTi.PhysicsButtons
{
    public static class Util_XRI_Support
    {
        /// <summary>
        /// Calculate Interaction point. Intersection between `interaction ray` and `interactible game object`
        /// </summary>
        /// <param name="eventData"></param>
        /// <param name="interactionPoint"></param>
        /// <param name="planePos">transform.position</param>
        /// <param name="planeNormal"> planeNormal = transform.forward</param>
        public static Vector3 XRI_InteractionPoint(PointerEventData eventData, Vector3 planePos, Vector3 planeNormal)
        {
            Vector3 interactionPoint = planePos;
            //Calculating hitPoint
            if (eventData != null && eventData is TrackedDeviceEventData trackedEventData) //If interaction was from XR.Interaction.Toolkit.UI system
            {
                if (trackedEventData.rayPoints != null && trackedEventData.rayPoints.Count >= 2)
                {
                    if (Calculate_InteractionPoint(planePos, planeNormal,
                        trackedEventData.rayPoints[0], (trackedEventData.rayPoints[1] - trackedEventData.rayPoints[0]).normalized,
                        out Vector3 hitPoint))
                    {
                        interactionPoint = hitPoint;
                    }
                }
            }
            return interactionPoint;
        }

        static bool Calculate_InteractionPoint(Vector3 planePos, Vector3 planeNormal, Vector3 rayPoint, Vector3 rayDirection, out Vector3 hitPoint)
        {
            Plane m_Plane = new Plane(planeNormal, planePos);

            Ray ray = new Ray(rayPoint, rayDirection);
            if (m_Plane.Raycast(ray, out float enter))
            {
                hitPoint = ray.GetPoint(enter);//Get the point that is clicked
                return true;
            }

            hitPoint = Vector3.zero;
            return false;
        }
    }
}
