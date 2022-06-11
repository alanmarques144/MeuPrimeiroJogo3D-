using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float forceMultiplier = 3f;
    [SerializeField]
    private float maximumVelocity = 3f;
    [SerializeField]
    private ParticleSystem deathParticles;
    
    private CinemachineImpulseSource cinemachineImpulseSource;
    

    // Start is called before the first frame update
    void Start()
    {
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance == null)
        {
            return; 
        }

        float horizontalInput = 0;
        
        if (Input.GetMouseButton(0))
        {
            var center = Screen.width/2;
            var mousePosition = Input.mousePosition;
            if (mousePosition.x > center)
            {
                horizontalInput = 1;
            }
            else if (mousePosition.x < center)
            {
                horizontalInput = -1;
            }
        }
        else
        {
            horizontalInput = Input.GetAxis("Horizontal");
        }    

        if(GetComponent<Rigidbody>().velocity.magnitude <= maximumVelocity)
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(horizontalInput * forceMultiplier * Time.deltaTime, 0, 0));
        }
    }

    private void OnEnable() 
    {
        transform.position = new Vector3(0, 0.75f, 0); 
        Vector3 rotationZero = new Vector3(0,0,0); 
        transform.rotation = Quaternion.identity;
        GetComponent<Rigidbody>().velocity = Vector3.zero;  
    }

    private void  OnCollisionEnter(Collision collision) {

        if (collision.gameObject.CompareTag("Hazard"))
        {
            GameOver();
            Instantiate(deathParticles, transform.position, Quaternion.identity);
            cinemachineImpulseSource.GenerateImpulse();
        }

    }
    private void OnTriggerExit(Collider other) {
        
    
        if (other.CompareTag("FallDown"))
        {
            GameOver();
        }    
    }

    private void GameOver()
    {
        GameManager.Instance.GameOver();
            gameObject.SetActive(false);
    }
}              
