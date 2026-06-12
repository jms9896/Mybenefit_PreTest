// VM_Messages.cs — UI와 Engine이 공유하는 통신 계약(메시지) 모음이다.
// 이 파일은 단일 클래스가 아니라 작은 메시지 클래스 여러 개 + enum을 담는다.




// ── 요청 메시지 UI → Engine ──

// InsertMoneyRequested — 필드 Amount(int). 금액 투입 버튼
public class InsertMoneyRequested
{
    public int Amount;

    public InsertMoneyRequested(int amount)
    {
        Amount = amount;
    }
}

// PurchaseRequested — 필드 ProductId(int). 구매 버튼
public class PurchaseRequested
{
    public int ProductId;
    public PurchaseRequested(int productId)
    {
        ProductId = productId;
    }
}

// ConsumeRequested — 필드 BeverageId(int). 소비(마시기) 버튼
public class ConsumeRequested
{
    public int BeverageId;
    public ConsumeRequested(int beverageId)
    {
        BeverageId = beverageId;
    }
}

// ── 결과 메시지 Engine → UI ──

// BalanceChanged — 필드 Balance(int). 잔액 변경 시 (투입·구매 공유)
public class BalanceChanged
{
    public int Balance;
    public BalanceChanged(int balance)
    {
        Balance = balance;
    }
}

// PurchaseResult — 필드 ProductId(int), IsSuccess(bool), Reason(PurchaseFailReason). 구매 처리 후
public class PurchaseResult
{
    public int ProductId;
    public bool IsSuccess;
    public PurchaseFailReason Reason;

    public PurchaseResult(int productId, bool isSuccess, PurchaseFailReason reason)
    {
        ProductId = productId;
        IsSuccess = isSuccess;
        Reason = reason;
    }
}

// ConsumeResult — 필드 BeverageId(int). 소비 처리 후
public class ConsumeResult
{
    public int BeverageId;
    public ConsumeResult(int beverageId)
    {
        BeverageId = beverageId;
    }
}

// ── 실패 사유 enum (PurchaseResult 전용) ──

// PurchaseFailReason enum — None / OutOfStock / NotEnoughBalance / MachineOff
public enum PurchaseFailReason
{
    None = 0,
    OutOfStock = 1,
    NotEnoughBalance,
    MachineOff
}