using UnityEngine;
using System;
using System.Collections.Generic;

public class CrimeManager : MonoBehaviour
{
    public event Action     OnCrimeEnded;
    public GridManager      gridManager;
    public GameObject       crimePrefab;
    public Transform        crimesContainer;
    public GameObject       crimeResultPrefab;

    public List<Crime> crimes;
    public static float CrimesPerSecond = 1f;
    public static float CrimesVariation = 0.3f;

    public int notseenCrimes = 0;
    public int seenCrimes = 0;
    public int stoppedCrimes = 0;

    public List<GridTile>   crimeFocusPoints;
    public int              crimeFocusRange;
    public List<float>      crimeRatesList;
    
    public Sprite stoppedSprite;

    private void Awake()
    {
        GridTile __targetTile;
        for(int i = 0; i < 4; i ++)
        {
            GridTile __crimeFocusTile = CreateCrimeFocusPoint();
            crimeFocusPoints.Add(__crimeFocusTile);
            //Row
            for (int __y = __crimeFocusTile.tileY - crimeFocusRange; __y <= __crimeFocusTile.tileY + crimeFocusRange; __y++)
            {
                //Column
                for (int __x = __crimeFocusTile.tileX - crimeFocusRange; __x <= __crimeFocusTile.tileX + crimeFocusRange; __x++)
                {
                    __targetTile = gridManager.GetTileByIndex(__x, __y);
                    if (__targetTile != null)
                        __targetTile.SetCrimeRate(GetCrimeFocusInfluence(__crimeFocusTile.tileX, __crimeFocusTile.tileY,
                            __targetTile.tileX, __targetTile.tileY));
                }
            }
        }
    }
    private GridTile CreateCrimeFocusPoint()
    {
        int __x = 0, __y = 0;
        bool __valid = true;
        while(true)
        {
            __valid = true;
            __x = UnityEngine.Random.Range(0, 14);
            __y = UnityEngine.Random.Range(0, 14);
            foreach (GridTile __focus in crimeFocusPoints)
            {
                if (Mathf.Abs(__focus.tileX - __x) + Mathf.Abs(__focus.tileY - __y) <= 5)
                {
                    __valid = false;
                    break;
                }
            }
            if (__valid && !crimeFocusPoints.Contains(gridManager.GetTileByIndex(__x, __y)))
            {
                break;
            }
        }
        return gridManager.GetTileByIndex(__x, __y);
    }
    private float GetCrimeFocusInfluence(int p_focusX, int p_focusY, int p_targetX, int p_targetY)
    {
        int __dist = GetDistanceBetweenPoints(p_focusX, p_focusY, p_targetX, p_targetY);

        //Same Tile
        if (p_focusX == p_targetX && p_focusY == p_targetY)
            return crimeRatesList[0];
        //Not Diagonal
        else if (p_focusX == p_targetX || p_focusY == p_targetY)
            return crimeRatesList[__dist];
        //Diagonal
        else if (Mathf.Abs(p_focusX - p_targetX) == Mathf.Abs(p_focusY - p_targetY))
            return crimeRatesList[(__dist/2)];
        //Other
        else
        {
            if (Mathf.Abs(p_focusX - p_targetX) > Mathf.Abs(p_focusY - p_targetY))
                return crimeRatesList[Mathf.Abs(p_focusX - p_targetX)];
            else
                return crimeRatesList[Mathf.Abs(p_focusY - p_targetY)];
        }
    }
    private int GetDistanceBetweenPoints(int p_focusX, int p_focusY, int p_targetX, int p_targetY)
    {
        return Mathf.Abs(p_focusX - p_targetX) + Mathf.Abs(p_focusY - p_targetY);
    }
    public void SpawnCrime(GridTile p_tile)
    {
        Crime __crime = ((GameObject)Instantiate(crimePrefab, p_tile.transform.position, Quaternion.identity, crimesContainer)).GetComponent<Crime>();
        __crime.transform.position = p_tile.transform.position;
        __crime.tile = p_tile;
        crimes.Add(__crime);
        __crime.OnCrimeEnd += CrimeEnded;
    }

    private void CrimeEnded(Crime p_crime)
    {
        if (p_crime.seen)
        {
            CrimeResult __result = ((GameObject)Instantiate(crimeResultPrefab, p_crime.transform.position, Quaternion.identity, crimesContainer))
               .GetComponent<CrimeResult>();
            if (p_crime.stopped)
            {
                stoppedCrimes++;
                __result.sprite.sprite = stoppedSprite;
            }
            else
                seenCrimes++;
        }
        else
            notseenCrimes++;

        crimes.Remove(p_crime);
        Destroy(p_crime.gameObject);
        if (OnCrimeEnded != null)
            OnCrimeEnded();
    }

    public void CheckSeenCrimes()
    {
        foreach(Crime __crime in crimes)
        {
            if (__crime.seen)
                continue;
            if (gridManager.monitoredTiles.Contains(__crime.tile))
                __crime.SetCrimeStatus(false);
        }
    }

    public void CheckStoppedCrimes()
    {
        foreach (Crime __crime in crimes)
        {
            if (__crime.stopped)
                continue;
            if (gridManager.patroledTiles.Contains(__crime.tile))
                __crime.SetCrimeStatus(true);
        }
    }

}
