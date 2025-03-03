using UnityEngine;
using UnityEngine.UI;

public class LogicManagerScript : MonoBehaviour
{
    public int shieldStrength = 5;
    public Text shieldStrengthText;

    [ContextMenu("Decrease Shield Strength")]
    public void decreaseShieldStrength(){
        shieldStrength = shieldStrength - 1;
        shieldStrengthText.text = shieldStrength.ToString();

        //will need to add function for gameover when shieldstrength = 0;
    }
}
