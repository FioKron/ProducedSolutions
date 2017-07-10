using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;


public class AddToHighScoreTableMenuTraversal : MonoBehaviour
{
    // For the high score system:    
    public const string PLAYER_SCORE = "PlayerScore";
    public const string PLAYER_NAME = "PlayerName";
    public const string TOTAL_SCORE_COUNT = "TotalScoreCount";
    public Dictionary<string, int> ScoreTable = new Dictionary<string, int>();

    // For a reference to the 'parent' object in the heirarchy (also; a few of its components):
    public Canvas AddToHighScoreMenuReference;
    public InputField NameEntryInputField;
    // For traversal back to the Main Menu:
    public Canvas MainMenuReference;

    // To get values from the Player:
    public GameObject PlayerReference;

    // To receive updating, whenever NameEntryInputField is updated:
    string PlayerID = "";

    // Store the scores in PlayerPreferences:
    void AddPlayerValuesToPlayerPreferences()
    {        
        ScoreTable.Add(PlayerID, PlayerReference.GetComponent<PlayerBehavior>().GetCurrentScore());

        // For a unique reference to each Player:
        int Counter = 0;

        // Set values in PlayerPrefs:
        foreach (KeyValuePair<string, int> Record in ScoreTable)
        {
            PlayerPrefs.SetInt(PLAYER_SCORE + Counter, Record.Value);
            PlayerPrefs.SetString(PLAYER_NAME + Counter, Record.Key);         
            Counter++; // THE ORDER WAS NOT QUITE CORRECT FOR INCREMENTING THE COUNTER
            PlayerPrefs.SetInt(TOTAL_SCORE_COUNT, Counter);
        }
    }

    // For whenever the text in this field is modified:
    public void UpdateActivePlayersName()
    {
        PlayerID = NameEntryInputField.text;
    }

    public void ReturnToMainMenuTraversal()
    {
        MainMenuReference.enabled = true;
        AddPlayerValuesToPlayerPreferences();
        HideAddToHighScoreTableMenuTraversal();
        PlayerReference.GetComponent<PlayerBehavior>().ResetPlayer();
    }

    // Upon traversal to the MainMenu:
    void HideAddToHighScoreTableMenuTraversal()
    {
        GetComponent<Canvas>().enabled = false;
    }
}
