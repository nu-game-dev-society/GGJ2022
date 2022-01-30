using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Ingredient_Data", menuName = "NUGameDev/IngredientData", order = 1)]
public class IngredientData : ScriptableObject
{
    public Guid Guid => this.guid;
    Guid guid = Guid.NewGuid();

    public string Name => this.ingredientName;
    [SerializeField] string ingredientName;

    public string[] Clues => this.ingredientClues;
    [TextArea(2, 10)][SerializeField] string[] ingredientClues;
}
