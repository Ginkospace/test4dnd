using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerUIManager : MonoBehaviour
{
    public FourDPlayer plr;
    public TextMeshProUGUI text;
    string headState, leftArmState, rightArmState, leftLegState, rightLegState;

    void Update()
    {
        if (plr.combat.head == true) { headState = "just fine"; } else { headState = "mydriasis"; }
        if (plr.combat.left_arm == true) { leftArmState = "usable"; } else { leftArmState = "lost"; }
        if (plr.combat.right_arm == true) { rightArmState = "usable"; } else { leftArmState = "lost"; }
        if (plr.combat.left_leg == true) { leftLegState = "unsteady"; } else { leftArmState = "lost"; }
        if (plr.combat.right_leg == true) { rightLegState = "movable"; } else { leftArmState = "lost"; }


        text.text = "hp: " + plr.combat.HP + " /" + plr.combat.maxHP +
        "\ndefense: " + plr.combat.defense +
        "\nstrength: " + plr.combat.power + " (" + plr.combat.maxPower + ")" +
        "\nhealing: " + plr.combat.healing +
        "\nfirmness: " + plr.combat.firmness +
        "\nstability: " + plr.combat.stability +
        "\nperception: " + plr.combat.perception +
        "\ntemperature: 18 °e" + //plr.combat.temperature +

        "\n\nhead: " + headState +
        "\nleft arm: " + leftArmState +
        "\nright arm: " + rightArmState +
        "\nleft leg: " + leftLegState +
        "\nright leg: " + rightLegState;
    }
}
