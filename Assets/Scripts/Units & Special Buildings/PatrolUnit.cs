using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatrolUnit : Unit
{
    public BuildingRoute route;
    private int routeIndex = 0;
    private float routeCounter;

    public float speed = 1f;

    public void UpdateRouteCounter()
    {
        //Increase route counter based on speed
        routeCounter += Time.deltaTime * speed;

        //If finished a tile
        if (routeCounter >= 1f)
        {
            //Reset counter
            routeCounter -= 1f;

            routeIndex++;
            //Reset route
            if (routeIndex == route.routeTiles.Count)
                routeIndex = 0;

            //Move unit to a tile
            linkedTile = route.routeTiles[routeIndex];
            unitAnimator.SetInteger("Orientation", (int)route.routeOrientations[routeIndex]);

            //Request a update on monitored tiles
            CallOnPositionChanged();
        }
        UpdatePoliceManPosition();
    }
    private void UpdatePoliceManPosition()
    {
        transform.position = Vector3.Lerp(route.routeTiles[routeIndex].transform.position, 
            route.GetNextTilePosition(routeIndex), 
            routeCounter);
        transform.position -= Vector3.forward;
    }
}
