using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuTraversal : MonoBehaviour
{
    // For traversal between canvases:
    public Canvas AddToHighScoreMenuReference;
    public Canvas DefaultPlayerHUDReference;

    // For the game objects in the game scene:
    public GameObject Target;
    public GameObject Urban;
    public GameObject Country;

    // For traversal back to the main menu:
    public void ShowAddToHighScoreMenu()
    {
        AddToHighScoreMenuReference.enabled = true;
        HideGameSceneComponents();
        ResetTimeScale();
        HidePauseMenu();
    }

    // For resuming the game:
    public void ResumeGame()
    {
        DefaultPlayerHUDReference.enabled = true;
        ResetTimeScale();
        HidePauseMenu();
    }

    // For hiding components in a shooting range's scene (upon traversal back to the Main Menu):
    void HideGameSceneComponents()
    {
        Target.GetComponent<MeshRenderer>().enabled = false;
        Urban.GetComponent<MeshRenderer>().enabled = false;
        Country.GetComponent<MeshRenderer>().enabled = false;

        foreach (GameObject ArrowProjectile in GameObject.FindGameObjectsWithTag("Arrow"))
        {
            ArrowProjectile.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    // Upon traversal to a sub-menu:
    void HidePauseMenu()
    {
        GetComponent<Canvas>().enabled = false;
    }

    // For reseting the time scale back to its default value:
    void ResetTimeScale()
    {
        Time.timeScale = 1.0f; // For normal time scale
    }
}
