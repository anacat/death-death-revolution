using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateColliders : MonoBehaviour
{
    public GameObject danceCollider;
    public int numberOfObjects;
    public int radius;

    void Start()
    {
        for (var i = 0; i < 30; i += 1)
        {
            var angle = i * Mathf.PI * 2 / numberOfObjects;
            var pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

            GameObject collider = Instantiate(danceCollider, transform);
            collider.transform.localPosition = pos;
        }
    }
}
