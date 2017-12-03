using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Interactable_Object : MonoBehaviour {

    internal bool is_moneysink = false;
    internal bool cost_paid = false;
    public int coin_cost = 0;
    protected bool accepts_bank = false;

    internal bool single_use = false;
    internal bool used = false;

    internal bool requires_item = false;

    internal GameManager gm;

    // Use this for initialization
    protected virtual void Start() {
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update() {

    }

    internal virtual void AttemptInteract(Inventory inv) {
        bool success = false;
        if(is_moneysink) {
            if(requires_item) {
                if(CheckAndPay(inv)) {
                    success = true;
                    DoInteract(inv);
                }
            } else {
                if(pay_cost(inv)) {
                    success = true;
                    DoInteract(inv);
                }
            }
        } else if(requires_item) {
            if(CheckItem(inv)) {
                success = true;
                DoInteract(inv);
            }
        }
        if(!success) {
            DoDefaultInteract(inv);
        }
        gm.ui.UpdateUI();
    }

    internal virtual bool pay_cost(Inventory inv) {
        if(!is_moneysink) {
            return false;
        }
        if(cost_paid && single_use) {
            if(!used) {
                return true;
            }
            return false;
        }
        if(accepts_bank) {
            if(!gm.SpendBankedCoins(coin_cost)) {
                return false;
            }
        } else {
            if(inv.coins < coin_cost) {
                return false;
            }
            inv.AdjustCoins(-coin_cost);
        }
        if(single_use) {
            cost_paid = true;
        }
        return true;
    }

    internal virtual bool CheckItem(Inventory inv) {
        if(!requires_item) {
            return false;
        }
        if(!inv.heldObject) {
            return false;
        }
        if(!inv.heldObjectScript) {
            return false;
        }
        if(single_use && used) {
            return false;
        }
        return true;
    }

    internal virtual bool CheckAndPay(Inventory inv) {
        if(!CheckItem(inv)) {
            return false;
        }
        if(!pay_cost(inv)) {
            return false;
        }
        if(single_use) {
            cost_paid = true;
        }
        return true;
    }

    internal virtual void DoInteract(Inventory inv) {

    }

    internal virtual void DoDefaultInteract(Inventory inv) {

    }
}

