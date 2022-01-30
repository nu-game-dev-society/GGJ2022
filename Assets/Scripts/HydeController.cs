using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydeController : MonoBehaviour
{
    [Tooltip("Starting Roll Interval")]
    public float nextRollTime;
    public float defaultRollInterval;
    public int rollLimit;
    public AudioSource audioSource;
    [Header("Debug")]
    public int livesLeft;
    // Start is called before the first frame update
    void Start()
    {
        nextRollTime = Time.time + nextRollTime;
        GameManager.Instance.cauldronController.IncorrectIngredientAdded += GameOver;
    }

    private void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextRollTime)
        {
            RollDice();
        }
    }

    private void RollDice()
    {
        nextRollTime = Time.time + defaultRollInterval; //if no candles left, 5 seconds to 
        if (CandleController.CountLitCandles() == 0)
        {
            GameManager.Instance.PlayerDeath();
        }
        else
        {
            if (Random.Range(0, 6) >= rollLimit)
            {
                //Blow out candle
                rollLimit += 2;
                Candle c = CandleController.BlowOutRandom();
                transform.position = c.transform.position;
                audioSource.Play();
                Debug.Log("Blow out, rollLimit: " + rollLimit);
            }
            else
            {
                LightningManager.Instance.MakeLightningHappen();
                rollLimit -= 1;
                Debug.Log("Do not blow out, rollLimit: " + rollLimit);
            }
        }
#if UNITY_EDITOR
        livesLeft = CandleController.CountLitCandles();
#endif
    }

    public void GameOver()
    {
        CandleController.BlowOutAll();
        audioSource.Play();
    }
}
