using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpDirectorRotate : MonoBehaviour
{
    private Vector3 mousePosition;

    // Update is called once per frame
    void Update()
    {
        MouseRotation();    
    }

    void MouseRotation()
    {
        //Get the Screen position of the mouse
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 rotation = mousePosition - transform.position;

        float rotationZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }
}
