using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour
{
    public event Action<Unit> OnUnitClicked;
    public event Action<Unit> OnPositionChange;

    public UnitType unitType;
    public UnitTilePlacement unitTilePlacement;

    public GridTile             linkedTile;
    [Range(0,10)] public int    viewRange;
    public UnitViewType         viewType;
    public List<GridTile>       monitoredTiles;

    public Orientation  unitOrientation;
    public bool         mouseHover = false;
    
    public Animator         unitAnimator;
    public SpriteRenderer   unitSprite;
    public SpriteRenderer   unitViewRangeSprite;
    public BoxCollider2D    unitCollider;

    private void Update()
    {
        //Update Collider
        if (GameSceneManager.gameState != GameState.BUYING)
            unitCollider.enabled = true;
        else
            unitCollider.enabled = false;

        //Update view range sprite
        if (unitViewRangeSprite == null || unitType != UnitType.POLICE_STATION)
            return;
        if (GameSceneManager.gameState == GameState.PLAYING && mouseHover)
            SetViewRangeSortingOrder(11);
        else if (GameSceneManager.gameState == GameState.EDITING && GameSceneManager.Instance.selectedUnit == this)
            SetViewRangeSortingOrder(11);
        else
            SetViewRangeSortingOrder(0);
    }
    private void SetViewRangeSortingOrder(int p_sortingOrder)
    {
        if (unitViewRangeSprite.sortingOrder != p_sortingOrder)
            unitViewRangeSprite.sortingOrder = p_sortingOrder;
    }
    private void OnMouseEnter()
    {
        mouseHover = true;
    }
    private void OnMouseExit()
    {
        mouseHover = false;
    }
    private void OnMouseUpAsButton()
    {
        if (OnUnitClicked != null)
            OnUnitClicked(this);
    }
    protected void CallOnPositionChanged()
    {
        if (OnPositionChange != null)
            OnPositionChange(this);
    }

    public void UpdateMonitoredTiles(List<GridTile> p_data)
    {
        monitoredTiles = new List<GridTile>();
        for(int i = 0; i < p_data.Count; i++)
        {
            if (p_data != null)
                monitoredTiles.Add(p_data[i]);
        }
    }
    
    public void RotateUnit()
    {
        transform.Rotate(Vector3.forward * -90f);
        unitOrientation++;
        if ((int)unitOrientation == 4)
            unitOrientation = Orientation.UP;
        if (unitType == UnitType.POLICE_CAMERA)
        {
            if (unitOrientation == Orientation.RIGHT)
            {
                unitSprite.flipX = true;
                unitSprite.flipY = true;
            }
            else
            {
                unitSprite.flipX = true;
                unitSprite.flipY = false;
            }
        }
        CallOnPositionChanged();
    }
}
