using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour
{
    public EventTrigger     trigger;
    public GameObject       tooltipContainer;

    public Image            triggerImage;
    public RectTransform    targetBar;

    public bool     changeSprite = false;
    public Sprite   spriteOnEnter;
    public Sprite   spriteOnExit;
   
    public bool     changeColor = false;
    public Color    colorOnEnter = Color.white;
    public Color    colorOnExit = Color.white;

    private void Start ()
    {
        EventTrigger.Entry __entry = new EventTrigger.Entry();
        
        __entry.eventID = EventTriggerType.PointerEnter;
        __entry.callback.AddListener((data) => { OnPointerEnterDelegate((PointerEventData)data); });
        trigger.triggers.Add(__entry);
        __entry = new EventTrigger.Entry();
        __entry.eventID = EventTriggerType.PointerExit;
        __entry.callback.AddListener((data) => { OnPointerExitDelegate((PointerEventData)data); });
        trigger.triggers.Add(__entry);

        ChangeState(false);
    }

    private void Update()
    {
        if (targetBar != null)
            triggerImage.rectTransform.sizeDelta = targetBar.sizeDelta;
    }

    public void ChangeState(bool p_onEnter)
    {
        tooltipContainer.gameObject.SetActive(p_onEnter);
        if (changeColor)
            triggerImage.color = p_onEnter ? colorOnEnter : colorOnExit;
        if (changeSprite)
            triggerImage.sprite = p_onEnter ? spriteOnEnter : spriteOnExit;
    }
    public void OnPointerEnterDelegate(PointerEventData data)
    {
        ChangeState(true);
    }
    public void OnPointerExitDelegate(PointerEventData data)
    {
        ChangeState(false);
    }
}
