# ASPractice
Gemini를 이용한 ASP.NET Core 공부
  
## 비쥬얼 스튜디오 설치
구글에 Visual Studio를 검색하여 공식 사이트에서 다운로드를 클릭하면 Installer가 다운된다.
  
<img width="1890" height="796" alt="VS 공식 사이트" src="https://github.com/user-attachments/assets/b95d9160-7c01-48bc-97e2-8e7f9e4f92d9" />

<img width="255" height="37" alt="VSsetup" src="https://github.com/user-attachments/assets/da237c76-91bb-49e7-87f2-8235654652f4" />

설치된 VisualStudioSetup.exe 파일을 실행하여 설치 관리자를 설치한 후

<img width="467" height="274" alt="설치 관리자" src="https://github.com/user-attachments/assets/9cbd7f5b-141b-4cf5-864d-21f00f522653" />

세부 설치 정보로 ASP.NET 밑 웹 개발과 .NET 데스크톱 개발을 추가로 설치해준다.

(용량이 꽤 크므로 시간이 걸리는 편이다.)

<img width="1260" height="693" alt="세부 설치 정보" src="https://github.com/user-attachments/assets/3c7babc6-5adf-4bd3-b7d8-27ff3b273e32" />

설치가 완료된 후 터미널에서 dotnet --version 명령어를 입력해보며 설치가 이루어졌는지 확인한다.

<img width="252" height="52" alt="설치 확인" src="https://github.com/user-attachments/assets/4fb851b3-66c3-466c-b8fc-e1dbf3113fc1" />


## 새 프로젝트 생성

설치한 Visual Studio를 실행한다.  
프로젝트 생성을 위한 옵션이 나오면 새 프로젝트 만들기를 선택한다.
![프로젝트 생성](dummy/1%20프로젝트%20생성/프로젝트%20생성.png)


프로젝트 템플릿 단계에서 ASP.NET Core 웹 앱을 선택하고 다음으로 이동한다.
![웹앱](dummy/1%20프로젝트%20생성/웹앱.png)

프로젝트 이름(ex. MyWeb)을 입력하고 프로젝트 경로를 설정해 준다.
![경로 및 프로젝트 이름 설정](dummy/1%20프로젝트%20생성/경로%20및%20프로젝트%20이름%20설정.png)

마지막으로 .NET 버전을 선택하고 생성을 완료한다.
![버전 선택 및 완료](dummy/1%20프로젝트%20생성/버전%20선택%20및%20완료.png)

생성한 프로젝트의 Program.cs 파일을 열어보면 다음과 같다.
![Program.Cs 파일](dummy/1%20프로젝트%20생성/Program.cs%20파일.png)