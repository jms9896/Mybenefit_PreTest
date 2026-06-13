using UnityEngine;

// Composition Root — 모듈을 생성·연결하는 조립만 담당. 런타임 연산엔 개입하지 않는다.
public class VM_Manager : MonoBehaviour
{
    // 제작 순서 EventBus->Data->Engine->UI->Manager
    [SerializeField] private VM_UI ui;   // UI는 MonoBehaviour라 인스펙터 할당

    private void Start()
    {
        // 1. 통신 채널
        VM_EventBus bus = new VM_EventBus();

        // 2. 데이터 (JSON 로드)
        VM_Data data = new VM_Data();
        data.LoadItemsData();

        // 3. 엔진 (요청 구독)
        VM_Engine engine = new VM_Engine();
        engine.Init(bus, data);

        // 4. UI (결과 구독 + 화면 구성)
        ui.Init(bus, data);
    }
}
