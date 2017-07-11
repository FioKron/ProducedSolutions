using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    // Public references:
    public float PropulsionForce = 3050.0f;

    // Private references:
    private GameObject PlayerGOReference;
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

        if ((OtherCollider.gameObject.tag == "LevelBound") ||
            (OtherCollider.gameObject.name == "HillLevelBound") ||
            (OtherCollider.gameObject.layer == 5))
        {
            
            print("DESTROY");
            Destroy();
        }
    }
	
	void Destroy()
    {
        gameObject.SetActive(false);
    }
    
    void OnDisable()
    {
        CancelInvoke();
    }
}
