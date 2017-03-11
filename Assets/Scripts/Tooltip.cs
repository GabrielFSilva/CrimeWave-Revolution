using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour
{
    public EventTrigger trigger;
    public GameObject   tooltipContainer;

	void Start ()
    {
        EventTrigger.Entry __entry = new EventTrigger.Entry();
        
        __entry.eventID = EventTriggerType.PointerEnter;
        __entry.callback.AddListener((data) => { OnPointerEnterDelegate((PointerEventData)data); });
        trigger.triggers.Add(__entry);

        __entry = new EventTrigger.Entry();
        __entry.eventID = EventTriggerType.PointerExit;
        __entry.callback.AddListener((data) => { OnPointerExitDelegate((PointerEventData)data); });
        trigger.triggers.Add(__entry);
    }

    public void OnPointerEnterDelegate(PointerEventData data)
    {
        tooltipContainer.gameObject.SetActive(true);
    }
    public void OnPointerExitDelegate(PointerEventData data)
    {
        tooltipContainer.gameObject.SetActive(false);
    }
}
