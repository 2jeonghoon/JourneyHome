using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomEffect : MonoBehaviour
{
    [SerializeField]
    private AudioClip clip;
    private void Start()
    {
        SoundManager.instance.SFXPlay("Boom", clip);
        StartCoroutine(DestroyEffect());
    }
    private IEnumerator DestroyEffect()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
