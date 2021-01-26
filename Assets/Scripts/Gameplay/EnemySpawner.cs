using UnityEngine;

namespace Assets.Scripts.Gameplay
{
    class EnemySpawner : MonoBehaviour
    {
        Timer spawnTimer;

        // saved for efficiency
        float horizontalBorderSize;
        float verticalOffset;

        /// <summary>
        /// Use this for initialization
        /// </summary>
        void Start()
        {
            // Save for efficiency
            GameObject enemy = ObjectPool.GetEnemy();
            Collider2D collider = enemy.GetComponent<BoxCollider2D>();
            verticalOffset = collider.bounds.size.y;
            horizontalBorderSize = collider.bounds.size.x * 4;
            ObjectPool.ReturnEnemy(enemy);

            // Set up spawn timer
            spawnTimer = gameObject.AddComponent<Timer>();
            spawnTimer.Duration = GameConstants.EnemySpawnDelaySeconds;
            spawnTimer.AddTimerFinishedListener(HandleSpawnTimerFinished);
            spawnTimer.Run();
        }

        // Update is called once per frame
        /*void Update()
        {
            if (Input.GetKeyUp(KeyCode.L))
            {
                SpawnEnemy();
            }
        }*/

        /// <summary>
        /// Spawns a new enemy and restarts the spawn timer
        /// </summary>
        void HandleSpawnTimerFinished()
        {
            SpawnEnemy();
            spawnTimer.Run();
        }

        /// <summary>
        /// Stops the enemy spawner
        /// </summary>
        public void Stop()
        {
            spawnTimer.Stop();
        }

        /// <summary>
        /// Spawns an enemy in the game
        /// </summary>
        void SpawnEnemy()
        {
            // Get random position
            Vector3 enemyPos = new Vector3(Random.Range(ScreenUtils.ScreenLeft - horizontalBorderSize, ScreenUtils.ScreenRight + horizontalBorderSize),
                                           ScreenUtils.ScreenTop + verticalOffset,
                                           0);

            // Spawn enemy object
            GameObject enemy = ObjectPool.GetEnemy();
            enemy.transform.position = enemyPos;
            enemy.SetActive(true);

            // Some enemies might be going faster when spawned
            enemy.GetComponent<Enemy>().Deactivate();

            enemy.GetComponent<Enemy>().Activate();
        }
    }
}
