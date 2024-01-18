using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] EnemyPrefabs;     // �����ϴ� �� ������
    [SerializeField]
    public Transform[] SpawnPointArr;   // ���� ����Ʈ �迭

    private void Start()
    {
        StartCoroutine("EnemySpawn");   // ���� �ڷ�ƾ ����
    }

    IEnumerator EnemySpawn()
    {
        while (true)
        {
            int num = Random.Range(0, SpawnPointArr.Length);    // ���� ����Ʈ �迭 �� �����ϰ� �ϳ� �̱�
            Transform SpawnPoint = SpawnPointArr[num];          // ���� ����Ʈ ����
            num = Random.RandomRange(0, EnemyPrefabs.Length);
            GameObject clone = Instantiate(EnemyPrefabs[num], SpawnPoint.position, SpawnPoint.rotation);     // �� ����
            yield return new WaitForSeconds(0.5f);              // 0.5�ʸ��� ����
        }
    }
}
