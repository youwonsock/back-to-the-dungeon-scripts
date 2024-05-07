using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoShotGun : Weapon
{
    public bool repeaterCheck;

    private int bulletshot = 7;

    public override void Fire()
    {
        if (WeaponState == State.ReadyToFire && onFire == true && cur_Bullet > 0)
        {
            SetWeaponState();

            muzzleFlash.Play();
            if (audioSource)
                audioSource.PlayOneShot(fireSound);

            onFire = false;
            float z = Fireposition.transform.rotation.z;

            for (int i = 0; i < bulletshot; i++)
            {
                var tmp = Instantiate(bullet, Fireposition.transform.position, transform.rotation).GetComponent<Bullet>(); // Ï¥ùÏïå ?Üå?ôò
                tmp.SetBullet(this.damage, Convert_V3ctor(z + (30 - i * 8)), max_distance, true, Bullet.Target.Enemy); // ?ç∞ÎØ∏Ï??, ?öå?†Ñ, ÏµúÎ??Í±∞Î¶¨ ?ì± ?†Ñ?ã¨
            }
            if (repeaterCheck)
                Invoke("Returntoture", attack_speed); //?ó∞?Ç¨ Î¨¥Í∏∞ ?ùº?ïå ?ôú?Ñ±?ôî(Ï£ºÏÑù ?ï¥?†ú) Î∞? attack_speed ?Ñ§?†ï

            cur_Bullet--;

            if (cur_Bullet <= 0)
                WeaponState = State.Empty;

        }
    }


    protected override void SetEmptyMagWeaponSprite()
    {
        SetWeaponSprite("Weapon/EmptyMagWeapon/SAIGA");
    }

    protected override void SetLoadedWeaponSprite()
    {
        SetWeaponSprite("Weapon/LoadedWeapon/SAIGA");
    }


    Vector3 Convert_V3ctor(float f)
    {
        Vector3 vector3 = (Vector3)(Fireposition.transform.right); // Î∞©Ìñ• Íµ¨Ìï®
        return (Quaternion.Euler(0f, 0f, f) * vector3).normalized; // ?ÉÑ?ôò?ù¥ ?Ç†?ïÑÍ∞? Í∞ÅÎèÑÎ°? Î∞úÏÇ¨
    }

}
