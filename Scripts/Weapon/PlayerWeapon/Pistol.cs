using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{

    public override void Fire()
    {
        if(WeaponState == State.ReadyToFire && onFire == true && cur_Bullet > 0)
        {
            SetWeaponState();

            muzzleFlash.Play();
            if (audioSource)
                audioSource.PlayOneShot(fireSound);

            onFire = false;
            var tmp = Instantiate(bullet, Fireposition.transform.position, transform.rotation).GetComponent<Bullet>(); // μ΄μ ??
            tmp.SetBullet(this.damage, Convert_V3ctor(), max_distance, true,Bullet.Target.Enemy); // ?°λ―Έμ??, ?? , μ΅λ??κ±°λ¦¬ ?± ? ?¬

            cur_Bullet--;

            if (cur_Bullet <= 0)
                WeaponState = State.Empty;
        }
    }

    protected override void SetEmptyMagWeaponSprite()
    {
    }

    protected override void SetLoadedWeaponSprite()
    {
    }


    Vector3 Convert_V3ctor()
    {
        float cur_recoil = UnityEngine.Random.Range(-recoil, recoil); // λ°λκ°? ??€?Όλ‘? ??±
        Vector3 vector3 = (Vector3)(Fireposition.transform.right); // λ°©ν₯ κ΅¬ν¨
        return (Quaternion.Euler(0f, 0f, cur_recoil) * vector3).normalized; // λ°λ? ?°λ₯? ??  ? ? κ·ν
    }
}
