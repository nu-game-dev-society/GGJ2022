using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CauldronController : MonoBehaviour
{
    public delegate void CorrectItemAddedHandler();
    public event CorrectItemAddedHandler CorrectItemAdded;

    public delegate void IncorrectItemAddedHandler();
    public event IncorrectItemAddedHandler IncorrectItemAdded;

    public List<Guid> expectedItems = new List<Guid>();
    public List<Guid> receivedItems = new List<Guid>();

    GameManager gameManager;

    GameObject cauldronObject;

    // Start is called before the first frame update
    void Start()
    {
        this.gameManager = FindObjectOfType<GameManager>();
        this.cauldronObject = this.transform.parent.gameObject;
        this.Initialise();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialise()
    {
        this.expectedItems = this.gameManager.ExpectedItems.Select(item=>item.Guid).ToList();
        this.receivedItems = new List<Guid>();
    }

    public void AddReceivedItem(Guid receivedItemGuid)
    {
        this.receivedItems.Add(receivedItemGuid);

        if (this.AreReceivedItemsValid())
        {
            this.CorrectItemAdded?.Invoke();
        }
        else
        {
            this.IncorrectItemAdded?.Invoke();
            this.GoBad();
        }

        this.CheckIfComplete();
    }

    public bool IsValidNextItem(Guid guid)
    {
        Guid expectedGuid = this.expectedItems.ElementAt(this.receivedItems.Count - 1);
        return expectedGuid == guid;
    }

    public bool AreReceivedItemsValid()
    {
        int numReceivedItems = this.receivedItems.Count;
        IEnumerable<Guid> expectedItemsSoFar = this.expectedItems.Take(numReceivedItems);

        return expectedItemsSoFar.SequenceEqual(this.receivedItems);
    }

    public bool CheckIfComplete()
    {
        // if they're not the same size, obv wrong
        if (this.expectedItems.Count == this.receivedItems.Count)
        {
            return false;
        }

        // go through and make sure each was right
        // we do this every time its added but might as well be sure
        if (!this.AreReceivedItemsValid())
        {
            this.GoBad();
            return false;
        }

        return true;
    }

    public void GoBad()
    {

    }
}
