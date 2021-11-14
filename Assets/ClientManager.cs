using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;

public class ClientManager : MonoBehaviourPun
{
    [SerializeField]
    List<Transform> SpawnPointsT1;

    [SerializeField]
    List<Transform> SpawnPointsT2;

    [SerializeField]
    CameraController cameraController;

    [SerializeField]
    C controller;

    [SerializeField]
    UI_View UI;

    [SerializeField]
    Team myTeam;

    public Team MyTeam { get => myTeam; }

    void Start()
    {
        cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();


        myTeam = (Team)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
        
    }

    [PunRPC]
    void CreatePlayer()
    {
        M character;

        if(MyTeam==Team.A)
        {
            var randomSpawnpoint = GetRandomSpawnPoint(SpawnPointsT1);
            character = PhotonNetwork.Instantiate("Char A", randomSpawnpoint.position, Quaternion.identity).GetComponent<M>();

        }
        else
        {
             var randomSpawnpoint = GetRandomSpawnPoint(SpawnPointsT2);
             character = PhotonNetwork.Instantiate("Char B", randomSpawnpoint.position, Quaternion.identity).GetComponent<M>();
        }
        
        cameraController.SetCharacter(character);
        controller.SetCharacter(character);


        UI.SetHandlers(character);

        photonView.RPC("RegisterCharacter",RpcTarget.MasterClient,character.photonView.ViewID,PhotonNetwork.LocalPlayer);
    }
    Transform GetRandomSpawnPoint(List<Transform> spawnPoints)
    {

        bool PossibleSpawn = false;
        int randomIndex = 0;

        while (!PossibleSpawn)
        {
            randomIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
            var SphereOverlap = Physics.OverlapSphere(spawnPoints[randomIndex].position, 1f);

            PossibleSpawn = true;

            foreach (var Collider in SphereOverlap)
            {
                if (Collider.gameObject.GetComponent<M>() != null)
                {
                    PossibleSpawn = false;
                    print("collider contra otro muñeco");
                }
            }


        }


        return spawnPoints[randomIndex];
    }
    public Transform GetRandomReSpawnPoint(Team respawnerTeam)
    {
        var spawnPoints= SpawnPointsT1;

        if (Team.B==respawnerTeam)
        {
            spawnPoints = SpawnPointsT2;
        }
        
            

        bool PossibleSpawn = false;
        int randomIndex = 0;

        while (!PossibleSpawn)
        {
            randomIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
            var SphereOverlap = Physics.OverlapSphere(spawnPoints[randomIndex].position, 1f);

            PossibleSpawn = true;

            foreach (var Collider in SphereOverlap)
            {
                if (Collider.gameObject.GetComponent<M>() != null)
                {
                    PossibleSpawn = false;
                    print("collider contra otro muñeco");
                }
            }


        }


        return spawnPoints[randomIndex];
    }
}
