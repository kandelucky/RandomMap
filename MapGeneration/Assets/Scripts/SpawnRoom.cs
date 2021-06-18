using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoom : MonoBehaviour
{
    // aq xdeba moqmedeba levelis gzis generaciis shemdeg
    public LayerMask whatIsRoom;
    public LevelGeneration levelGen;
    void Update()
    {
        Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, whatIsRoom);
        if (roomDetection == null && levelGen.stopGeneration == true)
        {
            // SPAWN RANDOM ROOM !
            int rand = Random.Range(0, levelGen.freeSpaces.Length);
            Instantiate(levelGen.freeSpaces[rand], transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }
}
