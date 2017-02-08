using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUnitPlacementManager : MonoBehaviour
{
    public Transform unitsBasePosition;
    public List<GameObject> units;

    public void ResetUnits()
    {
        for (int i = 0; i < units.Count; i++)
            units[i].transform.position = unitsBasePosition.position;
    }
    public void PlaceUnit(GridTile p_tile, UnitType p_unitType)
    {
        units[(int)p_unitType].transform.position = p_tile.transform.position + (Vector3.back * 2f);
    }
}
