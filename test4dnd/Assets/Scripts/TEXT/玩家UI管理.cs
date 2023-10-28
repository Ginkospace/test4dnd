using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class 玩家UI管理 : MonoBehaviour
{
    public FourDPlayer plr;
    public TextMeshProUGUI text;
    string 頭狀態, 左臂狀態, 右臂狀態, 左腿狀態, 右腿狀態;

    void Update()
    {
        if (plr.combat.head == true) { 頭狀態 = "還好吧"; } else { 頭狀態 = "瞳孔散大"; }
        if (plr.combat.left_arm == true) { 左臂狀態 = "還能用"; } else { 左臂狀態 = "丟了"; }
        if (plr.combat.right_arm == true) { 右臂狀態 = "還能打"; } else { 右臂狀態 = "丟了"; }
        if (plr.combat.left_leg == true) { 左腿狀態 = "還能走"; } else { 左腿狀態 = "丟了"; }
        if (plr.combat.right_leg == true) { 右腿狀態 = "還能游"; } else { 右腿狀態 = "丟了"; }


        text.text = "命: " + plr.combat.HP + " /" + plr.combat.maxHP +
        "\n防: " + plr.combat.defense +
        "\n力: " + plr.combat.power + " (" + plr.combat.maxPower + ")" +
        "\n癒: " + plr.combat.healing +
        "\n毅: " + plr.combat.firmness +
        "\n心: " + plr.combat.stability +
        "\n察: " + plr.combat.perception +
        "\n熱: 18度" + //plr.combat.temperature +

        "\n\n頭: " + 頭狀態 +
        "\n臂:" + "左:" + 左臂狀態 + " 右:" + 右臂狀態 +
        "\n腿:" + "左:" + 左腿狀態 + " 右:" + 右腿狀態;
    }
}
