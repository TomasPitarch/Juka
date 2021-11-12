using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Voice.Unity;
using Photon.Voice.PUN;

public class MicUI : MonoBehaviour
{
    public Sprite micOn;
    public Sprite micOff;
    public Image micImage;
    Recorder _recorder;

    
    void Start()
    {
        _recorder = PhotonVoiceNetwork.Instance.PrimaryRecorder;
    }

    private void Update()
    {
        OnMicChange();
    }

    public void OnMicChange()
    {
        if (_recorder.TransmitEnabled)
        {
            micImage.sprite = micOn;
        }
        else
        {
            micImage.sprite = micOff;
        }
    }


}
