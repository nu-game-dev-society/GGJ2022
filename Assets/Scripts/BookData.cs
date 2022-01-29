using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookData
{
    public static readonly List<string> titles = new List<string>
    {
        "The complete history of\nthe past",
        "Book 2\nline2",
        "Book 3\nanother one",
        "Book 4\nreally?",
        "Book 5\nim good",
    };

    public static readonly List<string> subtitles = new List<string>
    {
        "Volume I",
        "Volume II",
        "Volume III",
        "Volume IV",
        "Volume V",
        "Volume VI",
    };

    public static string RandomTitle()
    {
        return titles[Random.Range(0, titles.Count - 1)];
    }

    public static string RandomSubTitle()
    {
        return subtitles[Random.Range(0, subtitles.Count - 1)];
    }
}
