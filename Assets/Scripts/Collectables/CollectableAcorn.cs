using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameProperties;

public class CollectableAcorn : MonoBehaviour
{
    [SerializeField]
    private Levels collectableLevelLocation;
    private void OnTriggerEnter(Collider acornCollider)
    {
        if (acornCollider.attachedRigidbody != null)
        {
            AcornCollector ac = acornCollider.attachedRigidbody.gameObject.GetComponent<AcornCollector>();
            if (ac)
            {
                this.gameObject.SetActive(false);
                ac.CollectAcorn(collectableLevelLocation);
            }
        }
    }
}
