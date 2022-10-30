using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Input Input { get; private set; }

    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if(Instance is not null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Input = new();

        Input._5K.Disable();
    }
}
