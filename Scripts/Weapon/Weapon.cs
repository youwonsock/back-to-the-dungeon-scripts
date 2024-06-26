using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class Weapon : MonoBehaviour
{

    [Header("WeaponInfo")]
    [SerializeField] protected int damage;
    [SerializeField] [Space(10f)] protected float attack_speed;
    [SerializeField] protected float recoil;
    [SerializeField] protected float reload_time;
    [SerializeField] protected float swap_time;
    [SerializeField] protected float max_distance;
    [SerializeField] [Space(10f)] protected int max_Bullet;
    [SerializeField] protected int cur_Bullet;
    protected SpriteRenderer sprite;

    [Header("WeaponSource")]
    [SerializeField] protected AudioClip fireSound;
    [SerializeField] protected AudioClip reloadSound;
    protected AudioSource audioSource;
    protected GameObject bullet;
    public GameObject Fireposition;

    protected ParticleSystem muzzleFlash;
    private float lastAttackTime;
    private bool isFire;
    private bool isReload;

    /// <summary>
    /// λ°μ¬ μ²΄ν¬ ?¨?
    /// λ§μ°?€λ₯? ?΄λ¦??κ³? ?΄λ¦??΄? ?κΈ? ? κΉμ?? falseκ°? ?¨
    /// </summary>
    [SerializeField] public bool onFire = true;
    protected Transform Transform;

    public float Swap_time
    {
        get { return swap_time; }
    }

    /// <summary>
    /// PlayerInput? Fire? κ°μ λ°ν?? ?λ‘νΌ?°
    /// </summary>
    public bool IsFire
    {
        get { return isFire; }
        set { isFire = value; }
    }

    public float Reload_time
    {
        get { return reload_time; }
        set { reload_time = value; }
    }
    public bool IsReload { set; get; }

    public State WeaponState { get; protected set; }

    public enum State
    {
        ReadyToFire,
        OnFire,
        Empty,
        Reloading
    }

    protected virtual void OnEnable()
    {
        WeaponState = State.ReadyToFire;
    }

    //λ°λ
    // Start is called before the first frame update
    protected virtual void  Start()
    {
        Transform = GetComponent<Transform>();
        muzzleFlash = transform.Find("Fire Position").GetChild(0).GetComponent<ParticleSystem>();
        sprite = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        cur_Bullet = max_Bullet;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //μ΄μ?λ₯? λ³΄μ¬μ£Όλ UI ?€? 
        UIManager.Instance.SetBulletUI(cur_Bullet, max_Bullet);

        if (cur_Bullet == 0)
            Reload();
    }

    /// <summary>
    /// λ°μ¬ ?¨? κ°? λ¬΄κΈ°? ?Ή?±? λ§κ² ??±?λ©? ?¨
    /// instantiateλ₯? ?¬?©??¬ μ΄μ? ???? ??Όλ‘? κ΅¬ν
    /// ?? μ½λ? Pistol.cs μ°Έκ³ 
    /// </summary>
    public abstract void Fire();

    protected void SetWeaponState()
    {
        WeaponState = State.OnFire;

        CancelInvoke(nameof(SetWeaponStateToReady));
        Invoke("SetWeaponStateToReady",attack_speed);
    }

    void SetWeaponStateToReady()
    {

        if (WeaponState == State.Reloading)
            return;
        WeaponState = State.ReadyToFire;
    }


    //??¬ ?¬?©?μ§? ?? λ©μ?
    public virtual void TempFire()
    {
        if (WeaponState == State.ReadyToFire)
        {
            WeaponState = State.OnFire;
        }
        else if (WeaponState == State.OnFire && Time.time >= lastAttackTime + attack_speed)
        {
            WeaponState = State.ReadyToFire;
            lastAttackTime = Time.time;
        }
    }


    /// <summary>
    /// onFire λ³?? ?? λ³?? ?¨?
    /// ?΄λ¦? κ°μ???©
    /// ?΄λ¦?? ?κ³? ??Όλ©? Falseλ₯? λ°ν
    /// ?΄λ¦?? ?μ§? ?κ³? ??Όλ©? Trueλ₯? λ°ν?¨
    /// </summary>
    public void ChangeState()
    {
        onFire = !GetMouseInput();
    }

    /// <summary>
    /// λ§μ°?€ ?Έ? λ¦¬ν΄ λ©μ?
    /// </summary>
    /// <returns>λ§μ°?€ ?Έ?</returns>
    public bool GetMouseInput()
    {
        return IsFire;
    }

    /// <summary>
    /// ?₯? ?¨?
    /// κ°? λ¬΄κΈ°λ§λ€ ?€? ? ?₯? ?μΉ? ?΄??
    /// ?₯? ?΄ ?λ£λκ²λ λ§λ€?΄? Έ??
    /// </summary>
    public virtual void Reload()
    {
        if (WeaponState == State.Reloading || cur_Bullet >= max_Bullet)
            return;

        IsReload = true;
    
        if(audioSource)
            audioSource.PlayOneShot(reloadSound);

        SetEmptyMagWeaponSprite();
        StartCoroutine(IEReload());
    }
    protected IEnumerator IEReload()
    {
        WeaponState = State.Reloading;

        yield return new WaitForSeconds(reload_time);

        SetLoadedWeaponSprite();
        IsReload = false;
        cur_Bullet = max_Bullet;

        WeaponState = State.ReadyToFire;
    }

    protected abstract void SetLoadedWeaponSprite();

    protected abstract void SetEmptyMagWeaponSprite();

    protected void SetWeaponSprite(string path)
    {
        sprite.sprite = Resources.Load<Sprite>(path);
    }

    /// <summary>
    /// λ§μ°?€ ?΄λ¦? μ€μΌ ? onFireλ₯? ?€? Trueλ‘? λ§λ€?΄μ£Όλ ?¨?
    /// ?°?¬λ¬΄κΈ° κ΅¬ν ? Invoke??? ?¨κ»? ?¬?©?λ©? ? ?©
    /// </summary>
    private void Returntoture()
    {
        onFire = true;
    }


    public Vector2 Difference()
    {
        return transform.position - Fireposition.transform.position;
    }
}
