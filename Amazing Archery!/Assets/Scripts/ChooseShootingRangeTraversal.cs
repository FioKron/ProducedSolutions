using UnityEngine;
using UnityEngine.UI;

public class ChooseShootingRangeTraversal : MonoBehaviour
{
    // For traversal between canvases:
    public Canvas MainMenuTopLevelMenu;
    public Canvas DefaultPlayerHUDReference;

    // Ranges to show:
    public GameObject Target;
    public GameObject Urban;
    public GameObject Country;

    // For each button in the order they appear in this sub-menu:
    public void CountryRangeTraversal()
    {
        Country.GetComponent<MeshRenderer>().enabled = true;
        PlayGame();
    }

    public void UrbanRangeTraversal()
    {
        Urban.GetComponent<MeshRenderer>().enabled = true;
        PlayGame();
    }

    public void BackToMainMenuTraversal()
    {
        MainMenuTopLevelMenu.enabled = true;
        HideChooseShootingRangeSubMenu();
    }

    // Upon traversal to a sub-menu:
    void HideChooseShootingRangeSubMenu()
    {
        GetComponent<Canvas>().enabled = false;
    }

    // For the one level type there is at the moment:
    void PlayGame()
    {
        DefaultPlayerHUDReference.enabled = true;
        Target.GetComponent<MeshRenderer>().enabled = true;
        HideChooseShootingRangeSubMenu();
    }
}