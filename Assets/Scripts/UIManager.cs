using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    //Actions
    public event Action<int>    OnBuyButtonClicked;
    public event Action         OnSellButtonClicked;
    public event Action         OnRotateButtonClicked;

    //State Containers
    public RectTransform        normalUIContainer;
    public RectTransform        sellUIContanier;

    public List<RectTransform>  buyButtons;
    public RectTransform        cancelButton;
    public RectTransform        rotateButton;

    //Labels
    public Text timeLabel;
    public Text moneyLabel;
    public Text sellButtonLabel;

    public Text         crimeLimitLabel;
    public List<Text>   crimeLabels;
    
    public Image        sellIconImage;
    public List<Sprite> sellIconSprites;

 
    public void UpdateCrimeLimitLabel(int p_crimeLimit)
    {
        crimeLimitLabel.text = "Don't pass " + p_crimeLimit.ToString() + "!";
    }
    public void UpdateCrimeLabels(int p_notSeen, int p_seen, int p_stopped)
    {
        crimeLabels[0].text = p_stopped.ToString();
        crimeLabels[1].text = p_seen.ToString();
        crimeLabels[2].text = p_seen.ToString();
        crimeLabels[3].text = p_notSeen.ToString();
        crimeLabels[4].text = p_notSeen.ToString();
        crimeLabels[5].text = "= " + (p_notSeen + p_seen).ToString();
    }
    public void EnableNormalUI(bool p_enable)
    {
        normalUIContainer.gameObject.SetActive(p_enable);
        sellUIContanier.gameObject.SetActive(!p_enable);
        if (!p_enable && GameSceneManager.Instance.unitEditingType == UnitType.POLICE_CAMERA)
            EnableRotateButton(true);
        else
            EnableRotateButton(false);
    }
    public void EnableRotateButton(bool p_enable)
    {
        rotateButton.gameObject.SetActive(p_enable);
    }
    public void UpdateTimeLabel(int p_day)
    {
        if (p_day > 30)
            p_day = 30;
        timeLabel.text = "April " + p_day.ToString();
    }
    public int UpdateMoneyLabel()
    {
        if (GameSceneManager.Instance == null)
            return -2;
        if (moneyLabel == null)
            return -1;

        moneyLabel.text = "$ " + GameSceneManager.Money;
        return 0;
    }
    public void UpdateSellButtonLabel(UnitType p_unitType)
    {
        sellButtonLabel.text = "SELL: $" + GameEconomy.GetUnitSellPrice(p_unitType).ToString();
        sellIconImage.sprite = sellIconSprites[(int)GameSceneManager.Instance.unitEditingType];
        sellIconImage.rectTransform.sizeDelta = sellIconImage.sprite.rect.size;
    }
    public void BuyButtonPressed(int p_unitTypeIndex)
    {
        if (OnBuyButtonClicked != null)
            OnBuyButtonClicked(p_unitTypeIndex);
    }

    public void SellButtonPressed()
    {
        if (OnSellButtonClicked != null)
            OnSellButtonClicked();
    }
    public void UpdateCancelButton(int p_buttonIndex)
    {
        if (p_buttonIndex >= 0 && p_buttonIndex <= 4)
        {
            cancelButton.anchoredPosition = buyButtons[p_buttonIndex].anchoredPosition;
            cancelButton.gameObject.SetActive(true);
        }
        else
            cancelButton.gameObject.SetActive(false);
    }
}
