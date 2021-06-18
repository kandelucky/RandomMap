using System;
using UnityEngine;

// code by https://github.com/jinincarnate/unity-scrollbar-buttons

public class ScrollElementsContainer : MonoBehaviour
{
    public Action OnContainerChildrenChanged;

    private void OnTransformChildrenChanged()
    {
        if(OnContainerChildrenChanged != null)
        {
            OnContainerChildrenChanged.Invoke();
        }
    }
}
