using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  Effect : MonoSingleton<Player>
{
    public float healthPoint;
    public int postion;
    public abstract void Attack(int _amount);
    public abstract void Move(int _step);
}
