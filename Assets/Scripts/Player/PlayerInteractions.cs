/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField]
    private string _useButton = "Use";
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.GetComponentInParent<IUseable>() != null)
        {
            if(Input.GetButtonDown(_useButton))
            collision.GetComponentInParent<PlayerUseable>().Use(gameObject);
        }
    }
}*/
