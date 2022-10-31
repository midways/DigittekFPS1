using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fysiskverden
{
public class PlayerLook : MonoBehaviour
{
    Controls controls;
    public float musSens = 100f;
    float xRotation = 0f;
    public Transform spillerKrop;
    Vector2 musLook;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void OnEnable()
    {
        controls.Player.Enable();
    }

    void OnDisable()
    {
        controls.Player.Disable();
    }
    void Awake()
    {
        controls = new Controls();
    }

    // Update is called once per frame
    void Update()
    {
        musLook = controls.Player.Look.ReadValue<Vector2>();

        float musX = musLook.x * musSens * Time.deltaTime;
        float musY = musLook.y * musSens * Time.deltaTime;


        //Gør så man ikke kan lave backflips.
        xRotation -= musY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90);

        //Op og ned bevægelse
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        //Drejer hele spilleren højre eller venstre
        spillerKrop.Rotate(Vector3.up * musX);
    }
}
}
