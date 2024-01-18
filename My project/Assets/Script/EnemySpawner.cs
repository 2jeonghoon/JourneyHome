using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] EnemyPrefabs;     // 생성하는 적 프리팹
    [SerializeField]
    public Transform[] SpawnPointArr;   // 스폰 포인트 배열

    private void Start()
    {
        StartCoroutine("EnemySpawn");   // 스폰 코루틴 실행
    }

    IEnumerator EnemySpawn()
    {
        while (true)
        {
            int num = Random.Range(0, SpawnPointArr.Length);    // 스폰 포인트 배열 중 랜덤하게 하나 뽑기
            Transform SpawnPoint = SpawnPointArr[num];          // 스폰 포인트 설정
            num = Random.RandomRange(0, EnemyPrefabs.Length);
            GameObject clone = Instantiate(EnemyPrefabs[num], SpawnPoint.position, SpawnPoint.rotation);     // 적 생성
            yield return new WaitForSeconds(0.5f);              // 0.5초마다 생성
        }
    }
}
