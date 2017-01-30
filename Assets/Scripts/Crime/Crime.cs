using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Crime : MonoBehaviour
{
    public event    Action<Crime> OnCrimeEnd;
    public GridTile tile;

    public float    crimeDuration = 3f;
    [SerializeField]
    private float   crimeTimer;

    public bool     seen;
    public bool     stopped;

    public SpriteRenderer   sprite;
    public SpriteRenderer   clock;
    public List<Sprite>     clockSprites;
    public Sprite     stoppedSprite;
    private void Update()
    {
        crimeTimer += Time.deltaTime;
        if (stopped)
            crimeTimer += Time.deltaTime * 2f;

        UpdateClockSprite();
        if (crimeTimer >= crimeDuration && OnCrimeEnd != null)
            OnCrimeEnd(this);
    }

    private void UpdateClockSprite()
    {
        if (stopped)
            return;
        int __index = Mathf.CeilToInt(crimeTimer * 8/ crimeDuration);
        if (__index < 0 || __index >= 8)
            return;
        clock.sprite = clockSprites[__index - 1];
    }

    public void SetCrimeStatus(bool p_stopped)
    {
        tile.crimeSpotted = true;
        stopped = p_stopped;
        seen = true;
        sprite.color = Color.white;
        clock.color = Color.white;
        if (p_stopped)
        {
            sprite.sprite = stoppedSprite;
        }
    }
}
