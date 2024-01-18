using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        Heal,
        Coffee
    }

    [SerializeField]
    private float amount;

    [SerializeField]
    private ItemType itemType;

    private void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * 50f);    
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 플레이어와 충돌 시
        if (collision.gameObject.tag == "Player")
        {
            // 힐
            if (itemType == ItemType.Heal)
            {
                collision.gameObject.GetComponent<Player>().TakeHeal(amount);
            }
            // 커피
            else if(itemType == ItemType.Coffee)
            {
                collision.gameObject.GetComponent<Player>().TakeCoffee(amount);
            }
            Destroy(gameObject);
        }
    }
}
