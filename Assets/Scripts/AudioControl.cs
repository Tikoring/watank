using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioControl : MonoBehaviour
{
    // Start is called before the first frame update
    Text AudioText;

    private void Start()
    {
        AudioText = GetComponent<Text>();
    }

    public void AudioUpdate(float value)
    {
        AudioText.text = Mathf.RoundToInt(value * 100) + "%";
        AudioManager.Instance.setVolume(value);
    }
}
