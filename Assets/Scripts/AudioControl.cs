using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioControl : MonoBehaviour
{
    // Start is called before the first frame update
    Text BGMAudioText;
    Text SFXAudioText;

    private void Start()
    {
        BGMAudioText = GetComponent<Text>();
        SFXAudioText = GetComponent<Text>();
    }

    public void BGMAudioUpdate(float value)
    {
        BGMAudioText.text = Mathf.RoundToInt(value * 100) + "%";
        AudioManager.Instance.setBGMVolume(value);
    }
    public void SFXAudioUpdate(float value)
    {
        SFXAudioText.text = Mathf.RoundToInt(value * 100) + "%";
        AudioManager.Instance.setSFXVolume(value);
    }

}
