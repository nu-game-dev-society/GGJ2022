using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    public Transform target;
    void Start()
    {
        if (target == null)
            target = FindObjectOfType<PlayerController>()?.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));

    }
}
