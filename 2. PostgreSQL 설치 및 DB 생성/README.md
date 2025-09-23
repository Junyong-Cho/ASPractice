# 데이터베이스 설정

이전 단계에서 데이터에 대한 CRUD 요청을 수행할 때 코드에서 생성된 자료구조에서 데이터를 조회, 추가, 수정, 삭제를 수행하였다.  
이는 서버를 재시작했을 때 수행했던 요청이 전부 초기화되므로 데이터를 실제로 저장할 수 있는 데이터베이스에 데이터를 저장하여 요청을 처리하도록 해보겠다.

## PostgreSQL 설치
이번 단계에서는 PostgreSQL이라는 DBMS를 이용해볼 것이다.  
이 DBMS를 선택한 이유는 딱히 없다.

우선 PostgreSQL installer를 다운받기 위해 공식 사이트에 접속한다.

<img width="918" height="497" alt="PostgreSQL 공식 사이트" src="https://github.com/user-attachments/assets/3d59644d-4081-4c2d-bbac-fd54b532db41" />

운영체제에 맞는 installer를 선택하여 Download the installer를 클릭하고 다운받는다.  
~Download the installer 버튼이 조금 작다.~

<img width="1302" height="373" alt="운영체제에 맞는 installer" src="https://github.com/user-attachments/assets/af733986-a2f5-4a5e-add0-5d184f9a77de" />
<img width="1518" height="443" alt="installer 다운로드" src="https://github.com/user-attachments/assets/479aa09a-7d21-42d8-a026-336b5c97a41f" />
<img width="1209" height="370" alt="진짜 installer 다운 page" src="https://github.com/user-attachments/assets/4a65a5b5-be5d-4eb3-b329-9f600a967640" />

다운받은 installer를 실행하여 Default로 설정된 값들을 유지하면서 설치를 진행한다.

<img width="682" height="528" alt="installer1" src="https://github.com/user-attachments/assets/d81b707d-86ec-4d01-9647-46fa77038b85" />

후에 추가적인 설정을 위해 설치 경로를 기억해 둔다. (윈도우 기준 기본값은 C 드라이브의 Program Files일 것이다.)

<img width="675" height="527" alt="installer2" src="https://github.com/user-attachments/assets/530eab7b-b9a4-4695-af32-041733f5113d" />
<img width="676" height="525" alt="installer3" src="https://github.com/user-attachments/assets/a2e14058-85cf-424e-82a6-47ab777f8c7a" />
<img width="677" height="527" alt="installer4" src="https://github.com/user-attachments/assets/3fea7231-e270-47f0-9426-3d82fd9130ed" />

DBMS 접속을 위한 패스워드를 설정한다.

<img width="676" height="533" alt="installer5" src="https://github.com/user-attachments/assets/4298d607-f416-41d7-86e4-cce8bce6f56e" />

포트 번호는 기본 5432로 설정되어 있다.

<img width="682" height="532" alt="installer6" src="https://github.com/user-attachments/assets/f919a4f7-3755-48b2-b1c1-62880e4b16b2" />

설치는 10분 이내로 완료될 것이다.

<img width="671" height="525" alt="installing" src="https://github.com/user-attachments/assets/7534bddc-d659-4308-a772-8b804b5dbed1" />

설치가 완료되면 Stack build라는 것을 설치할 것인지 물어보는 체크박스를 해제하여 설치를 완료한다.

<img width="673" height="527" alt="install complete" src="https://github.com/user-attachments/assets/5cc56a94-9035-4f2c-b7a7-1044cd7d2ebf" />

## 환경 변수

설치가 완료되면 추가적인 설정이 필요하다.  
installer가 자동으로 환경 변수를 추가해주지 않기 때문에 환경 변수를 추가해 줘야 한다. (윈도우 기준으로 정리한다.)  
설치 과정에서 기억해 두었던 설치 경로를 찾아간다. (기본 경로: C:\Program Files\PostgreSQL)  

<img width="745" height="471" alt="환경 변수 경로" src="https://github.com/user-attachments/assets/6fcb4ace-950b-452e-8628-ab7b719630df" />

설치 경로에 있는 bin 디렉터리까지 들어간 후 F4를 눌러 경로를 선택하여 ctrl+c로 복사한다.

윈도우 검색창에 혹은 위도우 설정 검색창에서 환경 변수를 입력하여 '시스템 환경 변수 편집' 메뉴에 들어간다.

<img width="522" height="149" alt="환경 변수 메뉴" src="https://github.com/user-attachments/assets/2565b378-f9f7-4121-91bd-f37e62071e5d" />

환경 변수(N) 메뉴에 들어간 다음

<img width="471" height="527" alt="환경 변수1" src="https://github.com/user-attachments/assets/d24dfbb2-801f-4e19-a57e-cca1c93793b2" />

시스템 변수의 Path를 더블클릭한다.

<img width="440" height="492" alt="환경 변수2" src="https://github.com/user-attachments/assets/4d95bde8-e9ed-48b1-bcb7-6260cce55018" />

새로 만들기를 선택한 다음 복사한 경로를 붙여넣은 다음 저장한다.

<img width="389" height="114" alt="환경 변수3" src="https://github.com/user-attachments/assets/7d275468-4488-4afc-872d-f9e833a7ee68" />

환경 변수 설정을 완료하면 터미널에 ```psql --version``` 명령어를 입력하여 버전이 출력되는지 확인한다.  
(안 돼면 터미널을 재시작해본다.)

<img width="244" height="58" alt="psql version" src="https://github.com/user-attachments/assets/ae30d850-6d5e-4b7b-bd83-35496f840161" />

## 데이터베이스 생성

터미널에 ```psql -U postgres``` 명령어를 입력하여 DBMS에 접속한다.
그리고 설치 과정에서 설정했던 패스워드를 입력하면 접속할 수 있다.  
(입력이 터미널에 보이지 않을 것이다. 아무런 입력이 보이지 않는 것이 정상이니 본인의 타자 실력을 믿고 입력한다.)

<img width="375" height="157" alt="DBMS 접속" src="https://github.com/user-attachments/assets/3f399858-9e3e-4753-9984-dfa15a45e3a4" />

위 그림과 같이 나타나면 접속에 성공한 것이다.

접속에 성공하면 서버와 소통할 데이터베이스를 생성해 줄 것이다.  
```CREATE DATABASE mydb;```를 입력하여 mydb라는 이름의 데이터베이스를 생성한다. (대소문자 구분 X)

<img width="326" height="74" alt="Create database" src="https://github.com/user-attachments/assets/cafba01e-6acf-4448-924d-ecce8393ae25" />

위 그림과 같이 CREATE DATABASE라고 출력이 되면 생성이 성공한 것이다.

생성한 데이터베이스를 확인하기 위해 ```\l```을 입력해본다.

<img width="1192" height="96" alt="db 조회" src="https://github.com/user-attachments/assets/7b7cb017-ac43-4f51-885a-65a440cc7c3b" />

그림과 같이 mydb라는 이름의 데이터베이스가 생성되어 있는 것을 확인할 수 있다.


# 마무리
이렇게 DBMS 설치와 데이터베이스 생성을 완료했다.
