using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class VM_UI : MonoBehaviour
{
    private VM_EventBus bus;
    private VM_Data data;

    [Header("상단 정보")]
    [SerializeField] private TMP_Text idText;          // machineId
    [SerializeField] private TMP_Text moneyText;       // 잔액 (Current Money)
    [SerializeField] private Image powerLight;         // status 색상

    [Header("상품 목록")]
    [SerializeField] private GameObject itemPrefab;    // Vending Machine Item (VM_ItemView 부착)
    [SerializeField] private Transform itemContainer;  // ScrollView Content

    [Header("인벤토리(음료)")]
    [SerializeField] private GameObject beveragePrefab; // Beverage (VM_BeverageView 부착)
    [SerializeField] private Transform inventoryContainer;

    [Header("로그")]
    [SerializeField] private GameObject logPrefab;     // Log
    [SerializeField] private Transform logContainer;
    [SerializeField] private ScrollRect logScrollRect; // 자동 최하단 스크롤용

    // 구매 후 재고 갱신 위해 상품 id → 뷰 보관
    private readonly Dictionary<int, VM_ItemView> itemViews = new();

    // ── 초기화 ──
    public void Init(VM_EventBus bus, VM_Data data)
    {
        this.bus = bus;
        this.data = data;

        bus.Subscribe<BalanceChanged>(OnBalanceChanged);
        bus.Subscribe<InsertMoneyResult>(OnInsertMoneyResult);
        bus.Subscribe<PurchaseResult>(OnPurchaseResult);
        bus.Subscribe<ConsumeResult>(OnConsumeResult);

        BindHeader();
        BuildProductList();
        UpdateMoney(data.Balance);
    }

    private void OnDestroy()
    {
        if (bus == null) return;
        bus.Unsubscribe<BalanceChanged>(OnBalanceChanged);
        bus.Unsubscribe<InsertMoneyResult>(OnInsertMoneyResult);
        bus.Unsubscribe<PurchaseResult>(OnPurchaseResult);
        bus.Unsubscribe<ConsumeResult>(OnConsumeResult);
    }

    // ── 상단 정보 바인딩 ──
    private void BindHeader()
    {
        idText.text = data.MachineId;
        bool active = data.Status == "active";
        powerLight.color = active ? Color.green : Color.red;   // active 초록 / inactive 빨강
    }

    // ── 상품 목록 생성 ──
    private void BuildProductList()
    {
        foreach (ProductData product in data.Products)
        {
            GameObject go = Instantiate(itemPrefab, itemContainer);
            VM_ItemView view = go.GetComponent<VM_ItemView>();

            int id = product.Id;   // 클로저 캡처 안전하게 지역변수로
            view.Setup(product, () => bus.Publish(new PurchaseRequested(id)));
            itemViews[id] = view;
        }
    }

    // ── 재화 획득 버튼 (인스펙터 onClick → InsertMoney(금액) 연결) ──
    public void InsertMoney(int amount)
    {
        bus.Publish(new InsertMoneyRequested(amount));
    }

    // ── 결과 수신 ──
    private void OnBalanceChanged(BalanceChanged msg)
    {
        UpdateMoney(msg.Balance);
        AddLog($"잔액 변경: {msg.Balance:N0} Won");
    }

    private void OnInsertMoneyResult(InsertMoneyResult msg)
    {
        if (!msg.IsSuccess) // 실패의 경우만. 성공할 때는 출력 필요 없음
            AddLog($"재화 획득 실패: {ReasonText(msg.Reason)}");
    }

    private void OnPurchaseResult(PurchaseResult msg)
    {
        if (msg.IsSuccess)
        {
            ProductData product = data.Find(msg.ProductId);
            if (product != null && itemViews.TryGetValue(msg.ProductId, out VM_ItemView view))
                view.SetStock(product.Stock);   // 재고 표시 갱신

            RefreshInventory();
            AddLog($"구매 성공: {product?.Name}");
        }
        else
        {
            AddLog($"구매 실패: {ReasonText(msg.Reason)}");
        }
    }

    private void OnConsumeResult(ConsumeResult msg)
    {
        if (!msg.IsSuccess) // 실패상황
        {
            AddLog($"소비 실패: {ReasonText(msg.Reason)}");
            return;
        }

        RefreshInventory();
        AddLog($"음료 소비: {data.Find(msg.BeverageId)?.Name}");
    }

    // ── 인벤토리 재구성 (구매/소비 시) ──
    private void RefreshInventory()
    {
        for (int i = inventoryContainer.childCount - 1; i >= 0; i--)
            Destroy(inventoryContainer.GetChild(i).gameObject);

        foreach (ProductData beverage in data.Beverages)
        {
            GameObject go = Instantiate(beveragePrefab, inventoryContainer);
            VM_BeverageView view = go.GetComponent<VM_BeverageView>();

            int id = beverage.Id;
            view.Setup(beverage, () => bus.Publish(new ConsumeRequested(id)));
        }
    }

    // ── 잔액 표시 ──
    private void UpdateMoney(int balance)
    {
        moneyText.text = $"money : {balance} won";
    }

    // ── 화면 로그 (아래에 적재) ──
    private void AddLog(string message)
    {
        GameObject go = Instantiate(logPrefab, logContainer);
        TMP_Text text = go.GetComponentInChildren<TMP_Text>();
        if (text != null)
            text.text = message;
        go.transform.SetAsLastSibling();

        Canvas.ForceUpdateCanvases();                       // 레이아웃 즉시 갱신
        if (logScrollRect != null)
            logScrollRect.verticalNormalizedPosition = 0f;  // 0 = 최하단
    }

    private string ReasonText(FailReason reason)
    {
        switch (reason)
        {
            case FailReason.MachineOff: return "전원 꺼짐";
            case FailReason.OutOfStock: return "재고 없음";
            case FailReason.NotEnoughBalance: return "잔액 부족";
            default: return "알 수 없음";
        }
    }
}
