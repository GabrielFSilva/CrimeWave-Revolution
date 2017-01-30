using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Building : MonoBehaviour
{
    public event Action<Building, bool> OnBuildingHover;
    public event Action<Building>       OnBuildingClicked;

    
    public List<GridTile> buildingTiles;
    public BuildingRoute route;
    public List<GridTile> buildingRoute;
    public List<Orientation> buildingRouteOrientation;
    
    private bool _routePainted = false;

    public void Setup()
    {
        for (int i = 0; i < buildingTiles.Count; i++)
        {
            buildingTiles[i].linkedBuilding = this;
            buildingTiles[i].OnMouseHover += OnTileHover;
            buildingTiles[i].OnMouseClick += OnTileClick;
        }
        for (int i = 0; i < buildingRoute.Count; i++)
        {
            route.routeTiles.Add(buildingRoute[i]);
            route.routeOrientations.Add(buildingRouteOrientation[i]);
        }
    }
    private void OnTileClick(GridTile p_tile)
    {
        if (OnBuildingClicked != null)
            OnBuildingClicked(this);
    }

    private void OnTileHover(GridTile p_tile, int p_action)
    {
        if (GameSceneManager.gameState == GameState.BUYING && 
            (GameSceneManager.Instance.unitEditingType == UnitType.POLICE_MAN 
            || GameSceneManager.Instance.unitEditingType == UnitType.POLICE_CAR))
        {
            if (p_action == 0)
                ChangeRouteColor(true);
            else if (p_action == 2)
                ChangeRouteColor(false);
        }
        else if (p_action == 1)
            ChangeRouteColor(false);
    }
    private void ChangeRouteColor(bool p_paintRoute)
    {
        if (OnBuildingHover != null)
            OnBuildingHover(this, p_paintRoute);
        /*
        if (p_paintRoute == _routePainted)
            return;
        foreach (GridTile __tile in buildingRoute)
        {
            if (p_paintRoute)
                __tile.spriteRenderer.sortingOrder = 110;
            else
                __tile.spriteRenderer.sortingOrder = 0;
        }
        _routePainted = p_paintRoute;*/
    }
}
