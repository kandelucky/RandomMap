using UnityEngine;

public class SpawnObj : MonoBehaviour
{
    // skripti sachiroa ert wertilze winaswar mititebuli obieqtebidan shemtxvevit archeuli obieqtis shesaqmnelad.
    // magalitad bevri xeebidan mxolod erti
    // bevri qvevit mimavali gzebidan mxolo erti.
    public GameObject[] objects;
    void Awake()
    {
        int rand = Random.Range(0, objects.Length);
        GameObject instance  = Instantiate(objects[rand], transform.position, Quaternion.identity);
        instance.transform.parent = transform;
    }

    
}
