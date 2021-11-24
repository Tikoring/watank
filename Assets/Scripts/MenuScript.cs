using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject AudioSlider;
    public GameObject VolumeText;
    public void showMenu()
    {
        bool isActive = AudioSlider.activeSelf;

        AudioSlider.SetActive(!isActive);
        VolumeText.SetActive(!isActive);
    }

}
