using UnityEngine;

public enum EnemyTypes
{
    Fast,
    Strong,
    Tank
}

public sealed class EnemyStats
{
    private readonly float speed = 0.5f;
    private readonly float health = 0.5f;
    private readonly float shootSpeed = 0.5f;
    private readonly float activeTime = 0.5f;

    //Constructor
    public EnemyStats(float speed, float health, float shootSpeed, float activeTime)
    {
        this.speed = speed;
        this.health = health;
        this.shootSpeed = shootSpeed;
        this.activeTime = activeTime;
    }

    /// <summary>
    /// Returns the thrust speed of the ship
    /// </summary>
    public float Speed { get { return speed; } }

    /// <summary>
    /// Returns the max health of the ship
    /// </summary>
    public float Health { get { return health; } }

    /// <summary>
    /// Returns the rate of fire of the ship
    /// </summary>
    public float ShootSpeed { get { return shootSpeed; } }

    /// <summary>
    /// Returns how long the enemy ship will stand still at a Waypoint
    /// </summary>
    public float ActiveTime { get { return activeTime; } }
}

public class Enemy : MonoBehaviour
{
    Timer shootTimer;
    Timer activeTimer;

    Rigidbody2D rb2d;
    float colliderHalfHeight;
    float colliderHalfWidth;
    Vector2 forceVector;
    Vector2 destination;

    private float speed;
    private float health;
    private float shootSpeed;
    private float activeTime;

    [SerializeField]
    EnemyTypes enemyType = EnemyTypes.Fast;

    /// <summary>
    /// Initializes object. We don't use Start for this because
    /// we want to initialize objects as they're added to the
    /// pool
    /// </summary>
    public void Initialize()
    {
        // Save for efficiency
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        colliderHalfWidth = collider.size.x / 2;
        colliderHalfHeight = collider.size.y / 2;

        // Sets force vector
        forceVector = new Vector2(speed, 0);

        // Set up shoot timer
        shootTimer = gameObject.AddComponent<Timer>();
        shootTimer.AddTimerFinishedListener(HandleShootTimerFinished);

        // Set up active timer
        activeTimer = gameObject.AddComponent<Timer>();
        activeTimer.AddTimerFinishedListener(HandleActiveTimeFinished);
    }

    // Update is called once per frame
    void Update()
    {
        // Clamp the ship in the screen
        transform.position = CommonFunctions.ClampShipHorizontallyInScreen(transform.position, colliderHalfWidth, colliderHalfHeight);

        if (destination != null)
        {
            // Calculate the speed of the step
            float step = speed * Time.deltaTime;

            // Move sprite towards the target location
            transform.position = Vector2.MoveTowards(transform.position, destination, step);
        }
    }

    /// <summary>
    /// Starts the enemy moving and starts the shoot timer
    /// </summary>
    public void Activate()
    {
        // Sets a random enemy type
        enemyType = (EnemyTypes)Random.Range(0, System.Enum.GetValues(typeof(EnemyTypes)).Length);

        //Updates the necessary variables
        speed = GameConstants.EnemyShipStats[enemyType].Speed;
        health = GameConstants.EnemyShipStats[enemyType].Health;
        shootSpeed = GameConstants.EnemyShipStats[enemyType].ShootSpeed;
        activeTime = GameConstants.EnemyShipStats[enemyType].ActiveTime;

        //Sets a random destination 
        destination = GetRandomFixedWaypointPosition();

        //Sets and starts the timers
        activeTimer.Duration = activeTime;
        activeTimer.Duration = activeTime*2;
        shootTimer.Duration = shootSpeed;
        shootTimer.Run();
    }

    /// <summary>
    /// Stops the enemy and its shoot timer
    /// </summary>
    public void Deactivate()
    {
        if (rb2d != null) rb2d.velocity = Vector2.zero;
        if (shootTimer != null) shootTimer.Stop();
        if (activeTimer != null) activeTimer.Stop();
    }

    /// <summary>
    /// Called when the enemy becomes invisible
    /// </summary>
    void OnBecameInvisible()
    {
        // don't remove when spawned
        if (transform.position.y > ScreenUtils.ScreenTop)
        {
            // return to the pool
            ObjectPool.ReturnEnemy(gameObject);
        }
    }

    /// <summary>
    /// Called when a collision is detected
    /// </summary>
    /// <param name="collision">The collision target</param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Prevent overlapping with other ships
        if(collision.gameObject.CompareTag("EnemyShip"))
        {
            destination += new Vector2(Random.Range(-colliderHalfWidth, colliderHalfWidth) * 0.75f,// 0);
                                       Random.Range(-colliderHalfHeight, colliderHalfHeight) * 0.5f);
        }
        // Checks if the ship took damage
        else if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            //Return the bullet to the pool
            ObjectPool.ReturnBullet(collision.gameObject);

            // Instantiates an explosion
            GameObject explosion = ObjectPool.GetExplosion();
            explosion.transform.position = collision.gameObject.transform.position;

            // Plays the explosion
            explosion.SetActive(true);
            explosion.GetComponent<Explosion>().PlayExplosion(ExplosionSize.Small);

            // Deals damage to the enemy
            TakeDamage(collision.GetComponent<Bullet>().DamageDealt);
        }
        // Starts the active timer when arriving at destination
        else if(collision.gameObject.CompareTag("Waypoint"))
        {
            activeTimer.Run();
        }
    }

    /// <summary>
    /// Gets the position of a random fixed waypoint
    /// </summary>
    Vector2 GetRandomFixedWaypointPosition()
    {

        GameObject randomWaypoint = WaypointManager.Instance.FixedWaypoints[Random.Range(0,WaypointManager.Instance.FixedWaypoints.Length)];
        return randomWaypoint.transform.position;
    }

    /// <summary>
    /// Takes the given amount of damage
    /// </summary>
    /// <param name="damage">damage</param>
    void TakeDamage(float damage)
    {
        // Plays the player damaged sound
        AudioManager.PlaySFX(AudioClipNames.EnemyDeath);

        // Update the health
        health -= damage;
        if (health < 0)
        {
            health = 0;

            // If enemy is dead return it to the pool
            ObjectPool.ReturnEnemy(gameObject);
        }
    }

    /// <summary>
    /// Shoots a bullet and restarts the shoot timer
    /// </summary>
    void HandleShootTimerFinished()
    {
        shootTimer.Run();

        // shoot bullet
        GameObject bullet = ObjectPool.GetEnemyBullets(1)[0];
        Vector2 bulletPos = gameObject.transform.position;
        bulletPos += new Vector2(GameConstants.EnemyBulletXOffset, GameConstants.EnemyBulletYOffset);
        bullet.GetComponent<Bullet>().StopMoving();
        bullet.transform.position = bulletPos;
        bullet.SetActive(true);
        bullet.GetComponent<Bullet>().StartMoving(BulletDirection.Random);
    }

    /// <summary>
    /// Starts the retreat of the ship
    /// </summary>
    void HandleActiveTimeFinished()
    {
        // Move the ship back towards a random point on the Top Edge of the screen at half the speed of the ship
        destination = new Vector2(Random.Range(ScreenUtils.ScreenLeft, ScreenUtils.ScreenRight), 
                                               ScreenUtils.ScreenTop + colliderHalfHeight * 2);
        if(speed > 1.0f) speed /= 2;
    }
}
