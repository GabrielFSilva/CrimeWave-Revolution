using UnityEngine;
using System;
using System.Collections;

public class GridTile : MonoBehaviour
{
    public event Action<GridTile, int> OnMouseHover;
    public event Action<GridTile> OnMouseClick;
    public event Action<GridTile> OnCrimeStarted;
    public enum TileType
    {
        STREET,
        BUILDING
    }
    public TileType             tileType = TileType.STREET;
    public int                  tileX;
    public int                  tileY;
    public SpriteRenderer       tileSprite;

    public Building             linkedBuilding;
    public GameObject           routePrefab;
    public SpriteRenderer       routeSprite;

    public GameObject           crimeRatePrefab;
    public CrimeRateIndicator   crimeRateIndicator;
    public float                crimeRate;
    [SerializeField]
    private float               crimeBaseTimer;
    [SerializeField]
    private float               crimeStartTimer;
    public bool                 crimeSpotted = false;

    private void Awake()
    {
        name = "Tile_" + tileX.ToString() + "_" + tileY.ToString();
        tileSprite = GetComponent<SpriteRenderer>();
        if (tileSprite.color != Color.white)
        {
            tileType = TileType.BUILDING;
        }
        tileSprite.color = new Color(1f, 1f, 1f, 0.6f);
    }
    private void Start()
    {
        routeSprite =  ((GameObject)Instantiate(routePrefab, transform.position, Quaternion.identity, transform))
            .GetComponent<SpriteRenderer>();
        routeSprite.color = new Color(0f, 0f, 1f, 0.6f);
        routeSprite.enabled = false;
        crimeRateIndicator = ((GameObject)Instantiate(crimeRatePrefab, transform.position, Quaternion.identity, transform))
            .GetComponent<CrimeRateIndicator>();
        crimeBaseTimer = 70f / (CrimeManager.CrimesPerSecond * crimeRate);
        ResetCrimeTimer();
    }
    private void Update()
    {
        if (tileType == TileType.STREET)
        {
            crimeStartTimer -= Time.deltaTime;
            if (crimeStartTimer <= 0f)
            {
                if (OnCrimeStarted != null)
                    OnCrimeStarted(this);
                ResetCrimeTimer();
            }
        }
    }
    private void OnMouseEnter()
    {
        if (OnMouseHover != null)
            OnMouseHover(this, 0);
    }
    private void OnMouseOver()
    {
        if (OnMouseHover != null)
            OnMouseHover(this, 1);
    }
    private void OnMouseExit()
    {
        if (OnMouseHover != null)
            OnMouseHover(this, 2);
    }
    private void OnMouseUpAsButton()
    {
        if (OnMouseClick != null)
            OnMouseClick(this);
    }
    private void ResetCrimeTimer()
    {
        crimeStartTimer = UnityEngine.Random.Range(2f,
            crimeBaseTimer + UnityEngine.Random.Range(-CrimeManager.CrimesVariation, CrimeManager.CrimesVariation));
    }
    public void SetCrimeRate(float p_rate)
    {
        if (crimeRate < p_rate && tileType == TileType.STREET)
            crimeRate = p_rate;
    }
    public void EnableCrimeRateIndicator(bool p_enable, bool p_hacked)
    {
        if (enabled)
        {
            if (!p_hacked && !crimeSpotted)
                crimeRateIndicator.sprite.color = new Color(0f, 0f, 0f, 0.75f);
            else
                crimeRateIndicator.sprite.color = new Color(crimeRate, 0f, 0f, 0.75f);
        }
        crimeRateIndicator.gameObject.SetActive(p_enable);
    }
}
