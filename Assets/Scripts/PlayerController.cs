using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public static PlayerController Instance;

    [SerializeField]
    private float m_speed;

    private float m_horizontal;
    private float m_vertical;
    private Vector2 m_translate;
    private float m_speedModifier;
    private float m_health;

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
    }

    private void Update()
    {
        Movement();
    }

    private void Movement ()
    {
        m_horizontal = Input.GetAxisRaw("Horizontal");
        m_vertical = Input.GetAxisRaw("Vertical");

        m_translate = new Vector2(m_horizontal, m_vertical);

        transform.position += new Vector3(m_translate.x, m_translate.y) * Time.deltaTime * m_speed;
    }

    private void TakeDamage ()
    {

    }

    private void OnDestroy()
    {
        Area.onAreaStay -= OnAreaStay;
    }

    private void OnAreaStay(AreaDetails givenAreaDetails)
    {
        m_speedModifier = givenAreaDetails.SpeedModifier;
        print(givenAreaType);
    }



}