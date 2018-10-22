using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public static PlayerController Instance;

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

    private void OnDestroy()
    {
        Area.onAreaStay -= OnAreaStay;
    }

    private void OnAreaStay(Area.AreaType givenAreaType)
    {
        print(givenAreaType);
    }

}