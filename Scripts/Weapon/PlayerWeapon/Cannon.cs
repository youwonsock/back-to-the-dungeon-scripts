using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : Weapon
{

    public override void Fire()
    {
        if (WeaponState == State.ReadyToFire && onFire == true && cur_Bullet > 0)
        {
            SetWeaponState();

            muzzleFlash.Play();
            if (audioSource)
                audioSource.PlayOneShot(fireSound);

            onFire = false;
            var tmp = Instantiate(bullet, Fireposition.transform.position, transform.rotation).GetComponent<ExplosionBullet>(); // μ΄μ ??
            tmp.SetBullet(this.damage, Fireposition.transform.right, max_distance, false, Bullet.Target.Enemy); // ?°λ―Έμ??, ?? , μ΅λ??κ±°λ¦¬ ?± ? ?¬

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

}
