using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] Vector3 axis = Vector3.up;
    [SerializeField] float speed = 1;
    [SerializeField] Space space = Space.Self;

    // Start is called before the first frame update
    void Start()
    {
        axis.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(axis * speed * Time.deltaTime, space);
    }
}
