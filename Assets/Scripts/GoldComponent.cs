using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldComponent : MonoBehaviour
{
    public event Action<int> OnGoldUpdate;


    [SerializeField]
    int GoldAmount;
    

    internal bool CanPay(int skillCost)
    {
        if(GoldAmount<skillCost)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    internal void Pay(int skillCost)
    {
        GoldAmount -= skillCost;
        OnGoldUpdate(GoldAmount);
    }

    internal void AddGold(int gold)
    {
        GoldAmount += gold;
        OnGoldUpdate(GoldAmount);
    }
}
