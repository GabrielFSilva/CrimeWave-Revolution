using UnityEngine;
using System;
using System.Collections.Generic;

public class BuildingManager : MonoBehaviour
{
    public event Action<Building, bool> OnBuildingHovered;
    public event Action<Building>       OnBuildingClicked;

    public List<Building>   buildings;

    private void Start()
    {
        foreach (Building __building in buildings)
        {
            __building.OnBuildingClicked += BuildingClicked;
            __building.OnBuildingHover += OnBuildingHovered;
            __building.buildingRoute = SortBuildingRoute(__building);
            __building.Setup();
        }
    }

    private void BuildingClicked(Building p_building)
    {
        if (OnBuildingClicked != null)
            OnBuildingClicked(p_building);
    }

    //Organize the building route and create the orientations
    private List<GridTile> SortBuildingRoute(Building p_building)
    {
        //Store oldRoute and create a new one
        List<GridTile> __oldRoute = p_building.buildingRoute;
        List<GridTile> __newRoute = new List<GridTile>();
        
        //Adds the start position
        __newRoute.Add(__oldRoute[0]);
        __oldRoute.RemoveAt(0);
        p_building.buildingRouteOrientation = new List<Orientation>();

        GridTile __targetTile;
        int __newRouteCount;
       
        //While there's something on the old route
        while (__oldRoute.Count != 0)
        {
            //Gets the new route counter and 
            __newRouteCount = __newRoute.Count;
            //Sends the rest of the old route and the lasts added tile of the new route
            __targetTile = GetNextTileFromList(__oldRoute, __newRoute[__newRouteCount - 1].tileX,
                __newRoute[__newRouteCount - 1].tileY);
            //Get the next tile
            if (__targetTile != null)
            {
                //If there is a tile up, add it
                if (__newRoute[__newRouteCount - 1].tileY > __targetTile.tileY)
                    p_building.buildingRouteOrientation.Add(Orientation.UP);
                //If it's on the right
                else if (__newRoute[__newRouteCount - 1].tileX < __targetTile.tileX)
                    p_building.buildingRouteOrientation.Add(Orientation.RIGHT);
                //If it's down
                else if (__newRoute[__newRouteCount - 1].tileY < __targetTile.tileY)
                    p_building.buildingRouteOrientation.Add(Orientation.DOWN);
                //If it's on the left
                else
                    p_building.buildingRouteOrientation.Add(Orientation.LEFT);

                __oldRoute.Remove(__targetTile);
                __newRoute.Add(__targetTile);
            }
            else
                break;
        }
        //Add the last orientation, closing the loop
        p_building.buildingRouteOrientation.Add(Orientation.UP);
        return __newRoute;
    }

    //Get the next tile from the list
    private GridTile GetNextTileFromList(List<GridTile> p_tiles, int p_tileX, int p_tileY)
    {
        //Check every orientation
        for (int i = 0; i < 4; i ++)
        {
            //Check every tile
            foreach (GridTile __tile in p_tiles)
            {
                if (__tile.tileX == p_tileX + Enums.GetOrientationX((Orientation)i)
                    && __tile.tileY == p_tileY + Enums.GetOrientationY((Orientation)i))
                    return __tile;
            }
        }
        return null;
    }
}
