using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class GameManager : MonoBehaviour
{
    public delegate void CorrectIngredientAddedToCauldronHandler();
    public event CorrectIngredientAddedToCauldronHandler CorrectIngredientAddedToCauldron;
        
    public delegate void IncorrectIngredientAddedToCauldronHandler();
    public event IncorrectIngredientAddedToCauldronHandler IncorrectIngredientAddedToCauldron;

    
    public List<IngredientData> PossibleIngredients  = new List<IngredientData>();

    public const int NUM_EXPECTED_INGREDIENTS = 4;
    public List<IngredientData> ExpectedIngredients { get; private set; } = new List<IngredientData>();

    CauldronController cauldronController;

    [ExecuteInEditMode]
    void Awake()
    {
        this.PossibleIngredients = GetAllInstances<IngredientData>().ToList();
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialise();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Initialise()
    {
        this.GenerateExpectedIngredients();
        this.cauldronController = FindObjectOfType<CauldronController>();
        this.cauldronController.CorrectIngredientAdded += OnCorrectIngredientAddedToCauldron;
        this.cauldronController.IncorrectIngredientAdded += OnIncorrectIngredientAddedToCauldron;
    }

    private void OnCorrectIngredientAddedToCauldron()
    {
        this.CorrectIngredientAddedToCauldron?.Invoke();
    }

    private void OnIncorrectIngredientAddedToCauldron()
    {
        this.IncorrectIngredientAddedToCauldron?.Invoke();
    }

    void GenerateExpectedIngredients()
    {
        int[] randomNumbers = new int[NUM_EXPECTED_INGREDIENTS];
        for(int i = 0; i < NUM_EXPECTED_INGREDIENTS; i++)
        {
            int randomNumber = -1;
            do
            {
                randomNumber = UnityEngine.Random.Range(0, this.PossibleIngredients.Count);
            }
            while (randomNumbers.Contains(randomNumber));
            randomNumbers[i] = randomNumber;
        }

        this.ExpectedIngredients = randomNumbers.Select(index => this.PossibleIngredients.ElementAt(index)).ToList();
    }

    // useful in cases like rather than having to set all the scriptable objs in properties panel, just search db for them
    // prob terrible idea for large scale dev but for jam it'll be much nicer
    public static IEnumerable<T> GetAllInstances<T>() where T : UnityEngine.Object
        => AssetDatabase.FindAssets($"t:{typeof(T).Name}")
            .Select(assetGuid => AssetDatabase.GUIDToAssetPath(assetGuid))
            .Select(assetPath => AssetDatabase.LoadAssetAtPath<T>(assetPath));
}
