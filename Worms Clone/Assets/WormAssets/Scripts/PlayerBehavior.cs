using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    /** State flags used during movement */
    [HideInInspector] public bool FacingRightwards = true;
    [HideInInspector] public bool PlayerWantsToJump = false;

    // Other public references:

    /** The rate at which to update this sprite's position by */
    public float MovementForce = 365.0f;
    public float MaximumMovementSpeed = 10.0f;
    public float JumpForce = 300.0f;

    // For shooting:

    public float GunFiringDelay = 0.010f;
    public float GunRateOfFire = 0.10f;
    public Sprite MachineGunSprite;
    public SpriteRenderer ActiveWeaponRenderer;

    /** For pooling machinegun (or other) bullets */
    public GameObject Bullet;

    // Non-public flags:
    private bool Grounded = false;
    private bool PlayerWantsToAttack = false;
    private bool PlayerAlreadyAttacking = false;

    // Component references:
    private Rigidbody2D Sprite2DRigidBody;

    /** For refering to the initial rotation of this sprite */
    private Vector3 InitialSpriteRotation;

    // For object pooling:

    public int MaxBulletCount = 20;
    List<GameObject> BulletPool;

    /** For reference to the Player's active weapon */
    enum ActiveWeapon
    {
        None,
        Unarmed,
        MachineGun
    }

    /** For reference to the current active weapon */
    private ActiveWeapon CurrentActiveWeapon;
    
    // Initialise:
    private void Start()
    {
        BulletPool = new List<GameObject>();

        for (int CreationCount = 0; CreationCount < MaxBulletCount; CreationCount++)
        {
            GameObject BulletForPool = Instantiate(Bullet);
            BulletForPool.SetActive(false);
            BulletPool.Add(BulletForPool);
        }

        Sprite2DRigidBody = GetComponent<Rigidbody2D>();
        InitialSpriteRotation = transform.rotation.eulerAngles;
        print(ActiveWeaponRenderer.name);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && Grounded)
        {
            PlayerWantsToJump = true;
        }

        ManageInventory();

        if (Input.GetButtonDown("Fire1"))
        {
            if (!PlayerWantsToAttack)
            {
                PlayerWantsToAttack = true;
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            PlayerWantsToAttack = false;
            PlayerAlreadyAttacking = false;
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

    // For physics related actions:
    private void FixedUpdate()
    {
        // Make sure this sprite is perpendicular to the ground, as often as possible
        MaintainPerpendicularToGround();

        float HorizontalMovementValue = Input.GetAxis("Horizontal");

        if (HorizontalMovementValue * Sprite2DRigidBody.velocity.x < MaximumMovementSpeed)
        {
            Sprite2DRigidBody.AddForce(Vector2.right * HorizontalMovementValue * MaximumMovementSpeed);
        }

        if (Mathf.Abs(Sprite2DRigidBody.velocity.x) > MaximumMovementSpeed)
        {
            Sprite2DRigidBody.velocity = new Vector2(Mathf.Sign(Sprite2DRigidBody.velocity.x)
                * MaximumMovementSpeed, Sprite2DRigidBody.velocity.y);
        }

        if ((HorizontalMovementValue > 0) && !(FacingRightwards))
        {
            FlipSprite();
        }
        else if ((HorizontalMovementValue < 0) && FacingRightwards)
        {
            FlipSprite();
        }

        if (PlayerWantsToJump)
        {
            Sprite2DRigidBody.AddForce(new Vector2(0.0f, JumpForce));
            PlayerWantsToJump = false;
            Grounded = false;
        }

        if (PlayerWantsToAttack && !(PlayerAlreadyAttacking))
        {
            print("Managing Player attack");
            HandlePlayerAttack();
            PlayerAlreadyAttacking = true;
        }
    }

    private void HandlePlayerAttack()
    {
        switch (CurrentActiveWeapon)
        {          
            case ActiveWeapon.None:
                break;
            // No attack logic for now:
            case ActiveWeapon.Unarmed:
                break;
            case ActiveWeapon.MachineGun:
                InvokeRepeating("FireBullet", GunFiringDelay, GunRateOfFire);
                break;
        }
    }

    private void FireBullet()
    {
        for (int BulletCounter = 0; BulletCounter < BulletPool.Count; BulletCounter++)
        {
            if (!BulletPool[BulletCounter].activeInHierarchy)
            {
                BulletPool[BulletCounter].transform.position = transform.position;
                BulletPool[BulletCounter].transform.rotation = transform.rotation;
                BulletPool[BulletCounter].SetActive(true);
                break;
            }
        }
    }

    // Allow the Player to jump only if they are on a level that supports them:
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("SupportsPlayer"))
        {
            Grounded = true;
        }
    }

    // Keep the Player perpendicular (at 90 degrees), to the ground:
    private void MaintainPerpendicularToGround()
    {
        if (transform.rotation.eulerAngles != InitialSpriteRotation)
        {        
            Quaternion QuaternionSpriteRotation = Quaternion.Euler(InitialSpriteRotation);
            transform.SetPositionAndRotation(transform.position, QuaternionSpriteRotation);
        }
    }


    // Invert the x-axis scale to 'flip' the sprite:
    private void FlipSprite()
    {
        print("Sprite Flipped");

        Vector3 SpriteScale = transform.localScale;

        FacingRightwards = !FacingRightwards;
        SpriteScale.x *= -1.0f;
        transform.localScale = SpriteScale;
    }

    // Switch between weapons:
    private void SwitchActiveWeapon(ActiveWeapon WeaponToSwitchTo)
    {
        print("Weapon Switched");
        CurrentActiveWeapon = WeaponToSwitchTo;
    }
}
