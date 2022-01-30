using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBookController : MonoBehaviour, IAudioEvent
{
    public Animator anim;
    public AudioSource audioSource;
    public AudioClip openClip, closeClip;
    void Start()
    {
        if (anim == null)
            anim = GetComponent<Animator>();
        StartCoroutine(Open());
        Time.timeScale = 1.0f;
    }


    public IEnumerator Open(bool state = true)
    {
        yield return new WaitForSeconds(1);
        anim.SetBool("Open", state);
        audioSource.Play();
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
    }

    public void PlayGame()
    {
        Debug.Log("Play");
        PlayGameAsync();
    }

    private async void PlayGameAsync()
    {
        var bookClose = CloseBook();
        var scene = LoadGameSceneAsync();
        while (!bookClose.IsCompleted || !scene.IsCompleted)
        {
            await Task.Yield();
        }
        Debug.Log("LOAD SCENE NOW");
        scene.Result.allowSceneActivation = true;
    }
    public float value;
    private async Task<AsyncOperation> LoadGameSceneAsync()
    {
        AsyncOperation x = SceneManager.LoadSceneAsync(1);

        x.allowSceneActivation = false;

        while (x.progress < 0.899999)
        {
            value = x.progress;
            await Task.Yield();
        }
        Debug.Log("Scene Loaded");
        return x;
    }
    private async Task CloseBook()
    {
        anim.SetBool("Open", false);
        await Task.Delay(1000);
        ScreenFader.instance?.SetToBlack(true);
        await Task.Delay(1000);
        Debug.Log("Book Close");

    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Event()
    {
        audioSource.PlayOneShot(anim.GetBool("Open") ? openClip : closeClip);
    }
}
