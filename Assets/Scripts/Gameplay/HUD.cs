using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    //Timers
    Timer progressTimer;

    //Game Ended support
    EventMessages.GameEnded gameEndedEvent = new EventMessages.GameEnded();
    bool gameEnded = false;

    //Shield changed support
    EventMessages.ShieldsEnergyUpdated shieldChangedEvent = new EventMessages.ShieldsEnergyUpdated();

    //Upgrades support
    EventMessages.UpgradesUpdated upgradesUpdatedEvent = new EventMessages.UpgradesUpdated();
    bool[] bUpgradeSlotSoundPlayed = new bool[3] { false, false, false };
    float upgradeStep;

    [SerializeField]
    Slider healthBar;

    [SerializeField]
    Slider progressBar;

    [SerializeField]
    Slider shieldsSlider;

    [SerializeField]
    Slider upgradesSlider;

    [SerializeField]
    UpgradesSlot[] upgradeSlots = new UpgradesSlot[3];

    /// <summary>
    /// Awake is called before Start
    /// </summary>
    void Awake()
    {
        EventManager.AddHealthChangedListener(HandleHealthChangedEvent);

        //Add as event invoker for events
        EventManager.AddGameEndedInvoker(this);
        EventManager.AddShieldChangedInvoker(this);
        EventManager.AddUpgradesUpdatedInvoker(this);

        //Adds this as a listener to the Item Changed event
        EventManager.AddUpgradeItemDroppedListener(HandleUpgradeItemDropped);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Sends an event to update the shield value at the start of the game
        shieldChangedEvent.Invoke(shieldsSlider.value);

        //Setup level timer
        progressTimer = gameObject.AddComponent<Timer>();
        progressTimer.Duration = GameConstants.SecondsUntilEnd;
        progressTimer.AddTimerFinishedListener(HandleGameEndedTimer);
        progressTimer.Run();

        //Calculate the value of each step
        upgradeStep = upgradesSlider.maxValue / upgradeSlots.Length;
    }

    // Update is called once per frame
    void Update()
    {
        //Check for the hotkeys for changing the shields value
        if ((Input.GetAxis("ShieldIncrease") > 0) && (shieldsSlider.value < 1))
        {
            shieldsSlider.value += GameConstants.ShieldSliderStep;
        }
        if ((Input.GetAxis("ShieldDecrease") > 0) && (shieldsSlider.value > 0))
        {
            shieldsSlider.value -= GameConstants.ShieldSliderStep;
        }

        //Updates the progress slider
        progressBar.value = (GameConstants.SecondsUntilEnd - progressTimer.SecondsLeft) / GameConstants.SecondsUntilEnd;
    }

    /// <summary>
    /// Checks if the slider for the shields changed
    /// </summary>
    public void ShieldsSliderChanged()
    {
        //Updates the value of the shield
        shieldChangedEvent.Invoke(shieldsSlider.value);

        //Updates the upgrades bar
        upgradesSlider.value = 1 - shieldsSlider.value;
    }

    /// <summary>
    /// Checks if the slider for the upgrades changed, the upgrade is enabled once the slider reaches half of each step
    /// </summary>
    public void UpgradesSliderChanged()
    {
        // Check if the bar is bellow the first activation step
        if (upgradesSlider.value >= 0 && upgradesSlider.value < upgradeStep / 2)
        {
            //Send ths necessary events
            upgradesUpdatedEvent.Invoke(upgradeSlots[0].CurrentUpgradeInSlot.UpgradeType, false, 0);
            upgradesUpdatedEvent.Invoke(upgradeSlots[1].CurrentUpgradeInSlot.UpgradeType, false, 0);
            upgradesUpdatedEvent.Invoke(upgradeSlots[2].CurrentUpgradeInSlot.UpgradeType, false, 0);

            //Updates the animation
            ToggleUpgradeSlotAnimation(upgradeSlots[0].CurrentUpgradeInSlot.gameObject, false);
            ToggleUpgradeSlotAnimation(upgradeSlots[1].CurrentUpgradeInSlot.gameObject, false);
            ToggleUpgradeSlotAnimation(upgradeSlots[2].CurrentUpgradeInSlot.gameObject, false);
        }
        // Check if the first upgrade is enabled
        if (upgradesSlider.value >= upgradeStep / 2 && upgradesSlider.value < 3 * upgradeStep / 2)
        {
            //Send ths necessary events
            upgradesUpdatedEvent.Invoke(upgradeSlots[0].CurrentUpgradeInSlot.UpgradeType, true, 1);
            upgradesUpdatedEvent.Invoke(upgradeSlots[1].CurrentUpgradeInSlot.UpgradeType, false, 0);
            upgradesUpdatedEvent.Invoke(upgradeSlots[2].CurrentUpgradeInSlot.UpgradeType, false, 0);

            //Updates the animation
            ToggleUpgradeSlotAnimation(upgradeSlots[0].CurrentUpgradeInSlot.gameObject, true);
            ToggleUpgradeSlotAnimation(upgradeSlots[1].CurrentUpgradeInSlot.gameObject, false);
            ToggleUpgradeSlotAnimation(upgradeSlots[2].CurrentUpgradeInSlot.gameObject, false);

            //Updates the sound variable
            bUpgradeSlotSoundPlayed[0] = false;
        }
        // Check if the first and second upgrades are enabled
        else if (upgradesSlider.value >= 3 * upgradeStep / 2 && upgradesSlider.value < 5 * upgradeStep / 2)
        {
            //Send ths necessary events
            upgradesUpdatedEvent.Invoke(upgradeSlots[0].CurrentUpgradeInSlot.UpgradeType, true, 1);
            upgradesUpdatedEvent.Invoke(upgradeSlots[1].CurrentUpgradeInSlot.UpgradeType, true, 2);
            upgradesUpdatedEvent.Invoke(upgradeSlots[2].CurrentUpgradeInSlot.UpgradeType, false, 0);

            //Updates the animation
            ToggleUpgradeSlotAnimation(upgradeSlots[0].CurrentUpgradeInSlot.gameObject, true);
            ToggleUpgradeSlotAnimation(upgradeSlots[1].CurrentUpgradeInSlot.gameObject, true);
            ToggleUpgradeSlotAnimation(upgradeSlots[2].CurrentUpgradeInSlot.gameObject, false);

            //Updates the sound variable
            bUpgradeSlotSoundPlayed[1] = false;
        }
        // Check if the first, second and third upgrades are enabled
        else if (upgradesSlider.value >= 5 * upgradeStep / 2 && upgradesSlider.value <= upgradesSlider.maxValue)
        {
            //Send ths necessary events
            upgradesUpdatedEvent.Invoke(upgradeSlots[0].CurrentUpgradeInSlot.UpgradeType, true, 1);
            upgradesUpdatedEvent.Invoke(upgradeSlots[1].CurrentUpgradeInSlot.UpgradeType, true, 2);
            upgradesUpdatedEvent.Invoke(upgradeSlots[2].CurrentUpgradeInSlot.UpgradeType, true, 3);

            //Updates the animation
            ToggleUpgradeSlotAnimation(upgradeSlots[0].CurrentUpgradeInSlot.gameObject, true);
            ToggleUpgradeSlotAnimation(upgradeSlots[1].CurrentUpgradeInSlot.gameObject, true);
            ToggleUpgradeSlotAnimation(upgradeSlots[2].CurrentUpgradeInSlot.gameObject, true);

            //Updates the sound variable
            bUpgradeSlotSoundPlayed[2] = false;
        }
    }

    /// <summary>
    /// Enables or disables the blinking animation on an upgrade slot
    /// </summary>
    /// <param name="upgradeSlot">Slot to be updated</param>
    /// <param name="enableAnim">Should enable the anim</param>
    void ToggleUpgradeSlotAnimation(GameObject upgradeSlot, bool enableAnim)
    {
        Animator animator = upgradeSlot.GetComponent<Animator>();

        // Start or stop the animation
        if (enableAnim)
        {
            // Only play the animation when it's not already playing
            if (animator.speed != 1.0f)
            {
                // Play the animation at normal speed
                animator.speed = 1.0f;
                animator.Play("UpgradeSlot", 0, 1.0f);
            }
        }
        else
        {
            // Resetting the animation to the first frame and stopping the animation
            animator.speed = 0.0f;
            animator.Play("UpgradeSlot",0, 0.0f);
        }
    }

    /// <summary>
    /// Changes health text display
    /// </summary>
    /// <param name="health">new health</param>
    void HandleHealthChangedEvent(float health)
    {
        healthBar.value = health / GameConstants.MaxShipHealth;
    }

    /// <summary>
    /// Handles when a Upgrade Item is dropped into a slot
    /// </summary>
    private void HandleUpgradeItemDropped()
    {
        // Update the state of the upgrades
        UpgradesSliderChanged();
    }

    /// <summary>
    /// Handles the timer ending
    /// </summary>
    void HandleGameEndedTimer()
    {
        gameEndedEvent.Invoke();
    }

    /// <summary>
    /// Adds the given listener for the GameEnded event
    /// </summary>
    /// <param name="listener">listener</param>
    public void AddGameEndedListener(UnityAction listener)
    {
        gameEndedEvent.AddListener(listener);
    }

    /// <summary>
    /// Adds the given listener for the Shield Changed event
    /// </summary>
    /// <param name="listener">listener</param>
    public void AddShieldChangedListener(UnityAction<float> listener)
    {
        shieldChangedEvent.AddListener(listener);
    }

    /// <summary>
    /// Adds the given listener for the UpgradesUpdated event
    /// </summary>
    /// <param name="listener">listener</param>
    public void AddUpgradesUpdatedListener(UnityAction<UpgradeTypes, bool, int> listener)
    {
        upgradesUpdatedEvent.AddListener(listener);
    }
}
