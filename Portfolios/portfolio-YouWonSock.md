# Back to the Dungeon

## Developer Info
* 이름 : 유원석(You Won Sock)
* GitHub : https://github.com/youwonsock
* Mail : qazwsx233434@gmail.com

## Our Game
### Game trailer - Youtube

[![Back to The Dungeon Trailer](https://img.youtube.com/vi/hy_my0OQddc/0.jpg)](https://www.youtube.com/watch?v=hy_my0OQddc) 

### Downloads

* [itch.io](https://devslem.itch.io/back-to-the-dungeon)

### Genres

2D platformer shooting

<b><h2>Platforms</h2></b>

<p>
<img src="https://upload.wikimedia.org/wikipedia/commons/c/c7/Windows_logo_-_2012.png" height="30">
</p>

### Development kits

<p>
<img src="https://upload.wikimedia.org/wikipedia/commons/thumb/1/19/Unity_Technologies_logo.svg/1280px-Unity_Technologies_logo.svg.png" height="40">
</p>

<b><h2>Periods</h2></b>

* 2021-09 ~ 2022-03 (about 6 months) - main development and build for the pc version

## Contribution

### Enemy
* #### Entities
  * Astronaut
    * SelfExplosion
    * Bomb
      
    ![is](https://github.com/youwonsock/back-to-the-dungeon-scripts/assets/46276141/af955e55-ad8b-49af-8d2a-b58260acd575)
    자폭 몬스터로 플레이어 발견 시 빠른 속도로 접근합니다.  
    EnemyDetection에서 Target을 감지하고 있는경우 사망 시 그 자리에 폭탄을 생성하며,  
    감지한 Target이 없는 경우 폭탄을 생성하지 않습니다.
      
  * Beez
    * SpreadSkill  
      
    ![bee](https://github.com/youwonsock/back-to-the-dungeon-scripts/assets/46276141/6681e588-0dc5-4f4e-a7e6-c83a2db97445)
    비행 몬스터로 크기가 작으며 낮은 체력을 가지고 있습니다.  
    기본 원거리 공격과 SpreadSkill을 사용합니다.
    
  * Squirrel
    * DashSkill.cs - class
  
    ![다람이](https://github.com/youwonsock/back-to-the-dungeon-scripts/assets/46276141/f1ac8c76-9fa4-4217-ae65-296959d63bb6)
    돌진 스킬을 사용하는 몬스터입니다.  
    돌진 패턴만을 가지고 있으며 높은 데미지를 줍니다.  
    인스팩터에서 bool값을 이용하여 돌진 시 낭떠러지에서 떨어질지 반대로 돌진할지 설정해줄 수 있습니다.
    
* #### ETC
  * Enemy Health Bar
    ![Spread](https://github.com/youwonsock/back-to-the-dungeon-scripts/assets/46276141/d6353317-ddb8-4127-be28-91e6eeaca546)
    일반 몬스터들의 체력을 표시해주는 HealthBar입니다.

  * Boss Health Bar
    ![BossHealth-min](https://github.com/youwonsock/back-to-the-dungeon-scripts/assets/46276141/5b955525-8ff5-433c-bb73-37ad0605d0b6)
    특수 몬스터의 체력을 표시해주는 HealthBar입니다.

### Enemy Skills
* #### Boss Skill
  * BossSpreadSkill.cs - class
  
    ![Spread](https://github.com/youwonsock/back-to-the-dungeon-scripts/assets/46276141/2a434c60-2f95-43e2-851b-e1009d5b7356)
    SpreadSkill의 강화형으로 4방향으로 발사하면서 회전합니다.
    
### Item
* #### Field Item
  * Coin
    ![coin](https://github.com/youwonsock/back-to-the-dungeon-scripts/assets/46276141/35c17f3a-fcda-4e80-8e3e-43a7439da265)  
    
  * HealPotion
    ![potion](https://github.com/youwonsock/back-to-the-dungeon-scripts/assets/46276141/5b80709d-9164-4440-92e2-0311a757bf1a)  
    
  * Invincible
    ![invin](https://github.com/youwonsock/back-to-the-dungeon-scripts/assets/46276141/5db34b85-9067-47ca-9979-d1d431f310ef)  
    
* #### Store Item
  * IncreaseMaxHealth
    ![max health](https://github.com/youwonsock/back-to-the-dungeon-scripts/assets/46276141/0af8e66f-14c3-43fc-828e-a7271f78e617)  

  * IncreaseStamina
    ![max Ste](https://github.com/youwonsock/back-to-the-dungeon-scripts/assets/46276141/38aae36f-9eaf-47f4-8370-1f9b42ce6641)  

  * Resurrection
    ![re](https://github.com/youwonsock/back-to-the-dungeon-scripts/assets/46276141/ef0f4ed8-3001-4e64-a24b-84ae5f0bf6a7)

### Manager
* ETC
  * ItemManager
    아이템 생성을 위한 클래스로 Stage내에 존재하는 몬스터들의 OnDeath 이벤트에  
    확률에 따라 아이템을 생성하는 메서드 OnEnemyDeath를 등록합니다. 
    <details>
    <summary>ItemManager Code</summary>
    <div markdown="1">

      ```c#
      using System.Collections.Generic;
      using System.Linq;
      using UnityEngine;

      public class ItemManager : MonoBehaviour
      {
          //[SerializeField] private SerializableDictionary<Item, float> items = new SerializableDictionary<Item, float>();
          [SerializeField] private List<ItemProperty> items = new List<ItemProperty>();

      #if LEGACY
          [SerializeField] float coinPercent;
          [SerializeField] float potionPercent;
          [SerializeField] float InvinciblePercent;
          [SerializeField] float nonePercent;
      #endif
          private float sum;

          [System.Serializable]
          private struct ItemProperty
          {
              public Item itemPrefab;
              public float weight;
              public SceneData<Item> data;
          }

          private void Awake()
          {
      #if LEGACY
              float sum = coinPercent + potionPercent + nonePercent + InvinciblePercent;
              coinPercent = coinPercent / sum * 100;
              potionPercent = potionPercent / sum * 100;
              InvinciblePercent = InvinciblePercent / sum * 100;
              nonePercent = nonePercent / sum * 100;
      #endif
              this.sum = items.Sum(i => i.weight);

              var enemies = FindObjectsOfType<Enemy>();
              foreach (var enemy in enemies)
              {
                  enemy.OnDeath += () => OnEnemyDeath(enemy);
              }
          }

          private void OnEnemyDeath(Enemy enemy)
          {
              float prob = UnityEngine.Random.Range(0f, 1f);
              float current_prob = 0f;
              foreach (var item in items)
              {
                  current_prob += item.weight / this.sum;
                  if (prob <= current_prob)
                  {
                      if (!(item.itemPrefab is null))
                      {
                          var instantiated = Instantiate(item.itemPrefab, enemy.transform.position, Quaternion.identity);
                          try
                          {
                              item.data.TrySetValue(instantiated);
                          }
                          catch { }
                      }
                      return;
                  }
              }
          }


      #if LEGACY
          void Start()
          {
              GameObject[] temp = GameObject.FindGameObjectsWithTag("Enemy");
              GameObject[] prefebs = Resources.LoadAll<GameObject>("Item/");


              foreach (var i in temp)
              {
                  if (i.GetComponent<Entity>() != null)
                  {
                      i.GetComponent<Entity>().OnDeath +=
                          delegate ()
                          {
                              int n = Random.Range(0, (int)(coinPercent + potionPercent + InvinciblePercent + nonePercent));

                              if (n < coinPercent)
                                  Instantiate(prefebs[0], i.transform.position, Quaternion.identity);
                              else if (n < (coinPercent + potionPercent))
                                  Instantiate(prefebs[2], i.transform.position , Quaternion.identity);
                              else if (n < (coinPercent + potionPercent + InvinciblePercent))
                                  Instantiate(prefebs[1], i.transform.position, Quaternion.identity);
                          };
                  }
              }
          }
      #endif
      }
      ```
      
    </div>
    </details>
  </br>
  
  * UIManager
    UI를 관리하는 Singleton오브젝트로 PlayerUI, GameObjectUI, PauseUI등의 Ui처리를 위한 매니저입니다.
    <details>
    <summary>UIManager Code</summary>
    <div markdown="1">

      ```c#
      using System;
      using System.Collections.Generic;
      using System.Linq;
      using UnityEngine;
      using UnityEngine.SceneManagement;
      using UnityEngine.UI;
      using TMPro;

      public class UIManager : Singleton<UIManager>
      {

          [Header("Text UI")]
          [SerializeField] Text UIStageText;
          [SerializeField] Text UICoinText;
          [SerializeField] Text UIBulletText;
          [SerializeField] Text UIHpText;
          [SerializeField] Text UIRemainEnemyText;
          [SerializeField] Text UIRemainLife;

          [Header("Image UI")]
          [SerializeField] GameObject resurrectionImage;

          [Header("Other GameObject UI")]
          [SerializeField] Store UIStore;
          [SerializeField] RecordBoard UIRecordBoard;
          [SerializeField] GameObject UIGameOver;
          [SerializeField] SettingButtonUI settingButton;

          [Header("Object UI")]
          [SerializeField] GameObject UIPause;
          [SerializeField] GameObject UISetting;
          [SerializeField] Slider UIHpBar;
          [SerializeField] Slider UIStaminaBar;
          [SerializeField] Slider UIBossHpBar;
          [SerializeField] Graphic weaponPanel;
          [SerializeField] Image weaponImagePrefab;
          [SerializeField] float weaponImagePlacementInterval = 200;
          [SerializeField] Slider soundSlider;
          [SerializeField] GameObject StartSceneUI;

          private bool isEventActive;
          private bool isSetting = false;
          private bool isOtherUIActive = false;

          private List<Type> weaponTypes = new List<Type>();
          private List<Image> images = new List<Image>();
          private byte currentWeaponIdx = byte.MaxValue;
          
          protected UIManager() { }

          //restart 버튼 클릭시 실행되는 이벤트
          public event Action OnClickRestartButton;

          //일시정지 활성화시 실행되는 이벤트
          public event Action ActivatePause;

          //일시정지 비활성화시 실행되는 이벤트
          public event Action InactivatePause;

          //setting 버튼 클릭시 다른 버튼 제어 프로퍼티
          public bool IsSettingActive { get { return isSetting; } set { isSetting = value; } }

          public bool IsOtherUIActive { get { return isOtherUIActive; } set { isOtherUIActive = value; } }

          private void Awake()
          {
              if (!CheckSingletonInstance(true))
                  return;

              UIPause.SetActive(false);
              UIGameOver.SetActive(false);
              UIBossHpBar.gameObject.SetActive(false);

              GameManager.Instance.OnSceneLoaded += OnSceneLoaded;
              //SettingDataController.OnSettingDataLoaded += SetSound;

              GetUIGameObject();
          }

          // start Scene가 아닌 다른 곳에서 자동 설정
          private void Start()
          {
              if (SceneManager.GetActiveScene().buildIndex != 0)
              {
                  StartSceneUI.SetActive(false);

                  SetPlayerUI(true);
                  //this.transform.Find("Player UI").gameObject.SetActive(true);
              }
              isEventActive = false;

              SetSound(SettingDataController.Data);
          }

          private void OnSceneLoaded(Scene? previous, Scene? loaded, SceneLoadingTiming when)
          {
              switch (when)
              {
                  case SceneLoadingTiming.BeforeLoading:
                      StartSceneUI.SetActive(false);
                      break;
                  case SceneLoadingTiming.AfterLoading:
                      if (loaded.Value.buildIndex == GameManager.Instance.InGameStartSceneBuildIndex)
                      {
                          //this.transform.Find("Player UI").gameObject.SetActive(true);
                          SetPlayerUI(true);
                          GetUIGameObject();
                      }
                      UIStageText.text = SceneManager.GetActiveScene().name;
                      break;

              }
          }

          Stack<GameObject> stack = new Stack<GameObject>();

          private void Update()
          {
              if (SceneManager.GetActiveScene().buildIndex == 0 || isOtherUIActive)
                  return;

              if (stack.Count == 0 && Input.GetButtonDown("pause button"))
              {
                  UIPause.SetActive(true);
                  stack.Push(UIPause);
              }
              else if (Input.GetButtonDown("pause button"))
              {
                  stack.Pop().SetActive(false);
                  try
                  {
                      stack.Peek().SetActive(true);
                  }
                  catch (Exception ex){ }
                  finally
                  {
                      SettingDataController.SaveSettingData();
                  }
              }

              #region 기존 Pause logic
              //if (stack.Count == 0 && Input.GetButtonDown("pause button"))
              //{
              //    if (UIStore != null && UIRecordBoard != null)
              //    {
              //        if (UIStore.IsUIActive || UIRecordBoard.IsUIActive)
              //        {
              //            return;
              //        }
              //    }
              //    if (UIGameOver.activeSelf || isEventActive || UISetting.activeSelf)
              //        return;

              //    if (ActivatePause != null)
              //        ActivatePause();

              //    stack.Push(UIPause);
              //    UIPause.SetActive(true);

              //}
              //else if (stack.Count == 1 && Input.GetButtonDown("pause button"))
              //{
              //    if (stack.Peek().GetComponent<IUiActiveCheck>() != null)
              //    {
              //        stack.Pop();
              //        return;
              //    }

              //    stack.Pop().SetActive(false);

              //    if (InactivatePause != null)
              //        InactivatePause();
              //}
              //else if (stack.Count >= 2 && Input.GetButtonDown("pause button"))
              //{
              //    if (stack.Peek().GetComponent<IUiActiveCheck>() != null)
              //    {
              //        stack.Pop();
              //        return;
              //    }
              //    stack.Pop().SetActive(false);
              //    stack.Peek().SetActive(true);
              //    AudioListener.volume = soundSlider.value;
              //}
              #endregion

              //set timeScale
              if (UIPause.activeSelf)
              {
                  Time.timeScale = 0;
              }
              else
              {
                  Time.timeScale = 1;
              }
          }

          void SetSound(SettingData data)
          {
              // 현재 setting file이 없더라도 Setting에서 Data를 가져와 1로 초기화 시킴
              if (data == null)   //사용 x
              {
                  AudioListener.volume = 0.3f;
                  soundSlider.value = 0.3f;
              }
              else
              {
                  AudioListener.volume = data.audioVolume;
                  soundSlider.value = data.audioVolume;
              }
          }

          private void GetUIGameObject()
          {
              UIStore = FindObjectOfType<Store>();
              UIRecordBoard = FindObjectOfType<RecordBoard>();

          }

          public void SetPlayerUI(bool value)
          {
              transform.Find("Player UI").gameObject.SetActive(value);
          }
          
          /// <summary>
          /// StageText UI를 설정해주는 메서드
          /// </summary>
          /// <param name="text"></param>
          public void SetStageText(string text)
          {
              UIStageText.text = text;
          }

          /// <summary>
          /// 게임 재시작 버튼 활성화 메서드
          /// </summary>
          public void SetActiveRestartButton()
          {
              UIGameOver.SetActive(true);
          }

          /// <summary>
          /// 게임 재시작 메서드
          /// </summary>
          public void Restart()
          {
              OnClickRestartButton();

              UIGameOver.SetActive(false);
              UIPause.SetActive(false);
              Time.timeScale = 1;
          }

          /// <summary>
          /// Left Enemy Text UI를 설정해주는 메서드
          /// </summary>
          /// <param name="count"></param>
          public void SetRemainEnemyUI(int count)
          {
              if (!UIRemainEnemyText.enabled)
                  return;

              UIRemainEnemyText.text = "Enemy : " + count.ToString();
          }

          /// <summary>
          /// 남은 목숨 표시 UI
          /// </summary>
          /// <param name="count"></param>
          public void SetRemainLife(int count)
          {
              if (!UIRemainLife.enabled)
                  return;

              UIRemainLife.text = "Life  		: " + count.ToString();
          }

          /// <summary>
          /// Left Enemy Text UI의 enabled를 설정할수있는 메서드 b = true or false
          /// </summary>
          /// <param name="b"></param>
          public void EnabledRemainEnemyUI(bool b)
          {
              UIRemainEnemyText.enabled = b;
          }

          public void EnabledBossHealthUI(bool b)
          {
              UIBossHpBar.gameObject.SetActive(b);
          }


          /// <summary>
          /// coinText UI를 설정해주는 메서드
          /// </summary>
          /// <param name="gold"></param>
          public void SetGoldUI(int gold)
          {
              UICoinText.text = GameManager.Instance.Gold.ToString();
          }

          /// <summary>
          /// BulletText UI를 설정해주는 메서드
          /// </summary>
          /// <param name="curBullet"></param>
          /// <param name="maxBullet"></param>
          public void SetBulletUI(int curBullet, int maxBullet)
          {
              UIBulletText.text = curBullet.ToString() + "/" + maxBullet.ToString();
          }

          /// <summary>
          /// HText UI와 HpBar UI를 설정해주는 메서드
          /// </summary>
          /// <param name="health"></param>
          /// <param name="maxHealth"></param>
          public void SetHealthUI(float health, float maxHealth)
          {
              UIHpText.text = (Mathf.Ceil(health*10)/10).ToString() + "/" + maxHealth.ToString();
              UIHpBar.value = health / maxHealth;
          }

          public void SetStaminaUI(float Stamina, float maxStamina)
          {
              UIStaminaBar.value = Stamina / maxStamina;
          }

          /// <summary>
          /// Boss의 HpBar를 설정해주는 메서드
          /// </summary>
          /// <param name="health"></param>
          /// <param name="maxHealth"></param>
          public void SetBossHealthUI(float health, float maxHealth)
          {
              UIBossHpBar.value = health / maxHealth;
          }

          /// <summary>
          /// ResurrectionImage 설정 메서드
          /// </summary>
          /// <param name="val"></param>
          public void SetResurrectionImage(bool b)
          {
              
              resurrectionImage.SetActive(b);
          }

          public void ActiveteSetting()
          {
              stack.Peek().SetActive(false);
              stack.Push(UISetting);
              UISetting.SetActive(true);
          }

          public void ChangeSound()
          {
              AudioListener.volume = soundSlider.value;
          }

          /// <summary>
          /// 무기 슬롯 UI를 설정해주는 메서드
          /// </summary>
          public void SetWeaponSlotUI(PlayerShooter playerShooter, IEnumerable<Weapon> weaponSlot)
          {
              weaponSlot = weaponSlot.Where(w => w != null); // null 제외
              
              bool isTransformed = false;

              // 무기 이미지 생성 및 기본 세팅
              int weaponIdx = 0;
              foreach (var weapon in weaponSlot)
              {
                  if (weaponIdx < images.Count)
                  {
                      if (weaponTypes[weaponIdx] == weapon.GetType())
                      {
                          weaponIdx++;
                          continue;
                      }

                      // 기존 무기 슬롯 UI의 무기 이미지를 파괴 후 새롭게 생성 후 할당
                      Destroy(images[weaponIdx].gameObject);
                      images[weaponIdx] = Instantiate(weaponImagePrefab);
                      weaponTypes[weaponIdx] = weapon.GetType();
                  }
                  else
                  {
                      // 무기 슬롯 UI에 무기 이미지를 새롭게 추가
                      images.Add(Instantiate(weaponImagePrefab));
                      weaponTypes.Add(weapon.GetType());
                  }

                  images[weaponIdx].sprite = weapon.GetComponent<SpriteRenderer>().sprite; // 실제 이미지 할당
                  images[weaponIdx].transform.SetParent(weaponPanel.transform); // Weapon Panel UI Object의 자식 오브젝트로 할당
                  images[weaponIdx].SetNativeSize();
                  images[weaponIdx].rectTransform.localScale = Vector3.one;
                  images[weaponIdx].GetComponent<Shadow>().effectDistance = new Vector2(10, -10); // 그림자 효과

                  // 투명도 조정
                  Color temp = images[weaponIdx].color;
                  temp.a = 0.5f;
                  images[weaponIdx].color = temp;

                  // 무기 이미지의 앵커 프리셋을 중앙으로 조정
                  images[weaponIdx].rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                  images[weaponIdx].rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

                  isTransformed = true;
                  weaponIdx++;
              }

              // 현재 무기 슬롯의 무기 개수보다 초과된 이미지는 제거
              if (images.Count > weaponIdx)
              {
                  images.GetRange(weaponIdx, images.Count - weaponIdx).ForEach(i => Destroy(i.gameObject));
                  images.RemoveRange(weaponIdx, images.Count - weaponIdx);
                  weaponTypes.RemoveRange(weaponIdx, images.Count - weaponIdx);
                  isTransformed = true;
              }

              try
              {
                  // 현재 사용 중인 무기 강조 효과
                  if (currentWeaponIdx != playerShooter.CurrentWeaponSlotNumber || weaponTypes[currentWeaponIdx] != playerShooter.CurrentWeapon)
                  {
                      if (currentWeaponIdx < images.Count && images[currentWeaponIdx] != null)
                      {
                          Color temp = images[currentWeaponIdx].color;
                          temp.a = 0.5f;
                          images[currentWeaponIdx].color = temp;
                          images[currentWeaponIdx].rectTransform.localScale = Vector3.one;
                      }

                      currentWeaponIdx = playerShooter.CurrentWeaponSlotNumber;
                      Color temp1 = images[currentWeaponIdx].color;
                      temp1.a = 1f;
                      images[currentWeaponIdx].color = temp1;
                      images[currentWeaponIdx].rectTransform.localScale = new Vector3(1.2f, 1.2f, 1f);
                  }
              }
              catch (ArgumentOutOfRangeException)
              {

              }
              catch (NullReferenceException)
              {

              }
              

              // 새롭게 생성된 이미지가 없다면 즉시 종료
              if (!isTransformed)
                  return;

              // 무기 이미지 배치
              float current = -((weaponIdx - 1) / 2f * weaponImagePlacementInterval); // 시작 배치 위치
              weaponPanel.rectTransform.sizeDelta = new Vector2((Mathf.Abs(current) + 100) * 2, weaponPanel.rectTransform.sizeDelta.y); // Weapon Panel의 크기 조정
              for (int i = 0; i < images.Count; i++)
              {
                  // 무기 이미지 위치 조정
                  images[i].rectTransform.anchoredPosition = new Vector2(current, 0f);
                  current += weaponImagePlacementInterval;
              }

          }

          //for start scene Ui
          public void OnClickGameStartButton()
          {
              if (!isSetting)
              {
                  //StartGame 구현 완료 후 수정: GameManager.Instance.StartGame(settingButton.Level);
                  GameManager.Instance.LoadScene(GameManager.Instance.InGameStartSceneBuildIndex);
                  GameObject.Find("Start Button").GetComponent<Button>().interactable = false;
              }
          }

          public void OnClickGameQuitButton()
          {
              if (!isSetting)
                  Application.Quit(); // 어플리케이션 종료
          }
          public void OnClickGameSettingButton()
          {
              isSetting = true;
              GameObject.Find("Setting Canvas").transform.GetChild(0).gameObject.SetActive(true);
          }
      }

      ```
    
    </div>
    </details>
  </br>

  * SaveManager
    세이브 데이터 관리
    <details>
    <summary>SaveManager Code</summary>
    <div markdown="1">

      ```c#
      using System;
      using System.Collections.Generic;
      using System.IO;
      using System.Runtime.Serialization.Formatters.Binary;
      using UnityEngine;

      public class SaveManager
      {
          private static readonly Dictionary<SaveKey, List<ISaveable>> saveableObjects = new Dictionary<SaveKey, List<ISaveable>>();
          
          public static void Save(string fileName, SaveKey key)
          {
              List<SaveData> saveDatas = new List<SaveData>();

              if(saveableObjects.TryGetValue(key, out List<ISaveable> saveablelist))
              {
                  // getData from listener
                  foreach(ISaveable saveable in saveablelist)
                      saveDatas.Add(new SaveData(saveable.Save(), saveable.GetType(), saveable.ID));

                  string path = Application.persistentDataPath + "/" + fileName;
                  using (var fs = new FileStream(path, FileMode.Create))
                  {
                      BinaryFormatter bf = new BinaryFormatter();
                      bf.Serialize(fs, saveDatas);
                  }
              }
              else
              {
                  throw new KeyNotFoundException($"saveableObjects[key]�뿉 �빐�떦�븯�뒗 List媛� �뾾�뒿�땲�떎.");
              }
          }

          /// <summary>
          /// �벑濡앸맂 saveablelist��� match�릺�뒗 saveData媛� �뾾�쓣寃쎌슦 ArgumentNullException 諛쒖깮!
          /// </summary>
          /// <param name="fileName"></param>
          /// <param name="key"></param>
          /// <exception cref="FileNotFoundException"></exception>
          /// <exception cref="Exception"></exception>
          /// <exception cref="ArgumentNullException"></exception>
          public static void Load(string fileName, SaveKey key)
          {
              string path = Application.persistentDataPath + "/" + fileName;

              if (!IsFileExistence(path))
              {
                  throw new FileNotFoundException($"There is no DataFile.");
              }

              if(saveableObjects.TryGetValue(key, out List<ISaveable> saveablelist))
              {
                  //load
                  using (var fs = new FileStream(path, FileMode.Open))
                  {
                      var bf = new BinaryFormatter();
                      var data = bf.Deserialize(fs) as List<SaveData>;

                      foreach (ISaveable saveable in saveablelist)
                      {
                          SaveData d = data.Find(e => e.listenerID == saveable.ID && e.listenerType == saveable.GetType());
                        
                          if(d == null)
                              throw new ArgumentNullException($"load �떎�뜝 TYPE �샊��� ID 遺덉씪移�(SaveData��� match�릺�뒗 Saveable Object媛� �뾾�쓬) saveable.ID : {saveable.ID}, saveable.GetType() : {saveable.GetType()} ");

                          saveable.Load(d.data);
                      }
                  }
              }
              else
              {
                  throw new KeyNotFoundException($"saveableObjects[key]�뿉 �빐�떦�븯�뒗 List媛� �뾾�뒿�땲�떎.");
              }
          }

          /// <summary>
          /// Add saveable objece to saveableObject Dictionary
          /// </summary>
          /// <param name="saveable"></param>
          /// <param name="key"></param>
          public static void Add(ISaveable saveable, SaveKey key)//exception handle
          {
              if (saveableObjects.TryGetValue(key, out List<ISaveable> saveablelist))
                  saveablelist.Add(saveable);
              else
                  saveableObjects.Add(key, new List<ISaveable> { saveable });
          }

          /// <summary>
          /// remove saveable objece from saveableObject Dictionary. if remove fale return false
          /// </summary>
          /// <param name="saveable"></param>
          /// <param name="key"></param>
          public static bool Remove(ISaveable saveable, SaveKey key)
          {
              if (saveableObjects.TryGetValue(key, out List<ISaveable> saveablelist))
              {
                  saveablelist.Remove(saveable);
                  return true;
              }
              else
              {
                  return false;
              }
          }

          /// <summary>
          /// path : file path
          /// If the file exists return true, not exists return false
          /// </summary>
          /// <param name="path"></param>
          /// <returns></returns>
          public static bool IsFileExistence(string path)
          {
              if (File.Exists(path))
                  return true;
              return false;
          }
          public static void DeleteFile(string fileName)
          {
              File.Delete(Application.persistentDataPath + "/" + fileName);
              Debug.Log($"<{nameof(SaveManager)}> {fileName} �뙆�씪 �궘�젣 �셿猷�.");
          }
          ///<summary>
          /// disposable 以묒꺽 �겢�옒�뒪
          /// </summary>
          public sealed class TemporarySaveScope : IDisposable
          {
              private bool isDispose = false;
              string fileNameTemp;
              SaveKey keyTemp;
              public TemporarySaveScope(string fileName, SaveKey key)
              {
                  fileNameTemp = fileName;
                  keyTemp = key;
                  SaveManager.Save(fileName, key);
              }
              public void Dispose()
              {
                  if (!this.isDispose)
                  {
                      SaveManager.Load(fileNameTemp, keyTemp);
                      this.isDispose = true;
                      DeleteFile(fileNameTemp);
                  }
                  GC.SuppressFinalize(this);
              }
          }
      }

      public enum SaveKey
      {
          GameData,
          Setting
      }

      [Serializable]
      public class SaveData
      {
          public readonly object data;
          public readonly Type listenerType;
          public readonly string listenerID;

          public SaveData(object data, Type type, string id)
          {
              this.data = data;
              this.listenerType = type;
              this.listenerID = id;
          }
      }
      ```
    
    </div>
    </details>
  </br>

### Other Objects
* #### Interation Objects  
  * Door and Switch  
  ![door](https://github.com/youwonsock/back-to-the-dungeon-scripts/assets/46276141/9ceaff02-a56d-43d0-9d1c-c5c56e06ee61)
  
* #### Trap
  * BossEventTrigger
    ![bosseven](https://github.com/youwonsock/back-to-the-dungeon-scripts/assets/46276141/883a19e0-751d-4b48-bad4-83449798741d)  

### Player
  * 체력 및 사망처리
    
    ![p](https://github.com/youwonsock/back-to-the-dungeon-scripts/assets/46276141/c4859e7c-fbd9-42fc-ad5a-27998173f67f)

### UI
  * QuitButton
  * SettingButton
  
    ![ui](https://github.com/youwonsock/back-to-the-dungeon-scripts/assets/46276141/496ab77b-bb31-4883-92ba-72469a7a71b8)

### Weapon
* #### PlayerWeapon
  * Weapon
    * AssaultRifle
      ![ak](https://github.com/youwonsock/back-to-the-dungeon-scripts/assets/46276141/6a0058a7-3b47-44e7-879f-2df2810fc0c5)
  
    * AutoShutGun
      ![autoshot](https://github.com/youwonsock/back-to-the-dungeon-scripts/assets/46276141/96a8bb8f-9965-4c94-a336-df6dc9271d83)
  
    * AWP
      ![awp](https://github.com/youwonsock/back-to-the-dungeon-scripts/assets/46276141/c04c358e-68c8-409f-8a91-cd7ed655fcd8)
  
    * BurstRifle
      ![m16](https://github.com/youwonsock/back-to-the-dungeon-scripts/assets/46276141/1dcd52a1-3564-48a3-b75e-71318532b2db)
  
    * Cannon
      ![cannon](https://github.com/youwonsock/back-to-the-dungeon-scripts/assets/46276141/010f417c-de69-4069-b4e7-7f4422dbd1ae)
  
    * Minigun
      ![minigun](https://github.com/youwonsock/back-to-the-dungeon-scripts/assets/46276141/d6524d28-41b7-450d-8fdb-8705eb08fdba)
  
    * Pistol
      ![HandGun](https://github.com/youwonsock/back-to-the-dungeon-scripts/assets/46276141/deb734f9-4563-4fb0-b966-5982ec287998)
  
    * ShotGun
      ![shotgun](https://github.com/youwonsock/back-to-the-dungeon-scripts/assets/46276141/4f7bb0ca-a397-4b84-9ac7-686b069c0f6b)
  
    * Smg
      ![smg](https://github.com/youwonsock/back-to-the-dungeon-scripts/assets/46276141/8c0f99a0-b5e5-4811-804b-df52adc9665d)  

