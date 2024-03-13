using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyVisualCommand : MonoBehaviour
{
    
    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
