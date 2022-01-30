using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookData
{
    public static readonly List<string> subjects = new List<string>
    {
        "The complete history of",
        "The history of",
        "The science of",
        "The book of",
        "The records of",
        "A complete record of",
        "Learning for Dummies:",
    };

    public static readonly List<string> topics = new List<string>
    {
        "the past",
        "the future",
        "sceience",
        "biology",
        "physics",
        "love",
        "dance",
        "cooking",
        "murder",
        "law",
        "magic",
        "sex",
        "carpentry",
        "toilets",
    };


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
        return subjects[Random.Range(0, subjects.Count)] + "\n" + topics[Random.Range(0, topics.Count)];
    }

    public static string RandomSubTitle()
    {
        return subtitles[Random.Range(0, subtitles.Count)];
    }
}
