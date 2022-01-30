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
        "Eldritch:",
        "Lovecraft:",
        "The Horrors of",
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
        "hunting",
        "deffence",
        "Dunwich",
        "Color",
        "Caravans",
        "Broken Arms",
        "The Three Seas",
        "For Dummies",
    };

    public static readonly List<string> subtitles = new List<string>
    {
        "Volume I", 
        "Volume II",
        "Volume III",
        "Volume IV",
        "Volume V",
        "Volume VI",
        "Volume LXIX",
        "Volume CDXX",
        "Volume MCCCXXXVII",
    };

    public static string RandomTitle()
    {
        return subjects[Random.Range(0, subjects.Count)] + "\n" + topics[Random.Range(0, topics.Count)];
    }

    public static string RandomSubTitle()
    {
        return subtitles[Random.Range(0, subtitles.Count)];
    }

    public static string RandomContent()
    {
        return GameManager.Instance.Clues[Random.Range(0, GameManager.Instance.Clues.Count)];
    }
}
