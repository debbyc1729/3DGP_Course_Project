using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBotton : MonoBehaviour
{
    public GameObject Options;
    public void Play()
    {

    }
    public void Resume()
    {

    }
    public void Option()
    {
        Options.SetActive(true);
    }
    public void BackFromOptions()
    {
        Options.SetActive(false);
    }
    public void TryAgain()
    {

    }
    public void BackToTitle()
    {

    }
    public void Quit()
    {

    }
}
