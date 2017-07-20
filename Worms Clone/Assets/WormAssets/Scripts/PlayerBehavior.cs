using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : SpriteBehavior
{
    private void Start()
    {
        BulletPool = new List<GameObject>();

        for (int CreationCount = 0; CreationCount < MaxBulletCount; CreationCount++)
        {
            GameObject BulletForPool = Instantiate(Bullet);
            BulletForPool.SetActive(false);
            BulletPool.Add(BulletForPool);
        }

        InitialSpriteRotation = transform.rotation.eulerAngles;
        Sprite2DRigidBody = GetComponent<Rigidbody2D>();
        RifleShotSource = GetComponent<AudioSource>();
        CurrentHealthTextComponentReference.text = CurrentHealth.ToString();
        SpriteIsPlayer = true;
        //print(ActiveWeaponRenderer.name);
    }

    private void Update()
    {
        // For testing:
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (Time.timeScale != 1.0f)
            {
                Time.timeScale = 1.0f;
            }
            else
            {
                Time.timeScale = 0.010f;
            }           
        }

        if (Input.GetButtonDown("Jump") && Grounded)
        {
            CharacterWantsToJump = true;
        }

        ManageInventory();

        if (Input.GetButtonDown("Fire1"))
        {
            if (!CharacterWantsToAttack)
            {
                CharacterWantsToAttack = true;
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            CharacterWantsToAttack = false;
            CharacterAlreadyAttacking = false;

            if (CurrentActiveWeapon == ActiveWeapon.MachineGun)
            {
                RifleShotSource.Stop();
            }

            CancelInvoke();
        }
    }

    private void ManageInventory()
    {
        ActiveWeapon NewActiveWeapon = ActiveWeapon.None;
        
        // Check for whether the Player wants to
        // switch active weapons for now:

        if (Input.GetButtonDown("Inventory Slot 0"))
        {
            ActiveWeaponRenderer.sprite = null;
            SwitchActiveWeapon(ActiveWeapon.None);
        }

        if (Input.GetButtonDown("Inventory Slot 1"))
        {
            ActiveWeaponRenderer.sprite = MachineGunSprite;
            NewActiveWeapon = ActiveWeapon.MachineGun;
        }

        if (NewActiveWeapon != ActiveWeapon.None)
        {
            SwitchActiveWeapon(NewActiveWeapon);
        }
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
