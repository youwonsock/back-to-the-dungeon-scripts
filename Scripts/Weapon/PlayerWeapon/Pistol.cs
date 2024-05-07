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
            var tmp = Instantiate(bullet, Fireposition.transform.position, transform.rotation).GetComponent<Bullet>(); // Ï¥ùÏïå ?Üå?ôò
            tmp.SetBullet(this.damage, Convert_V3ctor(), max_distance, true,Bullet.Target.Enemy); // ?ç∞ÎØ∏Ï??, ?öå?†Ñ, ÏµúÎ??Í±∞Î¶¨ ?ì± ?†Ñ?ã¨

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
        float cur_recoil = UnityEngine.Random.Range(-recoil, recoil); // Î∞òÎèôÍ∞? ?ûú?ç§?úºÎ°? ?Éù?Ñ±
        Vector3 vector3 = (Vector3)(Fireposition.transform.right); // Î∞©Ìñ• Íµ¨Ìï®
        return (Quaternion.Euler(0f, 0f, cur_recoil) * vector3).normalized; // Î∞òÎèô?óê ?î∞Î•? ?öå?†Ñ ?õÑ ?†ïÍ∑úÌôî
    }
}
