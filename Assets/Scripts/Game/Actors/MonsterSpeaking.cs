using Game.Actors;
using Core.GameServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpeaking : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Character>(out var character))
        {
            Debug.Log("FRED");
        }
    }
}
