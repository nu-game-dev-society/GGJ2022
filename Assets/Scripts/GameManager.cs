using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteAlways]
public class GameManager : MonoBehaviour
{
    public delegate void CorrectIngredientAddedToCauldronHandler();
    public event CorrectIngredientAddedToCauldronHandler CorrectIngredientAddedToCauldron;

    public delegate void IncorrectIngredientAddedToCauldronHandler();
    public event IncorrectIngredientAddedToCauldronHandler IncorrectIngredientAddedToCauldron;


    public List<IngredientData> PossibleIngredients = new List<IngredientData>();

    public const int NUM_EXPECTED_INGREDIENTS = 4;
    public List<IngredientData> ExpectedIngredients = new List<IngredientData>();
    public List<string> Clues { get; private set; } = new List<string>();
    [HideInInspector]
    public CauldronController cauldronController;

    public static GameManager Instance;
    public GameObject DeathScreen;
    public GameObject WinScreen;

    void Awake()
    {
        if (Application.isPlaying)
        {
            InitialiseSingleton();
            Initialise();
        }
        else
        {
            InitialiseExpectedIngredients();
        }
    }

    void InitialiseSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
        Debug.Log("InitialiseInstance");
    }

    void InitialiseExpectedIngredients()
    {
#if UNITY_EDITOR
        this.PossibleIngredients = GetAllInstances<IngredientData>().ToList();
        Debug.Log("InitialiseExpectedIngredients");
#endif
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Application.isPlaying)
        {
            //Initialise();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("We closing now!");
            Application.Quit();
        }
    }

    void Initialise()
    {
        this.GenerateExpectedIngredients();
        this.CollateRiddles();

        if (DeathScreen == null)
            DeathScreen = GameObject.Find("Canvas/DeathScreen");
        this.cauldronController = FindObjectOfType<CauldronController>();
        this.cauldronController.CorrectIngredientAdded += OnCorrectIngredientAddedToCauldron;
        this.cauldronController.IncorrectIngredientAdded += OnIncorrectIngredientAddedToCauldron;
        this.cauldronController.RecipeComplete += OnRecipeComplete;
    }

    private void OnRecipeComplete()
    {
        PlayerWin();
    }

    private void PlayerWin()
    {
        FindObjectOfType<PlayerController>().GameOver();
        WinScreen.SetActive(true);
        ScreenFader.instance.SetToBlack(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnCorrectIngredientAddedToCauldron()
    {
        this.CorrectIngredientAddedToCauldron?.Invoke();
    }

    private void OnIncorrectIngredientAddedToCauldron()
    {
        this.IncorrectIngredientAddedToCauldron?.Invoke();
        PlayerDeath();
    }

    public void PlayerDeath()
    {
        FindObjectOfType<PlayerController>().GameOver();
        DeathScreen.SetActive(true);
        ScreenFader.instance.SetToBlack(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void GenerateExpectedIngredients()
    {
        int[] randomNumbers = new int[NUM_EXPECTED_INGREDIENTS];
        // Init the array to -1
        for (int i = 0; i < NUM_EXPECTED_INGREDIENTS; i++)
        {
            randomNumbers[i] = -1;
        }

        for (int i = 0; i < NUM_EXPECTED_INGREDIENTS; i++)
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

    void CollateRiddles()
    {
        Clues = new List<string>();

        foreach (IngredientData ingredient in PossibleIngredients)
        {
            Clues.AddRange(ingredient.Clues);
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene(0);
    }


#if UNITY_EDITOR
    // useful in cases like rather than having to set all the scriptable objs in properties panel, just search db for them
    // prob terrible idea for large scale dev but for jam it'll be much nicer
    public static IEnumerable<T> GetAllInstances<T>() where T : UnityEngine.Object
        => AssetDatabase.FindAssets($"t:{typeof(T).Name}")
            .Select(assetGuid => AssetDatabase.GUIDToAssetPath(assetGuid))
            .Select(assetPath => AssetDatabase.LoadAssetAtPath<T>(assetPath));
#endif


}
