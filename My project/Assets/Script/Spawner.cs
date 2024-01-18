using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // ������ ����� Plane�� �ڽ��� RespawnRange ������Ʈ
    public GameObject rangeObject;
    BoxCollider rangeCollider;

    // ��ȯ�� Object
    public GameObject[] prefabs;
    [SerializeField]
    private GameObject preObject; // ��ȯ�ϱ����� ǥ�ÿ�����Ʈ
    [SerializeField]
    private float spawnTime;
    [SerializeField]
    private int maxSpawnCount;

    private void Start()
    {
        StartCoroutine(RandomRespawn_Coroutine());
    }

    IEnumerator RandomRespawn_Coroutine()
    {
        for(int i = 0; i < maxSpawnCount; i++) 
        { 
            yield return new WaitForSeconds(spawnTime-1f);

            // ���� ��ġ �κп� ������ ���� �Լ� Return_RandomPosition() �Լ� ����
            int num = Random.RandomRange(0, prefabs.Length);

            Vector3 vec = Return_RandomPosition();
            GameObject pointObject = Instantiate(preObject, vec, transform.rotation);
            yield return new WaitForSeconds(1f);
            Destroy(pointObject);
            GameObject instantCapsul = Instantiate(prefabs[num], vec, Quaternion.identity);
        }
    }

    private void Awake()
    {
        rangeCollider = rangeObject.GetComponent<BoxCollider>();
    }

    Vector3 Return_RandomPosition()
    {
        Vector3 originPosition = rangeObject.transform.position;
        // �ݶ��̴��� ����� �������� bound.size ���
        float range_X = rangeCollider.bounds.size.x;
        float range_Z = rangeCollider.bounds.size.z;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);
        Vector3 RandomPostion = new Vector3(range_X, 0f, range_Z);

        Vector3 respawnPosition = originPosition + RandomPostion;
        return respawnPosition;
    }
}
