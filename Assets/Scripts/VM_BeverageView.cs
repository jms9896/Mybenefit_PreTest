using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

// 음료 프리팹(Beverage)에 붙는 표현 컴포넌트. 인벤토리에 적재되며 클릭 시 소비.
public class VM_BeverageView : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Button button;   // 프리팹의 Drink Button

    public void Setup(ProductData beverage, Action onClick)
    {
        nameText.text = beverage.Name;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick());
    }
}
