using UnityEditor;
using UnityEngine;

public class SpawnObj : MonoBehaviour
{
   
    public GameObject[] objects;
    public void Create(int randomIndex)
    {
        GameObject instanceR  = Instantiate(objects[randomIndex], transform.position, Quaternion.identity);
        instanceR.transform.parent = transform;
    }
    


}
