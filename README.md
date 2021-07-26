# 실행 화면
![4 1](https://user-images.githubusercontent.com/87646938/126977161-40ab21d0-75cf-491f-a2ff-ae16c56148d3.jpg)
 - 초기 실행 화면
 <br>
 
![4 2](https://user-images.githubusercontent.com/87646938/126977182-ed031ad4-ee96-4a9c-8564-7337745a6dbb.jpg)
 - 화면을 드래그하여 자동차가 이동한 모습과 깃발과의 거리를 UI로 나타낸 모습
<br>

![4 3](https://user-images.githubusercontent.com/87646938/126977227-d6cd9e88-bd11-4c98-86fa-40dd7c6a8ef2.jpg)
 - 자동차가 깃발을 넘어서면 "Gave Over!"가 출력되는 모습

<br>
<br>
<br>

# WEEK4_Unity

## Ch.4 UI & Director Object

### Swipe Car

#### 4.1 게임 설계하기

##### 4.1.1 게임 기획

- 자동차 멈추기 게임



##### 4.1.2 게임 리소스

1. 오브젝트 나열
   - 자동차, 깃발, 지면, UI
2. 컨트롤러 스크립트
   - 자동차의 움직임을 제어하는 컨트롤러 스크립트
3. 제너레이터 스크비트
   - 플레이할 때 생성될 오브젝트 -> X
4. 감독 스크립트
   -  자동차와 깃발 사이의 거리를 UI로 표시
5. 스크립트 흐름
   - 컨트롤러(자동차 스와이프하면 달리다가 감속) -> 제너레이터 -> 감독(거리 UI)

#### 4.2 프로젝트 & 씬

##### 4.2.1 프로젝트

- File -> New Project -> (SwipeCar) -> 2D -> 생성
- 리소스 추가 (자동차, 땅, 깃발, 자동차 효과음)
- 화면 설정 -> Game -> Free Aspect -> VSync 체크



##### 4.2.2 스마트폰용 설정

- File -> Build Settings -> Platform -> iOS/Android
- 화면 크기 -> Game -> Free Aspect에서 설정



##### 4.2.3 씬 저장

- File -> Save As -> (GameScene)





#### 4.3 씬에 오브젝트 배치

##### 4.3.1 지면 배치

- Scene -> (ground.png) 드래그&드롭 -> (Position 0, -5, 0), (Scale 18, 1, 1)



##### 4.3.2 자동차 배치

- Scene -> (car.png) 드래그&드롭 -> (Position -7, -3.7, 0)



##### 4.3.3 깃발 배치

- (flag.png) 드래그&드롭 -> (Position 7.5, -3.5, 0)



##### 4.3.4 배경색 변경

- Main Camera -> Background (DEDBD2)





#### 4.4 스와이프로 자동차 움직이기

##### 4.4.1 자동차 스크립트

- 화면을 누르면 자동차가 움직이고 감속
- Project -> Create -> C# Script -> (CarController)

```c#
public class CarController : MonoBehavior
{
	float speed = 0;
	void Start()
	{
        
    }

    void Update()
	{
 		if (Input.GetMouseButton(0))  // 마우스 클릭하면
 		{
  			this.speed = 0.2f;  // 초기 속도
  		}
 
        transform.Translate(this.speed, 0, 0);  // 이동
 		this.speed *= 0.98f;  // 감속
	}
}
```



##### 4.4.2 스크립트 적용

- CarController 스크립트를 car 오브젝트로 드래그&드롭



##### 4.4.3 스와이프 길이에 따라 자동차 이동 거리 바꾸기

-  GetMouseButtonDown과 GetMouseButtonUp 매서드로 구하기
-  각 지점의 마우스 좌표(Input.mousePosition)를 구하고 스와이프 길이 값 계산

```c#
public class CarController : MonoBehaviour
{
	float speed = 0;
	Vector2 startPos;
    
	void Start()
	{
        
    }

    void Update() 
    { 	
        // 스와이프 길이 구하기
 		if (Input.GetMouseButtonDown(0))
 		{ 
            this.startPos = Input.mousePosition; 
        }  // 마우스를 클릭한 좌표
 		else if (Input.GetMouseButtonUp(0))
 		{
            Vector2 endPos = Input.mousePosition;  // 마우스를 떼었을 때 좌표
  			float swipeLength = endPos.x - this.startPos.x;
  			this.speed = swipeLength / 500.0f;
        }  // 스와이프 길이를 처음 속도로 변경
 
        transform.Translate(this.speed, 0, 0);  // 이동
 		this.speed *= 0.98f;  // 감속
	}
}
```





#### 4.5 UI 표시

##### 4.5.1 UI 설계

- UI 부품 라이브러리를 사용해 자동차와 깃발 사이의 거리를 표시
  1. UI 부품을 Scene 뷰에 배치
  2. UI 갱신하는 감독 스크립트 작성
  3. 빈 오브젝트를 만들고 감독 스크립트 적용



##### 4.5.2 Text를 사용해 거리 표시

- Hierarchy -> + -> UI -> Text
- Text -> (Distance)로 이름 변경
- Distance -> (Rect Transform 0,0,0),(Width Height 700, 80), (Font Size 64), (Alignment 중앙 정렬)





##### 4.6 UI 갱신하는 감독

##### 4.6.1 UI 갱신하는 스크립트

- Create -> C# Script -> (GameDirector)

  ```c#
  using UnityEngine.UI;  // UI를 사용하는 데 필요
  
  public class GameDirector : MonoBehaviour
  {
  	GameObject car;
  	GameObject flag;
  	GameObject distance;
  
      void Start()
  	{
   		this.car = GameObject.Find("car");
   		this.flag = GameObject.Find("flag");
   		this.distance = GameObject.Find("Distance");
  	}
      
  	void Update()
  	{
  		float length = this.flag.tansform.position.x - this.car.transform.position.x;
   		this.distance.GetComponent<Text>().text = "목표 지점까지 " + length.ToString("F2") + "m";
  	}
  }
  ```

  - ToStirng 서식 지정자
     정수형 D[자릿수]  ..  (456).ToString("D5") -> 00456
     소수형 F[자릿수]  ..  (12.3456).ToString("F3") -> 12.345



##### 4.6.2 스크립트를 감독 오브젝트에 적용

- 감독 오브젝트는 빈 오브젝트를 만들고 스크립트를 전달
- Hierarchy -> + -> Create Empty -> (GameDirector)
- GameDirector 스크립트를 GameDirector 오브젝트로 드래그&드롭



- 도전이 성공했는지 실패했는지 표시

  ```c#
  void Update()
  {
  	float length = this.flag.transform.position.x - this.car.transform.position.x;
  	if (length >= 0)
  	{
   		this.distance.GetComponent<Text>().text = "목표 지점까지 " + length.Tostring("F2") + "m";
  	}
  	else
  	{
   		this.distance.GetComponent<Text>().text = "게임 오버!";
  	}
  }
  ```

  
  - 컴포넌트
    - 자체 기능을 늘리고 싶다면 스크립트 컴포넌트를 추가
    - 오브젝트의 좌표와 회전을 관리하는 Transform 컴포넌트
    - 접근 방법? : GetComponent 매서드
      - GetComponent<AudioSource>()
      - GetComponent<Text>()
      - GetComponent<Transform>()
      - 직접 만든 스크립트도 가능 : car.GetComponent<CarController>().Run()





#### 4.7 자동차 효과음

##### 4.7.1 AudioSource 컴포넌트를 사용하는 방법

1. 오브젝트에 AudioSource 컴포넌트를 적용
2. AudioSource 컴포넌트에 효과음 설정
3. 효과음을 울리고 싶은 시간에 스와이프하면 Play 매서드 호출



##### 4.7.2 AudioSource 컴포넌트 적용

- Hierarchy -> (car) -> Add Component -> Audio -> Audio Source



##### 4.7.3 효과음 설정

- Hierarchy -> (car) -> (car_se)를 AudioClip으로 드래그&드롭 -> Play on Awake 체크 해제



##### 4.7.4 스크립트에서 소리 재생

- Play 매서드를 호출해야함

  - CarController에 추가

  ```c#
  void Update()
  {
  	...
  	// 효과음을 재생
  	GetComponent<AudioSource>().Play();
  }
  
  
  ```





#### 4.8 스마트폰에서 가동

##### 4.8.1 아이폰

- 컴퓨터와 기기르 USB 연결
- Bundle Identifier -> com.이름.swipeCar -> Scenes/SampleScene 체크 해제 -> GameScene를 Scenes In
- Build로 드래그&드롭 -> Build -> (SwipeCar_iOS) -> Save
- Xcode용 프로젝트 폴더 -> Unity-iPhone.xcodeproj 더블클릭 -> Signing -> Team 선택하여 기기에 넣기



##### 4.8.2 안드로이드

- Package Name -> com.이름.swipeCar -> 똑같이 Scenes/SampleScene 체크 해제 -> GameScene 드래그&드롭 -> Build And Run -> (SwipeCar_Android) -> 저장 장소 SwipeCar 프로젝트 폴더 -> Save 
