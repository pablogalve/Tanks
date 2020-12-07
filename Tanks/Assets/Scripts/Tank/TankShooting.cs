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
    public GameObject m_EnemyTank;
    public GameObject m_Turret;
    public float m_Distance;
    public float m_InitialShellSpeed = 1000.0f;
    public int shell_num = 2;
    public int shell_recharge = 2;

    private float m_FireRate = 0.0f;
    private float m_NextFire = 0.0f;
    private string m_FireButton;
    private float m_CurrentLaunchForce;
    private float m_ChargeSpeed;
    private float m_MinDistance = 0.0f;
    private bool m_Fired;

    TankMovement tankMovement;

    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }


    private void Start()
    {
        tankMovement = GetComponent<TankMovement>();

        m_MinDistance = UnityEngine.Random.Range(35.0f, 45.0f);
        m_FireRate = UnityEngine.Random.Range(2.0f, 6.0f);

        m_FireButton = "Fire" + m_PlayerNumber;

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;

        if (m_PlayerNumber == 1)//PlayerNumber 1 is Red Tank 
        {
            m_EnemyTank = GameObject.FindGameObjectWithTag("bluetank");
        }
        else if (m_PlayerNumber == 2)//PlayerNumber 1 is Blue Tank 
        {
            m_EnemyTank = GameObject.FindGameObjectWithTag("redtank");
        }
    }

    private void Update()
    {
        m_Distance = Vector3.Distance(m_EnemyTank.transform.position, transform.position);

        m_Turret.transform.LookAt(m_EnemyTank.transform.position);

        if (m_Distance < m_MinDistance )
        {
            if (Time.time > m_NextFire)
            {
                m_NextFire = Time.time + m_FireRate;
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

        if(shell_num <= 0){
            //Return to base to recharge
            tankMovement.GoToBase();
        }
    }

    private void Fire()
    {
        if(shell_num > 0){
            shell_num--;

            // Set the fired flag so only Fire is only called once.
            m_Fired = true;

            float angle = 0.5f * Mathf.Asin((-Physics.gravity.y * m_Distance) / (m_InitialShellSpeed * m_InitialShellSpeed));
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
}