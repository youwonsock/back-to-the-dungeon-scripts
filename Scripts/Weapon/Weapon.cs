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
    /// ë°œì‚¬ ì²´í¬ ?•¨?ˆ˜
    /// ë§ˆìš°?Š¤ë¥? ?´ë¦??•˜ê³? ?´ë¦??•´? œ?•˜ê¸? ? „ê¹Œì?? falseê°? ?¨
    /// </summary>
    [SerializeField] public bool onFire = true;
    protected Transform Transform;

    public float Swap_time
    {
        get { return swap_time; }
    }

    /// <summary>
    /// PlayerInput?˜ Fire?˜ ê°’ì„ ë°˜í™˜?•˜?Š” ?”„ë¡œí¼?‹°
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

    //ë°˜ë™
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
        //ì´ì•Œ?ˆ˜ë¥? ë³´ì—¬ì£¼ëŠ” UI ?„¤? •
        UIManager.Instance.SetBulletUI(cur_Bullet, max_Bullet);

        if (cur_Bullet == 0)
            Reload();
    }

    /// <summary>
    /// ë°œì‚¬ ?•¨?ˆ˜ ê°? ë¬´ê¸°?˜ ?Š¹?„±?— ë§ê²Œ ?‘?„±?•˜ë©? ?¨
    /// instantiateë¥? ?‚¬?š©?•˜?—¬ ì´ì•Œ?„ ?†Œ?™˜?•˜?Š” ?‹?œ¼ë¡? êµ¬í˜„
    /// ?˜ˆ?‹œ ì½”ë“œ?Š” Pistol.cs ì°¸ê³ 
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


    //?˜„?¬ ?‚¬?š©?˜ì§? ?•Š?Š” ë©”ì„œ?“œ
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
    /// onFire ë³??ˆ˜ ?ƒ?ƒœ ë³??™˜ ?•¨?ˆ˜
    /// ?´ë¦? ê°ì???š©
    /// ?´ë¦??„ ?•˜ê³? ?ˆ?œ¼ë©? Falseë¥? ë°˜í™˜
    /// ?´ë¦??„ ?•˜ì§? ?•Šê³? ?ˆ?œ¼ë©? Trueë¥? ë°˜í™˜?•¨
    /// </summary>
    public void ChangeState()
    {
        onFire = !GetMouseInput();
    }

    /// <summary>
    /// ë§ˆìš°?Š¤ ?¸?’‹ ë¦¬í„´ ë©”ì†Œ?“œ
    /// </summary>
    /// <returns>ë§ˆìš°?Š¤ ?¸?’‹</returns>
    public bool GetMouseInput()
    {
        return IsFire;
    }

    /// <summary>
    /// ?¥? „?•¨?ˆ˜
    /// ê°? ë¬´ê¸°ë§ˆë‹¤ ?„¤? ˆ?œ ?¥? „?‹œì¹? ?´?›„?—
    /// ?¥? „?´ ?™„ë£Œë˜ê²Œë” ë§Œë“¤?–´? ¸?ˆ?Œ
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
    /// ë§ˆìš°?Š¤ ?´ë¦? ì¤‘ì¼ ?•Œ onFireë¥? ?‹¤?‹œ Trueë¡? ë§Œë“¤?–´ì£¼ëŠ” ?•¨?ˆ˜
    /// ?—°?‚¬ë¬´ê¸° êµ¬í˜„ ?•Œ Invoke??? ?•¨ê»? ?‚¬?š©?•˜ë©? ?œ ?š©
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
