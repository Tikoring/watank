using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject BGMSlider;
    public GameObject BGMText;
    public GameObject BGMVolumeText;
    public GameObject SFXSlider;
    public GameObject SFXText;
    public GameObject SFXVolumeText;
    public GameObject popupPanel;
    public GameObject quickButton;
    public void showMenu()
    {
        BGMSlider.SetActive(true);
        BGMText.SetActive(true);
        BGMVolumeText.SetActive(true);
        SFXSlider.SetActive(true);
        SFXText.SetActive(true);
        SFXVolumeText.SetActive(true);
        popupPanel.SetActive(true);
        quickButton.SetActive(true);

    }

    public void quickMenu()
    {
        BGMSlider.SetActive(false);
        BGMText.SetActive(false);
        BGMVolumeText.SetActive(false);
        SFXSlider.SetActive(false);
        SFXText.SetActive(false);
        SFXVolumeText.SetActive(false);
        popupPanel.SetActive(false);
        quickButton.SetActive(false);
    }

}
