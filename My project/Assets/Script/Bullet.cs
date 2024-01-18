using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage;      // �Ѿ� ������
    private float transfusion; // ���� ��
    private float explosionRange; // ���� ����
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private GameObject bulletEffect;
    private bool isExplosionOn; // ���� �Ѿ� ON/OFF

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

    // 2�� �� �ı�
    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(2f);
        BulletDestroy();
    }


    // �浹 ó��
    private void OnCollisionEnter(Collision collision)
    {
        // ���� �浹 �� ������ �ֱ�
        if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamge(damage);
            BulletDestroy();
        }
        // �ٴڰ� �浹 �� ����
        if (collision.gameObject.tag == "Floor")
        {
            BulletDestroy();
        }
        // ���� �浹 �� ����
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
