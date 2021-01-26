using UnityEngine;

public class WaypointManager : Singleton<WaypointManager>
{
    [SerializeField]
    GameObject[] fixedWaypoints;

    /// <summary>
    /// Gets the fixed waypoints on the screen
    /// </summary>
    public GameObject[] FixedWaypoints { get { return fixedWaypoints; } }
}
