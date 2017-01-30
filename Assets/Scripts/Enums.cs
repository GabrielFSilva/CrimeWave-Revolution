using UnityEngine;
using System.Collections;

public enum GameState
{
    PLAYING,
    PAUSED,
    BUYING,
    EDITING,
    END_GAME
}
public enum UnitType
{
    POLICE_CAMERA,
    POLICE_MAN,
    POLICE_STATION,
    POLICE_CAR,
    SCHOOL
}
public enum TileType
{
    STREET,
    BUILDING
}
public enum UnitTilePlacement
{
    STREET,
    BUILDING,
    STREET_OR_BUILDING
}
public enum UnitViewType
{
    LINEAR,
    AREA
}
public enum Orientation
{
    UP,
    RIGHT,
    DOWN,
    LEFT
}
public class Enums
{
    public static int GetOrientationX (Orientation p_ori)
    {
        if (p_ori == Orientation.LEFT)
            return -1;
        else if (p_ori == Orientation.RIGHT)
            return 1;
        return 0;
    }
    public static int GetOrientationY(Orientation p_ori)
    {
        if (p_ori == Orientation.UP)
            return -1;
        else if (p_ori == Orientation.DOWN)
            return 1;
        return 0;
    }
    public static bool IsTurningRight(Orientation p_startOri, Orientation p_endOri)
    {
        if (p_startOri == Orientation.LEFT && p_endOri == Orientation.UP)
            return true;
        else if (p_startOri + 1 == p_endOri)
            return true;
        return false;
    }
}
