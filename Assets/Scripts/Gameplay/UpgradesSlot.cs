using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UpgradesSlot : MonoBehaviour, IDropHandler
{
    private RectTransform rect;

    // events fired by class
    EventMessages.UpgradeItemDroppedOnSlot upgradeItemDroppedEvent = new EventMessages.UpgradeItemDroppedOnSlot();

    [SerializeField]
    private UpgradesItem currentUpgradeInSlot = null;

    /// <summary>
    /// The Rect of this object
    /// </summary>
    public RectTransform Rect { get { return rect; } }

    /// <summary>
    /// What upgrade is currently in this slot
    /// </summary>
    public UpgradesItem CurrentUpgradeInSlot { get { return currentUpgradeInSlot; } }

    void Awake()
    {
        //Saving the transform for later
        rect = GetComponent<RectTransform>();

        //Add as event invoker for events
        EventManager.AddUpgradeItemDroppedInvoker(this);
    }

    /// <summary>
    /// Called when the mouse is released inside the slot
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
        {
            //Updates the value of the droppedOnSlot variable
            eventData.pointerDrag.GetComponent<UpgradesItem>().DroppedOnSlot = true;

            //Updates the old upgrade's slot to the new item's one
            currentUpgradeInSlot.InitialSlot = eventData.pointerDrag.GetComponent<UpgradesItem>().InitialSlot;

            //Snaps the old upgrade into the from the new item slot
            currentUpgradeInSlot.Rect.anchoredPosition = eventData.pointerDrag.GetComponent<UpgradesItem>().InitialSlot.Rect.anchoredPosition;

            //Updates the new item's old slot with the current item
            eventData.pointerDrag.GetComponent<UpgradesItem>().InitialSlot.currentUpgradeInSlot = currentUpgradeInSlot;

            //Snaps the new upgrade into the current slot
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = rect.anchoredPosition;

            //Updates the upgrade item in this slot
            currentUpgradeInSlot = eventData.pointerDrag.GetComponent<UpgradesItem>();

            //Updates the upgrade's slot number
            currentUpgradeInSlot.InitialSlot = this;

            //Fires the necessary event
            upgradeItemDroppedEvent.Invoke();
        }
    }

    /// <summary>
    /// Adds the listener for the Upgrade Item Dropped event
    /// </summary>
    /// <param name="listener">listener</param>
    public void AddUpgradeItemDroppedListener(UnityAction listener)
    {
        upgradeItemDroppedEvent.AddListener(listener);
    }
}
