using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TableController : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Transform bookPlacement;
    [SerializeField]
    private Transform bookStack;

    private BookController currentBook;

    public void Interact(PlayerController interactor)
    {
        BookController book = interactor.currentItem as BookController;
        if (book == null)
        {
            return;
        }

        interactor.currentItem = null;

        StartCoroutine(HandleBookPlacement(book));

    }

    private IEnumerator HandleBookPlacement(BookController book) {

        if (currentBook)
        {
            // Move and close
            yield return currentBook.Open(false);
            yield return new WaitForSeconds(0.5f);
            currentBook.transform.position = bookStack.position;
            currentBook.transform.rotation = bookStack.rotation;
            Debug.Log("Moved current"); 
        }

        book.transform.position = bookPlacement.position;
        book.transform.rotation = bookPlacement.rotation;
        book.gameObject.SetActive(true);
        currentBook = book;
        yield return book.Open();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
