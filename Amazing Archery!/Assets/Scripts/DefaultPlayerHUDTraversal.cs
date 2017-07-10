using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DefaultPlayerHUDTraversal : MonoBehaviour
{
    // PlayerHUD Component References:
    public Text PowerLevelLblReference;
    public Text CurrentScoreLblReference;
    public Canvas PauseMenuReference;
    public Canvas DefaultPlayerHUDReference;

    // For getting members of the Player:
    public GameObject Player;

    // Arrow handling:
    public GameObject ArrowPrefabricationOriginal;
    Queue ArrowsInPlay = new Queue();

    // Constant values:
    const int MAX_ARROWS_IN_PLAY = 10;

    // Initilise:
    void Start ()
    {
        // For firing arrows:
        ArrowsInPlay = new Queue(MAX_ARROWS_IN_PLAY);
    }
	
	// Update the dynamic components of the Player's HUD:
	void Update ()
    {
        PowerLevelLblReference.text = Player.GetComponent<PlayerBehavior>().GetCurrentPowerLevel().ToString();
        CurrentScoreLblReference.text = Player.GetComponent<PlayerBehavior>().GetCurrentScore().ToString();
	}

    // Handle pausing:
    public void PauseGame()
    {
        if (Time.timeScale == 1.0f)
        {
            Time.timeScale = 0.0f;
            DefaultPlayerHUDReference.enabled = false;
            PauseMenuReference.enabled = true;
        }
    }

    // The callback function bound to the FireBtn's OnClick() event:
    public void FireArrow()
    {
        if (ArrowsInPlay.Count < MAX_ARROWS_IN_PLAY)
        {
            CreateArrow();
        }
        else
        {
            // Remove the 1st arrow, replacing it with the second arrow
            NullifyArrow();
            CreateArrow();
        }
    }

    // For cleanup:
    void NullifyArrow()
    {
        GameObject ArrowToNullify = (GameObject)ArrowsInPlay.Dequeue();

        Destroy(ArrowToNullify);
    }

    // For instantiation of arrows:
    void CreateArrow()
    {
        GameObject LatestArrow = Instantiate(ArrowPrefabricationOriginal);
        LatestArrow.transform.position = Player.transform.position + new Vector3(0.0f, 0.0f, 1.0f);
        LatestArrow.transform.rotation = Quaternion.Euler(new Vector3(45.0f, 0.0f, 0.0f));
        LatestArrow.GetComponent<Arrow>().PropelArrow(Player.GetComponent<PlayerBehavior>().GetCurrentPowerLevel());
        ArrowsInPlay.Enqueue(LatestArrow);
    }
}
