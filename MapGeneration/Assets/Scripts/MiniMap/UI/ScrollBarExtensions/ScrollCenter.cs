using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollCenter : MonoBehaviour
{
    [SerializeField] private Scrollbar scrollbar;
    private void Start()
    {
        float horValue = scrollbar.size/2;
        scrollbar.value = 0.5f;
    }
    
}
