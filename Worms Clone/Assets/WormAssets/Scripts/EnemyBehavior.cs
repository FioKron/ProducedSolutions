using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : SpriteBehavior
{
    // Initialise:
    private void Start()
    {
        //BulletPool = new List<GameObject>();

        //for (int CreationCount = 0; CreationCount < MaxBulletCount; CreationCount++)
        //{
        //    GameObject BulletForPool = Instantiate(Bullet);
        //    BulletForPool.SetActive(false);
        //    BulletPool.Add(BulletForPool);
        //}

        InitialSpriteRotation = transform.rotation.eulerAngles;
        Sprite2DRigidBody = GetComponent<Rigidbody2D>();
        RifleShotSource = GetComponent<AudioSource>();
        CurrentHealthTextComponentReference.text = CurrentHealth.ToString();
        SpriteIsPlayer = false;
        //print(ActiveWeaponRenderer.name);
    }

    // Handle updates:
    void Update ()
    {
		
	}

    private void OnCollisionEnter2D(Collision2D OtherInCollision)
    {     
        if (OtherInCollision.gameObject.layer == GROUND_LAYER)
        {
            print("Character now grounded");
            Grounded = true;
        }
    }
}
