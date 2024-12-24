using UnityEngine;

public class score : MonoBehaviour
{
    // TODO: add actual scoreing 
    public LayerMask targetLayer; 

    private void OnTriggerEnter(Collider other)
    {
        if ((targetLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            Destroy(other.gameObject);
        }
    }
}
