using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RecipeController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BookController[] ingredientBooks = FindObjectsOfType<BookController>().OrderBy(arg => Guid.NewGuid()).Take(GameManager.NUM_EXPECTED_INGREDIENTS).ToArray();
        int i = 0;
        foreach (IngredientData ingredient in GameManager.Instance.ExpectedIngredients)
        {
            string clue = ingredient.Clues[UnityEngine.Random.Range(0, ingredient.Clues.Length)];

            ingredientBooks[i].SetContents(clue);

            i++;
        }

        // Log the books we need to display
        foreach (BookController book in ingredientBooks)
        {
            Debug.Log(book.GetName());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
