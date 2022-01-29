using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TableController : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Transform bookPlacement;
    [SerializeField]
    private Transform bookStackLocation;

    [SerializeField]
    private float bookHeight;

    private List<BookController> bookStack = new List<BookController>();

    private BookController currentBook;

    public void Interact(PlayerController interactor)
    {
        BookController book = interactor.currentItem as BookController;
        if (book == null)
        {
            return;
        }

        if (interactor.currentItem is MonoBehaviour monoBehaviour)
        {
            monoBehaviour.transform.parent = null;
        }
        interactor.currentItem = null;

        StartCoroutine(HandleBookPlacement(book));
    }

    private IEnumerator HandleBookPlacement(BookController book) {

        if (currentBook)
        {
            // Move and close
            yield return currentBook.Open(false);
            yield return new WaitForSeconds(1f);
            // Alter the rotation slightly for nice variation
            currentBook.transform.rotation = Quaternion.Euler(bookStackLocation.rotation.eulerAngles + (Vector3.forward * Random.Range(-15, 15)));
            bookStack.Add(currentBook);

            CalcStackPositions();
        }

        book.transform.position = bookPlacement.position;
        book.transform.rotation = bookPlacement.rotation;
        book.gameObject.SetActive(true);
        currentBook = book;
        yield return book.Open();
    }

    // Start is called before the first frame update
    void FixedUpdate()
    {
        // Remove inactive objects
        int removedCount = bookStack.RemoveAll((book) => !book.gameObject.active);
        if (removedCount > 0)
        {
            CalcStackPositions();
        }
    }

    // Update is called once per frame
    void CalcStackPositions()
    {
        int i = 0;
        foreach (BookController book in bookStack)
        {
            book.transform.position = bookStackLocation.position + ((Vector3.up * bookHeight) * i);
            i++;
        }
        
    }

    public bool CanInteract(PlayerController interactor) => interactor.currentItem is BookController;
}
