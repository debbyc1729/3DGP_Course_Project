using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (transform.parent.name == "MasterVolume")
        {
            transform.GetComponent<Slider>().value = GameManager.instance.getMasterVolume();
        }
        if (transform.parent.name == "BGMVolume")
        {
            transform.GetComponent<Slider>().value = GameManager.instance.getBGMVolume();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.parent.name == "MasterVolume")
        {
            GameManager.instance.setMasterVolume(transform.GetComponent<Slider>().value);
        }
        if (transform.parent.name == "BGMVolume")
        {
            GameManager.instance.setBGMVolume(transform.GetComponent<Slider>().value);
        }
    }
}
