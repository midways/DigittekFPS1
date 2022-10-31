using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Fysiskverden
{
public class Player : MonoBehaviour
{
    Controls handlinger;
    public Rigidbody projectile;
    Rigidbody playerRigidbody;

    private float hastighed = 25F;
    public float walkHastighed = 10f;

    Vector2 moveInput;


    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    
    }
    void Awake()
    {
        handlinger = new Controls();

        handlinger.Player.Skyd.performed += ctx => Skyd();
        handlinger.Player.LukSpil.performed += ctx => LukSpillet();
    }

    //Lukker spillet når kaldt
    void LukSpillet() {
        Application.Quit();
        Debug.Log("Spil Lukket");
    }

    void OnEnable()
    {
        handlinger.Player.Enable();
    }

    void OnDisable()
    {
        handlinger.Player.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
    }

    //Instantiater et nyt skud i den direktion som kameraet kigger i.
    void Skyd() {
        print("skud");
        Rigidbody instantiatedProjectile = Instantiate(projectile, transform.position + Camera.main.transform.forward, transform.rotation)
        as Rigidbody;
        instantiatedProjectile.velocity = Camera.main.transform.forward * hastighed;
        Destroy(instantiatedProjectile.gameObject, 3f);
    }

    //Bevægelse.
    void Run()
    {
        Vector3 playerVelocity = new Vector3(moveInput.x * walkHastighed, playerRigidbody.velocity.y, moveInput.y * walkHastighed);
        playerRigidbody.velocity = transform.TransformDirection(playerVelocity);
    }
    void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>();
    }

}
}
