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
    GameObject Character;

    //[SerializeField]
    //float scrolSpeed = 20f;
    //[SerializeField]
    //float minY = 20f;
    //[SerializeField]
    //float maxY = 120f;

    private void Update()
    {
        Vector3 pos = transform.position;

        if(Input.GetKey(KeyCode.W)||Input.mousePosition.y >=Screen.height-panBorderThickness)
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <=  panBorderThickness)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= panBorderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.F1)|| Input.GetKey(KeyCode.Space))
        {
            pos.x = Character.transform.position.x;
            pos.z = Character.transform.position.z - 8f;
        }


        //float scroll = Input.GetAxis("Mouse ScrollWhell");
        //pos.y -= scroll * scrolSpeed * 100f * Time.deltaTime;


        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        //pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        transform.position= pos;
    }
}
