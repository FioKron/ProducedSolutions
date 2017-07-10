using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour
{
    // Constant values:
    const float PROPULSION_FORCE_MULTIPLYER = 50.0f;

    // For increasing the Player's score:
    GameObject PlayerCharacterReference;

    // Flags:
    bool TargetReceivedHit = false;

    void Start()
    {
        PlayerCharacterReference = GameObject.FindGameObjectWithTag("Player");
    }

    // Apply force to this Arrow:
    public void PropelArrow(float PowerLevelMultiplyer)
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, 0.50f, 1.0f) * PROPULSION_FORCE_MULTIPLYER * PowerLevelMultiplyer * Time.deltaTime, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision Collision)
    {
        // Verification first:
        if (Collision.gameObject.tag == "Target" && !(TargetReceivedHit))
        {
            PlayerCharacterReference.GetComponent<PlayerBehavior>().IncreasePlayerScore();
            TargetReceivedHit = true;
        }
    }
}
