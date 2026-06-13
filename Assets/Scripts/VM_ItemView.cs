using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

// 상품 프리팹(Vending Machine Item)에 붙는 표현 컴포넌트.
// VM_UI가 Setup()으로 데이터를 꽂아주고, 클릭 콜백을 연결한다. (EventBus 직접 안 씀)
public class VM_ItemView : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private TMP_Text stockText;
    [SerializeField] private Image image;
    [SerializeField] private Button button;   // 프리팹에 Button 추가 필요 (지금 없음)

    public void Setup(ProductData product, Action onClick)
    {
        nameText.text = product.Name;
        priceText.text = $"{product.Price:N0} Won";   // "1,500 Won"
        stockText.text = $"{product.Stock} ea";        // "10 ea"

        Sprite sprite = Resources.Load<Sprite>(product.Name);   // 이미지는 name 기준
        if (sprite != null)
            image.sprite = sprite;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick());
    }

    // 구매 후 재고 표시 갱신용
    public void SetStock(int stock)
    {
        stockText.text = $"{stock} ea";
    }
}
