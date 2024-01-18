using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // 위에서 언급한 Plane의 자식인 RespawnRange 오브젝트
    public GameObject rangeObject;
    BoxCollider rangeCollider;

    // 소환할 Object
    public GameObject[] prefabs;
    [SerializeField]
    private GameObject preObject; // 소환하기전에 표시오브젝트
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

            // 생성 위치 부분에 위에서 만든 함수 Return_RandomPosition() 함수 대입
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
        // 콜라이더의 사이즈를 가져오는 bound.size 사용
        float range_X = rangeCollider.bounds.size.x;
        float range_Z = rangeCollider.bounds.size.z;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);
        Vector3 RandomPostion = new Vector3(range_X, 0f, range_Z);

        Vector3 respawnPosition = originPosition + RandomPostion;
        return respawnPosition;
    }
}
