using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Voice.Unity;
using Photon.Voice.PUN;

public class C : MonoBehaviourPun
{
    [SerializeField]
    LayerMask GroundLayer;

    [SerializeField]
    LayerMask SkillLayer;

    [SerializeField]
    M Model;

    Vector3 Point;

    bool _isLocked;
    Recorder _recorder;


    private void Start()
    {
        if (photonView.IsMine)
        {
            var chatManager = FindObjectOfType<ChatManager>();
            if (chatManager)
            {
                chatManager.OnSelect += () => _isLocked = true;
                chatManager.OnDeselect += () => _isLocked = false; //Lambda
            }

            //_recorder = PhotonVoiceNetwork.Instance.PrimaryRecorder;
        }

    }

    void Update()
    {
       
        //Input de movimiento//
        if (Input.GetMouseButtonUp(1))
        {
            if (Utility.GetPointUnderCursor(GroundLayer, out Point))
            {
                  Model.Move(Point);
//                Model.MoveRequest(Point);
            }
        }

        //Si el chat esta activado no podemos hacer ninguna Accion mas que movernos,
        //o volver a desactivar el chat//
        if (_isLocked)
        {
            return;
        }

        //Input de skills//
        if (Input.GetKeyUp(KeyCode.Q))
        {
            Utility.GetPointUnderCursor(SkillLayer, out Point);


            Model.Skill1(Point);
            //Model.Skill1Request(Point);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {

            Utility.GetPointUnderCursor(SkillLayer, out Point);
            Model.Skill2(Point);
            //Model.Skill2Request(Point);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            Model.Skill3();
            //Model.Skill3Request();
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            Model.Skill4();
            //Model.Skill4Request();
        }

        //Input de Voice//
        if (_recorder != null)
        {
            if (Input.GetKey(KeyCode.V))
            {
                _recorder.TransmitEnabled = true;
            }
            else
            {
                _recorder.TransmitEnabled = false;
            }
        }

    }

    internal void SetCharacter(M character)
    {
        Model = character;
    }

}
