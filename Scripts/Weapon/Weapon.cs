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
    /// 발사 체크 ?��?��
    /// 마우?���? ?���??���? ?���??��?��?���? ?��까�?? false�? ?��
    /// </summary>
    [SerializeField] public bool onFire = true;
    protected Transform Transform;

    public float Swap_time
    {
        get { return swap_time; }
    }

    /// <summary>
    /// PlayerInput?�� Fire?�� 값을 반환?��?�� ?��로퍼?��
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

    //반동
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
        //총알?���? 보여주는 UI ?��?��
        UIManager.Instance.SetBulletUI(cur_Bullet, max_Bullet);

        if (cur_Bullet == 0)
            Reload();
    }

    /// <summary>
    /// 발사 ?��?�� �? 무기?�� ?��?��?�� 맞게 ?��?��?���? ?��
    /// instantiate�? ?��?��?��?�� 총알?�� ?��?��?��?�� ?��?���? 구현
    /// ?��?�� 코드?�� Pistol.cs 참고
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


    //?��?�� ?��?��?���? ?��?�� 메서?��
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
    /// onFire �??�� ?��?�� �??�� ?��?��
    /// ?���? 감�???��
    /// ?���??�� ?���? ?��?���? False�? 반환
    /// ?���??�� ?���? ?���? ?��?���? True�? 반환?��
    /// </summary>
    public void ChangeState()
    {
        onFire = !GetMouseInput();
    }

    /// <summary>
    /// 마우?�� ?��?�� 리턴 메소?��
    /// </summary>
    /// <returns>마우?�� ?��?��</returns>
    public bool GetMouseInput()
    {
        return IsFire;
    }

    /// <summary>
    /// ?��?��?��?��
    /// �? 무기마다 ?��?��?�� ?��?��?���? ?��?��?��
    /// ?��?��?�� ?��료되게끔 만들?��?��?��?��
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
    /// 마우?�� ?���? 중일 ?�� onFire�? ?��?�� True�? 만들?��주는 ?��?��
    /// ?��?��무기 구현 ?�� Invoke??? ?���? ?��?��?���? ?��?��
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
