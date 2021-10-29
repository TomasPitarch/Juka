using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C : MonoBehaviour
{
    [SerializeField]
    LayerMask GroundLayer;

    [SerializeField]
    M Model;

    Vector3 Point;

    void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            if (Utility.GetPointUnderCursor(GroundLayer, out Point))
            {
                Model.Move(Point);
            }
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            Utility.GetPointUnderCursor(GroundLayer, out Point);
            Model.Skill1(Point);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            Utility.GetPointUnderCursor(GroundLayer, out Point);
            Model.Skill2(Point);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            Model.Skill3();
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            Model.Skill4();
        }

    }
}
