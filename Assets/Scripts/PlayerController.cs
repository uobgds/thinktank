using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public static PlayerController Instance;

    private InputManager m_inputManager;

    private float m_health;

    private InputManager.RobotInput m_robotInput;

    [SerializeField]
    private float m_maxHealth;

    public float Health
    {
        get
        {
            return m_health;
        }
        set
        {
            m_health = Mathf.Clamp(value, 0f, m_maxHealth);
        }
    }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        Area.onAreaStay += OnAreaStay;

        m_inputManager = GetComponent<InputManager>();
        m_health = m_maxHealth;
    }

    private void Update()
    {
        Movement();
    }

    private void Movement ()
    {
        m_robotInput = m_inputManager.getRobotInput();

        transform.position = m_robotInput.position;
    }

    private void OnDestroy()
    {
        Area.onAreaStay -= OnAreaStay;
    }

    private void OnAreaStay(AreaDetails givenAreaDetails)
    {
        TakeDamage(givenAreaDetails.DamagePerSecond);
    }

    private void TakeDamage (float givenDamage)
    {
        Health -= givenDamage * Time.deltaTime;
        print(Health);
    }

}