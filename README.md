# WATANK
## Client 개발
Team 3 "김원범, 이제현, 김재현, 김태훈"

### 환경
- Unity version = "2020.3.18f1"
- Play scene: P2

### 기능 설명
1. P2 Scene에서 플레이
2. 좌우키로 플레이어 조작 (이동거리 제한)
3. 상하키로 포신 각도 조절
4. Space로 차징 공격
5. 1~4번 키(AlphaNumber) 누르고 Enter로 스킬 변경 (스킬 랜덤 배정)
    - 포탄이 떨어진 곳으로 순간이동하는 스킬
    - 두 번 공격하는 스킬
    - 데미지가 낮아지지만 범위가 늘어나는 스킬
    - 데미지가 높아지지만 범위가 줄어드는 스킬
    - 탄도학 적용이 안되고 직선으로 쏘는 스킬
    - 지형을 관통하는 스킬
6. WASD, 마우스 휠로 카메라 조작, Y키로 플레이어 카메라 고정
7. 우측 상단 사운드 조절 UI, 상단 시간 제한 UI, 좌측 상단 바람 상수

### Scene 설명
#### P0
1. TankTest: Tank의 움직임 제어를 확인
2. CameraTest: Camera가 Tank를 focusing하는지 확인
3. BulletTest: Bullet의 발사와 탄도학 적용을 확인
4. PO: TankTest, CameraTest, BulletTest scene 통합 (프리팹 수정으로 작동하지 않음)

#### P2
1. AssetTest: Tank 및 Projectile에 Asset 및 애니메이션 적용 
2. SoundTest: 효과음 및 배경음 적용
3. UItest: 사용자가 보게될 임시 UI 배치
4. PlayerUITest: player 체력 Bar를 player 위에 설정
5. mapTest: map 구성 scene
6. MenuFixScene: Bgm 및 SE 볼륨 조절 추가 Scene
7. P2: 1~6 test scene 통합

* 현재 P2단계는 Script 수정으로P2, MenuFixScene, UITest, mapTest만 작동
