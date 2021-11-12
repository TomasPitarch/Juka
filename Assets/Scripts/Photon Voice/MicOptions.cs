using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Voice.Unity;

public class MicOptions : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public Recorder recorder;

    
    void Start()
    {
        
        string[] devices = Microphone.devices;
        if (devices.Length != 0)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < devices.Length; i++)
            {
                list.Add(devices[i]);
            }
            dropdown.AddOptions(list);
            SetMic(0);
        }
                
    }

    public void SetMic(int i)
    {
        string[] devices = Microphone.devices;
        if (devices.Length > i)
        {
            recorder.UnityMicrophoneDevice = devices[i];
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
