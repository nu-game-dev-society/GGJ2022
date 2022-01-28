using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CandleController : MonoBehaviour
{
    private List<Candle> candles;

    // Start is called before the first frame update
    void Start()
    {
        candles = FindObjectsOfType<Candle>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BlowOutRandom()
    {
        IEnumerable<Candle> onCandles = candles.Where((candle) => candle.on);

        if (onCandles.Count() == 0)
        {
            Debug.Log("No candles to blow out");
            return;
        }

        int toSkip = Random.Range(0, onCandles.Count());
        Candle candle = candles.Where((candle) => candle.on).Skip(toSkip).Take(1).First();
        candle.on = false;
    }

    public void SetAll(bool onState)
    {
        foreach (Candle candle in candles)
        {
            candle.on = onState;
        }
    }
}
