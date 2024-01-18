using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage;      // 총알 데미지
    private float transfusion; // 흡혈 양
    private float explosionRange; // 폭발 범위
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private GameObject bulletEffect;
    private bool isExplosionOn; // 폭발 총알 ON/OFF

    public AudioClip clip;

    // Start is called before the first frame update
    public void SetUp(bool isExplosionOn, float damage, float Transfusion, float explosionRange)
    {
        SoundManager.instance.SFXPlay("shot", clip);
        this.explosionRange = explosionRange;
        this.transfusion = Transfusion;
        this.damage = damage;
        this.isExplosionOn = isExplosionOn;
    }
    void Start()
    {
        StartCoroutine(DestroyBullet());
    }

    // Update is called once per frame
    void Update()
    {
        
        //rigidbody.MovePosition(transform.position + targetPos);
    }

    // 2초 후 파괴
    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(2f);
        BulletDestroy();
    }


    // 충돌 처리
    private void OnCollisionEnter(Collision collision)
    {
        // 적과 충돌 시 데미지 주기
        if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamge(damage);
            BulletDestroy();
        }
        // 바닥과 충돌 시 삭제
        if (collision.gameObject.tag == "Floor")
        {
            BulletDestroy();
        }
        // 벽과 충돌 시 삭제
        else if (collision.gameObject.tag == "Wall")
        {
            BulletDestroy();
        }
    }

    private void BulletDestroy()
    {
        if (isExplosionOn)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
            explosion.GetComponent<Explosion>().SetUp(explosionRange, 0.5f);
        }
        GameObject clone = Instantiate(bulletEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
