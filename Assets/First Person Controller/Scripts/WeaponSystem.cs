using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour {
    [SerializeField] private Transform weapon;
    
    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
        
            Debug.Log(Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)));
        
        }
    }
}
