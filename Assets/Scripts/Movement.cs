using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Camera mainCamera = null;
    [SerializeField] float movementSpeed = 1f;
    float pitch = 0f;
    float yaw = 0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Horizontal"))
        {
            print("Horizontal");

            transform.position = transform.position + Camera.main.transform.right * movementSpeed * Input.GetAxis("Mouse X") * Time.deltaTime;
        }
        if (Input.GetButton("Vertical"))
        {
            transform.position = transform.position + Camera.main.transform.forward * movementSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime;

        }

        pitch += Input.GetAxis("Mouse Y");
        yaw += Input.GetAxis("Mouse X");

        transform.eulerAngles = new Vector3(-pitch, yaw);


    }
}
