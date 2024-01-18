using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private float damage;

    [SerializeField]
    private GameObject explosionParticlePrefab;

    private float time;

    public AudioClip clip;

    public void SetUp(float range, float time)
    {
        SoundManager.instance.SFXPlay("explosion", clip);
        this.time = time;
        transform.localScale = new Vector3(range, range, range);
        StartCoroutine(Boom());
    }



    private IEnumerator Boom()
    {
        GameObject clone = Instantiate(explosionParticlePrefab, transform.position, transform.rotation);
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collision)
    {
        // 적과 충돌 시 데미지 주기
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamge(damage);
        }
    }
}
