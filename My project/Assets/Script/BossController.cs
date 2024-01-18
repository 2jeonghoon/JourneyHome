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

    [Header("보스 스킬")]
    [SerializeField]
    private float skillCoolTime;         // 스킬 쿨타임
    [SerializeField]
    private float skillDamage;           // 스킬 데미지
    [SerializeField]
    private GameObject skillPointPrefab; // 스킬이 떨어질 위치 표시할 오브젝트
    [SerializeField]
    private GameObject skillPrefab;      // 하늘에서 떨어질 스킬

    Vector3[] skillPosition = new Vector3[4];

    protected override void Start()
    {
        int size = 3;
        skillPosition[(int)SkillPosition.Left] = Vector3.left * size;
        skillPosition[(int)SkillPosition.Right] = Vector3.right * size;
        skillPosition[(int)SkillPosition.Up] = Vector3.back * size;
        skillPosition[(int)SkillPosition.Down] = Vector3.forward * size;
        //skillPosition[(int)SkillPosition.Mid] = Vector3.zero * size;

        // HP 세팅
        curHP = maxHP;
        isHitCoolTime = false;
        target = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어 타겟 위치 가져오기
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
            
            // 스킬이 떨어질 위치 높이
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
