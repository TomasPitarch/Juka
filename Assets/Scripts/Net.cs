using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed;
  

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up,rotationSpeed*Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
