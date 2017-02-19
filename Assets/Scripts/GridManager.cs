using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    public event Action<GridTile> OnStreetTileClicked;
    public event Action<GridTile> OnBuildingTileClicked;
    public event Action<GridTile> OnCrimeStarted;
    public event Action<GridTile, int> OnTileHover;

    public List<GridTile>   tiles;
    public List<GridTile>   streetTiles;
    public List<GridTile>   buildingTiles;
    public List<GridTile>   monitoredTiles;
    public List<GridTile>   patroledTiles;

    public List<Sprite>     routeSprites;

    public bool showCrimeRates = false;


    private void Start ()
    {
        Instance = this;
        streetTiles = new List<GridTile>();
        buildingTiles = new List<GridTile>();
        monitoredTiles = new List<GridTile>();
        patroledTiles = new List<GridTile>();
	    foreach(GridTile __tile in tiles)
        {
            __tile.OnMouseClick += TileClicked;
            __tile.OnCrimeStarted += CrimeStarted;
            __tile.OnMouseHover += OnTileHover;
            if (__tile.tileType == GridTile.TileType.STREET)
                streetTiles.Add(__tile);
            else
                buildingTiles.Add(__tile);
        }
	}
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            foreach (GridTile __tile in streetTiles)
                __tile.EnableCrimeRateIndicator(true, true);
        }
        else if (Input.GetKeyUp(KeyCode.F4))
        {
            foreach (GridTile __tile in streetTiles)
                __tile.EnableCrimeRateIndicator(false, true);
        }

        if (showCrimeRates)
        {
            foreach (GridTile __tile in streetTiles)
            {
                __tile.EnableCrimeRateIndicator(true, false);
            }
        }
    }
    private void CrimeStarted(GridTile p_tile)
    {
        if (OnCrimeStarted != null)
            OnCrimeStarted(p_tile);
    }

    private void TileClicked(GridTile p_tile)
    {
        if (p_tile.tileType == GridTile.TileType.STREET && OnStreetTileClicked != null)
            OnStreetTileClicked(p_tile);
        else if (p_tile.tileType == GridTile.TileType.BUILDING && OnBuildingTileClicked != null)
            OnBuildingTileClicked(p_tile);
    }
    public void EnableTilePlacementIcons(bool p_enable, TileType p_type)
    {
        if (p_type == TileType.STREET)
        {
            foreach (GridTile __tile in streetTiles)
                __tile.tileSprite.enabled = p_enable;
        }
        else
        {
            foreach (GridTile __tile in buildingTiles)
                __tile.tileSprite.enabled = p_enable;
        }
    }
    public void ShowBuildingRoute(BuildingRoute p_route, bool p_showRoute)
    {
        Orientation __currentOri;
        Orientation __nextOri;
        //FirstTile
        SetTileRoute(p_route.routeTiles[0], p_showRoute, 0, Orientation.UP);
        for (int i = 1; i < p_route.routeTiles.Count - 1; i ++)
        {
            __currentOri = p_route.routeOrientations[i - 1];
            __nextOri = p_route.routeOrientations[i];
            //Straight Line
            if (p_route.routeOrientations[i - 1] ==  __nextOri)
            {
                if (__nextOri == Orientation.UP || __nextOri == Orientation.DOWN)
                    SetTileRoute(p_route.routeTiles[i], p_showRoute, 1, Orientation.UP);
                else
                    SetTileRoute(p_route.routeTiles[i], p_showRoute, 1, Orientation.RIGHT);
            }
            //Curves
            //Turning Right
            else if (Enums.IsTurningRight(__currentOri, __nextOri))
                SetTileRoute(p_route.routeTiles[i], p_showRoute, 2, __currentOri);
            //Turning Left
            else
                SetTileRoute(p_route.routeTiles[i], p_showRoute, 2, __currentOri, true);
        }
        SetTileRoute(p_route.routeTiles[p_route.routeTiles.Count - 1], p_showRoute, 3, Orientation.UP);
    }
    private void SetTileRoute(GridTile p_tile, bool p_showRoute, int p_routeSprite, Orientation p_orientation, bool p_flipX = false)
    {
        p_tile.routeSprite.enabled = p_showRoute;
        if (p_showRoute)
        {
            p_tile.routeSprite.sortingOrder = 110;
            p_tile.routeSprite.sprite = routeSprites[p_routeSprite];
            p_tile.routeSprite.flipX = p_flipX;
            p_tile.transform.rotation = Quaternion.Euler(0f, 0f, (int)p_orientation * -90f);
        }
    }
    public void MouseEnterCrimeRates()
    {
        showCrimeRates = true;
    }
    public void MouseExitCrimeRates()
    {
        showCrimeRates = false;
        if (!Input.GetKey(KeyCode.F1))
        {
            foreach (GridTile __tile in streetTiles)
            {
                __tile.EnableCrimeRateIndicator(false, false);
            }
        }
    }

    public void UpdateMonitoredTiles(List<Unit> p_units)
    {
        monitoredTiles = new List<GridTile>();
        patroledTiles = new List<GridTile>();
        foreach(Unit __unit in p_units)
        {
            SetMonitoredTilesToUnit(__unit);
            AddTilesToMonitoredList(__unit.monitoredTiles);
            if (__unit.unitType == UnitType.POLICE_MAN || __unit.unitType == UnitType.POLICE_CAR)
                AddTilesToPatroledList(__unit.monitoredTiles);
        }
    }
    private void SetMonitoredTilesToUnit(Unit p_unit)
    {
        List<GridTile> __monitoredTiles = new List<GridTile>();
        if (p_unit.viewType == UnitViewType.LINEAR)
        {
            GridTile __tile;
            //__monitoredTiles.Add()
            for (int i = 0; i <= p_unit.viewRange; i++)
            {
                __tile = GetTileByIndex(p_unit.linkedTile.tileX + (Enums.GetOrientationX(p_unit.unitOrientation) * i),
                    p_unit.linkedTile.tileY + (Enums.GetOrientationY(p_unit.unitOrientation) * i));
                if (__tile == null || __tile.tileType == GridTile.TileType.BUILDING)
                    break;
                else
                    __monitoredTiles.Add(__tile);
            }
        }
        else
        {
            //Row
            for (int i = p_unit.linkedTile.tileY - p_unit.viewRange; i <= p_unit.linkedTile.tileY + p_unit.viewRange; i++)
            {
                //Column
                for (int j = p_unit.linkedTile.tileX - p_unit.viewRange; j <= p_unit.linkedTile.tileX + p_unit.viewRange; j++)
                {
                    __monitoredTiles.Add(GetTileByIndex(j, i));
                }
            }
        }
        p_unit.UpdateMonitoredTiles(__monitoredTiles);
    }
    private void AddTilesToMonitoredList(List<GridTile> p_tiles)
    {
        foreach (GridTile __tile in p_tiles)
            if (__tile != null && !monitoredTiles.Contains(__tile))
                monitoredTiles.Add(__tile);
    }
    private void AddTilesToPatroledList(List<GridTile> p_tiles)
    {
        foreach (GridTile __tile in p_tiles)
            if (__tile != null && !patroledTiles.Contains(__tile))
                patroledTiles.Add(__tile);
    }
    public GridTile GetTileByIndex(int p_indexX, int p_indexY)
    {
        if (p_indexX < 0 || p_indexX > 13 || p_indexY < 0 || p_indexY > 13)
            return null;
        foreach (GridTile __tile in tiles)
            if (__tile.tileX == p_indexX && __tile.tileY == p_indexY)
                return __tile;
        return null;
    }
}
