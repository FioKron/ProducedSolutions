using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SpriteBehavior : MonoBehaviour
{
    /** For refering to the initial rotation of this sprite */
    protected Vector3 InitialSpriteRotation;

    /** State flags used during movement */
    [HideInInspector] public bool FacingRightwards = true;
    [HideInInspector] public bool CharacterWantsToJump = false;

    /** Other public flags */
    public bool SpriteIsPlayer;

    // Protected flags:
    protected bool Grounded = false;
    protected bool CharacterWantsToAttack = false;
    protected bool CharacterAlreadyAttacking = false;

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
    public GameObject BulletSpawnLocationReference;

    /** For pooling machinegun (or other) bullets */
    public GameObject Bullet;

    // Component references:
    protected Rigidbody2D Sprite2DRigidBody;
    protected AudioSource RifleShotSource;

    /** The "ground" layer index (check layer settings for indices) */
    protected const int GROUND_LAYER = 8;

    /** Was previously 1/2 a second (to attemept to have the bullet avoid collision with the Player) */
    private const float BULLET_ACTIVATION_DELAY = 0.0f;

    // For object pooling:

    public int MaxBulletCount = 20;
    protected List<GameObject> BulletPool;

    // For the HUD on this character sprite:
    public Text CurrentHealthTextComponentReference;
    public int CurrentHealth = 100;
    public int MaximumHealth = 100;

    /** For reference to this Character's active weapon */
    protected enum ActiveWeapon
    {
        None,
        Unarmed,
        MachineGun
    }

    /** For reference to the current active weapon */
    protected ActiveWeapon CurrentActiveWeapon;

    // Keep the Character perpendicular (at 90 degrees), to the ground:
    protected void MaintainPerpendicularToGround()
    {
        if (transform.rotation.eulerAngles != InitialSpriteRotation)
        {
            Quaternion QuaternionSpriteRotation = Quaternion.Euler(InitialSpriteRotation);
            transform.SetPositionAndRotation(transform.position, QuaternionSpriteRotation);
        }
    }

    /**
    // Handle updates:
    void Update()
    {

    }
    */
    protected void FixedUpdate()
    {
        // Make sure this sprite is perpendicular to the ground, as often as possible
        MaintainPerpendicularToGround();

        float HorizontalMovementValue = Input.GetAxis("Horizontal");

        if (SpriteIsPlayer)
        {
            if (HorizontalMovementValue * Sprite2DRigidBody.velocity.x < MaximumMovementSpeed)
            {
                Sprite2DRigidBody.AddForce(Vector2.right * HorizontalMovementValue * MaximumMovementSpeed);
            }

            if (Mathf.Abs(Sprite2DRigidBody.velocity.x) > MaximumMovementSpeed)
            {
                Sprite2DRigidBody.velocity = new Vector2(Mathf.Sign(Sprite2DRigidBody.velocity.x)
                    * MaximumMovementSpeed, Sprite2DRigidBody.velocity.y);
            }

            if (CharacterWantsToJump)
            {
                Sprite2DRigidBody.AddForce(new Vector2(0.0f, JumpForce));
                CharacterWantsToJump = false;
                Grounded = false;
            }

            if (CharacterWantsToAttack && !(CharacterAlreadyAttacking))
            {
                print("Managing Sprite attack");
                HandleCharacterAttack();
                CharacterAlreadyAttacking = true;
            }
        }

        if ((HorizontalMovementValue > 0) && !(FacingRightwards))
        {
            FlipSprite();
        }
        else if ((HorizontalMovementValue < 0) && FacingRightwards)
        {
            FlipSprite();
        }    
    }

    // Invert the x-axis scale to 'flip' the sprite:
    protected void FlipSprite()
    {
        print("Sprite Flipped");

        Vector3 SpriteScale = transform.localScale;

        FacingRightwards = !FacingRightwards;
        SpriteScale.x *= -1.0f;
        transform.localScale = SpriteScale;
    }

    protected void HandleCharacterAttack()
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

    protected void FireBullet()
    {
        for (int BulletCounter = 0; BulletCounter < BulletPool.Count; BulletCounter++)
        {
            if (!BulletPool[BulletCounter].activeInHierarchy)
            {
                BulletPool[BulletCounter].transform.position = BulletSpawnLocationReference.transform.position;
                BulletPool[BulletCounter].transform.rotation = BulletSpawnLocationReference.transform.rotation;

                //AudioSource TestAud = new AudioSource();
                //TestAud.clip
                if (!RifleShotSource.isPlaying)
                {
                    RifleShotSource.PlayDelayed(0.0f);
                }
             
                BulletPool[BulletCounter].GetComponent<BulletBehavior>().Initialise(BULLET_ACTIVATION_DELAY);
                break;
            }
        }
    }


    // Switch between weapons:
    protected void SwitchActiveWeapon(ActiveWeapon WeaponToSwitchTo)
    {
        print("Weapon Switched");
        CurrentActiveWeapon = WeaponToSwitchTo;
    }
    
    // Functions to manage health:

    public void DamageCharacter(int IncomingDamage)
    {
        // As IncomingDamage will get subtracted from this sprite's health:
        IncomingDamage = Mathf.Abs(IncomingDamage);

        CurrentHealth -= IncomingDamage;

        ValidateHealth();
    }

    private void ValidateHealth()
    {
        if (CurrentHealth >= MaximumHealth)
        {
            CurrentHealth = MaximumHealth;
        }
        // 0 is the minimum level of health
        else if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            DestroySprite();
        }

        UpdateHealthDisplay();
    }

    private void UpdateHealthDisplay()
    {
        CurrentHealthTextComponentReference.text = CurrentHealth.ToString();
    }

    // Custom method for 'destroying' a sprite
    protected void DestroySprite()
    {
        gameObject.SetActive(false);
    }
}
