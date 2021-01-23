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

    //Shooting support
    Timer shootTimer;
    bool canShoot = true;

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

        // Move based on input
        Vector3 position = transform.position;
        float verticalInput = Input.GetAxis("Vertical");
        if (verticalInput != 0)
        {
            position.y += verticalInput * GameConstants.ShipMoveUnitsPerSecond * Time.deltaTime;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput != 0)
        {
            position.x += horizontalInput * GameConstants.ShipMoveUnitsPerSecond * Time.deltaTime;
        }

        // move character to new position and clamp in screen
        transform.position = position;
        ClampInScreen();

        // check for shooting input
        if (Input.GetAxis("Shoot") > 0)
        {
            // only shoot if cooldown is up
            if (canShoot)
            {
                canShoot = false;
                shootTimer.Run();

                ShootBullet();

                //Plays the shooting sound
                AudioManager.PlaySFX(AudioClipNames.PlayerBullet);
            }
        }
        else
        {
            //Resets the cooldown if the player is pressing continuously
            canShoot = true;
        }
    }

    /// <summary>
    /// Shoots bullets
    /// </summary>
    void ShootBullet()
    {
        GameObject[] bullets = null;
        Vector2[] bulletPos = null;
        BulletDirection[] bulletDirection = null;

        //Prepares the necessary variables
        //Creates the bullet vector
        bullets = ObjectPool.GetPlayerBullets(1);

        //Positions the bullets at the right places
        bulletPos = new Vector2[bullets.Length];
        bulletPos[0] = gameObject.transform.position;
        bulletPos[0].x += GameConstants.ShipBulletXOffset;
        bulletPos[0].y += GameConstants.ShipBulletYOffset;

        //Sets the shooting direction
        bulletDirection = new BulletDirection[bullets.Length];
        bulletDirection[0] = BulletDirection.Up;

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
    /// Clamps the ship in the screen
    /// </summary>
    void ClampInScreen()
    {
        // clamp position as necessary
        Vector3 position = transform.position;

        //Horizontal Clamp
        if (position.x + colliderHalfWidth > ScreenUtils.ScreenRight)
        {
            position.x = ScreenUtils.ScreenRight - colliderHalfWidth;
        }
        else if (position.x - colliderHalfWidth < ScreenUtils.ScreenLeft)
        {
            position.x = ScreenUtils.ScreenLeft + colliderHalfWidth;
        }

        //Vertical Clamp
        if (position.y + colliderHalfHeight > ScreenUtils.ScreenTop)
        {
            position.y = ScreenUtils.ScreenTop - colliderHalfHeight;
        }
        else if (position.y - colliderHalfHeight < ScreenUtils.ScreenBottom)
        {
            position.y = ScreenUtils.ScreenBottom + colliderHalfHeight;
        }
        transform.position = position;
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
