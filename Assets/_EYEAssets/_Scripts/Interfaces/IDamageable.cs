using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    //Health Property
    int Health { get; set; }

    //Damage Method
    void Damage(int damageAmount);
}
