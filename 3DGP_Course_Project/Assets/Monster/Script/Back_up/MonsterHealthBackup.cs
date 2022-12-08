using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthBackup : MonoBehaviour
{
    public float Health;
    public float MixHealth;
    public GameObject HealthBar;
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        Health = MixHealth;
        slider.value = ComputeHealth();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = ComputeHealth();
        if (Health < MixHealth) {
            HealthBar.SetActive(true);
        }

        if (Health < 0) {
            //monster be killed
        }

        if (Health > MixHealth)
        {
            Health = MixHealth;
        }
    }

    float ComputeHealth() {
        return Health / MixHealth;
    }
}
