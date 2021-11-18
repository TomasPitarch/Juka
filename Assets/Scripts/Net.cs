using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Photon.Pun;
using UnityEngine;

public class Net : MonoBehaviourPun
{
    
    public event Action OnObjectCollision =delegate{ };

    [SerializeField]
    float rotationSpeed;

    float skillSpeed=0;
    float lifeTime = 0;

    Vector3 skillDirection=Vector3.zero;

    int CasterID;
    public Team team;

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        transform.Rotate(Vector3.up,rotationSpeed*Time.deltaTime);
        transform.position += skillDirection * Time.deltaTime * skillSpeed;
    }

    public void Init(Vector3 position, Vector3 direction,float speed,float newLifeTime,int CasterID)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        SetDirectionAndSpeed(direction, speed);
        SetPosition(position);
        SetLifeTime(newLifeTime);

        this.CasterID = CasterID;

        SkillLifeTime();
    }

    void SetLifeTime(float newLifeTime)
    {
        lifeTime = newLifeTime;
    }

    void SetDirectionAndSpeed(Vector3 direction,float speed)
    {
        var NormalizedDir = direction.normalized;
        skillDirection = NormalizedDir;
        skillSpeed = speed;
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if (other.gameObject.tag == "Character")
        {

            var CharacterTraped = other.gameObject.GetComponent<M>();
            var charPV_ID = CharacterTraped.photonView.ViewID;
            var playerTraped= ServerManager.Instance.GetPlayer(charPV_ID);

            var Caster = PhotonView.Find(CasterID).GetComponent<M>();

            //int[] IDs = new int[] { charPV_ID, CasterID };
            if(CharacterTraped.photonView.ViewID==CasterID)
            {
                print("la red le pego al dueño");
                return;
            }
            else if(CharacterTraped.myTeam==Caster.myTeam)
            {
                print("mismo equipo, no hago nada");
            }
            else
            {
                CharacterTraped.photonView.RPC("CatchedByNet", playerTraped, CasterID);
            }

            OnObjectCollision();
        }
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    async void SkillLifeTime()
    {
        await Task.Delay((int)(lifeTime * 1000));
        Destroy();
    }

    private void Destroy()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        PhotonNetwork.Destroy(gameObject);
    }
}
