using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineMazeDemo : MonoBehaviour
{
    float moveSpeed;
    float rotateSpeed;

    void Start()
    {
        moveSpeed = 3f;
        rotateSpeed = 100f;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0f, -rotateSpeed * Time.deltaTime, 0f);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
        }
    }
}
