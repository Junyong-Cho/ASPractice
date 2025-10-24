# docker �����̳ʿ��� postgres �����ϱ�

```docker pull postgres:latest``` ��ɾ�� postgres �̹����� �޴´�.

![1. Pull Postgres](../.dummy/1001%20��Ŀ%20postgres/1.%20pull%20postgres.png)

```docker volume create postgres-data``` ��ɾ�� postgres-data��� �̸��� ������ �����Ѵ�.

![2. ���� ����](../.dummy/1001%20��Ŀ%20postgres/2.%20����%20����.png)

```docker run -d --name postgres-db -e POSTGRES_PASSWORD=password -p 5555:5432 -v postgres-data:/var/lib/postgresql postgres:latest``` ��ɾ�� �����̳ʸ� �����Ѵ�.

```-d``` �����̳ʸ� ��׶��忡�� ����  
```-e``` ȯ�� ���� ���� ```POSTGRES_PASSWORD=password``` �н����带 password�� ����  
```-p``` ��Ʈ ���� ���� ������ 5555��Ʈ�� �����̳��� 5432 ��Ʈ�� ����(���ÿ� 5432��Ʈ�� �����쿡 ��ġ�� postgres�� ������̹Ƿ� 5555�� ����)  
```-v``` ���� ������ ������ �����̳ʿ� ���� ```/var/lib/postgresql``` ��ο� ����

![3. �����̳� ����](../.dummy/1001%20��Ŀ%20postgres/3.%20�����̳�%20����.png)

�����ϰ� ```docker ps```��ɾ �������� �� �����̳ʰ� ����Ǵ� ���̸� ����  
```docker ps```�� �� ������ ```docker ps -a```�� �����ؾ߸� ������ ����

## DBMS ����

�͹̳ο� ```psql -h localhost -p 5555 -U postgres```��ɾ �����ϰ� �н����带 �Է��Ͽ� ����

![4. ����](../.dummy/1001%20��Ŀ%20postgres/4.%20����.png)