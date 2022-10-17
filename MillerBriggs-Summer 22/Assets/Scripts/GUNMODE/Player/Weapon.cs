using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons")]
public class Weapon : ScriptableObject
{
    public GameObject bulletType;
    public float gunBulletDelay;
    public float bulletSpreadRange;
    public float maxAmmo;
    public int weaponProjectileNumber;

    public int currentMinDamage;
    public int currentMaxDamage;

    public int bulletID; 
}
