﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager
{
    #region Fields

    static List<Ship> healthChangedInvokers = new List<Ship>();
    static List<UnityAction<float>> healthChangedListeners = new List<UnityAction<float>>();

    static List<Ship> gameOverInvokers = new List<Ship>();
    static List<UnityAction> gameOverListeners = new List<UnityAction>();

    static List<HUD> gameEndedInvokers = new List<HUD>();
    static List<UnityAction> gameEndedListeners = new List<UnityAction>();

    static List<HUD> shieldChangedInvokers = new List<HUD>();
    static List<UnityAction<float>> shieldChangedListeners = new List<UnityAction<float>>();

    static List<UpgradesSlot> upgradeItemDroppedInvokers = new List<UpgradesSlot>();
    static List<UnityAction> upgradeItemDroppedListeners = new List<UnityAction>();

    static List<HUD> upgradesUpdatedInvokers = new List<HUD>();
    static List<UnityAction<UpgradeTypes, bool, int>> upgradesUpdatedListeners = new List<UnityAction<UpgradeTypes, bool, int>>();

    #endregion

    #region Public methods

    /// <summary>
    /// Adds a HealthChanged invoker
    /// </summary>
    /// <param name="invoker">invoker</param>
    public static void AddHealthChangedInvoker(Ship invoker)
    {
        // add listeners to new invoker and add new invoker to list
        foreach (UnityAction<float> listener in healthChangedListeners)
        {
            invoker.AddHealthChangedListener(listener);
        }
        healthChangedInvokers.Add(invoker);
    }

    /// <summary>
    /// Adds a HealthChanged listener
    /// </summary>
    /// <param name="listener">listener</param>
    public static void AddHealthChangedListener(UnityAction<float> listener)
    {
        // add as listener to all invokers and add new listener to list
        foreach (Ship invoker in healthChangedInvokers)
        {
            invoker.AddHealthChangedListener(listener);
        }
        healthChangedListeners.Add(listener);
    }

    /// <summary>
    /// Adds a GameOver invoker
    /// </summary>
    /// <param name="invoker">invoker</param>
    public static void AddGameOverInvoker(Ship invoker)
    {
        // add listeners to new invoker and add new invoker to list
        foreach (UnityAction listener in gameOverListeners)
        {
            invoker.AddGameOverListener(listener);
        }
        gameOverInvokers.Add(invoker);
    }

    /// <summary>
    /// Adds a GameOver listener
    /// </summary>
    /// <param name="listener">listener</param>
    public static void AddGameOverListener(UnityAction listener)
    {
        // add as listener to all invokers and add new listener to list
        foreach (Ship invoker in gameOverInvokers)
        {
            invoker.AddGameOverListener(listener);
        }
        gameOverListeners.Add(listener);
    }

    /// <summary>
    /// Adds a GameEnded invoker
    /// </summary>
    /// <param name="invoker">invoker</param>
    public static void AddGameEndedInvoker(HUD invoker)
    {
        // add listeners to new invoker and add new invoker to list
        foreach (UnityAction listener in gameEndedListeners)
        {
            invoker.AddGameEndedListener(listener);
        }
        gameEndedInvokers.Add(invoker);
    }

    /// <summary>
    /// Adds a GameEnded listener
    /// </summary>
    /// <param name="listener">listener</param>
    public static void AddGameEndedListener(UnityAction listener)
    {
        // add as listener to all invokers and add new listener to list
        foreach (HUD invoker in gameEndedInvokers)
        {
            invoker.AddGameEndedListener(listener);
        }
        gameEndedListeners.Add(listener);
    }

    /// <summary>
    /// Adds a Shield Changed invoker
    /// </summary>
    /// <param name="invoker">invoker</param>
    public static void AddShieldChangedInvoker(HUD invoker)
    {
        // add listeners to new invoker and add new invoker to list
        foreach (UnityAction<float> listener in shieldChangedListeners)
        {
            invoker.AddShieldChangedListener(listener);
        }
        shieldChangedInvokers.Add(invoker);
    }

    /// <summary>
    /// Adds a GameEnded listener
    /// </summary>
    /// <param name="listener">listener</param>
    public static void AddShieldChangedListener(UnityAction<float> listener)
    {
        // add as listener to all invokers and add new listener to list
        foreach (HUD invoker in shieldChangedInvokers)
        {
            invoker.AddShieldChangedListener(listener);
        }
        shieldChangedListeners.Add(listener);
    }

    /// <summary>
    /// Adds an Upgrade Item Droppped invoker
    /// </summary>
    /// <param name="invoker">invoker</param>
    public static void AddUpgradeItemDroppedInvoker(UpgradesSlot invoker)
    {
        // add listeners to new invoker and add new invoker to list
        foreach (UnityAction listener in upgradeItemDroppedListeners)
        {
            invoker.AddUpgradeItemDroppedListener(listener);
        }
        upgradeItemDroppedInvokers.Add(invoker);
    }

    /// <summary>
    /// Adds an Upgrade Item Droppped listener
    /// </summary>
    /// <param name="listener">listener</param>
    public static void AddUpgradeItemDroppedListener(UnityAction listener)
    {
        // add as listener to all invokers and add new listener to list
        foreach (UpgradesSlot invoker in upgradeItemDroppedInvokers)
        {
            invoker.AddUpgradeItemDroppedListener(listener);
        }
        upgradeItemDroppedListeners.Add(listener);
    }

    /// <summary>
    /// Adds an UpgradesUpdated invoker
    /// </summary>
    /// <param name="invoker">invoker</param>
    public static void AddUpgradesUpdatedInvoker(HUD invoker)
    {
        // add listeners to new invoker and add new invoker to list
        foreach (UnityAction<UpgradeTypes, bool, int> listener in upgradesUpdatedListeners)
        {
            invoker.AddUpgradesUpdatedListener(listener);
        }
        upgradesUpdatedInvokers.Add(invoker);
    }

    /// <summary>
    /// Adds an UpgradesUpdated listener
    /// </summary>
    /// <param name="listener">listener</param>
    public static void AddUpgradesUpdatedListener(UnityAction<UpgradeTypes, bool, int> listener)
    {
        // add as listener to all invokers and add new listener to list
        foreach (HUD invoker in upgradesUpdatedInvokers)
        {
            invoker.AddUpgradesUpdatedListener(listener);
        }
        upgradesUpdatedListeners.Add(listener);
    }

    #endregion
}
