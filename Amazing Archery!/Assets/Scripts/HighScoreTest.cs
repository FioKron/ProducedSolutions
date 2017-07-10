using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// For testing the high score system the game will use:
public class HighScoreTest : MonoBehaviour
{
    private const string PLAYER_SCORE = "PlayerScore";
    private const string PLAYER_NAME = "PlayerName";
    private const string TOTAL_SCORE_COUNT = "TotalScoreCount";
    private Dictionary<string, int> ScoreTable = new Dictionary<string, int>();

    // Store the scores in PlayerPreferences:
    void AddScoresToPlayerPreferences()
    {
        ScoreTable.Add("Tim", 1);
        ScoreTable.Add("Jack Malone", 150);
        ScoreTable.Add("John Bishop", 640);
        ScoreTable.Add("Daniel Adakai", 470);

        int Counter = 0;

        foreach (KeyValuePair<string, int> Record in ScoreTable)
        {
            PlayerPrefs.SetInt(PLAYER_SCORE + Counter, Record.Value);
            PlayerPrefs.SetString(PLAYER_NAME + Counter, Record.Key);
            PlayerPrefs.SetInt(TOTAL_SCORE_COUNT, Counter);
            Counter++;
        }
    }

    // Add scores to the score table, sort them
    void ReadScoresFromPlayerPreferences()
    {
        ScoreTable.Clear();

        int TotalNumberOfRecords = PlayerPrefs.GetInt(TOTAL_SCORE_COUNT);

        for (int Counter = 0; Counter < TotalNumberOfRecords; Counter++)
        {
            int ThisScore = PlayerPrefs.GetInt(PLAYER_SCORE + Counter);
            string ThisName = PlayerPrefs.GetString(PLAYER_NAME + Counter);
            ScoreTable.Add(ThisName, ThisScore);
        }

        IOrderedEnumerable<KeyValuePair<string, int>> LinqDesendingOrderedList;

        LinqDesendingOrderedList = from HighScorePair in ScoreTable
                                   orderby HighScorePair.Value descending,
                                       HighScorePair.Key
                                   select HighScorePair;

        Dictionary<string, int> OrderedScores = LinqDesendingOrderedList.ToDictionary(X => X.Key, X => X.Value);

        for (int Counter = 0; Counter < TotalNumberOfRecords; Counter++)
        {
            print(OrderedScores.Keys.ToArray()[Counter] + " " + OrderedScores.Values.ToArray()[Counter]);
        }
    }

	// Initilisation:
	void Start ()
    {
        AddScoresToPlayerPreferences();
	}
	
	// Update is called once per frame
	void Update ()
    {
        ReadScoresFromPlayerPreferences();
	}
}
