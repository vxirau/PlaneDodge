using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;

    private float baseSpeed = 10.0f;
    private float rotSpeedX = 3.0f;
    private float rotSpeedY = 1.5f;

    private float deathTime;
    private float deathDuration = 2;

    public GameObject deathExplosion;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        GameObject trail = Instantiate(Manager.Instance.playerTrails[SaveManager.Instance.state.activeTrail]);

        trail.transform.SetParent(transform.GetChild(0));

        trail.transform.localEulerAngles = Vector3.forward * -90f;
    }

    private void Update()
    {
        if (deathTime != 0)
        {
            if (Time.time - deathTime > deathDuration)
            {
                SceneManager.LoadScene("Game");
            }

            return;
        }

        Vector3 moveVector = transform.forward * baseSpeed;

        Vector3 inputs = Manager.Instance.GetPlayerInput();

        Vector3 yaw = inputs.x * transform.right * rotSpeedX * Time.deltaTime;
        Vector3 pitch = inputs.y * transform.up * rotSpeedY * Time.deltaTime;
        Vector3 dir = yaw + pitch;

        float maxX = Quaternion.LookRotation(moveVector + dir).eulerAngles.x;

        if (maxX < 90 && maxX > 70 || maxX > 270 && maxX < 290)
        {
        }
        else
        {
            moveVector += dir;

            transform.rotation = Quaternion.LookRotation(moveVector);
        }

        controller.Move(moveVector * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        deathTime = Time.time;

        GameObject go = Instantiate(deathExplosion) as GameObject;
        go.transform.position = transform.position;

        transform.GetChild(0).gameObject.SetActive(false);

    }
}
