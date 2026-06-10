// TODO : UnityEngine이 필요한지 확인
// TODO : MonoBehaviour가 필요한지 확인

public class VM_Data
{
    // TODO: 자판기 상태 보관 (잔액, 전원 status, 상품 목록, 음료 목록)
    //       !! price/stock 은 JSON 상 숫자(int) — 화면 표시 포맷("1,500 Won"/"10 ea")은 UI 책임
    //       !! 음료(Beverage) 목록의 출처 결정 필요 (JSON 로드 X → 구매 성공 시 채워지는 런타임 리스트?)

    // TODO: Items.json 로드 & 파싱 → 상품 목록 초기화
    // TODO: 상태 조회/변경 통로 제공 (Engine이 연산할 때 사용)
}
