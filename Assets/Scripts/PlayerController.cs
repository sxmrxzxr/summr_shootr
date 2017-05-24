using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed = 5f;
    private PlayerMotor motor;
    private float lookSensetivity = 3f;

    void Start()
    {
        motor = GetComponent<PlayerMotor>();
    }

    void Update()
    {
        if (PauseMenu.isOn)
            return;

        // calculate movement velocity as a 3d vector
        float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");

        Vector3 movHorizontal = transform.right * xMov;
        Vector3 movVertical = transform.forward * zMov;

        // final velocity vector
        Vector3 velocity = (movHorizontal + movVertical).normalized * speed;

        motor.Move(velocity);

        // calculate rotation
        float yRot = Input.GetAxisRaw("Mouse X");

        Vector3 rotation = new Vector3(0f, yRot, 0f) * lookSensetivity;

        // apply rotation
        motor.Rotate(rotation);

        // calculate camera rotation
        float xRot = Input.GetAxisRaw("Mouse Y");

        Vector3 cameraRotation = new Vector3(xRot, 0f, 0f) * lookSensetivity;

        // apply rotation
        motor.RotateCamera(cameraRotation);
    }

}
