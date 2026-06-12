using UnityEngine;
using System.Collections.Generic;

public class VM_Data
{
    #region fields
    // ── 상태 필드 ──
    private ItemsData itemsData; // Json 파싱한 결과들

    private int balance;
    public int Balance => balance;

    private List<ProductData> beverages = new();
    public List<ProductData> Beverages => beverages;

    // ── 조회 (읽기) ──
    public List<ProductData> Products => itemsData.Products;
    public string MachineId => itemsData.MachineId;
    public string Status => itemsData.Status;

    #endregion

    // ── 로드 ──
    public void LoadItemsData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Items");
        if (jsonFile == null) // 예외처리
        {
            Debug.LogError("[VM_Data] jsonFile is null!");
            return;
        }
        else
        {
            itemsData = JsonUtility.FromJson<ItemsData>(jsonFile.text);
        }
    }

    // ── 변경 통로 (쓰기) ──  
    // [TODO: Engine 단계에서 시그니처 확정하며 추가]
    //   AddBalance(amount)            — 잔액 증감, 상한 10,000 적용
    //   DecreaseStock(id)             — 구매 성공 시 재고 -1
    //   AddBeverage / RemoveBeverage  — 구매 시 추가 / 소비 시 제거
    //   Find(id)                      — 상품 id로 ProductData 찾기
}

/* 데이터 형식 참고
  "machineId": "VM-2026-A1",
  "status": "active",
  "updatedAt": "2026-04-20T15:00:00Z",
  "products": [
    {
      "id": 101,
      "name": "Energy Drink",
      "price": 1500,
      "stock": 10,
      "type": "drink",
      "imageUrl": "images/energy_drink"
    },
    ...
*/

// ItemsData — JSON 루트 매칭 클래스
[System.Serializable]
public class ItemsData
{
    // 데이터 부분이므로, private로 변수 설정후 프로퍼티로 처리
    [SerializeField] private string machineId;
    [SerializeField] private string status;
    [SerializeField] private string updatedAt;
    [SerializeField] private List<ProductData> products;

    public string MachineId => machineId;
    public string Status => status;
    public string UpdatedAt => updatedAt;
    public List<ProductData> Products => products;
}

// ProductData - JSON 아이템(product) 매칭 클래스
[System.Serializable]
public class ProductData
{
    [SerializeField] private int id;
    [SerializeField] private string name;
    [SerializeField] private int price;
    [SerializeField] private int stock;
    [SerializeField] private string type;
    [SerializeField] private string imageUrl;

    public int Id => id;
    public string Name => name;
    public int Price => price;
    public int Stock => stock;



}
