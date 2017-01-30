using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UnitsManager : MonoBehaviour
{
    public event Action<Unit> OnUnitSelected;
    public event Action<Unit> OnUnitMoved;
    public Transform unitsContainer;
    public List<GameObject> unitPrefabs;
    public List<Unit> units;
    public List<PatrolUnit> patrolUnits;
    public Unit selectedUnit;

    private void Update()
    {
        for (int i = 0; i < patrolUnits.Count; i++)
        {
            patrolUnits[i].UpdateRouteCounter();
        }
    }
    public void SpawnUnit(UnitType p_unitType, GridTile p_tile)
    {
        SpawnUnit(p_unitType, p_tile, null);
    }
    public void SpawnUnit(UnitType p_unitType, GridTile p_tile, Building p_building)
    {
        Unit __unit = ((GameObject)Instantiate(unitPrefabs[(int)p_unitType],
            p_tile.transform.position - Vector3.forward,
            Quaternion.identity,
            unitsContainer)).GetComponent<Unit>();

        __unit.linkedTile = p_tile;
        __unit.OnUnitClicked += OnUnitSelected;
        __unit.OnPositionChange += OnUnitMoved;
        units.Add(__unit);

        if (p_unitType == UnitType.POLICE_CAR || p_unitType == UnitType.POLICE_MAN)
        {
            patrolUnits.Add(__unit.GetComponent<PatrolUnit>());
            patrolUnits[patrolUnits.Count - 1].route = p_building.route;
        }
    }

    public void RemoveUnit(Unit p_unit)
    {
        units.Remove(p_unit);
        if (p_unit.unitType == UnitType.POLICE_MAN || p_unit.unitType == UnitType.POLICE_CAR)
            patrolUnits.Remove(p_unit.GetComponent<PatrolUnit>());
        Destroy(p_unit.gameObject);
    }
}
