using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float hp = 100f;

    public static event Action onPlayerDeath;

    private void OnValidate()
    {
        if (hp < 0)
        {
            onPlayerDeath?.Invoke();
        }
    }

}
