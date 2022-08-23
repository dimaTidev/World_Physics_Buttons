using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ButtonInteractionData", menuName = "ScriptableObjects/PhysicsButton/ButtonInteractionData", order = 30)]

public class UI_PhysicsButtonInteractionData : ScriptableObject
{
    [SerializeField] float pressDepth = 0.1f;
    public float PressDepth => pressDepth;

    [SerializeField] Color 
        colorPressed,
        colorDefault,
        colorHovered,
        colorSelected;

    public Color ColorPressed => colorPressed;
    public Color ColorDefault => colorDefault;
    public Color ColorHovered => colorHovered;
    public Color ColorSelected => colorSelected;

    [SerializeField] float springSpeed = 10;
    public float SpringSpeed => springSpeed;
}
