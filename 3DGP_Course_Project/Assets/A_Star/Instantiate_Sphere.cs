using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate_Sphere : MonoBehaviour
{
    //public GameObject Instantiate_Position;

    public GameObject Sphere;

    void Generate(Vector3 position)
    {

        Instantiate(Sphere, position, Quaternion.identity);

    }
}
