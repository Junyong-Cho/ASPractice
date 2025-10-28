# Internet Information Service(IIS)를 이용하여 배포하는 법

```윈도우 기능 켜기/끄기``` 윈도우 검색창에서 검색

<img width="785" height="449" alt="1  윈도우 기능 켜기 끄기" src="https://github.com/user-attachments/assets/70b7cde9-4220-402f-81e1-fc2a3c96b865" />

인터넷 정보 서비스를 찾아서 ```World Wide Web 서비스``` ```웹 관리 도구```를 체크한 다음에 ```World Wide Web 서비스```/```응용 프로그램 개발 기능```/ 아래 경로에 존재하는 .NET 최상위 버전을 체크한 다음에 확인 버튼을 눌러서 설치를 완료한다.

<img width="310" height="75" alt="2  인터넷 정보 서비스" src="https://github.com/user-attachments/assets/f56da3b6-b8a8-4694-8811-dd576693d2e2" />
<br>
<img width="263" height="283" alt="3   net 버전 선택" src="https://github.com/user-attachments/assets/d0c6a433-3ff5-45d2-98b9-962bbb597321" />

[.NET 호스팅 번들 다운로드 페이지](https://dotnet.microsoft.com/ko-kr/download/dotnet/9.0)로 이동하여 ASP.NET 코어 런타임 부분의 Window 설치 관리자에 Hosting Bundle을 다운로드한다.

<img width="339" height="627" alt="4  호스팅 번들 다운로드" src="https://github.com/user-attachments/assets/9c227be2-6442-4e87-aee4-8dc0fd043cef" />

다운로드한 ```dotnet-hosting-[버전]-win.exe``` 프로그램을 실행하여 Window Server Hosting을 설치한다.

<img width="497" height="356" alt="5  호스팅 번들 설치" src="https://github.com/user-attachments/assets/426a0f82-7c98-46ee-b621-471fdd334677" />

관리자로 cmd를 실행한 다음에 ```net stop was /y``` 명령어로 IIS를 종료하고 ```net start w3svc``` 명령어로 IIS를 재시작한다.

프로젝트 경로에서 ```dotnet publish -c Release``` 명령어를 실행하여 배포 디렉터리를 생성한다.  
디렉터리 생성 경로는 ```bin/Release/net[버전]/publish```이다.

그 다음 윈도우 검색창에 iis 관리자를 검색하여 실행한다.

<img width="765" height="397" alt="6  iis 관리자" src="https://github.com/user-attachments/assets/27f00f25-fc75-45ad-9f34-702aa2e23636" />

왼쪽의 연결 부분의 본인의 컴퓨터 이름 밑으로 사이트 부분에 웹 사이트 추가를 선택한다.

<img width="235" height="137" alt="7  웹사이트 추가" src="https://github.com/user-attachments/assets/5ec6e9aa-f5ee-4e07-b3b8-da0a6188cfe5" />

사이트 이름과 publish 경로, 포트 번호를 설정한 다음에 확인을 눌러서 생성을 완료한다.

<img width="560" height="361" alt="8  웹 사이트 설정" src="https://github.com/user-attachments/assets/39ee0945-eaf6-428a-a6c7-34f40573d400" />

애플리케이션 풀에 들어가서 생성한 사이트의 기본 설정 메뉴에 들어간다.

<img width="831" height="321" alt="9  애플리케이션 풀" src="https://github.com/user-attachments/assets/49c0540e-eafe-4673-a85c-58d0e055e3b3" />

<img width="573" height="345" alt="10  기본 설정" src="https://github.com/user-attachments/assets/5e18b0ab-582c-4cfe-a0ee-5b339e54a749" />

.NET CLR 버전을 관리 코드 없음으로 설정해 준다.

<img width="313" height="279" alt="11  관리 코드 없음" src="https://github.com/user-attachments/assets/c83c1211-16b7-405c-9c28-6c1ec165cf83" />

```localhost:[포트번호]```를 브라우저에 검색하여 정상적으로 서버가 실행되었는지 확인한다.

# docker와 nginx를 이용하여 배포하는 법

docker desktop이 설치되어 있어야 하고 ubuntu 이미지를 설치해야 한다.

도커를 설치하고 ```docker pull ubuntu:latest``` 명령어로 ubuntu의 이미지를 다운받은 다음에 ```docker run -dit --name myServer -p 8080:80 ubuntu:latest``` 명령어로 컨테이너를 생성한다.

```run``` 컨테이너 생성  
```-d``` 백그라운드에서 컨테이너 실행  
```-i``` 키보드 입력 활성화  
```-t``` 리눅스 터미널 실행  
```--name``` 컨테이너 이름 설정  
```-p 포트1:포트2``` 로컬의 포트1을 컨테이너의 포트2와 연결
```ubuntu:latest``` 우분투 이미지로 우분투 컨테이너 생성



도커에 dotnet runtime을 설치해주어야 한다.
본 프로젝트는 .net 9.0 버전을 사용하고 있어서 9.0 버전의 runtime을 설치해주어야 하는데 ```apt install aspnetcore-runtime-9.0``` 명령어로 설치해 주면 된다.  

그러나 .net 9.0 버전은 우분투 최신 버전 이후에 출시되었으므로 추가적인 작업이 필요하다.

```apt install software-properties-common -y```  
```add-apt-repository ppa:dotnet/backports```  
```apt update```  
```apt install aspnetcore-runtime-9.0 -y```  



# 외부 접속 하는 법

## 방화벽 설정

외부에서 내 서버에 접속하려면 방화벽을 열어야 한다.

<img width="759" height="395" alt="12  방화벽 설정" src="https://github.com/user-attachments/assets/4814be30-618a-4829-8c2b-848d4f397111" />

고급 설정에 들어간다.

<img width="494" height="625" alt="13  고급 설정" src="https://github.com/user-attachments/assets/3e837e34-4690-429d-8be4-be096be3931e" />

인바운드 규칙으로 들어간다.

<img width="192" height="106" alt="14  인바운드 규칙" src="https://github.com/user-attachments/assets/6e9159ea-d38d-4216-a9d3-b56b7a8b709d" />

새 규칙으로 들어간다.

<img width="1033" height="307" alt="15  새 규칙" src="https://github.com/user-attachments/assets/2f62e6cd-12e5-4d37-ba69-0f2f45fda1ce" />

포트 -> 특정 로컬 포트에 설정한 포트 번호 입력 -> 연결 허용(아직 https로 접속이 불가하므로) -> 규칙 적용 -> 이름 설정으로 포트 번호 개방을 완료한다.

<img width="356" height="267" alt="16  포트" src="https://github.com/user-attachments/assets/2729a971-baea-4125-8769-fbfa79d0793a" />
<br>
<img width="589" height="247" alt="18  연결 허용" src="https://github.com/user-attachments/assets/95982f68-9548-437e-a169-ba11622db9ec" />
<br>
<img width="454" height="268" alt="19  규칙 적용" src="https://github.com/user-attachments/assets/3917fbd0-ef36-41b8-b7d8-7aeefd6197a5" />
<br>
<img width="600" height="253" alt="20  이름 설정" src="https://github.com/user-attachments/assets/f3eaecb1-1a1a-49ca-9e6e-1b95b83e874a" />
<br>

## 포트 포워딩

그 다음 네이버 등에 내 아이피를 검색하면 현재 나의 공인 ip 주소를 알 수 있다.
