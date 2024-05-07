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
                var tmp = Instantiate(bullet, Fireposition.transform.position, transform.rotation).GetComponent<Bullet>(); // 총알 ?��?��
                tmp.SetBullet(this.damage, Convert_V3ctor(z + (30 - i * 8)), max_distance, true, Bullet.Target.Enemy); // ?��미�??, ?��?��, 최�??거리 ?�� ?��?��
            }
            if (repeaterCheck)
                Invoke("Returntoture", attack_speed); //?��?�� 무기 ?��?�� ?��?��?��(주석 ?��?��) �? attack_speed ?��?��

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
        Vector3 vector3 = (Vector3)(Fireposition.transform.right); // 방향 구함
        return (Quaternion.Euler(0f, 0f, f) * vector3).normalized; // ?��?��?�� ?��?���? 각도�? 발사
    }

}
