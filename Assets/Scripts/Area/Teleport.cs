using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform _destination;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           // AudioManager.Instance.PlaySound("Magic Spell_Electricity Spell_1", true);
            other.gameObject.transform.position = new Vector3(_destination.position.x, _destination.position.y + 4, _destination.position.z);
        }
            
    }
}
