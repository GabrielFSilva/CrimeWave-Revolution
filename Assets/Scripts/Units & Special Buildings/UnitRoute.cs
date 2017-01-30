using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BuildingRoute
{
    public List<GridTile>       routeTiles;
    public List<Orientation>    routeOrientations;

    public Vector3 GetNextTilePosition(int p_currentIndex)
    {
        if (p_currentIndex + 1 < routeTiles.Count)
            return routeTiles[p_currentIndex + 1].transform.position;
        return routeTiles[0].transform.position;
    }
    public Orientation GetNextOrientation(int p_currentIndex)
    {
        if (p_currentIndex + 1 < routeTiles.Count)
            return routeOrientations[p_currentIndex + 1];
        return routeOrientations[0];
    }
}
