using System;
using System.Linq;
using UnityEngine;

public class RecipeController : MonoBehaviour
{
    [SerializeField]
    private Transform itemSpawnsParent;

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

        if (itemSpawnsParent == null)
        {
            Debug.LogError("No item spawns set!");
            return;
        }

        // Spawn items
        int spawnedCount = 0;
        Transform[] itemSpawns = itemSpawnsParent.GetComponentsInChildren<Transform>();
        foreach (Transform position in itemSpawns)
        {
            // Ignore the parent pos
            if (position == itemSpawnsParent)
            {
                continue;
            }

            IngredientData data;
            if (spawnedCount < GameManager.Instance.ExpectedIngredients.Count)
            {
                data = GameManager.Instance.ExpectedIngredients[spawnedCount];
            }
            else
            {
                data = GameManager.Instance.PossibleIngredients[UnityEngine.Random.Range(0, GameManager.Instance.PossibleIngredients.Count)];
            }

            GameObject item = Instantiate(data.Model);
            item.transform.position = position.position;
            item.AddComponent<IngredientController>().Data = data;

            spawnedCount++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
