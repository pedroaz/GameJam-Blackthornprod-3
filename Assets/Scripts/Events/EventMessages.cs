using UnityEngine;
using UnityEngine.Events;

public static class EventMessages
{
    /// <summary>
    /// An event for when a timer has finished
    /// </summary>
    public class TimerFinished : UnityEvent { }

    /// <summary>
    /// An event that indicates that health has changed
    /// </summary>
    public class HealthChanged : UnityEvent<float> { }

    /// <summary>
    /// An event that indicates that the game is over
    /// </summary>
    public class GameOver : UnityEvent { }

    /// <summary>
    /// An event that indicates that the game is over
    /// </summary>
    public class GameEnded : UnityEvent { }

    /// <summary>
    /// An event that indicates that the shields slider was changed
    /// </summary>
    public class ShieldsEnergyUpdated : UnityEvent<float> { }

    /// <summary>
    /// An event that indicates what are the new update slots for where the new item now is and was before
    /// </summary>
    public class UpgradeItemDroppedOnSlot : UnityEvent { }

    /// <summary>
    /// An event to update the upgrades of the ship (type of upgrade, isEnabled and powerLevel)
    /// </summary>
    public class UpgradesUpdated : UnityEvent<UpgradeTypes, bool, int> { }
}
