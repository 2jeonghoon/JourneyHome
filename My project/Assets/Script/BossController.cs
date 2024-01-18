using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossController : Enemy
{
    private enum SkillPosition
    {
        Left = 0,
        Right = 1,
        Up = 2,
        Down = 3,
        Mid = 4
    }

    [Header("���� ��ų")]
    [SerializeField]
    private float skillCoolTime;         // ��ų ��Ÿ��
    [SerializeField]
    private float skillDamage;           // ��ų ������
    [SerializeField]
    private GameObject skillPointPrefab; // ��ų�� ������ ��ġ ǥ���� ������Ʈ
    [SerializeField]
    private GameObject skillPrefab;      // �ϴÿ��� ������ ��ų

    Vector3[] skillPosition = new Vector3[4];

    protected override void Start()
    {
        int size = 3;
        skillPosition[(int)SkillPosition.Left] = Vector3.left * size;
        skillPosition[(int)SkillPosition.Right] = Vector3.right * size;
        skillPosition[(int)SkillPosition.Up] = Vector3.back * size;
        skillPosition[(int)SkillPosition.Down] = Vector3.forward * size;
        //skillPosition[(int)SkillPosition.Mid] = Vector3.zero * size;

        // HP ����
        curHP = maxHP;
        isHitCoolTime = false;
        target = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾� Ÿ�� ��ġ ��������
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = Speed;
        StartCoroutine(BossSkill());
    }

    public override void TakeDamge(float damage)
    {
        curHP -= damage;
        target.gameObject.GetComponent<Player>().TransfusionAttack();
        if (curHP <= 0)
        {
            GameObject clone = Instantiate(DieEffect, transform.position, transform.rotation);
            target.gameObject.GetComponent<Player>().ScoreUP(bonus, money);
            
            Destroy(gameObject);
        }
    }

    private IEnumerator BossSkill()
    {
        while (true)
        {
            Vector3 skillPos = target.position;
            
            // ��ų�� ������ ��ġ ����
            skillPos.y = 30;
            for (int i = 0; i < skillPosition.Length; i++)
            {
                GameObject skill = Instantiate(skillPrefab, skillPos + skillPosition[i], target.rotation);
                skill.GetComponent<BossSkill>().Setup(skillDamage);
            }
            yield return new WaitForSeconds(skillCoolTime);
        }
    }
}
