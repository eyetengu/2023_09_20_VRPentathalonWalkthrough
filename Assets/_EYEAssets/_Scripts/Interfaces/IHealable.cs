using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealable
{
    //Health Property
    int Health { get; set; }

    //Healing Method
    void Heal(int health);
}
