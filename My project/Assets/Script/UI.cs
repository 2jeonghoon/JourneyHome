using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{

    public enum UpgradeName
    {
        UpgradeAttackPower = 0,
        UpgradeSkillCoolTimeDown = 1,
        ExplosionRangeUp = 2,
        MaxHealthUp = 3,
        TransfusionUp = 4,
        DecreaseCoffeine = 5
    }

    private float hp;     // HP������ ����� �ؽ�Ʈ

    [SerializeField]
    private Slider hpSlider;
    [SerializeField]
    private TextMeshProUGUI textQCoolTime;     // Q��ų ��Ÿ���� ����� �ؽ�Ʈ

    [SerializeField]
    private TextMeshProUGUI textScore;     // ���� ��Ÿ���� ����� �ؽ�Ʈ

    [SerializeField]
    private Slider coffeineSlider;

    [SerializeField]
    private TextMeshProUGUI textMoney;     // ���� ����� �ؽ�Ʈ
    [SerializeField]
    private GameObject Panel;       // ī������ �����Ҽ��� ��ο����� ���
    private Image PanelImage;

    [SerializeField]
    private Image qImage;

    [SerializeField]
    private Player player;

    [SerializeField]
    private GameObject Shop; //����(��ũ�ѹ�)�� ���

    [Header("���׷��̵� �� ǥ��")]
    [SerializeField]
    private TextMeshProUGUI textUpgradeDamage;     // ������ �� ��� �ؽ�Ʈ
    [SerializeField]
    private TextMeshProUGUI textUpgradeSkillCoolDown;     // ��ų ��Ÿ�� ���� ��� �ؽ�Ʈ
    [SerializeField]
    private TextMeshProUGUI textUpgradeExplosionRangeUp;     // ���� ���� �� ��� �ؽ�Ʈ
    [SerializeField]
    private TextMeshProUGUI textUpgradeMaxHealth;     // �ִ�ü�� �� ��� �ؽ�Ʈ
    [SerializeField]
    private TextMeshProUGUI textUpgradeTransfusion;     // ������ ���� ��� �ؽ�Ʈ
    [SerializeField]
    private TextMeshProUGUI textDecreaseCoffeine;     // ī���� ���ҷ� ���� ��� �ؽ�Ʈ


    bool isOpen = false; // ���׷��̵� â ON/OFF

    
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        PanelImage = Panel.GetComponent<Image>();
        Shop.SetActive(false);

    }
    // Update is called once per frame
    void Update()
    {
        SetHPValue();
        SetCoffeineValue();
        SetQCoolValue();
        textScore.text = "Score : " + player.score.ToString();
        textMoney.text = "Money : " + player.Money.ToString();
        PanelImage.color = new Color(0, 0, 0, (100-player.coffeine)/100);


        // ���׷��̵� �ؽ�Ʈ ������Ʈ
        textUpgradeDamage.text = player.needMoney[(int)UpgradeName.UpgradeAttackPower].ToString();
        textUpgradeExplosionRangeUp.text = player.needMoney[(int)UpgradeName.ExplosionRangeUp].ToString();
        textUpgradeMaxHealth.text = player.needMoney[(int)UpgradeName.MaxHealthUp].ToString();
        textUpgradeSkillCoolDown.text = player.needMoney[(int)UpgradeName.UpgradeSkillCoolTimeDown].ToString();
        textUpgradeTransfusion.text = player.needMoney[(int)UpgradeName.TransfusionUp].ToString();
        textDecreaseCoffeine.text = player.needMoney[(int)UpgradeName.DecreaseCoffeine].ToString();

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!isOpen)
            {
                Shop.SetActive(true);
                player.isShoping = true;
                isOpen = true;
                Time.timeScale = 0;
            }
            else
            {
                Shop.SetActive(false);
                player.isShoping = false;
                isOpen = false;
                Time.timeScale = 1;
            }
        }
    }

    private void SetQCoolValue()
    {
        qImage.fillAmount = (player.CurQCoolTime / player.explosionCoolTime);
    }

    public void SetHPValue()
    {
        hpSlider.value = player.CurHP/player.MaxHP;
    }  

    public void SetCoffeineValue()
    {
        coffeineSlider.value = player.coffeine / 100;
    }
}
