# 몽고디비 설치

[몽고디비 Installer](https://www.mongodb.com/try/download/community)

위 url로 접속한다.

<img width="369" height="589" alt="1  다운로드 사이트" src="https://github.com/user-attachments/assets/15c25a22-0f6b-45c9-9cd6-c0efce0331d7" />

운영체제를 설정한 다음에 .msi 패키지로 다운로드한다.  
(.zip으로 받으면 설정이 귀찮아진다.)

<img width="617" height="477" alt="2  인스톨러 시작" src="https://github.com/user-attachments/assets/9b979662-7551-43f6-8fe7-84af62e9c07b" />  
<img width="611" height="475" alt="3  complete 선택" src="https://github.com/user-attachments/assets/20b67fd5-cb9d-472c-ace9-3a37c20edad1" />

Complete를 선택하고 모든 설정을 기본값으로 유지하며 진행한다.

<img width="610" height="475" alt="4  네트워크 서비스 유저" src="https://github.com/user-attachments/assets/062d3f55-0cc7-4dba-99dd-b90f4f3f7f99" />  
<img width="606" height="470" alt="5  몽고디비 컴패스" src="https://github.com/user-attachments/assets/db81f085-7806-4517-9411-5a5e6783bafb" />  

MongoDB Compass는 몽고디비를 GUI로 이용할 수 있는 도구인데 필요한 사람은 설치하자.  

<img width="607" height="472" alt="6  설치" src="https://github.com/user-attachments/assets/65339688-4ac3-4796-b306-bb7173e0c085" />  
<img width="606" height="477" alt="7  설치중" src="https://github.com/user-attachments/assets/679d8ca7-1e3d-4f8b-8fa8-83318cd208f5" />  
<img width="606" height="471" alt="8  설치 완료" src="https://github.com/user-attachments/assets/e03e989c-23a8-4c8f-9f48-d01ac0c022ca" />  

이제 몽고디비를 설치했으니 몽고디비를 사용할 CLI 도구인 몽고디비 shell을 설치해야 한다.

[몽고디비 shell](https://www.mongodb.com/try/download/shell)

위 url로 접속한다.

<img width="610" height="832" alt="9  mongosh 다운로드 사이트" src="https://github.com/user-attachments/assets/b54e720d-ab50-4e64-b154-7b2f0bb6cd79" />

마찬가지로 운영체제를 선택하고 .msi 패키지로 다운받는다.

<img width="606" height="470" alt="10  몽고디비 쉘 인스톨러 시작" src="https://github.com/user-attachments/assets/25f9c949-0843-4aab-8649-901117b92c35" />

<img width="605" height="470" alt="11  설치 경로" src="https://github.com/user-attachments/assets/afe1fc10-6e6c-4a8a-828c-2503e4f508c2" />

설치 경로는 경로 관리를 편하게 하기 위해서 몽고디비가 설치된 경로인 Program Files의 MongoDB 디렉터리 밑으로 설정해주겠다.

<img width="606" height="467" alt="12  설치중" src="https://github.com/user-attachments/assets/b7ec6f27-e6b6-4920-98d1-475bce2a47c0" />
<img width="602" height="471" alt="13  설치 완료" src="https://github.com/user-attachments/assets/605793df-5fe6-4231-a2fc-05e0f4952f17" />

설치가 완료되면 터미널에 mongosh를 입력해보고 몽고디비에 접속이 되는지 확인한다.

<img width="1462" height="622" alt="14  설치 확인" src="https://github.com/user-attachments/assets/aabd7a6c-e7bd-4b9a-9521-e27d23a3e0bf" />

대부분 자동으로 설정되겠지만 만약 접속이 안 되면 환경 변수가 설정을 해 줘야 한다.

방금 설치했던 Mongosh 경로를 복사해서 PostgreSQL 때 설정했던 방법대로 ```시스템 환경 변수 편집 -> 환경 변수(N) -> 시스템 변수의 PATH```에 붙여넣기하면 된다.  
[환경 변수 단계 참조](https://github.com/Junyong-Cho/ASPractice/tree/master/2.%20PostgreSQL%20%EC%84%A4%EC%B9%98%20%EB%B0%8F%20DB%20%EC%83%9D%EC%84%B1)

## 유저네임 패스워드 설정

몽고디비는 설치 과정에서 PostgreSQL처럼 패스워드를 설정하지 않았다.  

그렇기에 ```mongosh``` 명령어를 통해서 모든 db에 접근할 수 있었다.

따라서 이번에는 몽고디비의 유저와 패스워드를 등록하는 것을 해보겠다.

## 인증 활성화

몽고디비가 설치된 경로의 bin 디렉터리로 이동한다.  
윈도우 기준 기본 경로가 ```C:\Program Files\MongoDB\Server\버전\bin```일 것이다.  

mongod.cfg 파일을 메모장, vs코드, 노트패드 등 텍스트 편집기로 실행한다.  
<img width="210" height="208" alt="15  몽고디컨피그" src="https://github.com/user-attachments/assets/f4afd053-d0b0-438d-9524-8965355e56cd" />

Program Files 경로 밑에 있는 데이터들을 수정할 때 관리자 권한이 필요한데 메모장으로 수정하려면 mongod.cfg 파일을 다른 곳으로 옮긴 다음 수정하고 원래대로 돌려놓는 방법으로 수정해야 한다.  

```cfg
security:
  authorization : enabled
```

주석처리되어 있던 security: 부분의 주석을 제거하고 그 밑으로 authorization : enabled 코드를 위와 같이 추가한다.

그리고 윈도우 검색에 서비스를 입력하거나 ```윈도우+r``` 단축키로 실행된 창에 ```services.msc```를 입력해서 서비스 옵션을 실행한 다음에

<img width="764" height="343" alt="16  서비스" src="https://github.com/user-attachments/assets/f2c49ed9-bca0-47e3-8484-a915b9ee7177" />  
<img width="395" height="198" alt="17  서비스" src="https://github.com/user-attachments/assets/3e37f3b6-3d71-4d62-9b03-608abe955090" />

MongoDB Server를 찾아 다시 시작을 누른다.

<img width="804" height="589" alt="18  서비스 다시시작" src="https://github.com/user-attachments/assets/3a1b7a38-d877-4a2f-bc89-25fcbace0a47" />

## admin 유저 등록

인증을 활성화해주었으니 이제 admin 유저를 등록해보겠다.

```mongosh``` 명령어로 몽고디비로 접속한 다음 ```use admin```으로 admin db로 접속한다.

그 다음 다음 명령어를 실행하여 admin 유저를 생성한다.
```js
db.createUser({
	user: 이름,
	pwd: passwordPrompt(), // 안전한 패스워드 입력
	roles:
		[{role: "userAdminAnyDatabase", db: "admin"},	// 모든 db 접근 권한
		{role: "readWriteAnyDatabase", db: "admin"}]	// 모든 db 읽고 쓰기 권한
});
```

해당 명령어를 실행하면 passwordPrompt()에 의해 패스워드를 입력받는 프롬프트가 실행되고 그곳에서 패스워드를 입력함으로써 admin 유저 등록이 완료된다.  
(패스워드 재입력 확인 과정이 없으므로 신중히 입력한다.)

이제 ```exit```로 몽고디비를 종료한 다음부터 admin db에 접근하기 위해서는 유저네임과 패스워드를 입력해서 접속해야 한다.

접속하는 방법은 ```mongosh -u 유저네임 -p 패스워드 --authenticationDatabase admin``` 명령어를 입력하는 것이다.

단순히 ```mongosh```로 몽고디비에 접속은 가능하나 db나 컬렉션을 조회하는 것이 거부된다.

<img width="751" height="360" alt="19  조회 실패" src="https://github.com/user-attachments/assets/91751d0a-4aaa-4607-888c-1f2a6d21e340" />


# 마무리
몽고디비 설치와 admin 유저 등록을 완료했다.
