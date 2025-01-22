using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct Status 
{
    public int hp;
    public int hpMax;
    public int attack;
    public int defense;

    public Status(int hp, int hpMax, int attack, int defense)
    {
        this.hp = hp;  
        this.hpMax = hpMax;
        this.attack = attack;
        this.defense = defense;
    }
}
