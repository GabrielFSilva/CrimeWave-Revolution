using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchCountdownManager : MonoBehaviour
{
    private SoundManager soundManager;
    public float    countdown = 10f;
    public int      lastTick;
    public bool     counting;

    
    private void Start()
    {
        soundManager = SoundManager.GetInstance();
    }
	private void Update ()
    {
        if (!counting || countdown < 0f)
            return;
        countdown -= Time.deltaTime;
        if (lastTick != Mathf.CeilToInt(countdown))
        {
            soundManager.PlaySFX(SFXType.MATCH_COUNTDOWN_TICK, SoundVolumes.sfxUnitBought_MatchCountdownTick);
            lastTick = Mathf.CeilToInt(countdown);
        }
	}
    public void StartCountdown(float p_countdownTimer)
    {
        if (counting)
            return;
        counting = true;
        countdown = p_countdownTimer;
        lastTick = Mathf.CeilToInt(p_countdownTimer);
    }
}
