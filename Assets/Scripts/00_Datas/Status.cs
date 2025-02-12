using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct Status 
{
    public int num;
    public int hp;
    public int hpMax;
    public int attack;

    public Status(int num,int hp, int hpMax, int attack)
    {
        this.num = num;
        this.hp = hp;  
        this.hpMax = hpMax;
        this.attack = attack;
    }
}
