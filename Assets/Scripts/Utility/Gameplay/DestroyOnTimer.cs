using UnityEngine;
using System.Collections;

public class DestroyOnTimer : MonoBehaviour
{

    public float time = 5f;

    void Start()
    {
        StartCoroutine(DestroyTimer());
    }

    IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    public void OverrideTime(float _time)
    {
        time = _time;
        StopAllCoroutines();
        StartCoroutine(DestroyTimer());
    }
}