using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class ChatManager : MonoBehaviour,IChatClientListener
{
    public event Action OnSelect = delegate { };
    public event Action OnDeselect = delegate { };

    [SerializeField]
    public TextMeshProUGUI content;

    [SerializeField]
    public TMP_InputField inputField;

    [SerializeField]
    public ScrollRect scroll;

    [SerializeField]
    public int maxLines = 10;

    ChatClient _chatClient;

    int _currentChat;
    Dictionary<string, int> _chatDic = new Dictionary<string, int>();
    float _limitScrollAutomation = 0.2f;
    
    string[] _channels;
    string[] _chats;

    bool _chatOn = false;

    // Start is called before the first frame update
    void Start()
    {
        //Photon chat no inicializa si no esta conectado a photonNetwork, y sale de la funcion//
        if (!PhotonNetwork.IsConnected)
        {
            return;
        }


        _channels = new string[] { "World", PhotonNetwork.CurrentRoom.Name };
        _chats = new string[2];
        _chatDic["World"] = 0;
        _chatDic[PhotonNetwork.CurrentRoom.Name] = 1;
        print("chat start");
        _chatClient = new ChatClient(this);
        print(_chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat,
                            PhotonNetwork.AppVersion,
                            new AuthenticationValues(PhotonNetwork.LocalPlayer.NickName)));
     
        DontShowChat();
    }

    void DontShowChat()
    {
        _chatOn = false;

        content.GetComponent<CanvasRenderer>().SetAlpha(0.5f);
        scroll.GetComponent<CanvasRenderer>().SetAlpha(0);
        inputField.GetComponent<CanvasRenderer>().SetAlpha(0);
        inputField.interactable=false;

        OnDeselect();
    }

    void ShowChat()
    {
        _chatOn = true;

        content.GetComponent<CanvasRenderer>().SetAlpha(1);
        scroll.GetComponent<CanvasRenderer>().SetAlpha(1);
        inputField.GetComponent<CanvasRenderer>().SetAlpha(1);
        inputField.interactable = true;

        EventSystem.current.SetSelectedGameObject(inputField.gameObject);

        OnSelect();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            print("chatmanager/Aprentamos Enter");
            if (_chatOn)
            {
                SendChat();
                DontShowChat();
            }
            else
            {
                ShowChat();
            }
        }

        

        _chatClient.Service(); // Actualiza constantemente el chat
    }

    void UpdateChatUI ()
    {
        content.text = _chats[_currentChat];
        if (content.textInfo.lineCount >= maxLines)
        {
            StartCoroutine(WaitToDeleteLine());
        }
        if (scroll.verticalNormalizedPosition < _limitScrollAutomation)
        {
            StartCoroutine(WaitToScroll());
        }
    }

    IEnumerator WaitToScroll()
    {
        yield return new WaitForEndOfFrame();
        scroll.verticalNormalizedPosition = 0;
    }

    IEnumerator WaitToDeleteLine()
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < content.textInfo.lineCount - maxLines; i++)
        {
            var index = _chats[_currentChat].IndexOf("\n");
            _chats[_currentChat] = _chats[_currentChat].Substring(index + 1);
        }
        content.text = _chats[_currentChat];
    }
    public void SendChat()
    {
        print("chatmanager/sendchat");

        if (string.IsNullOrEmpty(inputField.text) || string.IsNullOrWhiteSpace(inputField.text)) return;

        string[] words = inputField.text.Split(' ');
        if(words[0] == "/w" && words.Length > 2)
        {
            _chatClient.SendPrivateMessage(words[1], string.Join(" ",words,2, words.Length-2));
        }
        else
        {
            _chatClient.PublishMessage(_channels[_currentChat], inputField.text);
        }
                
       
        inputField.text = "";
        //EventSystem.current.SetSelectedGameObject(null);
        //EventSystem.current.SetSelectedGameObject(inputField.gameObject);
    }
    public void DebugReturn(DebugLevel level, string message)
    {
        print("Level:"+level+".Debug Return:" + message);
    }

    public void OnChatStateChange(ChatState state)
    {
        print(state.ToString());
    }
    
    public void OnConnected()
    {
        print("Chat conectado");
        _chatClient.Subscribe(_channels);
    }

    public void OnDisconnected()
    {
        print("Chat desconectado");
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        string color;
        for (int i = 0; i < senders.Length; i++)
        {
            if(senders[i] == PhotonNetwork.NickName)
            {
                color = "<color=red>";
            }
            else
            {
                color = "<color=blue>";
            }
            int indexChat = _chatDic[channelName];
            _chats[indexChat] += color + senders[i] + ": " + "</color>"+ messages[i] + "\n";
        }
        UpdateChatUI();
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        for (int i = 0; i < _chats.Length; i++)
        {
            _chats[i] = "<color=orange>" + sender + ": " + "</color>" + message + "\n";
        }
        UpdateChatUI();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        for (int i = 0; i < channels.Length; i++)
        {
            _chats[0] += "<color=green>" + "Suscrito a los canales: "+ channels[i] + "</color>" +  "\n";
        }
        UpdateChatUI();
    }

    public void OnUnsubscribed(string[] channels)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    public void SelectChat()
    {
        OnSelect();
    }
    public void DeselectChat()
    {
        OnDeselect();
    }
}
