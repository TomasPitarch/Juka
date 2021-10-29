using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldComponent : MonoBehaviour
{
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
    }

    
}
