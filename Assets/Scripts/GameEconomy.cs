using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameEconomy
{
    private static int[] prices = { 100, 400, 400, 800, 1000, 100, 200, 300, 300, 500};

    public static int GetUnitBuyPrice(UnitType p_unitType)
    {
        return prices[(int)p_unitType];
    }
    public static int GetUnitSellPrice(UnitType p_unitType)
    {
        return prices[(int)p_unitType + 5];
    }
    public static bool HaveEnoughMoney(UnitType p_type, int p_money)
    {
        if (p_money >= prices[(int)p_type])
            return true;
        return false;
    }
}
