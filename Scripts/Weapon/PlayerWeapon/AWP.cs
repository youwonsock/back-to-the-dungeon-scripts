using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AWP : Weapon
{
    LineRenderer lineRenderer;

    [SerializeField] AudioClip boltSound;

    protected override void OnEnable()
    {
        base.OnEnable();

        Camera.main.orthographicSize += 1f;
        lineRenderer = GetComponent<LineRenderer>();

    }

    private void OnDisable()
    {
        Camera.main.orthographicSize -= 1f;
    }

    public override void Fire()
    {
        if (WeaponState == State.ReadyToFire && onFire == true && cur_Bullet > 0)
        {
            SetWeaponState();

            muzzleFlash.Play();
            if (audioSource)
                audioSource.PlayOneShot(fireSound);

            onFire = false;
            var tmp = Instantiate(bullet, Fireposition.transform.position, transform.rotation).GetComponent<Bullet>(); // μ΄μ ??
            tmp.SetBullet(this.damage, Fireposition.transform.right, max_distance, true, Bullet.Target.Enemy); // ?°λ―Έμ??, ?? , μ΅λ??κ±°λ¦¬ ?± ? ?¬

            cur_Bullet--;

            if (cur_Bullet <= 0)
                WeaponState = State.Empty;

            Invoke("PullBoltSound",0.2f);
        }
    }

    protected override void Update()
    {
        base.Update();

        RaycastHit2D hitInfo = Physics2D.Raycast(Fireposition.transform.position, Fireposition.transform.right, max_distance);
        lineRenderer.SetPosition(0, Fireposition.transform.position);

        if (hitInfo && hitInfo.transform.gameObject.layer != 0)
            lineRenderer.SetPosition(1, hitInfo.point);
        else
            lineRenderer.SetPosition(1, Fireposition.transform.position + Fireposition.transform.right * max_distance);
    }

    void PullBoltSound()
    {
        audioSource.PlayOneShot(boltSound);
    }

    protected override void SetEmptyMagWeaponSprite()
    {
        SetWeaponSprite("Weapon/EmptyMagWeapon/AWP");
    }

    protected override void SetLoadedWeaponSprite()
    {
        SetWeaponSprite("Weapon/EmptyMagWeapon/AWP");
    }


    Vector3 Convert_V3ctor()
    {
        float cur_recoil = UnityEngine.Random.Range(-recoil, recoil); // λ°λκ°? ??€?Όλ‘? ??±
        Vector3 vector3 = (Vector3)(Fireposition.transform.right); // λ°©ν₯ κ΅¬ν¨
        return (Quaternion.Euler(0f, 0f, cur_recoil) * vector3).normalized; // λ°λ? ?°λ₯? ??  ? ? κ·ν
    }
}
