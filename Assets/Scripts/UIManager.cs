﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    //Actions
    public event Action<int, bool>  OnBuyButtonClicked;
    public event Action             OnSellButtonClicked;
    public event Action             OnRotateButtonClicked;
    public event Action<bool>       OnFowardButtonClicked;
    public event Action             OnPauseButtonClicked;
    public event Action<int>        OnPauseScreenButtonClicked;

    [Header("Managers")]
    public UIUnitPlacementManager   unitPlacement;

    [Header("State Containers")]
    public RectTransform        normalUIContainer;
    public RectTransform        sellUIContanier;
    public RectTransform        pauseScreen;

    [Header("Buttons")]
    public List<RectTransform>  buyButtons;
    public RectTransform        cancelButton;
    public RectTransform        rotateButton;

    [Header("Labels")]
    public Text timeLabel;
    public Text moneyLabel;
    public Text sellButtonLabel;

    public Text crimeLimitLabel;
    public Text notStoppedCrimesLabel;
    public Text stoppedCrimesLabel;

    [Header("CrimeBars")]
    public List<RectTransform>  crimeBars;
    public List<Animator>       crimeBarsAnimators;
    public List<int>            crimeBarsMinSize;
    public int                  sizeUntilArrowMark;

    [Header("Images")]
    public Image        sellIconImage;
    public List<Sprite> sellIconSprites;
    public Image        bgmIcon;
    public List<Sprite> bgmIcons;
    public Image        sfxIcon;
    public List<Sprite> sfxIcons;

    private void Start()
    {
        UpdateAudioButton();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            OnBuyButtonClicked(0, true);
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            OnBuyButtonClicked(1, true);
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            OnBuyButtonClicked(2, true);
        else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            OnBuyButtonClicked(3, true);
    }
    public void UpdateAudioButton()
    {
        if (SoundManager.GetInstance().GetBgmMute())
            bgmIcon.sprite = bgmIcons[1];
        else
            bgmIcon.sprite = bgmIcons[0];
        if (SoundManager.GetInstance().GetSfxMute())
            sfxIcon.sprite = sfxIcons[1];
        else
            sfxIcon.sprite = sfxIcons[0];
    } 
    public void UpdateCrimeLimitLabel(int p_crimeLimit)
    {
        crimeLimitLabel.text = "LIMIT " + p_crimeLimit.ToString();
    }
    public void UpdateCrimeBars(int p_crimeLimit, int p_notSeen, int p_seen, int p_stopped)
    {
        notStoppedCrimesLabel.text = (p_notSeen + p_seen).ToString();
        stoppedCrimesLabel.text = p_stopped.ToString();

        UpdateBar(crimeBars[0], crimeBarsMinSize[0], p_notSeen, p_crimeLimit, crimeBarsMinSize[1]);
        UpdateBar(crimeBars[1], crimeBarsMinSize[1], p_seen, p_crimeLimit, crimeBarsMinSize[1], 0f);
        UpdateBar(crimeBars[2], crimeBarsMinSize[2], p_stopped, p_crimeLimit);
    }
    public void BlinkCrimeBar(int p_barIndex)
    {
        crimeBarsAnimators[p_barIndex].SetTrigger("Blink");
    }
    private void UpdateBar(RectTransform p_bar, int p_minSize, int p_crimeCount, int p_crimeLimit, 
        int p_extraBarOffset = 0, float p_anchorYDelta = 1f)
    {
        p_bar.sizeDelta = new Vector2(p_bar.sizeDelta.x,
            (float)p_minSize + ((float)(sizeUntilArrowMark - p_minSize - p_extraBarOffset) * (float)p_crimeCount / (float)p_crimeLimit));
        p_bar.anchoredPosition = new Vector2 (p_bar.anchoredPosition.x,  p_anchorYDelta + p_bar.sizeDelta.y / 2f);
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
            OnBuyButtonClicked(p_unitTypeIndex, false);
    }
    public void SellButtonPressed()
    {
        if (OnSellButtonClicked != null)
            OnSellButtonClicked();
    }
    public void RotateButtonPressed()
    {
        if (OnRotateButtonClicked != null)
            OnRotateButtonClicked();
    }
    public void PauseButtonPressed()
    {
        if (OnPauseButtonClicked != null)
            OnPauseButtonClicked();
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

    public void ForwardButtonPressed(bool p_pressed)
    {
        if (OnFowardButtonClicked != null)
            OnFowardButtonClicked(p_pressed);
    }

    public void PauseScreenButtonClicked(int p_buttonID)
    {
        if (OnPauseScreenButtonClicked != null)
            OnPauseScreenButtonClicked(p_buttonID);
    }
}
