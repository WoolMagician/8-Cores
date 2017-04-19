using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BatteryCollectibleItem : BaseCollectibleItem
{
    public float batteryValue;

    [HideInInspector]
    public GameObject inventoryGameObject;

    [HideInInspector]
    public Inventory inventoryCopy;

    public bool inside;
    public Transform magnet;
    public float radius = 5f;
    public float force = 200f;

    private void Start()
    {
        base.type = Type.Battery;

        inventoryGameObject = GameObject.Find("Inventory");
        inventoryCopy = (Inventory)inventoryGameObject.GetComponent(typeof(Inventory));
        inventory = inventoryCopy;
        magnet = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        inside = false;
    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inside = false;
        }

        if (inside)
        {
            Vector3 magnetField = magnet.position - transform.position;
            float index = (radius - magnetField.magnitude) / radius;
            GetComponent<Rigidbody>().AddForce(force * magnetField * index);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (inventory.AddItem(this) == Inventory.ActionResult.Success)
            {
                inside = true;
                Destroy(this.gameObject);
            }
        }
    }
}
