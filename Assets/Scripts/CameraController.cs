using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float panSpeed = 20f;
    [SerializeField]
    float panBorderThickness = 10f;
    [SerializeField]
    Vector2 panLimit;

    [SerializeField]
    M Character;

    bool _cameraLock = false;


    Vector3 AuxiliarPos;

    //[SerializeField]
    //float scrolSpeed = 20f;
    //[SerializeField]
    //float minY = 20f;
    //[SerializeField]
    //float maxY = 120f;

    private void Update()
    {
        AuxiliarPos = transform.position;

        if(Input.mousePosition.y >=Screen.height-panBorderThickness)
        {
            AuxiliarPos.z += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <=  panBorderThickness)
        {
            AuxiliarPos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            AuxiliarPos.x += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= panBorderThickness)
        {
            AuxiliarPos.x -= panSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.F1)|| Input.GetKey(KeyCode.Space))
        {
            CameraFollowPlayerBehaviour();
        }
        if (Input.GetKeyUp(KeyCode.Y))
        {
            _cameraLock = !_cameraLock;
        }

        if(_cameraLock)
        {
            CameraFollowPlayerBehaviour();
        }

        //float scroll = Input.GetAxis("Mouse ScrollWhell");
        //pos.y -= scroll * scrolSpeed * 100f * Time.deltaTime;


        AuxiliarPos.x = Mathf.Clamp(AuxiliarPos.x, -panLimit.x, panLimit.x);
        //pos.y = Mathf.Clamp(pos.y, minY, maxY);
        AuxiliarPos.z = Mathf.Clamp(AuxiliarPos.z, -panLimit.y, panLimit.y);

        transform.position = AuxiliarPos;
    }


    void CameraFollowPlayerBehaviour()
    {
        AuxiliarPos.x = Character.transform.position.x;
        AuxiliarPos.z = Character.transform.position.z - 8f;
    }
    public void SetCharacter(M characterToSet)
    {
        Character = characterToSet;
    }
    
}
