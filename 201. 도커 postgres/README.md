# docker 컨테이너에서 postgres 실행하기

```docker pull postgres:latest``` 명령어로 postgres 이미지를 받는다.

<img width="1031" height="593" alt="1  pull postgres" src="https://github.com/user-attachments/assets/26cb8a53-0364-4201-af75-672bc47bf6c5" />

```docker volume create postgres-data``` 명령어로 postgres-data라는 이름의 볼륨을 생성한다.

<img width="625" height="172" alt="2  볼륨 생성" src="https://github.com/user-attachments/assets/40d3a629-90a0-41c3-a7ae-436ce7202506" />

```docker run -d --name postgres-db -e POSTGRES_PASSWORD=password -p 5555:5432 -v postgres-data:/var/lib/postgresql postgres:latest``` 명령어로 컨테이너를 생성한다.

```-d``` 컨테이너를 백그라운드에서 실행  
```-e``` 환경 변수 설정 ```POSTGRES_PASSWORD=password``` 패스워드를 password로 설정  
```-p``` 포트 연결 설정 로컬의 5555포트를 컨테이너의 5432 포트에 연결(로컬에 5432포트는 윈도우에 설치된 postgres가 사용중이므로 5555로 설정)  
```-v``` 볼륨 생성한 볼륨을 컨테이너에 연결 ```/var/lib/postgresql``` 경로에 연결


<img width="1630" height="176" alt="3  컨테이너 생성" src="https://github.com/user-attachments/assets/14361fe9-7a7c-4905-94f7-3eb928e35b49" />

생성하고 ```docker ps```명령어를 실행했을 때 컨테이너가 실행되는 중이면 성공  
```docker ps```로 안 나오고 ```docker ps -a```로 실행해야만 나오면 실패

## DBMS 접속

터미널에 ```psql -h localhost -p 5555 -U postgres```명령어를 실행하고 패스워드를 입력하여 접속

<img width="840" height="262" alt="4  접속" src="https://github.com/user-attachments/assets/66feab08-5020-4d09-ad55-480b2abd0aed" />

## docker 네트워크 연결

docker 컨테이너별로 배포 컨테이너와 db 컨테이너를 따로 만들었기 때문에 그 두 컨테이너의 소통을 위해서는 docker network로 연결해 주어야 한다.

```docker network create [네트워크 이름]``` 명령어로 도커 네트워크를 생성한다.

```docker network ls``` 명령어로 네트워크가 정상적으로 생성되었는지 확인할 수 있다.

```docker network connection [네트워크 이름] [컨테이너 이름]```으로 두 컨테이너를 같은 네트워크에 속하도록 한다.

```docker network inspect [네트워크 이름]```로 두 컨테이너가 네트워크에 제대로 속했는지 확인할 수 있다.

## ConnectionString 수정

로컬에 연결하기 위해 사용중이던 ConnectionString의 Port부분만 컨테이너의 5555로 변경한다.

```dotnet ef database update``` 명령어로 구조를 초기화한다.

## 데이터 복사

로컬 db에서 사용중이던 데이터를 컨테이너로 복사해 보겠다.

파워셀에서는 기능하지 않는 명령어이므로 명령 프롬프트로 실행해 주어야 한다.

```pg_dump -h localhost -p 5432 -U postgres --data_only [db이름] > out.sql``` 명령어를 사용하여 백업 파일을 생성한다. 이때 포트 번호는 로컬 db의 포트 번호이다.

```psql -h localhost -p 5555 -U postgres -d [db이름] < out.sql```명령어로 컨테이너에 데이터를 복사한다.
