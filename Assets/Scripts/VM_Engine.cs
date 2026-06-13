using System;

public class VM_Engine
{
    // TODO: 요청 메시지 구독 (금액 투입 / 구매 / 소비)
    private VM_EventBus bus;
    private VM_Data data;

    public void Init(VM_EventBus bus, VM_Data data)
    {
        this.bus = bus;
        this.data = data;

        bus.Subscribe<InsertMoneyRequested>(OnInsertMoney);
        bus.Subscribe<PurchaseRequested>(OnPurchase);
        bus.Subscribe<ConsumeRequested>(OnConsume);
    }


    // TODO: 금액 투입 처리 — 잔액 상한(10,000) 검증 → 잔액 변경 결과 발행
    private void OnInsertMoney(InsertMoneyRequested msg)
    {
        data.AddBalance(msg.Amount);
        bus.Publish(new BalanceChanged(data.Balance));
    }

    // TODO: 구매 처리 — 전원 차단 / 재고 / 잔액 검증 → 구매 결과 발행
    private void OnPurchase(PurchaseRequested msg)
    {
        // 전원 꺼짐
        if (data.Status != "active")
        {
            bus.Publish(new PurchaseResult(msg.ProductId, false, PurchaseFailReason.MachineOff));
            return;
        }
        ProductData productData = data.Find(msg.ProductId);

        // 재고 없음
        if (productData.Stock <= 0)
        {
            bus.Publish(new PurchaseResult(msg.ProductId, false, PurchaseFailReason.OutOfStock));
            return;
        }

        // 잔액부족
        if (data.Balance < productData.Price)
        {
            bus.Publish(new PurchaseResult(msg.ProductId, false, PurchaseFailReason.NotEnoughBalance));
            return;
        }

        // 성공
        data.AddBalance(-productData.Price);
        data.DecreaseStock(msg.ProductId);
        data.AddBeverage(productData);
        bus.Publish(new PurchaseResult(msg.ProductId, true));
        bus.Publish(new BalanceChanged(data.Balance));
    }   
    // TODO: 소비 처리 — 소비 결과 발행
    private void OnConsume(ConsumeRequested msg)
    {
        data.RemoveBeverage(msg.BeverageId);
        bus.Publish(new ConsumeResult(msg.BeverageId));
    }
    //       !! status=Inactive 면 로직 거부 + 실패 사유 발행 + 로그
}
