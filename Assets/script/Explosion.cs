using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FirstGearGames.SmoothCameraShaker;

public class Explosion : MonoBehaviour
{
    public ShakeData explosionShakeData;

    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            CameraShakerHandler.Shake(explosionShakeData);
        }
    }
}
