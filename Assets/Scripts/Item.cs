using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Item : MonoBehaviour
{

    internal string item_name = "Item";
    internal bool is_consumable = false;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    internal virtual void OnInteract(Inventory inv) {
        inv.DefaultInteract();
    }

    internal virtual void OnDrop(GameObject dropper) {

    }

    internal virtual bool OnPickup(GameObject holder, Inventory inv) {
        if(is_consumable) {
            Consume(holder, inv);
            return false;
        }
        inv.heldObject = this.gameObject;
        inv.heldObjectScript = this;
        this.gameObject.SetActive(false);
        return true;
    }

    internal virtual void Consume(GameObject consumer, Inventory inv) {
        gameObject.SetActive(false);
    }

}
