using UnityEngine;
using UnityEngine.UI;

public class MainMenuTraverasl : MonoBehaviour
{
    // For traversal to these sub-menus:
    public Canvas ChooseShootingRangeSubMenu;
    public Canvas HighScoreTableSubMenu;

    // For each button in the order they appear in the top-level menu:
    public void ChooseShootingRangeTraversal()
    {
        ChooseShootingRangeSubMenu.enabled = true;
        HideMainMenu();
    }

    public void HighScoreTableTraversal()
    {
        InitiliseHighScoreTableSubMenu();
        HideMainMenu();
    }

    // Initilise this canvas for showing the current scores on the high score list, to the Player:
    void InitiliseHighScoreTableSubMenu()
    {
        HighScoreTableSubMenu.enabled = true;
        HighScoreTableSubMenu.SendMessage("InitiliseHighScoreTable");
    }

    public void ExitGameTraversal()
    {
        Application.Quit();
    }

    // Upon traversal to a sub-menu:
    void HideMainMenu()
    {
        GetComponent<Canvas>().enabled = false;
    }
}
