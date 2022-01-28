using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// creating this here because I need it but just replace whenever we actually make items
public class Item
{
    public Guid Guid { get; private set; }
}

public class GameManager : MonoBehaviour
{
    public delegate void CorrectItemAddedToCauldronHandler();
    public event CorrectItemAddedToCauldronHandler CorrectItemAddedToCauldron;
        
    public delegate void IncorrectItemAddedToCauldronHandler();
    public event IncorrectItemAddedToCauldronHandler IncorrectItemAddedToCauldron;

    public List<Item> ExpectedItems { get; private set; } = new List<Item>();

    CauldronController cauldronController;
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
        this.GenerateExpectedItems();
        this.cauldronController = FindObjectOfType<CauldronController>();
        this.cauldronController.CorrectItemAdded += OnCorrectItemAddedToCauldron;
        this.cauldronController.IncorrectItemAdded += OnIncorrectItemAddedToCauldron;
    }

    private void OnCorrectItemAddedToCauldron()
    {
        this.CorrectItemAddedToCauldron?.Invoke();
    }

    private void OnIncorrectItemAddedToCauldron()
    {
        this.IncorrectItemAddedToCauldron?.Invoke();
    }

    void GenerateExpectedItems()
    {

    }
}
