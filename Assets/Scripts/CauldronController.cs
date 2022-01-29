using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CauldronController : MonoBehaviour, IInteractable
{
    public delegate void CorrectIngredientAddedHandler();
    public event CorrectIngredientAddedHandler CorrectIngredientAdded;

    public delegate void IncorrectIngredientAddedHandler();
    public event IncorrectIngredientAddedHandler IncorrectIngredientAdded;

    public List<IngredientData> ExpectedIngredients => this.gameManager?.ExpectedIngredients ?? new List<IngredientData>();
    public List<IngredientData> ReceivedIngredients = new List<IngredientData>();

    GameManager gameManager;

    GameObject cauldronObject;

    ParticleSystem particleSystem;

    PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        this.Initialise();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // allows us to reset the cauldron without restarting the scene (if we should want to)
    public void Initialise()
    {
        this.gameManager = FindObjectOfType<GameManager>();
        this.cauldronObject = this.gameObject;
        this.ReceivedIngredients.Clear();
        this.player = FindObjectOfType<PlayerController>();
        this.particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    public void AddReceivedIngredient(IngredientData receivedIngredient)
    {
        this.ReceivedIngredients.Add(receivedIngredient);

        if (this.AreReceivedIngredientsValidSoFar())
        {
            OnCorrectIngredientAdded();
        }
        else
        {
            OnIncorrectIngredientAdded();
        }

        this.CheckIfComplete();
    }


    public bool IsValidNextIngredient(IngredientData ingredient)
    {
        IngredientData expectedIngredient = this.ExpectedIngredients.ElementAt(this.ReceivedIngredients.Count - 1);
        return expectedIngredient.Guid == ingredient.Guid;
    }

    public bool AreReceivedIngredientsValidSoFar()
    {
        int numReceivedIngredients = this.ReceivedIngredients.Count;
        IEnumerable<IngredientData> expectedIngredientsSoFar = this.ExpectedIngredients.Take(numReceivedIngredients);

        return expectedIngredientsSoFar
            .Select(ingredient => ingredient.Guid)
            .SequenceEqual(
                this.ReceivedIngredients.Select(ingredient => ingredient.Guid)
            );
    }

    public bool CheckIfComplete()
    {
        // if they're not the same size, obv wrong
        if (this.ExpectedIngredients.Count != this.ReceivedIngredients.Count)
        {
            return false;
        }

        // go through and make sure each was right
        // we do this every time its added but might as well be sure
        if (!this.AreReceivedIngredientsValidSoFar())
        {
            OnIncorrectIngredientAdded();
            return false;
        }

        return true;
    }

    private void OnCorrectIngredientAdded()
    {
        this.PlayParticles();
        this.DisableInteraction(this.particleSystem.duration);
        this.CorrectIngredientAdded?.Invoke();
    }

    private void OnIncorrectIngredientAdded()
    {
        this.PlayParticles();
        StartCoroutine(this.DisableInteraction(this.particleSystem.main.duration));
        this.IncorrectIngredientAdded?.Invoke();
    }

    void PlayParticles()
    {
        this.particleSystem.Play();
    }

    IEnumerator DisableInteraction(float seconds = 0)
    {
        this.canInteract = false;

        yield return new WaitForSeconds(seconds);
        
        if (seconds > 0)
        {
            this.canInteract = true;
        }
    }

    public void Interact(PlayerController interactor)
    {
        //TEMP
        this.OnIncorrectIngredientAdded();

        if (interactor.currentItem is IngredientController ingredient)
        {
            this.AddReceivedIngredient(ingredient.Data);
            interactor.currentItem = null;
        }
        else
        {
            // do we want to show text when the player clicks the cauldron without an ingredient?
            // something like "you can't throw a book in here!"
        }
    }

    public bool canInteract = true;
    public bool CanInteract(PlayerController interactor)
    {
        Debug.Log(canInteract);
        return canInteract;
    }
}
