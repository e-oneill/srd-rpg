using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Transform target;

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position;
    }
}
