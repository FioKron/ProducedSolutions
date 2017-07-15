using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    // Public references:
    public float PropulsionForce = 3050.0f;

    // Private references:
    private GameObject PlayerGOReference;

    // Constant values:

    private const float MAX_BULLET_LIFE_TIME = 2.0f;
    private const float COLLISION_DESTRUCTION_INVOCATION_DELAY = 0.010f;

    // For the position to 'hide' the bullet in:
    public Vector3 HIDDEN_POSITION = new Vector3(-100.0f, -100.0f, 0.0f);

    void OnEnable()
    {
        foreach (GameObject ThisGO in FindObjectsOfType<GameObject>())
        {
            if (ThisGO.tag == "Player")
            {
                PlayerGOReference = ThisGO;
            }
        }
            
        // For the appropriate direction this bullet is set to travel in:
        float ShootingDirectionModifier = 1.0f;

        // Verify this...
        if (!(PlayerGOReference.GetComponent<PlayerBehavior>().FacingRightwards))
        {          
            ShootingDirectionModifier = -1.0f;
        }
        //print(PlayerGOReference.GetComponent<PlayerBehavior>().FacingRightwards);

        // ...then set the bullet off on its path:
        GetComponent<Rigidbody2D>().AddForce(new Vector2((PropulsionForce * ShootingDirectionModifier), 0.0f));
        Invoke("Destroy", 2.0f);
    }

    void OnCollisionEnter2D(Collision2D OtherCollider)
    {
        if (OtherCollider.gameObject.CompareTag("LevelBound"))
        {         
            print("DESTROY");
            Destroy();
            //Invoke("Destroy", 0.010f);
        }
    }
	
	void Destroy()
    {
        // Relocate the bullet before setting it to an inactive state:
        RelocateBullet();
        gameObject.SetActive(false);
    }

    // Set the bullet's location to somewhere quite far from what the Player can see:
    void RelocateBullet()
    {
        gameObject.transform.position = HIDDEN_POSITION;
    }
    
    void OnDisable()
    {
        CancelInvoke();
    }
}
