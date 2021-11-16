using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Voice.Unity;
using Photon.Voice.PUN;
using Photon.Realtime;

public class C : MonoBehaviourPunCallbacks
{
    [SerializeField]
    LayerMask GroundLayer;

    [SerializeField]
    LayerMask SkillLayer;

    [SerializeField]
    M Model;

    Vector3 Point;

    bool _isLocked;

    [SerializeField]
    Recorder _recorder;

   

    private void Awake()
    {
        PhotonVoiceNetwork.Instance.PrimaryRecorder = _recorder;

    }
    private void Start()
    {
      
            var chatManager = FindObjectOfType<ChatManager>();
            if (chatManager)
            {
                chatManager.OnSelect += () => _isLocked = true;
                chatManager.OnDeselect += () => _isLocked = false; //Lambda
            }

        OnPUNJoin();


        if ((Team)PhotonNetwork.LocalPlayer.CustomProperties["Team"] == Team.A)
        {
            print("seteo el recorder en grupo de interes 1");
            _recorder.InterestGroup = 1;

        }
        else
        {

            print("seteo el recorder en grupo de interes 2");
            _recorder.InterestGroup = 2;

        }
    }

    async void OnPUNJoin()
    {
        print("arranmco la funcion async");

        while( PhotonVoiceNetwork.Instance.ClientState != ClientState.Joined)
        {
            await Task.Yield();
        }

        print("el cliente pun joinio");


        if ((Team)PhotonNetwork.LocalPlayer.CustomProperties["Team"] == Team.A)
        {
            print("OpChangeGroups(null, new byte[1] { (byte)1 })");

            PhotonVoiceNetwork.Instance.Client.OpChangeGroups(null, new byte[1] { (byte)1 });

        }
        else
        {
            print("OpChangeGroups(null, new byte[1] { (byte)2 })");
            PhotonVoiceNetwork.Instance.Client.OpChangeGroups(null, new byte[1] { (byte)2 });

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
            print("hay recorder");
            if (Input.GetKey(KeyCode.V))
            {

                print("V apretada, recorder transmision habilitada");
                _recorder.TransmitEnabled = true;
            }
            else
            {
                print("V soltada, recorder transmision deshabilitado");
                _recorder.TransmitEnabled = false;
            }
        }

    }

    internal void SetCharacter(M character)
    {
        Model = character;
    }

}
