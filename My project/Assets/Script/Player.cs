using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // ���׷��̵�
    public enum UpgradeName
    {
        UpgradeAttackPower = 0,
        UpgradeSkillCoolTimeDown = 1,
        ExplosionRangeUp = 2,
        MaxHealthUp = 3,
        TransfusionUp = 4,
        DecreaseCoffeine = 5
    }
    [SerializeField]
    private GameObject head1;   // ���������� ������ ���� ���̰� ��� �÷��̾�
    [SerializeField]
    private GameObject head2;
    [SerializeField]
    private GameObject head3;

    public float currentSpeed;
    public float walkSpeed;   // �÷��̾� �̵��ӵ�
    public float runSpeed;   // �÷��̾� �̵��ӵ�
    Rigidbody rigidbody;        // rigidbody
    Vector3 movement;           // �÷��̾� �̵�

    [Header("�÷��̾� �ɷ�ġ")]
    // ���ݷ�
    [SerializeField]
    private float damage;
    [SerializeField]
    private float explosionRange = 3; // ���� ����
    private float Transfusion = 0; // ������
    [SerializeField]
    private float ExplosionCoolTime; // ���� ��ų ��Ÿ��
    private float curQCoolTime; // ���� ��ų ���� �ð�
    // ü��
    [SerializeField]
    private float maxHP;
    private float curHP;
    // ī���� ���ҷ�
    [SerializeField]
    private float decreaseCoffeine;

    [Header("������Ʈ")]
    public Camera followCamera; // ī�޶� ����
    // �Ѿ�
    [SerializeField]
    public GameObject bullet;   // �Ѿ� ������Ʈ
    public Transform bulletPos; // �Ѿ� ���� ��ġ ����

    [SerializeField]
    private GameObject HitPanel;

    private PlayerAnimationController playerAnimationController;

    // ��ų
    private bool isExplosionOn = false; // ��ų ON/OFF
    private bool isExplosionCoolTime = false;



    [Header("��������")]
    // ����
    public int score = 0;   // ���� ����
    [SerializeField]
    private int currentStage;   // ���� ��������
    private int[] targetScore;  // ��ǥ ����
    public int TargetScore => targetScore[currentStage - 1];    // ���� ���������� Score ������
    private string[] sceneName;

    // ��
    private int money = 0;
    // ���׷��̵忡 �ʿ��� �� ��
    public int[] needMoney;

    // ī����
    public float coffeine = 100f;

    public bool isShoping;

    public float DecreaseCoffeine => decreaseCoffeine;
    public float CurHP => curHP;
    public float MaxHP => maxHP;
    public int Money => money;
    public float CurQCoolTime => curQCoolTime;
    public float explosionCoolTime => ExplosionCoolTime;

    public bool isClearScene;
    private void Awake()
    {
        isClearScene = false;
        DontDestroyOnLoad(gameObject);
        needMoney = new int[6];
        for (int i = 0; i < needMoney.Length; i++)
        {
            needMoney[i] = 1000;
        }
        
        rigidbody = GetComponent<Rigidbody>(); // rigidbody������Ʈ ��������
        playerAnimationController = GetComponent<PlayerAnimationController>();
        curHP = maxHP;
        currentSpeed = walkSpeed;

        targetScore = new int[4];
        targetScore[0] = 5000;
        targetScore[1] = 10000;
        targetScore[2] = 15000;
        targetScore[3] = 20000;

        sceneName = new string[4];
        sceneName[0] = "";
        sceneName[1] = "2_Summer";
        sceneName[2] = "3_Fall";
        sceneName[3] = "4_Winter";



        isShoping = false;
    }

    private void Update()
    {
        // �Ѿ� �߻�
        //if (Input.GetMouseButtonDown(0))
        if (Input.GetKeyDown(KeyCode.Q) && !isExplosionCoolTime)
        {
            isExplosionOn = true;
            isExplosionCoolTime = true;
            StartCoroutine(ExplosionCoolTimeOn());
        }
        if (Input.GetMouseButtonDown(0) && !isShoping)
        {
            StartCoroutine("Shot");
            playerAnimationController.OnShoot();
        }

        if (movement != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            //Debug.Log("��");
            currentSpeed = runSpeed;
            playerAnimationController.OnRun();

        }
        else if (movement != Vector3.zero)
        {
            //Debug.Log("����");
            currentSpeed = walkSpeed;
            playerAnimationController.OnWalk();
        }
        else
        {
            //Debug.Log("����");
            playerAnimationController.OnIdle();
        }
        if(!isClearScene)
            coffeine -= Time.deltaTime * decreaseCoffeine;
    }
    void FixedUpdate()
    {
        // Ű �Է�
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // �̵�
        Run(h, v);
        // ȸ��
        Turn();
    }

    // �÷��̾� �̵�
    void Run(float h, float v)
    {
        movement.Set(h, 0, v);

        movement = movement.normalized * currentSpeed * Time.deltaTime;

        rigidbody.MovePosition(transform.position + movement);
    }

    // �÷��̾� ���콺 ��ġ �ٶ󺸱�
    void Turn()
    {
        int layerMask = (-1) - (1 << LayerMask.NameToLayer("Player"));
        Ray ray;
        // ���콺 ��ġ ���� �ٶ󺸱�
        try
        {
            ray = followCamera.ScreenPointToRay(Input.mousePosition);
        }
        catch
        {
            followCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            ray = followCamera.ScreenPointToRay(Input.mousePosition);
        }
        RaycastHit rayHit;

        if (Physics.Raycast(ray, out rayHit, 100, layerMask))
        {
            Vector3 nextVec = rayHit.point;
            nextVec.y = transform.position.y;
            // �ٶ󺸱�
            transform.LookAt(nextVec);
        }
    }

    // �Ѿ� �߻�
    IEnumerator Shot()
    {

        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);  // �Ѿ� ����
        intantBullet.GetComponent<Bullet>().SetUp(isExplosionOn, damage, Transfusion, explosionRange);
        isExplosionOn = false;
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();                         // �Ѿ� Rigidbody ��������
        bulletRigid.velocity = bulletPos.forward * 50;                                          // ������ �̵�

        yield return null;
    }

    // ���� ��ų ��Ÿ��
    private IEnumerator ExplosionCoolTimeOn()
    {
        curQCoolTime = ExplosionCoolTime;
        while (curQCoolTime >= 0)
        {
            curQCoolTime -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        isExplosionCoolTime = false;
    }


    private IEnumerator Hit()
    {
        try
        {
            HitPanel.GetComponent<Image>().color = new Color(1, 0, 0, 0.2f);
        }
        catch
        {
            HitPanel = GameObject.FindGameObjectWithTag("HitPanel");
            HitPanel.GetComponent<Image>().color = new Color(1, 0, 0, 0.2f);
        }
            yield return new WaitForSeconds(0.5f);
        try
        {
            HitPanel.GetComponent<Image>().color = new Color(1, 0, 0, 0);
        }
        catch
        {
            HitPanel = GameObject.FindGameObjectWithTag("HitPanel");
            HitPanel.GetComponent<Image>().color = new Color(1, 0, 0, 0);
        }
    }

    // �������� ����
    public void TakeDamage(float damage)
    {
        curHP -= damage;
        StartCoroutine("Hit");
        if (curHP <= 0)
        {
            curHP = 0;
            StartCoroutine(GameOver());
        }
        //Debug.Log(curHP);
    }

    // ȸ��
    public void TakeHeal(float heal)
    {
        curHP += heal;
        if (curHP >= maxHP)
        {
            curHP = maxHP;
        }
    }

    // ī���� ����
    public void TakeCoffee(float coffeine)
    {
        this.coffeine += coffeine;
        if (this.coffeine >= 100)
        {
            this.coffeine = 100;
        }
    }

    public void TransfusionAttack()
    {
        TakeHeal(damage * (Transfusion / 100));
    }

    // ���� ȹ��
    public void ScoreUP(int bonus, int money)
    {
        score += bonus;
        this.money += money;
        if (score >= TargetScore)
        {
            NextStage();
        }
    }

    IEnumerator GameOver()
    {
        playerAnimationController.OnDie();
        yield return new WaitForSeconds(1.25f);
        SceneManager.LoadScene("GameOver");
    }

    public void NextStage()
    {
        if(currentStage == 1)
        {
            head1.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            head2.transform.localScale = new Vector3(1, 1, 1);
        } else if(currentStage == 3)
        {
            head3.transform.localScale = new Vector3(1, 1, 1);
        }

        SceneManager.LoadScene(sceneName[currentStage++]);
        isClearScene = true;
    }

    //***********************Upgrade �Լ���*****************************/

    // ������ ����
    public void UpgradeAttackPower()
    {
        if(money >= needMoney[(int)UpgradeName.UpgradeAttackPower])
        {
            money -= needMoney[(int)UpgradeName.UpgradeAttackPower];
            damage += 5;
            needMoney[(int)UpgradeName.UpgradeAttackPower] += 500;
        }
    }

    // ��ų ��Ÿ�� ����
    public void UpgradeSkillCoolTimeDown()
    {
        if (money >= needMoney[(int)UpgradeName.UpgradeSkillCoolTimeDown])
        {
            money -= needMoney[(int)UpgradeName.UpgradeSkillCoolTimeDown];
            ExplosionCoolTime -= 1;
            needMoney[(int)UpgradeName.UpgradeSkillCoolTimeDown] += 500;
        }
    }

    // ��ų ���� ����
    public void UpgradeExplosionRangeUp()
    {
        if (money >= needMoney[(int)UpgradeName.ExplosionRangeUp])
        {
            money -= needMoney[(int)UpgradeName.ExplosionRangeUp];
            explosionRange += 1;
            needMoney[(int)UpgradeName.ExplosionRangeUp] += 500;
        }
    }

    // �ִ� ü�� ����
    public void UpgradeMaxHealthUp()
    {
        if (money >= needMoney[(int)UpgradeName.MaxHealthUp])
        {
            money -= needMoney[(int)UpgradeName.MaxHealthUp];
            maxHP += 20;
            needMoney[(int)UpgradeName.MaxHealthUp] += 500;
        }
    }

    // ���� ��ų
    public void UpgradeTransfusionUp()
    {
        if (money >= needMoney[(int)UpgradeName.TransfusionUp])
        {
            money -= needMoney[(int)UpgradeName.TransfusionUp];
            Transfusion += 2;
            needMoney[(int)UpgradeName.TransfusionUp] += 500;
        }
    }

    // ī���� ������ ����
    public void UpgradeDecreaseCoffeine()
    {
        if (money >= needMoney[(int)UpgradeName.DecreaseCoffeine])
        {
            money -= needMoney[(int)UpgradeName.DecreaseCoffeine];
            decreaseCoffeine -= 0.1f;
            needMoney[(int)UpgradeName.DecreaseCoffeine] += 500;
        }
    }
}
