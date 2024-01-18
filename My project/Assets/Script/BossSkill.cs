using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill : MonoBehaviour
{
    private Rigidbody rigidbody;
    private Vector3 movement;
    private float damage;

    public GameObject effectPrefab;

    [SerializeField]
    private GameObject skillPointPrefab; // 스킬이 떨어질 위치 표시할 오브젝트
    private GameObject skillPoint;
    Vector3 skillPointPos;

    void Start()
    {
    }

    public void Setup(float skillDamage)
    {
        skillPointPos = transform.position;
        skillPointPos.y = 0;
        skillPoint = Instantiate(skillPointPrefab, skillPointPos, transform.rotation);
        rigidbody = GetComponent<Rigidbody>();                         // 총알 Rigidbody 가져오기
        rigidbody.velocity = transform.up * -50;
        this.damage = skillDamage;
    }

    private void FixedUpdate()
    {
        movement.Set(transform.position.x, 0, transform.position.z);

        movement = movement.normalized * 1 * Time.deltaTime;

        rigidbody.MovePosition(transform.position + movement);
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag == "Player")
        {
            Debug.Log(other.gameObject.tag);
            other.gameObject.GetComponent<Player>().TakeDamage(damage);
        }
        GameObject clone = Instantiate(effectPrefab, transform.position, transform.rotation);
        Destroy(skillPoint);
        Destroy(gameObject);
    }
}
