using UnityEngine;
using UnityEngine.UI;
using System;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;       
    public Rigidbody m_Shell;            
    public Transform m_FireTransform;    
    public Slider m_AimSlider;           
    public AudioSource m_ShootingAudio;  
    public AudioClip m_ChargingClip;     
    public AudioClip m_FireClip;         
    public float m_MinLaunchForce = 15f; 
    public float m_MaxLaunchForce = 30f; 
    public float m_MaxChargeTime = 0.75f;
    public GameObject m_enemyTank;
    public GameObject turret;
    public float distance;
    public float m_InitialShellSpeed = 1000.0f;

    private float fireRate = 0.0f;
    private float nextFire = 0.0f;
    private string m_FireButton;         
    private float m_CurrentLaunchForce;  
    private float m_ChargeSpeed;
    private float m_MinDistance = 0.0f;
    private bool m_Fired;                


    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }


    private void Start()
    {
        m_MinDistance = UnityEngine.Random.Range(35.0f, 45.0f);
        fireRate = UnityEngine.Random.Range(2.0f, 6.0f);

        m_FireButton = "Fire" + m_PlayerNumber;

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;

        if(m_PlayerNumber == 1)//PlayerNumber 1 is Red Tank 
        {
            m_enemyTank = GameObject.FindGameObjectWithTag("bluetank");
        }
        else if(m_PlayerNumber == 2)//PlayerNumber 1 is Blue Tank 
        {
            m_enemyTank = GameObject.FindGameObjectWithTag("redtank");
        }
 
    }
    

    private void Update()
    {
        distance = Vector3.Distance(m_enemyTank.transform.position, transform.position);

        turret.transform.LookAt(m_enemyTank.transform.position);

        if (distance < m_MinDistance) 
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                Fire();
            }
            
        }
        

        // Track the current state of the fire button and make decisions based on the current launch force.
        // The slider should have a default value of the minimum launch force.
        m_AimSlider.value = m_MinLaunchForce;

        // If the max force has been exceeded and the shell hasn't yet been launched...
        if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
        {
            // ... use the max force and launch the shell.
            m_CurrentLaunchForce = m_MaxLaunchForce;
            Fire();
        }
        // Otherwise, if the fire button has just started being pressed...
        else if (Input.GetButtonDown(m_FireButton))
        {
            // ... reset the fired flag and reset the launch force.
            m_Fired = false;
            m_CurrentLaunchForce = m_MinLaunchForce;

            // Change the clip to the charging clip and start it playing.
            m_ShootingAudio.clip = m_ChargingClip;
            m_ShootingAudio.Play();
        }
        // Otherwise, if the fire button is being held and the shell hasn't been launched yet...
        else if (Input.GetButton(m_FireButton) && !m_Fired)
        {
            // Increment the launch force and update the slider.
            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;

            m_AimSlider.value = m_CurrentLaunchForce;
        }
        // Otherwise, if the fire button is released and the shell hasn't been launched yet...
        else if (Input.GetButtonUp(m_FireButton) && !m_Fired)
        {
            // ... launch the shell.
            Fire();
        }

        
    }

    private void Fire()
    {
        // Set the fired flag so only Fire is only called once.
        m_Fired = true;

        float angle = 0.5f * Mathf.Asin((-Physics.gravity.y * distance) / (m_InitialShellSpeed * m_InitialShellSpeed));
        angle = angle * Mathf.Rad2Deg;

        m_FireTransform.localRotation = Quaternion.identity;
        m_FireTransform.localRotation = Quaternion.Euler(-angle, 0, 0);

        // Create an instance of the shell and store a reference to it's rigidbody.
        Rigidbody shellInstance =
            Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        // Set the shell's velocity to the launch force in the fire position's forward direction.
        shellInstance.velocity = m_InitialShellSpeed * m_FireTransform.forward;

        // Change the clip to the firing clip and play it.
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

    }

}