using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderCard : MonoBehaviour
{
    public Customer.MyOrder Order { get; private set; }
    public Customer FromCustomer { get; private set; }
    [HideInInspector]
    public RectTransform RectTransform { get => (RectTransform)transform; }

    public void SetInfo(Customer.MyOrder order, Customer from = null)
    {
        Order = order;
        FromCustomer = from;
    }

    public void Move(Vector2 position, bool relative)
    {
        if (relative)
        {
            transform.position += new Vector3(position.x, position.y);
        }
        else
        {
            transform.position = new Vector3(position.x, position.y);
        }
    }
}
