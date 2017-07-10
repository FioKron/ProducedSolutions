using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class HighScoreTableTraversal : MonoBehaviour
{
    // For traversal between canvases:
    public Canvas MainMenuTopLevelMenu;

    // For adding to the high score table, visible to the Player:
    public Text NameTxt;
    public Text ScoreTxt;

    // For the high score system:    
    public const string PLAYER_SCORE = "PlayerScore";
    public const string PLAYER_NAME = "PlayerName";
    public const string TOTAL_SCORE_COUNT = "TotalScoreCount";
    public Dictionary<string, int> ScoreTable = new Dictionary<string, int>();
    
    public void BackToMainMenuTraversal()
    {
        MainMenuTopLevelMenu.enabled = true;
        HideHighScoreTableSubMenu();
    }

    // Upon traversal to a sub-menu:
    void HideHighScoreTableSubMenu()
    {
        GetComponent<Canvas>().enabled = false;
    }

    // Populate HighScoreTable:
    void InitiliseHighScoreTable()
    {
        ReadPlayerValuesFromPlayerPreferences();
    }

    // Add scores to ScoreTable, sort them and then; add them to the HighScoreTable:
    void ReadPlayerValuesFromPlayerPreferences()
    {
        // Before reading values from Player Preferences...
        ScoreTable.Clear();
        // Clear the text fields as well:
        NameTxt.text = "";
        ScoreTxt.text = "";

        int TotalNumberOfRecords = PlayerPrefs.GetInt(TOTAL_SCORE_COUNT);

        for (int Counter = 0; Counter < TotalNumberOfRecords; Counter++)
        {
            int ThisScore = PlayerPrefs.GetInt(PLAYER_SCORE + Counter);
            string ThisName = PlayerPrefs.GetString(PLAYER_NAME + Counter);
            ScoreTable.Add(ThisName, ThisScore);
        }

        IOrderedEnumerable<KeyValuePair<string, int>> LinqDesendingOrderedList;

        // Perform this query, to sort the Player Name's and their scores, for their scores; in desending order:
        LinqDesendingOrderedList = from HighScorePair in ScoreTable
                                   orderby HighScorePair.Value descending,
                                       HighScorePair.Key
                                   select HighScorePair;

        Dictionary<string, int> OrderedScores = LinqDesendingOrderedList.ToDictionary(X => X.Key, X => X.Value);

        for (int Counter = 0; Counter < TotalNumberOfRecords; Counter++)
        {
            NameTxt.text += OrderedScores.Keys.ToArray()[Counter] + "\n";
            ScoreTxt.text += OrderedScores.Values.ToArray()[Counter].ToString() + "\n";
        }
    }
}