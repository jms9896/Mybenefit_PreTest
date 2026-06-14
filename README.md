# (주)마이베네핏 사전과제

## 개요

* 마이베네핏 사전과제를 기록하기 위한 문서이다.
* Unity 6000.3.11f1(LTS) 버전을 사용한다.
* 요구사항 명세 → 아키텍처 → 구현 순서로 작성/작업 진행한다.

---


## 결과

* 결과 영상 : [https://minseong.synology.me:5008/sharing/aTLT1XPHs](https://minseong.synology.me:5008/sharing/aTLT1XPHs)

---

## 요구사항 분석

* 목표
  * 요구사항에 맞는 적절한 시스템 설계(Architecture)
  * 코드 구조화 능력
  * 의도를 중점적으로 확인하는 것

### 자판기(Vending Machine) 시스템 구현 사전과제 요구 사항

**1. 데이터 로드 및 초기화**

* 데이터 소스 : Assets/Resources/Items.json
* 파싱 데이터 : machineid, status, products

---

**2. UI 상태 및 데이터 바인딩**

* 상단 정보(Canvas/Vending Machine/Top/Group)
  * ID Text: machineid 문자열 바인딩
  * Power Light(Image component): status 값에 따라 색상 변경
    * Active: 초록 (0, 255, 0)
    * Inactive: 빨강 (255, 0, 0)
    * Power Light가 Inactive라면 아래 3. 핵심 비즈니스 로직이 동작하지 않고 4. 로그 시스템을 통해 로그 제출
* 상품 목록(Canvas/Vending Machine/Body/ScrollView)
  * 프리팹: Assets/Prefabs/Vending Machine Item
  * 초기화: products 데이터를 기반으로 Vending Machine Item 게임 오브젝트 생성, 리스트 동적 생성 및 데이터, Resources 폴더 내 이미지 바인딩
  * 제약 조건: 가격 및 잔여 갯수의 원본 string format 유지

---

**3. 핵심 비즈니스 로직**

* 재화 획득 (Canvas/Optional/Editor)
  * 버튼 클릭 시 명시 된 금액 획득
  * 제약 조건
    * Current Money(Text component) 최대 10,000원 제한
    * Current Money(Text component) 원본 string format 유지
* 상품 구매
  * 상목 목록 - 초기화를 통해 생성 된 Item을 선택 시 금액 차감
  * 프리팹: Assets/Prefabs/Beverage
  * 초기화: Inventory(Canvas/Vending Machine/Inventory/ScrollView) 아이템 생성 및 적재
* 상품 소비
  * Inventory내 Item 선택 시 Item 비표시

---

**4. 로그 시스템**

* 트리거: 재화 획득, 상품 구매, 음료 소비 등 모든 주요 사용자 행위에 대해
* 프리팹: Assets/Prefabs/Log
* Log(Canvas/Optional/Log/ScrollView)내에 하단부터 위로 쌓이도록 UI Layout 구성

### 요구사항 명세서

| 분류           | 기능명                    | 세부 내용                                                                   | 진행일             | 공수                |
| -------------- | ------------------------- | --------------------------------------------------------------------------- | ------------------ | ------------------- |
| 준비           | 클래스 다이어그램         | C# 모델·클래스 설계, 다이어그램 작성                                       | 06/10              | 1.5 h               |
| 준비           | 네이밍컨벤션              | method·변수 네이밍 규칙 작성                                               | 06/10              | 1 h                 |
| 준비           | EventBus 구현             | Publish·Subscribe·Unsubscribe (제네릭 타입 라우팅)                        | 06/12              | 2 h                 |
| 준비           | 메시지 정의               | 요청 3 + 결과 4(7종) + FailReason enum                                      | 06/12 (14 보강)    | 1.5 h               |
| 준비           | Data 로드·상태           | JSON 파싱, 잔액·재고·보유음료 상태 보관                                   | 06/12~13           | 1.5 h               |
| 준비           | Manager 구현              | Bus 생성, 각 모듈 Init 조립(Composition Root)                               | 06/13              | 0.5 h               |
| UI             | Machine ID 바인딩         | 상단 ID Text ← machineId                                                   | 06/13              | 0.5 h               |
| UI             | Power Light 색상          | active 녹색(0,255,0) / inactive 빨강(255,0,0)                               | 06/13              | 0.5 h               |
| UI             | 상품 목록 생성            | Vending Machine Item 프리팹 동적 생성                                       | 06/13              | 1 h                 |
| UI             | 상품 이미지 바인딩        | Resources.Load `<Sprite>`(상품명) → Image 적용                           | 06/13              | 0.5 h               |
| UI             | 가격·재고 포맷           | `1,500 Won`/`10 ea`원본 포맷 유지                                       | 06/13~14(수정)     | 0.5 h               |
| UI             | 프리팹 뷰 컴포넌트        | VM_ItemView·VM_BeverageView 분리                                           | 06/13              | 1.5 h               |
| UI             | 한글 폰트 베이킹          | NanumGothic 한글·특수문자 베이크*(계획 외)*                                | 06/13              | 1 h                 |
| UI             | 로그 스크롤·레이아웃     | 하단 정렬·자동 최하단 스크롤 디버깅*(계획 외)*                             | 06/13              | 1 h                 |
| 비즈니스       | 재화 획득                 | 명시 금액 추가, Current Money 갱신                                          | 06/13              | 0.5 h               |
| 비즈니스       | 재화 상한                 | 최대 10,000원, 초과분 무시(고정)                                            | 06/13              | 0.5 h               |
| 비즈니스       | 상품 구매                 | 클릭 → 잔액 확인 → 차감 → 재고 감소 → Inventory 추가                    | 06/13              | 1 h                 |
| 비즈니스       | Beverage 생성             | Beverage 프리팹을 Inventory Content에 적재                                  | 06/13              | 0.5 h               |
| 비즈니스       | 상품 소비                 | Inventory 클릭 → 리스트 제거 후 재구성(비표시)                             | 06/13              | 0.5 h               |
| 비즈니스       | 상품 초기화               | JSON 기반 Product 리스트 초기화 + 목록 생성 트리거                          | 06/13              | 0.5 h               |
| 비즈니스       | Inactive 차단             | status=inactive 시 구매·획득·소비 로직을 Engine에서 거부 + 실패 로그 발행 | 06/14              | 2 h                 |
| 로그           | UI 로그                   | UI 상호작용 로그 출력                                                       | 06/13              | 0.5 h               |
| 로그           | 비즈니스 로그             | 획득·구매·소비 및 실패 로그 출력                                          | 06/13~14           | 0.5 h               |
| 디버깅         | QA                        | 자체 QA 진행                                                                | 06/14              | 1 h                 |
| 디버깅         | 버그 수정                 | 전원 OFF 잔액 누수 등 수정                                                  | 06/14              | 1.5 h               |
| 디버깅         | 미사용 코드 정리          | 죽은 필드·중복 생성자·불필요 using 제거                                   | 06/14              | 0.5 h               |
| 문서           | 설계문서·다이어그램 갱신 | 메시지표·클래스/시퀀스 다이어그램 코드 동기화                              | 06/14              | 1.5 h               |
| **합계** |                           |                                                                             | **06/10~14** | **약 25.5 h** |

## 아키텍쳐 설계

### 설계 목표

* 모듈간 결합도를 최소화한 확장 가능한 구조를 목표로 한다.
  * 각 모듈은 단일책임 원칙을 고수한다.
  * UI와 비즈니스(Engine) 로직은 완전분리한다.
  * 새로운 기능 추가시, 기존 코드 수정을 최소화 하게 한다.

### 핵심 패턴 : EventBus 구조

* 모듈간 통신을 ‘중앙 이벤트 버스를 통한 이벤트 관리(발행/구독)’ 방식으로 처리한다.

### 네이밍 컨벤션

* 프로젝트명 **VendingMachine**으로 설정한다. 따라서, 클래스명은 `VM_~` 의 형태로 진행한다.

### 다이어그램

* **클래스 다이어그램**
  * 전체(메인) 클래스 다이어그램 구조
    ```mermaid
    classDiagram
        class VM_Manager
        class VM_EventBus
        class VM_Data
        class VM_Engine
        class VM_UI

        VM_Manager *-- VM_EventBus : creates
        VM_Manager *-- VM_Data : creates
        VM_Manager *-- VM_Engine : creates
        VM_Manager *-- VM_UI : creates
        VM_Engine ..> VM_Data : reads / updates
        VM_UI ..> VM_EventBus : publish / subscribe
        VM_Engine ..> VM_EventBus : publish / subscribe
    ```
  * UI - View 별도 구조
    ```mermaid
    classDiagram
        class VM_UI
        class VM_ItemView
        class VM_BeverageView
        VM_UI ..> VM_ItemView : creates / setup
        VM_UI ..> VM_BeverageView : creates / setup

    ```
* 클래스 계층
  * `VM_Manager` : 총괄 매니저 클래스
    * `VM_EventBus` : 발행/구독 관리 이벤트 버스 클래스
    * `VM_Data` : 데이터 관련 클래스
    * `VM_UI` : UI 관련 클래스
      * Log 프리팹으로 화면 로그 기록
    * `VM_Engine` : 구매, 소비 등 상호작용 관련 클래스
* 메시지 - 통신 계약
  * 메시지는 `VM_UI`와 `VM_Engine`이 공유하는 통신 계약이다.
    * 모듈 분리를 위해 `VM_Messages`에 모아 정의한다.
  * 요청은 UI→Engine, 결과는 Engine→UI 방향으로 고정된다.
  * 트리거 / 발행 시점 (다이어그램에 없는 정보)| 메시지               | 구분 | 트리거 / 발행 시점                           |
    | -------------------- | ---- | -------------------------------------------- |
    | InsertMoneyRequested | 요청 | 금액 투입 버튼 클릭                          |
    | PurchaseRequested    | 요청 | 상품(구매) 버튼 클릭                         |
    | ConsumeRequested     | 요청 | 보유 음료(소비) 버튼 클릭                    |
    | BalanceChanged       | 결과 | 잔액이 실제로 바뀔 때 (투입·구매 성공 공유) |
    | InsertMoneyResult    | 결과 | 재화 획득 처리 후 (성공 / 전원 차단)         |
    | PurchaseResult       | 결과 | 구매 처리 후 (성공 / 실패 + 사유)            |
    | ConsumeResult        | 결과 | 소비 처리 후 (성공 / 실패 + 사유)            |
  * `FailReason` enum은 구매·소비·획득 결과가 공유하는 실패 사유다.
* **시퀀스 다이어그램**
  * 초기화(시작)
    ```mermaid
    sequenceDiagram
        participant Manager as VM_Manager
        participant Bus as VM_EventBus
        participant Data as VM_Data
        participant Engine as VM_Engine
        participant UI as VM_UI

        Manager->>Bus: new VM_EventBus()
        Manager->>Data: new VM_Data()
        Manager->>Data: LoadItemsData() (JSON 파싱)
        Manager->>Engine: new VM_Engine()
        Manager->>Engine: Init(bus, data)
        Note over Engine: 요청 메시지 구독
        Manager->>UI: Init(bus, data)
        Note over UI: 결과 메시지 구독 + 화면 구성

    ```
  * 금액 투입
    ```mermaid
    sequenceDiagram
        actor User as 사용자
        participant UI as VM_UI
        participant Bus as VM_EventBus
        participant Engine as VM_Engine
        participant Data as VM_Data

        User->>UI: 투입 버튼 클릭
        UI->>Bus: Publish(InsertMoneyRequested)
        Bus->>Engine: OnInsertMoney()
        alt 전원 꺼짐(Inactive)
            Engine->>Bus: Publish(InsertMoneyResult(Fail, MachineOff))
            Bus->>UI: OnInsertMoneyResult()
            UI->>UI: AddLog("재화 획득 실패")
        else 전원 켜짐(Active)
            Engine->>Data: AddBalance(amount) (상한 10,000)
            Engine->>Bus: Publish(InsertMoneyResult(Success))
            Engine->>Bus: Publish(BalanceChanged)
            Bus->>UI: OnBalanceChanged()
            UI->>UI: 잔액 표시 갱신 + AddLog("잔액 변경")
        end

    ```
  * 구매
    ```mermaid
    sequenceDiagram
        actor User as 사용자
        participant UI as VM_UI
        participant Bus as VM_EventBus
        participant Engine as VM_Engine
        participant Data as VM_Data

        User->>UI: 구매 버튼 클릭
        UI->>Bus: Publish(PurchaseRequested)
        Bus->>Engine: OnPurchase()
        Engine->>Data: status 확인
        alt 전원 꺼짐(Inactive)
            Engine->>Bus: Publish(PurchaseResult(Fail, MachineOff))
        else 전원 켜짐(Active)
            Engine->>Data: 잔액·재고 확인
            alt 구매 성공
                Engine->>Data: Balance -= price, Stock--
                Engine->>Data: Beverages에 추가
                Engine->>Bus: Publish(PurchaseResult(IsSuccess))
                Engine->>Bus: Publish(BalanceChanged)
            else 잔액 부족 / 재고 없음
                Engine->>Bus: Publish(PurchaseResult(Fail, NotEnoughBalance/OutOfStock))
            end
        end
        Bus->>UI: OnPurchaseResult()
        Bus->>UI: OnBalanceChanged()
        UI->>UI: UI 갱신
    ```
  * 소비
    ```mermaid
    sequenceDiagram
        actor User as 사용자
        participant UI as VM_UI
        participant Bus as VM_EventBus
        participant Engine as VM_Engine
        participant Data as VM_Data

        User->>UI: 소비(마시기) 버튼 클릭
        UI->>Bus: Publish(ConsumeRequested)
        Bus->>Engine: OnConsume()
        alt 전원 꺼짐(Inactive)
            Engine->>Bus: Publish(ConsumeResult(Fail, MachineOff))
            Bus->>UI: OnConsumeResult()
            UI->>UI: AddLog("소비 실패")
        else 전원 켜짐(Active)
            Engine->>Data: Beverages에서 제거
            Engine->>Bus: Publish(ConsumeResult(Success))
            Bus->>UI: OnConsumeResult()
            UI->>UI: 보유 음료 목록 갱신 + AddLog("음료 소비")
        end

    ```


---


## QA

### A. 초기화 / 데이터 로드

| 항목           | 기대 결과                                | 완료여부 |
| -------------- | ---------------------------------------- | -------- |
| 플레이 시작    | 콘솔 에러 없음                           | O        |
| ID Text        | machineId(VM-2026-A1) 표시               | O        |
| 상품 목록 생성 | products 전부 동적 생성, 누락·중복 없음 | O        |
| 상품 이미지    | Resources 이미지 정상 바인딩             | O        |
| 가격/재고 포맷 | 0,000 Won / 0 ea 원본 포맷 유지          | O        |
| 초기 잔액      | money : 0 won 포맷 유지                  | O        |

### B. 전원 표시

| 항목            | 기대 결과             | 완료여부 |
| --------------- | --------------------- | -------- |
| status=active   | 라이트 초록 (0,255,0) | O        |
| status=inactive | 라이트 빨강 (255,0,0) | O        |

### C. 재화 획득 (active)

| 항목                   | 기대 결과                       | 완료여부 |
| ---------------------- | ------------------------------- | -------- |
| 금액 버튼 클릭         | 잔액 증가, 표시 갱신, 로그 기록 | O        |
| 10,000 상한            | 초과 시 10,000에서 고정         | O        |
| 상한 도달 후 추가 클릭 | 10,000 유지                     | O        |
| 잔액 표시 포맷         | money : N won 유지              | O        |

### D. 상품 구매 (active)

| 항목                  | 기대 결과                          | 완료여부 |
| --------------------- | ---------------------------------- | -------- |
| 잔액 충분 + 재고 있음 | 차감, 재고 -1, 인벤토리 적재, 로그 | O        |
| 잔액 부족             | 차감·적재 없음, 실패 로그         | O        |
| 재고 0                | 구매 불가, 실패 로그               | O        |
| 마지막 1개 구매       | 재고 0 표시 후 차단                | O        |
| 잔액 = 가격           | 구매 성공, 잔액 0                  | O        |

### E. 상품 소비 (active)

| 항목                          | 기대 결과            | 완료여부 |
| ----------------------------- | -------------------- | -------- |
| 음료 클릭                     | 해당 음료 제거, 로그 | O        |
| 동일 음료 여러 개 중 1개 소비 | 1개만 제거           | O        |
| 인벤토리 전부 소비            | 빈 상태 에러 없음    | O        |

### F. inActive 차단

| 항목           | 기대 결과                  | 완료여부  |
| -------------- | -------------------------- | --------- |
| 라이트         | 빨강                       | O         |
| 재화 획득 클릭 | 잔액 변화 없음, 실패 로그  | O         |
| 상품 클릭      | 차감·적재 없음, 실패 로그 | O         |
| 음료 소비 클릭 | 제거 없음, 실패 로그       | 확인불가* |

* 확인불가* : 전원 Off 상태에서는 음료가 없기 때문에(구매 불가) 소비 확인 불가.

### G. 로그 시스템

| 항목        | 기대 결과                             | 완료여부 |
| ----------- | ------------------------------------- | -------- |
| 트리거 범위 | 획득·구매·소비 및 각 실패 전부 기록 | O        |
| 적재 방향   | 하단부터 위로 쌓임                    | O        |
| 자동 스크롤 | 새 로그 시 최하단 자동 표시           | O        |
| 다량 로그   | 20줄 이상에서도 스크롤 정상           | O        |

### H. 경계 / 예외

| 항목             | 기대 결과                   | 완료여부 |
| ---------------- | --------------------------- | -------- |
| 잔액 0에서 구매  | 잔액 부족 처리, 크래시 없음 | O        |
| 빈 인벤토리 소비 | NullReference 없음          | O        |
| 한글/특수문자    | 폰트 깨짐 없음              | O        |
| 연타 입력        | 상태 꼬임·중복 차감 없음   | O        |
