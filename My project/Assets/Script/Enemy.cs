using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float maxHP;     // �ִ� HP
    public float curHP;     // ���� HP
    public float Speed;    // �̵� �ӵ�
    [SerializeField]
    protected float damage;
    [SerializeField]
    protected float hitCoolTime;
    [SerializeField]
    protected int bonus; // ����� �� ��� ����

    [SerializeField]
    protected int money; // ����� �� �ִ� ��

    [SerializeField]
    protected GameObject DieEffect;

    protected Transform target; //���� ���� ���(�÷��̾�)
    protected NavMeshAgent navMeshAgent; //�̵���� ���� NavMeshAgent
    protected bool isSlow = false;
    protected bool isHitCoolTime;


    protected virtual void Start()
    {
        // HP ����
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

    // ������ �ޱ�
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
