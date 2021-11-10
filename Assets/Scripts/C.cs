using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C : MonoBehaviour
{
    [SerializeField]
    LayerMask GroundLayer;

    [SerializeField]
    LayerMask SkillLayer;

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
//                Model.MoveRequest(Point);
            }
        }

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

    }

    internal void SetCharacter(M character)
    {
        Model = character;
    }

    
}
