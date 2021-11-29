# WATANK
## Server 개발
Team 3 "김원범, 이제현, 김재현, 김태훈"

### 환경
- Unity version = "2020.3.18f1"
- Play scene: P2

### 기능 설명
1. P2 Scene에서 플레이
2. 좌우키로 플레이어 조작 (이동거리 제한)
3. 상하키로 포신 각도 조절
4. Space로 차징 공격
5. WASD, 마우스 휠로 카메라 조작, Y키로 플레이어 카메라 고정
6. UI를 이용해 Lobby를 구현. 처음 접속자가 HOST권한을 갖고, 양 팀의 인원수가 동일할 때 HOST가 게임 시작 가능

### Scene 설명
#### P0
1. TankTest: Tank의 움직임 제어를 동기화 (player 1-2)
3. Bullet: Bullet의 발사시 움직임 및 기능 동기화 (player 1-2)
4. PO: TankTest, CameraTest, BulletTest scene 통합 (프리팹 수정으로 작동하지 않음) 을 바탕으로 NetworkManager생성, 변수 관리 및 동기화 진행

#### P2
1. AssetTest: Tank 및 Projectile에 Asset 및 애니메이션 Photon View Animation 이 ComponentIn Chiledren을 허용하지 않아 동기화 불가능으로 삭제.
2. SoundTest: 효과음 및 배경음 Single Tone issue로 탱크가 2개이상 만들어지지 않아 삭제
3. UItest: Login UI,Lobb UI ( player4 team setting ) 동기화. Master Client는 모든 플레이어들의 UI Panel에 접근하여 실질적인 게임시작 알고리즘을 구현 
4. Lobby UI : 플레이어 접속한 순서대로 팀을 배치, 닉네임으로 구분 
5. PlayerUITest: player 체력 Bar를 player 위에 설정 및 피격시 체력 감소 동기화
6. Map 동기화 : 포탄에 의해 맵이 파괴되는 함수가 모든 플레이어에게 동일하게 진행되도록 동기화
8. P2: 1~6 test scene 를 클라이언트가 통합하고, 그 씬을 바탕으로 게임 내 모든 오브젝트들의 변수 태그 설정, 동기화 진행 
9. TankControll.cs 및 Attack ,PlayerHp 등의 컴퍼넌트들의 링크관계 재설정 및 Tag를 바탕으로 변수 할당
10. 바람관련 Wind Script 및 타 스크립트 삭제. Env 에서 다루지않고 UI 에서 접근하여 동기화 불가능
11. 단순한 발사 및 제어 관련한 Player Skill 일부 동기화 
12. 플레이어를 관리하는 PlayerScript.cs 새롭게 생성
