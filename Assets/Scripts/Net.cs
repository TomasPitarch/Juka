using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class Net : MonoBehaviour
{
    
    public event Action OnObjectCollision;

    [SerializeField]
    GameObject NetStatusPrefab;

    [SerializeField]
    float rotationSpeed;

    float skillSpeed=0;
    float lifeTime = 0;

    Vector3 skillDirection=Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up,rotationSpeed*Time.deltaTime);
        transform.position += skillDirection * Time.deltaTime * skillSpeed;
    }

    public void Init(Vector3 position, Vector3 direction,float speed,float newLifeTime)
    {
        SetDirectionAndSpeed(direction, speed);
        SetPosition(position);
        SetLifeTime(newLifeTime);
        SkillLifeTime();
    }

    private void SetLifeTime(float newLifeTime)
    {
        lifeTime = newLifeTime;
    }

    void SetDirectionAndSpeed(Vector3 direction,float speed)
    {
        var NormalizedDir = direction.normalized;
        skillDirection = NormalizedDir;
        skillSpeed = speed;
    }
    private void OnCollisionEnter(Collision collision)
    {
        print("colision de red con :" + collision.gameObject.name);
        if (collision.gameObject.tag=="Character")
        {
            collision.gameObject.GetComponent<M>().CatchedByNet(NetStatusPrefab);
            OnObjectCollision();
            Destroy();
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
        Destroy(gameObject);
    }
}
