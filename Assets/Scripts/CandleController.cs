using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteAlways]
public class CandleController : MonoBehaviour
{
    private static List<Candle> candles;

    void Awake()
    {
        candles = FindObjectsOfType<Candle>().ToList();
    }

    public static Candle BlowOutRandom()
    {
        IEnumerable<Candle> onCandles = candles.Where((candle) => candle.on);

        if (onCandles.Count() == 0)
        {
            Debug.LogError("No candles to blow out");
            return null;
        }

        int toSkip = Random.Range(0, onCandles.Count());
        Candle candle = candles.Where((candle) => candle.on).Skip(toSkip).Take(1).First();
        candle.on = false;
        return candle;
    }

    public static void BlowOutAll()
    {
        foreach(Candle candle in candles)
        {
            candle.on = false;
        }
    }

    public static void SetAll(bool onState)
    {
        foreach (Candle candle in candles)
        {
            candle.on = onState;
        }
    }

    public static int CountLitCandles()
    {
        return candles.Where((candle) => candle.on).Count();
    }
}
