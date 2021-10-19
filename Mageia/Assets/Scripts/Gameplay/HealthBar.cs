using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MageiaGame
{
    public class HealthBar : MonoBehaviour
    {
        public Character character;

        [Header("UI")]
        public Slider hpBar1;
        public Slider hpBar2;
        public TextMeshProUGUI hpText;

        //////////////////////////////////////////////////////////////////////
        //                                                                  //
        //                              Core                                //
        //                                                                  //
        //////////////////////////////////////////////////////////////////////

        void Update()
        {
            transform.LookAt(Camera.main.transform);

            if (hpBar1 != null && hpBar2 != null && hpText != null)
            {
                hpBar1.value = Mathf.Lerp(hpBar1.value, character.GetHealth() / character.GetMaxHealth(), Time.deltaTime * 5f);
                hpBar2.value = Mathf.Lerp(hpBar2.value, character.GetHealth() / character.GetMaxHealth(), Time.deltaTime * 5f);
                hpText.text = ((int)character.GetHealth()).ToString();
            }
        }
    }

}