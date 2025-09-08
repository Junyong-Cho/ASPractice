# 데이터베이스 설정

이전 단계에서 데이터에 대한 CRUD 요청을 수행할 때 코드에서 생성된 자료구조에서 데이터를 조회, 추가, 수정, 삭제를 수행하였다.  
이는 서버를 재시작했을 때 수행했던 요청이 전부 초기화되므로 데이터를 실제로 저장할 수 있는 데이터베이스에 데이터를 저장하여 요청을 처리하도록 해보겠다.

## PostgreSQL 설치
이번 단계에서는 PostgreSQL이라는 DBMS를 이용해볼 것이다.  
이 DBMS를 선택한 이유는 딱히 없다.

우선 PostgreSQL installer를 다운받기 위해 공식 사이트에 접속한다.
![Postgre S Q L 공식 사이트](../dummy/3%20DB%20연결/PostgreSQL%20공식%20사이트.png)

운영체제에 맞는 installer를 선택하여 Download the installer를 클릭하고 다운받는다.  
~Download the installer 버튼이 조금 작다.~
![운영체제에 맞는 Installer](../dummy/3%20DB%20연결/운영체제에%20맞는%20installer.png)
![Installer 다운로드](../dummy/3%20DB%20연결/installer%20다운로드.png)
![진짜 Installer 다운 Page](../dummy/3%20DB%20연결/진짜%20installer%20다운%20page.png)

다운받은 installer를 실행하여 Default로 설정된 값들을 유지하면서 설치를 진행한다.
![Installer1](../dummy/3%20DB%20연결/installer1.png)

후에 추가적인 설정을 위해 설치 경로를 기억해 둔다. (윈도우 기준 기본값은 C 드라이브의 Program Files일 것이다.)

![Installer2](../dummy/3%20DB%20연결/installer2.png)
![Installer3](../dummy/3%20DB%20연결/installer3.png)
![Installer4](../dummy/3%20DB%20연결/installer4.png)

DBMS 접속을 위한 패스워드를 설정한다.

![Installer5](../dummy/3%20DB%20연결/installer5.png)

포트 번호는 기본 5432로 설정되어 있다.

![Installer6](../dummy/3%20DB%20연결/installer6.png)

설치는 10분 이내로 완료될 것이다.

![Installing](../dummy/3%20DB%20연결/installing.png)

설치가 완료되면 Stack build라는 것을 설치할 것인지 물어보는 체크박스를 해제하여 설치를 완료한다.

![Install Complete](../dummy/3%20DB%20연결/install%20complete.png)

설치가 완료되면 추가적인 설정이 필요하다.  
installer가 자동으로 환경 변수를 추가해주지 않기 때문에 환경 변수를 추가해 줘야 한다. (윈도우 기준으로 정리한다.)  
설치 과정에서 기억해 두었던 설치 경로를 찾아간다. (기본 경로: C:\Program Files\PostgreSQL)  

![환경 변수 경로](../dummy/3%20DB%20연결/환경%20변수%20경로.png)

설치 경로에 있는 bin 디렉터리까지 들어간 후 F4를 눌러 경로를 선택하여 ctrl+c로 복사한다.

윈도우 검색창에 혹은 위도우 설정 검색창에서 환경 변수를 입력하여 '시스템 환경 변수 편집' 메뉴에 들어간다.

![환경 변수 메뉴](../dummy/3%20DB%20연결/환경%20변수%20메뉴.png)

환경 변수(N) 메뉴에 들어간 다음

![환경 변수1](../dummy/3%20DB%20연결/환경%20변수1.png)

시스템 변수의 Path를 더블클릭한다.

![환경 변수2](../dummy/3%20DB%20연결/환경%20변수2.png)

새로 만들기를 선택한 다음 복사한 경로를 붙여넣은 다음 저장한다.

![환경 변수3](../dummy/3%20DB%20연결/환경%20변수3.png)

환경 변수 설정을 완료하면 터미널에 ```psql --version``` 명령어를 입력하여 버전이 출력되는지 확인한다.  
(안 돼면 터미널을 재시작해본다.)

![Psql Version](../dummy/3%20DB%20연결/psql%20version.png)

## 데이터베이스 생성

터미널에 ```psql -U postgres``` 명령어를 입력하여 DBMS에 접속한다.
그리고 설치 과정에서 설정했던 패스워드를 입력하면 접속할 수 있다.  
(입력이 터미널에 보이지 않을 것이다. 아무런 입력이 보이지 않는 것이 정상이니 본인의 타자 실력을 믿고 입력한다.)

![D B M S 접속](../dummy/3%20DB%20연결/DBMS%20접속.png)

위 그림과 같이 나타나면 접속에 성공한 것이다.

접속에 성공하면 서버와 소통할 데이터베이스를 생성해 줄 것이다.  
```CREATE DATABASE mydb;```를 입력하여 mydb라는 이름의 데이터베이스를 생성한다. (대소문자 구분 X)

![Create Database](../dummy/3%20DB%20연결/Create%20database.png)

위 그림과 같이 CREATE DATABASE라고 출력이 되면 생성이 성공한 것이다.

생성한 데이터베이스를 확인하기 위해 ```\l```을 입력해본다.

![Db 조회](../dummy/3%20DB%20연결/db%20조회.png)

그림과 같이 mydb라는 이름의 데이터베이스가 생성되어 있는 것을 확인할 수 있다.


# 마무리
이렇게 DBMS 설치와 데이터베이스 생성을 완료했다.