using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float maxHP;     // 최대 HP
    public float curHP;     // 현재 HP
    public float Speed;    // 이동 속도
    [SerializeField]
    protected float damage;
    [SerializeField]
    protected float hitCoolTime;
    [SerializeField]
    protected int bonus; // 잡았을 때 얻는 점수

    [SerializeField]
    protected int money; // 잡았을 때 주는 돈

    [SerializeField]
    protected GameObject DieEffect;

    protected Transform target; //적의 공격 대상(플레이어)
    protected NavMeshAgent navMeshAgent; //이동제어를 위한 NavMeshAgent
    protected bool isSlow = false;
    protected bool isHitCoolTime;


    protected virtual void Start()
    {
        // HP 세팅
        curHP = maxHP;
        isHitCoolTime = false;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = Speed;
    }

    protected virtual void Update()
    {
        navMeshAgent.SetDestination(target.position);
    }

    // Hit Cool Time
    private IEnumerator HitCoolTime()
    {
        isHitCoolTime = true;
        yield return new WaitForSeconds(hitCoolTime);
        isHitCoolTime = false;
    }

    // 데미지 받기
    public virtual void TakeDamge(float damage)
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!isHitCoolTime)
            {
                StartCoroutine(HitCoolTime());
                collision.gameObject.GetComponent<Player>().TakeDamage(damage);
            }
        }
    }
}
