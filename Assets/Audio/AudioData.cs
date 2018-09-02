using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AudioData : ScriptableObject {

    [Header("Station")]
    public AudioClip UsedStation_Preparation;
    public AudioClip UsedStation_Shape;
    public AudioClip UsedStation_Oven;
    public AudioClip UsedStation_Clean;
    public AudioClip UsedStation_Gloss;
    public AudioClip UsedStation_Color;
    public AudioClip UsedStation_Bell;

    [Header("Inventory")]
    public AudioClip ObjectInteraction_Grab;
    public AudioClip ObjectInteraction_StationSelection;

    [Header("UI")]
    public AudioClip Objective_NewRecipe;

}
