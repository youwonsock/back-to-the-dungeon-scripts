using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigun : Weapon
{
    [Header("SetOperatingTime")]
    [SerializeField] float overHeatingTime;
    [SerializeField] float idlingTime;
    [SerializeField] float coolingTime;

    public bool repeaterCheck;
    Color baseColor;
    [SerializeField] bool playerUseMinigun;

    PlayerShooter playerShooter;
    PlayerMovement playerMovement;
    PlayerInput playerInput;

    private void Awake()
    {
        baseColor = new Color(1,1,1);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    private float speedDifferences;

    private void OnDisable()
    {
        if (playerMovement && playerShooter)
        {
            playerMovement.ChangeBasicSpeed += this.speedDifferences;          //player? ?΄??? ? ?      
            //playerMovement.ChangeSpeed += 2.0f;          //player? ?΄??? ? ?
            playerUseMinigun = false;
        }
    }

    public override void Fire()
    {
        if (WeaponState == State.ReadyToFire && onFire == true && cur_Bullet > 0 && !isOverheating)
        {
            SetWeaponState();

            muzzleFlash.Play();
            if (audioSource)
                audioSource.PlayOneShot(fireSound);

            onFire = false;
            var tmp = Instantiate(bullet, Fireposition.transform.position, transform.rotation).GetComponent<Bullet>(); // μ΄μ ??
            tmp.SetBullet(this.damage, Convert_V3ctor(), max_distance, true, Bullet.Target.Enemy); // ?°λ―Έμ??, ?? , μ΅λ??κ±°λ¦¬ ?± ? ?¬

            if (repeaterCheck)
                Invoke("Returntoture", attack_speed); //?°?¬ λ¬΄κΈ° ?Ό? ??±?(μ£Όμ ?΄? ) λ°? attack_speed ?€? 

            cur_Bullet--;

            if (cur_Bullet <= 0)
                WeaponState = State.Empty;

        }
    }

#if LEGACY
    //?¬κΈ°μ AddWeapon or ChangeWeapon?Έμ§? κ²°μ ?? μ½λ ?½??? !
    private void GetMinigun(byte idx)
    {
        try
        {
            playerShooter.AddWeapon(this, idx);
            Debug.Log("add Weapon");
        }
        catch (System.InvalidOperationException)
        {
            playerShooter.ChangeWeapon(this, idx);
            Debug.Log("change Weapon");
        }
    }
#endif

    private void GetPlayerComponent()
    {
        GameObject player = FindObjectOfType<Hero>().gameObject;

        playerInput = player.GetComponent<PlayerInput>();
        playerMovement = player.GetComponent<PlayerMovement>();
        playerShooter = player.GetComponent<PlayerShooter>();

        float temp = playerMovement.ChangeBasicSpeed;
        playerMovement.ChangeBasicSpeed -= 2.0f;          //player? ?΄??? ? ?      
        this.speedDifferences = Mathf.Round(temp - playerMovement.ChangeBasicSpeed);
        //playerMovement.ChangeSpeed -= 2.0f;          //player? ?΄??? ? ?     
        playerUseMinigun = true;
    }

    float time;
    bool isOverheating = false;

    protected override void Update()
    {
        base.Update();

        if (!playerUseMinigun)
            GetPlayerComponent();

        if ( playerInput.Fire && !isOverheating)
        {
            time += Time.deltaTime;
            if (time > overHeatingTime)                                   //κ³Όμ΄ ?κ°?
            {
                sprite.color = new Color(1, 0, 0, 1);
                audioSource.PlayOneShot(reloadSound);                                       //reloadsoundλ₯? κ³Όμ΄?λ¦¬λ‘ ???μ²?!
                isOverheating = true;
            }
            else if (time > idlingTime)                              //κ³΅ν?  ?κ°?
            {
                onFire = true;
                sprite.color = Color.Lerp(baseColor, Color.red, (time-idlingTime)/(overHeatingTime-idlingTime));
            }
            else
            {
                onFire = false;
            }
        }
        else if(isOverheating)
        {
            time -= Time.deltaTime;
            sprite.color = Color.Lerp(Color.red, baseColor, (overHeatingTime - time) / (Mathf.Abs(coolingTime) + overHeatingTime));

            if (time < coolingTime)
            {
                isOverheating = false;
                sprite.color = new Color(1, 1, 1, 1);
                time = 0;
            }
        }
        else if(!playerInput.Fire)
        {
            onFire = false;
            //time -= Time.deltaTime * 0.3f;
            time -= Time.deltaTime * 2f;

            if (time < 0)
                time = 0;

            sprite.color = Color.Lerp(Color.red, baseColor, (overHeatingTime - time) / overHeatingTime);
        }

    }

    protected override void SetEmptyMagWeaponSprite()
    {
        SetWeaponSprite("Weapon/EmptyMagWeapon/MINIGUN");
    }

    protected override void SetLoadedWeaponSprite()
    {
        SetWeaponSprite("Weapon/LoadedWeapon/MINIGUN");
    }

    Vector3 Convert_V3ctor()
    {
        float cur_recoil = UnityEngine.Random.Range(-recoil, recoil); // λ°λκ°? ??€?Όλ‘? ??±
        Vector3 vector3 = (Vector3)(Fireposition.transform.right); // λ°©ν₯ κ΅¬ν¨
        return (Quaternion.Euler(0f, 0f, cur_recoil) * vector3).normalized; // λ°λ? ?°λ₯? ??  ? ? κ·ν
    }
}
