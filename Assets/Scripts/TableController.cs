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

        interactor.currentItem = null;

        StartCoroutine(HandleBookPlacement(book));
    }

    private IEnumerator HandleBookPlacement(BookController book) {

        if (currentBook)
        {
            // Move and close
            yield return currentBook.Open(false);
            yield return new WaitForSeconds(0.3f);
            // Alter the rotation slightly for nice variation
            currentBook.transform.rotation = Quaternion.Euler(bookStackLocation.rotation.eulerAngles + (Vector3.forward * Random.Range(-15, 15)));
            bookStack.Add(currentBook);

            yield return new WaitForSeconds(0.1f);

            currentBook.gameObject.layer = LayerMask.NameToLayer("Interactable");

            CalcStackPositions();
        }

        book.transform.parent = null;

        book.transform.position = bookPlacement.position;

        LeanTween.cancel(book.gameObject);
        LeanTween.move(book.gameObject, bookPlacement.position, 0.3f);
        book.transform.eulerAngles = bookPlacement.eulerAngles;
        book.gameObject.SetActive(true);
        currentBook = book;
        currentBook.gameObject.layer = LayerMask.NameToLayer("Default");
        yield return book.Open();
    }

    // Start is called before the first frame update
    void FixedUpdate()
    {
        // Remove inactive objects
        int removedCount = bookStack.RemoveAll((book) => book.gameObject.transform.GetComponentInParent<PlayerController>() != null);
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
            Vector3 positionn = bookStackLocation.position + ((Vector3.up * bookHeight) * i);

            if (!Mathf.Approximately(Vector3.Distance(positionn, transform.position), 0))
            {
                if (!LeanTween.isTweening(book.gameObject))
                    LeanTween.move(book.gameObject, positionn, 0.3f);
            }

            i++;
        }
        
    }

    public bool CanInteract(PlayerController interactor) => interactor.currentItem is BookController;
}
