using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Ship : MonoBehaviour
{
    float colliderHalfHeight;
    float colliderHalfWidth;
    SpriteRenderer spriteRenderer;

    // events fired by class
    EventMessages.HealthChanged healthChangedEvent = new EventMessages.HealthChanged();
    EventMessages.GameOver gameOverEvent = new EventMessages.GameOver();

    // Ship stats
    float health = 100;

    // Shooting support
    Timer shootTimer;
    bool canShoot = true;

    // Support for the death animation
    bool isDieing = false;

    // Upgrades support
    bool bIsRegenEnabled = false;
    int regenMultiplier = 1;
    int bulletLevel = 0;
    float speedUpgradeMultiplier = 1;

    // Start is called before the first frame update
    void Start()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        colliderHalfWidth = collider.size.x / 2;
        colliderHalfHeight = collider.size.y / 2;

        spriteRenderer = GetComponent<SpriteRenderer>();

        // add as event invoker for events
        EventManager.AddHealthChangedInvoker(this);
        EventManager.AddGameOverInvoker(this);

        //Adds this as a listener to the UpgradesUpdated event
        EventManager.AddUpgradesUpdatedListener(HandleUpgradesUpdatedEvent);

        // set up shoot timer
        shootTimer = gameObject.AddComponent<Timer>();
        shootTimer.Duration = GameConstants.PlayerShootDelaySeconds;
        shootTimer.AddTimerFinishedListener(HandleShootTimerFinished);
    }

    // Update is called once per frame
    void Update()
    {
        // Check for the game being paused
        if (!MenuManager.IsGamePaused && Input.GetKeyUp(KeyCode.Escape))
        {
            MenuManager.IsGamePaused = true;
            MenuManager.GotoMenu(MenuNames.PauseMenu);
        }

        // do not accept inputs while the death animations are going on
        if (isDieing) return;

        // Move based on input
        Vector3 position = transform.position;
        float verticalInput = Input.GetAxis("Vertical");
        if (verticalInput != 0)
        {
            position.y += verticalInput * speedUpgradeMultiplier * GameConstants.ShipMoveUnitsPerSecond * Time.deltaTime;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput != 0)
        {
            position.x += horizontalInput * speedUpgradeMultiplier * GameConstants.ShipMoveUnitsPerSecond * Time.deltaTime;
        }

        // move character to new position and clamp in screen
        transform.position = position;
        transform.position = CommonFunctions.ClampShipInScreen(transform.position, colliderHalfWidth, colliderHalfHeight);

        // Check for shooting input
        if (Input.GetAxis("Shoot") > 0)
        {
            // Only shoot if cooldown is up
            if (canShoot)
            {
                canShoot = false;
                shootTimer.Run();

                ShootBullet();

                // Plays the shooting sound
                AudioManager.PlaySFX(AudioClipNames.PlayerBullet);
            }
        }
        else
        {
            // Resets the cooldown if the player is pressing continuously
            canShoot = true;
        }

        // Check for enabled Regen Upgrade
        if (bIsRegenEnabled)
        {
            // Only recover health if there's damage
            if(health < GameConstants.MaxShipHealth)
            {
                health += regenMultiplier * GameConstants.PlayerRecoveryUpgradePercent * GameConstants.MaxShipHealth;
                healthChangedEvent.Invoke(health);
            }
        }
    }

    /// <summary>
    /// Shoots bullets depending on the bulletLevel of the ship
    /// </summary>
    void ShootBullet()
    {
        GameObject[] bullets = null;
        Vector2[] bulletPos = null;
        BulletDirection[] bulletDirection = null;

        //Prepares the necessary variables
        switch (bulletLevel)
        {
            case 0:
                //Creates the bullet vector
                bullets = ObjectPool.GetPlayerBullets(1);

                //Positions the bullets at the right places
                bulletPos = new Vector2[bullets.Length];
                bulletPos[0] = gameObject.transform.position;
                bulletPos[0] += GameConstants.ShipBulletOffset["Central1"];

                //Sets the shooting direction
                bulletDirection = new BulletDirection[bullets.Length];
                bulletDirection[0] = BulletDirection.Up;
                break;
            case 1:
                //Creates the bullet vector
                bullets = ObjectPool.GetPlayerBullets(3);

                //Positions the bullets at the right places
                bulletPos = new Vector2[bullets.Length];
                bulletPos[0] = gameObject.transform.position;
                bulletPos[0] += GameConstants.ShipBulletOffset["Central1"];
                bulletPos[1] = gameObject.transform.position;
                bulletPos[1] += GameConstants.ShipBulletOffset["Central2"];
                bulletPos[2] = gameObject.transform.position;
                bulletPos[2] += GameConstants.ShipBulletOffset["Central3"];

                //Sets the shooting direction
                bulletDirection = new BulletDirection[bullets.Length];
                bulletDirection[0] = BulletDirection.Up;
                bulletDirection[1] = BulletDirection.Up;
                bulletDirection[2] = BulletDirection.Up;
                break;
            case 2:
                //Creates the bullet vector
                bullets = ObjectPool.GetPlayerBullets(5);

                //Positions the bullets at the right places
                bulletPos = new Vector2[bullets.Length];
                bulletPos[0] = gameObject.transform.position;
                bulletPos[0] += GameConstants.ShipBulletOffset["Central1"];
                bulletPos[1] = gameObject.transform.position;
                bulletPos[1] += GameConstants.ShipBulletOffset["Central2"];
                bulletPos[2] = gameObject.transform.position;
                bulletPos[2] += GameConstants.ShipBulletOffset["Central3"];
                bulletPos[3] = gameObject.transform.position;
                bulletPos[3] += GameConstants.ShipBulletOffset["CentralLayer1Left"];
                bulletPos[4] = gameObject.transform.position;
                bulletPos[4] += GameConstants.ShipBulletOffset["CentralLayer1Right"];

                //Sets the shooting direction
                bulletDirection = new BulletDirection[bullets.Length];
                bulletDirection[0] = BulletDirection.Up;
                bulletDirection[1] = BulletDirection.Up;
                bulletDirection[2] = BulletDirection.Up;
                bulletDirection[3] = BulletDirection.UpLeftLayer1;
                bulletDirection[4] = BulletDirection.UpRightLayer1;
                break;
            case 3:
                //Creates the bullet vector
                bullets = ObjectPool.GetPlayerBullets(7);

                //Positions the bullets at the right places
                bulletPos = new Vector2[bullets.Length];
                bulletPos[0] = gameObject.transform.position;
                bulletPos[0] += GameConstants.ShipBulletOffset["Central1"];
                bulletPos[1] = gameObject.transform.position;
                bulletPos[1] += GameConstants.ShipBulletOffset["Central2"];
                bulletPos[2] = gameObject.transform.position;
                bulletPos[2] += GameConstants.ShipBulletOffset["Central3"];
                bulletPos[3] = gameObject.transform.position;
                bulletPos[3] += GameConstants.ShipBulletOffset["CentralLayer1Left"];
                bulletPos[4] = gameObject.transform.position;
                bulletPos[4] += GameConstants.ShipBulletOffset["CentralLayer1Right"];
                bulletPos[5] = gameObject.transform.position;
                bulletPos[5] += GameConstants.ShipBulletOffset["CentralLayer2Left"];
                bulletPos[6] = gameObject.transform.position;
                bulletPos[6] += GameConstants.ShipBulletOffset["CentralLayer2Right"];

                //Sets the shooting direction
                bulletDirection = new BulletDirection[bullets.Length];
                bulletDirection[0] = BulletDirection.Up;
                bulletDirection[1] = BulletDirection.Up;
                bulletDirection[2] = BulletDirection.Up;
                bulletDirection[3] = BulletDirection.UpLeftLayer1;
                bulletDirection[4] = BulletDirection.UpRightLayer1;
                bulletDirection[5] = BulletDirection.UpLeftLayer2;
                bulletDirection[6] = BulletDirection.UpRightLayer2;
                break;
            default:
                break;
        }

        //Shoot bullets
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i].GetComponent<Bullet>().StopMoving();
            bullets[i].transform.position = bulletPos[i];
            bullets[i].SetActive(true);
            bullets[i].GetComponent<Bullet>().StartMoving(bulletDirection[i]);
        }
    }

    /// <summary>
    /// Processes trigger collisions with other game objects
    /// </summary>
    /// <param name="other">information about the other collider</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        //Only check for collision if the ship is not dieing
        if (isDieing) return;

        // Instantiate an explosion
        GameObject explosion = ObjectPool.GetExplosion();

        // if colliding with a bullet, return bullet to pool and take damage
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            ObjectPool.ReturnBullet(other.gameObject);
            TakeDamage(GameConstants.ShipBulletCollisionDamage);

            //Plays the explosion
            explosion.transform.position = other.gameObject.transform.position;
            explosion.SetActive(true);
            explosion.GetComponent<Explosion>().PlayExplosion(ExplosionSize.Small);
        }
        else if (other.gameObject.CompareTag("EnemyShip"))
        {
            // if colliding with an enemy, return enemy to pool and take damage
            ObjectPool.ReturnEnemy(other.gameObject);
            TakeDamage(GameConstants.ShipEnemyCollisionDamage);

            //Plays the explosion
            explosion.transform.position = gameObject.transform.position;
            explosion.SetActive(true);
            explosion.GetComponent<Explosion>().PlayExplosion(ExplosionSize.Default);
        }
    }

    /// <summary>
    /// Takes the given amount of damage
    /// </summary>
    /// <param name="damage">damage</param>
    void TakeDamage(int damage)
    {
        //Plays the player damaged sound
        AudioManager.PlaySFX(AudioClipNames.PlayerDamage);

        health -= damage;
        if (health < 0) health = 0;
        healthChangedEvent.Invoke(health);

        if (health <= 0)
        {
            isDieing = true;
            StartCoroutine(PlayerDieing());
        }
    }

    /// <summary>
    /// Coroutine for playing the player death animation
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayerDieing()
    {
        //Plays the animation
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        Animator anim = gameObject.transform.GetChild(0).GetComponent<Animator>();

        //Adds a small delay to let the button sound play
        yield return new WaitForSeconds(anim.runtimeAnimatorController.animationClips[0].length - 0.05f);

        // Disable all the children present
        for(int i=0; i < gameObject.transform.childCount; i++)
            gameObject.transform.GetChild(i).gameObject.SetActive(false);

        //Dispatches the game over event
        gameOverEvent.Invoke();
    }

    /// <summary>
    /// Toggles the health upgrade on or off
    /// </summary>
    /// <param name="isEnabled">What should the new state be</param>
    /// <param name="powerLevel">The power of the upgrade to be used</param>
    void ToggleHealthUpgrade(bool isEnabled, int powerLevel)
    {
        bIsRegenEnabled = isEnabled;
        regenMultiplier = powerLevel;
    }

    /// <summary>
    /// Toggles the speed upgrade on or off
    /// </summary>
    /// <param name="isEnabled">What should the new state be</param>
    /// <param name="powerLevel">The power of the upgrade to be used</param>
    void ToggleSpeedUpgrade(bool isEnabled, int powerLevel)
    {
        if (isEnabled)
        {
            speedUpgradeMultiplier = 1 + GameConstants.PlayerSpeedIncrementStep * powerLevel;
        }
        else
        {
            speedUpgradeMultiplier = 1.0f;
        }
    }

    /// <summary>
    /// Toggles the weapons upgrade on or off
    /// </summary>
    /// <param name="isEnabled">What should the new state be</param>
    /// <param name="powerLevel">The power of the upgrade to be used</param>
    void ToggleWeaponsUpgrade(bool isEnabled, int powerLevel)
    {
        if (isEnabled)
        {
            bulletLevel = powerLevel;
        }
        else
        {
            bulletLevel = 0;
        }
    }

    /// <summary>
    /// Handle for updating the ships upgrades
    /// </summary>
    /// <param name="upgradeType">The upgrade that will be updated</param>
    /// <param name="isEnabled">Should enable or disable this upgrade</param>
    /// <param name="powerLevel">The power of the upgrade to use</param>
    void HandleUpgradesUpdatedEvent(UpgradeTypes upgradeType, bool isEnabled, int powerLevel)
    {
        // Update the correct upgrade
        switch (upgradeType)
        {
            case UpgradeTypes.Health:
                ToggleHealthUpgrade(isEnabled, powerLevel);
                break;
            case UpgradeTypes.Speed:
                ToggleSpeedUpgrade(isEnabled, powerLevel);
                break;
            case UpgradeTypes.Weapons:
                ToggleWeaponsUpgrade(isEnabled, powerLevel);
                break;
        }
    }

    /// <summary>
    /// Cooldown the shooting timer
    /// </summary>
    void HandleShootTimerFinished()
    {
        canShoot = true;
    }

    /// <summary>
    /// Adds the given listener for the HealthChanged event
    /// </summary>
    /// <param name="listener">listener</param>
    public void AddHealthChangedListener(UnityAction<float> listener)
    {
        healthChangedEvent.AddListener(listener);
    }

    /// <summary>
    /// Adds the given listener for the GameOver event
    /// </summary>
    /// <param name="listener">listener</param>
    public void AddGameOverListener(UnityAction listener)
    {
        gameOverEvent.AddListener(listener);
    }
}
