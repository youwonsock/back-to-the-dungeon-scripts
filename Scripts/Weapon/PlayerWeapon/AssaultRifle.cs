using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : Weapon
{

    public bool repeaterCheck;

    public override void Fire()
    {
        if (WeaponState == State.ReadyToFire && onFire == true && cur_Bullet > 0)
        {
            SetWeaponState();

            muzzleFlash.Play();
            if(audioSource)
                audioSource.PlayOneShot(fireSound);
            
            onFire = false;
            var tmp = Instantiate(bullet, Fireposition.transform.position, transform.rotation).GetComponent<Bullet>(); // Ï¥ùÏïå ?Üå?ôò
            tmp.SetBullet(this.damage, Convert_V3ctor(), max_distance, true,Bullet.Target.Enemy); // ?ç∞ÎØ∏Ï??, ?öå?†Ñ, ÏµúÎ??Í±∞Î¶¨ ?ì± ?†Ñ?ã¨

            if (repeaterCheck)
                Invoke("Returntoture", attack_speed); //?ó∞?Ç¨ Î¨¥Í∏∞ ?ùº?ïå ?ôú?Ñ±?ôî(Ï£ºÏÑù ?ï¥?†ú) Î∞? attack_speed ?Ñ§?†ï

            cur_Bullet--;

            if (cur_Bullet <= 0)
                WeaponState = State.Empty;
        }
    }

    protected override void SetEmptyMagWeaponSprite()
    {
        SetWeaponSprite("Weapon/EmptyMagWeapon/AK47");
    }

    protected override void SetLoadedWeaponSprite()
    {
        SetWeaponSprite("Weapon/LoadedWeapon/AK47");
    }

    Vector3 Convert_V3ctor()
    {
        float cur_recoil = UnityEngine.Random.Range(-recoil, recoil); // Î∞òÎèôÍ∞? ?ûú?ç§?úºÎ°? ?Éù?Ñ±
        Vector3 vector3 = (Vector3)(Fireposition.transform.right); // Î∞©Ìñ• Íµ¨Ìï®
        return (Quaternion.Euler(0f, 0f, cur_recoil) * vector3).normalized; // Î∞òÎèô?óê ?î∞Î•? ?öå?†Ñ ?õÑ ?†ïÍ∑úÌôî
    }
}
