using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    //Game Ended support
    EventMessages.GameEnded gameEndedEvent = new EventMessages.GameEnded();
    bool gameEnded = false;

    //Shield changed support
    EventMessages.ShieldsEnergyUpdated shieldChangedEvent = new EventMessages.ShieldsEnergyUpdated();

    [SerializeField]
    Slider healthBar;

    [SerializeField]
    Slider shieldsSlider;

    [SerializeField]
    Slider upgradesSlider;

    /// <summary>
    /// Awake is called before Start
    /// </summary>
    void Awake()
    {
        EventManager.AddHealthChangedListener(HandleHealthChangedEvent);

        //Add as event invoker for events
        EventManager.AddGameEndedInvoker(this);
        EventManager.AddShieldChangedInvoker(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Sends an event to update the shield value at the start of the game
        shieldChangedEvent.Invoke(shieldsSlider.value);
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
    }

    /// <summary>
    /// Checks if the slider for the shields changed
    /// </summary>
    public void ShieldsSliderChanged()
    {
        //Updates the value of the shield
        shieldChangedEvent.Invoke(shieldsSlider.value);

        //Updates teh upgrades bar
        upgradesSlider.value = 1 - shieldsSlider.value;
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
}
