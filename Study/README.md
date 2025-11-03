# 저번 시간 복습

## 프로젝트 생성
[프로젝트 생성](https://github.com/junyong-cho/aspractice?tab=readme-ov-file#%EC%83%88-%ED%94%84%EB%A1%9C%EC%A0%9D%ED%8A%B8-%EC%83%9D%EC%84%B1)하는 방법은 해당 url을 참고하자

## C# 코드 기본 양식

C#은 엄격한 객체지향 언어이므로 프로그램을 실행하는 객체도 존재해야 한다.

따라서 우리가 동작하기 원하는 코드를 객체 안에 작성해야 한다.

아래 코드에서 나오다시피 Program이 바로 객체다.

```C#
public class Program    // 코드 실행용 객체
{
    static public void Main(string[] args)  // Main 메서드
    {
        /* 여기에 코드를 작성 */
    }
}
```

다음과 같이 코드를 작성하고 실행하면 ```Hello world```가 출력된다.

```C#
public class Program
{
    static public void Main(string[] args)  
    {
        Console.WriteLine("Hello world");
    }
}
```

프로그램을 실행할 때 Program 객체를 만든 다음 그 객체의 Main 메서드를 실행하는 것이다.

## 코드 실행 방법

비쥬얼 스튜디오에서 ````Ctrl + ` ```` 단축키로 터미널을 연다.

```dir``` 명령어로 현재 경로에 .csproj 파일이 존재하는지 확인하고 존재하지 않으면 ```cd [프로젝트 이름]``` 명령어로 디렉터리를 이동한 다음에 다시 ```dir``` 명령어로 .csproj 파일이 존재하는지 확인한다.

<img width="958" height="778" alt="1  경로" src="https://github.com/user-attachments/assets/c3602c52-7d3c-497f-9990-c5996393d890" />

```dotnet run``` 명령어로 프로그램을 실행한다.

<img width="383" height="63" alt="2  실행" src="https://github.com/user-attachments/assets/b92145fd-19cf-4019-83b6-3bf44459cf57" />

정상적으로 Hello world가 출력되면 성공이다.


# 객체 지향 프로그래밍

## 객체 지향 등장 배경 - 코드의 유지보수를 위해서

## 정적 타입 언어와 동적 타입 언어

파이썬 같은 경우 다음과 같은 코드가 하나의 코드 블럭 안에 있어도 전혀 문제가 되지 않는다.  
파이썬이 동적 타입 언어이기 때문  
동적 타입 언어란 변수의 타입이 변화할 수 있다는 것을 말한다.

```python
a = 5       # 정수
a = 'Hello' # 문자열
```

그러나 C#은 변수의 타입이 선언과 동시에 고정되는 정적 타입 언어다.

따라서 위와 같은 코드는 타입 에러를 발생시키며 에디터 상에서 빨간 줄이 표시될 것이다.
(엄밀히 말하면 C#에서도 위와 같은 코드를 짤 수는 있다.)

<img width="584" height="186" alt="3  TypeError" src="https://github.com/user-attachments/assets/0af9fafb-38c7-4706-a6a6-0bcb0fe297e1" />

그렇다면 변수가 정수인지 문자열인지 그 외에 다른 타입인지 컴퓨터가 알 수 있도록 해야 하는데 이것이 바로 변수 선언이다.

C#에서는 변수 선언을 다음과 같이 한다.

```[타입] [변수이름]```  
```int a;``` 이것은 a라는 이름의 변수를 정수로 사용할 것이라고 컴파일러에게 선언하는 것이다.  
```string st;```도 마찬가지로 st라는 이름의 변수를 문자열로 사용할 것이라고 선언하는 것이다.

```C#
class Program
{
    static public void Main(string[] args)
    {
        int a = 1;
        string st = "hello";
    }
}
```

## 클래스

클래스는 일종의 데이터 타입으로 생각하면 쉽다. int, string과 같은 데이터 타입을 생성하는 키워드가 class인 것이다.

이 클래스를 선언하는 방법도 변수 선언 방법과 동일하다.

```class [클래스이름]```

앞서 설명했던 것처럼 ```class Robot``` 이것은 Robot이라는 이름의 데이터 타입을 만들었다고 컴파일러에게 알리는 것이다.

```C#
public class Program
{
    static public void Main(string[] args)
    {

    }
}

class Robot
{

}
```

위 코드처럼 클래스를 선언할 수 있다.

## 객체 - 인스턴스

선언한 클래스는 일종의 데이터 타입이라고 설명했다.  
따라서 정수 타입, 문자열 타입과 마찬가지로 Robot 타입의 변수를 생성할 수 있다.  
이렇게 생성한 변수를 바로 객체(인스턴스)라고 한다.

앞으로 객체는 인스턴스라고 표현하겠다.

```C#
public class Program
{
    static public void Main(string[] args)
    {
        int a;          // 정수 인스턴스
        string st;      // 문자열 인스턴스
        Robot robot;    // Robot 인스턴스
    }
}

class Robot
{

}
```

C#은 어쩌면 자바보다도 더 엄격한 객체지향 언어이므로 정수와 같은 자바에서는 기본형 데이터 타입으로 취급하는 타입들도 전부 인스턴스에 속한다.

즉 위 코드의 a, st, robot은 모두 인스턴스라고 할 수 있다.

다만 int, string 같은 클래스들은 C#에서 특별하게 취급되는 타입이라 Robot 타입의 변수에 데이터를 할당하기 위해서는 조금 다른 방법이 필요하다.

```Robot robot = new Robot();``` 이것은 Robot 타입의 변수 robot에 새로운(new) Robot 인스턴스를 할당하겠다는 뜻이며 이것이 바로 인스턴스 생성 방법이다.

```C#
public class Program
{
    static public void Main(string[] args)
    {
        int a = 1;          // 정수 인스턴스
        string st = "";      // 문자열 인스턴스
        Robot robot = new Robot();    // Robot 인스턴스

        Console.WriteLine(a.GetType());
        Console.WriteLine(st.GetType());
        Console.WriteLine(robot.GetType());
    }
}

class Robot
{

}
```

위 코드를 보면 int와 string 인스턴스를 생성하는 방법과 Robot 인스턴스를 생성하는 방법의 차이를 볼 수 있다.  
그리고 ```GetType()```이라는 메서드를 사용하여 각 인스턴스의 데이터 타입을 출력해볼 수 있다.

<img width="384" height="99" alt="4  타입" src="https://github.com/user-attachments/assets/fc98d379-70af-45b2-a2ef-5dadf933540a" />

a의 타입은 Int32(32비트(4바이트) 정수) st의 타입은 String, roboto의 타입은 Robot이라고 잘 출력되었다.

## 클래스 내부 구조

우리가 만든 Robot이라는 클래스는 내부에 아무것도 정의해 놓지 않았다.  
우리가 만든 로봇은 이름이 있어야 할 거 같다.

로봇이 이름을 가지기 위해서는 클래스 내부에 이름을 만들어 주어야 한다.

```C#
class Robot
{
    public string Name { get; set; }
}
```

문자열 타입의 Name이라는 변수를 Robot 클래스 내부에 선언해 주었다.  
```{ get; set;}```은 인스턴스의 Name에 접근하기 위한 장치라고 생각하면 된다. get으로 값을 읽을 수 있고 set으로 값을 설정할 수 있다.

또 로봇이 말을 할 수 있으면 좋겠다.  
그래서 말을 하는 기능을 간단하게 구현해보자.

```C#
class Robot
{
    public string Name { get; set; }

    public void Say()
    {
        Console.WriteLine("내 이름은 " + Name);
    }
}
```

이제 생성한 로봇 인스턴스는 이름을 가지고 있고 자기 이름을 말할 수 있다.

이제 이 기능을 테스트해 보겠다.

```C#
public class Program
{
    static public void Main(string[] args)
    {
        Robot robot = new Robot();          // Robot 인스턴스 생성

        robot.Name = "김철수";              // 인스턴스의 이름 설정

        robot.Say();                        // 인스턴스가 말하도록 함
    }
}

class Robot
{
    public string Name { get; set; }

    public void Say()
    {
        Console.WriteLine("내 이름은 " + Name);
    }
}
```

<img width="1648" height="100" alt="5  테스트" src="https://github.com/user-attachments/assets/4142d3bf-56c6-4279-9df8-c97410d08ce8" />

마지막 줄에 내 이름은 김철수라고 설정해 놓은 이름이 잘 출력되는 것을 확인할 수 있다.  
코드를 실행하니 경고가 나왔는데 이는 string 타입이 null값을 허용하지 않는데 처음 Robot 인스턴스를 생성할 때 Name이 null값이 되기 때문에 컴파일러에서 경고를 보내는 것이다.

이 경고가 출력되지 않도록 하는 방법들은 다음과 같다. (중요 X)
- 생성자를 구현해서 생성자로만 인스턴스를 생성할 수 있도록 한다.
- string? 처럼 ?를 뒤에 붙여 선언함으로써 null값을 허용한다.
- 기본값을 설정한다.
- null!을 할당하여 컴파일러에게 "절대 null값을 허용하지 않을 테니 안심하고 경고하지 마"라고 안심시킨다.

이렇게 로봇의 이름과 기능을 간단하게 구현해보았다.  
여기서 이름은 필드라고 하고 기능은 메서드라고 한다.  
그리고 이름을 읽고 쓰기 위해 ```{ get; set; }```으로 설정한 변수를 프로퍼티라고 한다.

```
필드 - 객체가 가지고 있는 데이터
메서드 - 객체가 하는 동작
```

## 상속

이전 시간에 설명했던 것처럼 로봇 코드를 만들고 난 후 야구선수 로봇 혹은 가수 로봇처럼 기능이 확장된 로봇을 만들고 싶을 때 객체 지향 이전 시기에는 코드를 복붙하는 방법이 일반적이었다.  

이는 기존 로봇 코드에 수정 사항이 생기면 기껏 복붙해 놓은 다른 코드들도 같이 수정해 줘야 하는 등 유지보수에 애로사항이 있다고 했다.

객체 지향에서는 그저 로봇 코드를 참조하도록 하여 기존 코드가 수정되면 참조하고 있는 코드들에 자동으로 반영되도록 할 수 있다. 이를 <b>상속</b>이라 한다.

그럼 가수 로봇을 새로 만들어보겠다.

```C#
class Robot
{
    public string Name { get; set; } = null!;

    public Robot() { }
    public void Say()
    {
        Console.WriteLine("내 이름은 " + Name);
    }
}

class Singer : Robot
{

}
```

이렇게 하면 로봇의 Name과 Say()를 전부 Singer 인스턴스가 사용할 수 있게 된다.

```C#
public class Program
{
    static public void Main(string[] args)
    {
        Singer singer = new Singer();
        singer.Name = "김철수";

        singer.Say();
    }
}

class Robot
{
    public string Name { get; set; } = null!; // 컴파일러 안심시키기

    public Robot() { }
    public void Say()
    {
        Console.WriteLine("내 이름은 " + Name);
    }
}

class Singer : Robot
{

}
```

위 코드를 실행하면 Singer 클래스에 아무것도 구현한 것이 없음에도 Name이 설정되고 Say 메서드로 자신의 이름을 말하는 것을 확인할 수 있을 것이다.

여기서 Robot의 Say 메서드를 수정하여도 singer가 수정본으로 동작하는 것도 확인할 수 있다.

```C#
public class Program
{
    static public void Main(string[] args)
    {
        Singer singer = new Singer();
        singer.Name = "김철수";

        singer.Say();
    }
}

class Robot
{
    public string Name { get; set; } = null!; // 컴파일러 안심시키기(경고 출력 방지)

    public Robot() { }
    public void Say()
    {
        Console.WriteLine("내 이름은 " + Name + "\n나는 로봇이다."); // 나는 로봇이다 추가
    }
}

class Singer : Robot
{

}
```

<img width="617" height="84" alt="6  테스트" src="https://github.com/user-attachments/assets/e79ea64e-e474-4d07-808f-2cde7ae68fd3" />

## 오버라이드

그런데 Singer는 로봇이면서 가수이기 때문에 기존 로봇에 "나는 가수다" 라고 부연설명하고 싶다.  
Singer에 "나는 가수다"를 출력하는 메서드를 새로 만들어도 되는데 상속받은 Say 메서드를 조금 튜닝하여도 된다. 이를 <b>오버라이드(override)</b>라고 한다.

오버라이드를 하기 위해서는 기존 코드를 튜닝할 수 있도록 하는 키워드가 필요하다.  
Robot 클래스의 Say 메서드에 ```virtual```라는 키워드를 추가한다.


```C#
public class Program
{
    static public void Main(string[] args)
    {
        Singer singer = new Singer();
        singer.Name = "김철수";

        singer.Say();
    }
}

class Robot
{
    public string Name { get; set; } = null!;

    public Robot() { }
    public virtual void Say()       // virtual 키워드 추가
    {
        Console.WriteLine("내 이름은 " + Name + "\n나는 로봇이다.");
    }
}

class Singer : Robot
{
    public override void Say()      // override
    {
        base.Say();                 // 부모 클래스의 Say 메서드 실행
        Console.WriteLine("그리고 나는 가수다.");
    }
}
```

위 코드처럼 수정하고 실행하고 결과를 확인해보자.

<img width="433" height="100" alt="7  나는 가수다" src="https://github.com/user-attachments/assets/e52cb707-8038-4c76-a98e-ac7bb239a0a6" />

## 매개변수

```C#
public class Program
{
    static public void Main(string[] args)
    {
        Robot robot = new Robot();

        robot.Name = "김철수";

        Singer singer = new Singer();

        singer.Name = "박가수";

        Say(robot);
        Console.WriteLine();
        Say(singer);
    }

    static void Say(Robot robot)        // Robot 타입을 매개변수로 받는 메서드
    {
        robot.Say();
    }

}

class Robot
{
    public string Name { get; set; } = null!;

    public Robot() { }
    public virtual void Say()
    {
        Console.WriteLine("내 이름은 " + Name + "\n나는 로봇이다.");
    }
}

class Singer : Robot
{
    public override void Say()      // override
    {
        base.Say();
        Console.WriteLine("그리고 나는 가수다.");
    }
}
```

Program.cs 클래스에 Robot 타입을 매개변수로 받는 메서드를 하나 구현했다.

그리고 이 매개변수의 자리에 Robot을 상속받는 Singer 타입도 들어갈 수 있음을 코드에서 확인할 수 있다.

그리고 코드를 실행해 보면 오버라이드된 메서드에 따라 다르게 동작하는 것도 확인할 수 있을 것이다.

이것이 가능한 이유는 Robot 클래스에 정의된 것들이 Singer 클래스에도 포함되어 있기 때문에 그렇다.

따라서 Singer 타입을 매개변수로 받는 메서드에는 Robot 타입이 들어갈 수 없다.  
Singer 클래스에는 Robot 클래스에 없는 추가적인 구현이 존재할 수 있기 때문이다.


## 다중 상속

C#에서 다중 상속은 문법적으로 금지되어 있다.

내가 Robot 클래스를 상속받은 야구선수 로봇 클래스를 만든 다음에 가수 로봇 클래스와 야구선수 로봇 클래스를 동시에 상속받아 노래하는 야구선수 로봇을 만들고 싶다고 해도 그렇게 할 수 없는 것이다.

그 이유는 다음 그림을 보자

<img width="454" height="513" alt="8  다이아몬드 상속" src="https://github.com/user-attachments/assets/52ffbe10-c62f-4c39-8976-a110fcf4140e" />

가수 로봇과 야구선수 로봇 모두 Say 메서드를 가지고 있고 각자 자신에 맞게 오버라이드했다.

그리고 그것을 노래하는 야구선수가 상속받았다고 할 때 노래하는 야구선수가 가수의 Say를 상속받아야 할지 야구선수의 Say를 상속받아야 할지 알 수가 없다.  
결국 뭐든 상속받게 될 텐데 문제는 무엇을 상속받게 될지 모른다는 것이고 프로그래밍에서 이런 불확실성은 이상적이지 않다.

프로그래머의 실력에 따라 이런 불확실성의 문제는 얼마든지 회피할 수 있으나 그 가능성을 완전히 배제하기 위해 C#에서는 다중상속을 원천적으로 차단하고 있다.

## 인터페이스

그러나 다중 상속이 필요할 때가 분명히 있다.  
그 경우를 위해서 인터페이스(interface)라는 개념도 존재한다.

인터페이스는 클래스와 마찬가지로 `interface [이름]`으로 선언할 수 있고 비슷하게 `:`(콜론)으로 상속도 할 수 있다.

인터페이스와 클래스의 차이는 기본적으로 메서드로만 구성되어 있고 메서드가 구현되지 않고 선언되어 있다는 차이가 있다.  
또 클래스는 그 자체로 인스턴스를 가질 수 있지만 인터페이스는 인스턴스를 생성할 수 없다. 반드시 인터페이스를 상속받은 클래스로만 생성할 수 있다.

```C#
interface IInterface
{
    public void Say();
}
```

위 예시코드처럼 메서드에 아무 구현이 없이 선언만 되어 있고 상속을 받을 경우 반드시 해당 메서드를 클래스 내에 구현해야 한다. 그래서 클래스는 상속이라 하고 인터페이스는 구현이라 한다.
(C#에서는 상속과 구현 모두 `:`으로 퉁치지만 자바에서는 상속은 `extends` 구현은 `implements`로 표시한다.)

반드시 상속받은 메서드를 구현해야 하므로 다중 구현으로 같은 이름의 메서드가 존재한다고 해도 문제가 되지 않는다.

마지막으로 인터페이스는 C#의 약속 문법으로 이름이 인터페이스의 I로 시작한다.  
(ex. `IValidator`, `IMapper`, `IMongoClient`)

## 추상 클래스

클래스에는 인터페이스와 비슷한 추상 클래스라는 것도 존재한다.

추상 클래스는 클래스와 인터페이스의 중간 정도라고 보면 된다.

클래스처럼 필드와 구현된 메서드를 가지고 있을 수 있고 인터페이스처럼 구현되지 않은 메서드(추상 메서드를) 가지고 있을 수 있으며 인터페이스와 마찬가지로 인스턴스를 생성할 수 없다.
추상 메서드는 메서드에 `abstract` 키워드를 붙이면 된다.

```C#
abstract class AbstractClass
{
    String Name;                                    // 필드
    public void Say()                               // 메서드
    {
        Console.WriteLine("내 이름은 " + Name);
    }

    public abstract void Method();                  // 추상 메서드
}
```

다중 상속을 위해서 인터페이스가 존재한다면 추상 클래스는 왜 존재할까? (물론 인터페이스의 존재 의미는 다중 상속뿐만이 아니다.)

그건 바로 클래스는 그 자체로 인스턴스가 되어 사용할 수 있지만 추상클래스는 인터페이스처럼 상속받은 클래스로 인스턴스를 생성해야 하기 때문이다.

즉 인터페이스와 다르게 메서드가 구현되어 있으면서도 그 자체로 인스턴스를 생성하지 말고 반드시 상속받은 클래스를 사용해야 할 때 추상 클래스로 구현하면 된다. (마찬가지로 이것이 존재 이유의 전부는 아니다.)

추상클래스도 인터페이스처럼 약속으로 이름이 Abstract로 시작한다.

(ex. `AbstractValidator`)


## Obejct 클래스 - 모든 클래스의 조상

우리가 맨 처음에 아무것도 없는 Robot 클래스를 만들었을 때 GetType 메서드를 사용했던 것을 기억해보자.

우리는 Robot 클래스에 어떠한 메서드도 구현한 적이 없다.  
그럼에도 GetType이라는 메서드는 사용할 수 있었다.

이는 C#이 클래스를 생성할 때 상속받는 클래스를 명시하지 않을 경우 자동으로 Object 클래스를 상속받도록 구현되어 있기 때문이다.

GetType이라는 메서드는 Object 클래스에 구현되어 있고 따라서 Object 클래스를 상속받은 Robot 클래스에서 GetType()이라는 메서드를 사용할 수 있던 것이다. (이외에도 ToString(), GetHashCode() 등 여러 가지가 구현되어 있다.)

맨 처음에 C#에서 파이썬처럼 변수에 정수를 저장했다가 문자열을 저장할 수 있다고 말했었는데 변수를 Object로 선언하면 그렇게 할 수 있다.

Robot 타입 변수에 Robot을 상속받은 Singer를 할당할 수 있었던 것처럼 Object 타입으로 변수를 선언하면 Object를 상속받은 int, string, Robot, Singer 등이 모두 해당 변수에 할당될 수 있다. (Singer는 Object를 상속받은 Robot을 상속받았으므로 결과적으로 Object도 상속받았다고 표현할 수 있다.)

<img width="510" height="235" alt="9  Object" src="https://github.com/user-attachments/assets/f587f630-8213-42c9-bbe6-130f24134178" />
*타입 에러가 발생하지 않는다.*

그러나 이런 프로그래밍 방식은 박싱과 언박싱하는 과정이 존재하여 성능이 떨어지고 정적 타입의 장점을 해치므로 권장하지 않는다. 그냥 알아만 두자

## 축약

우리는 지금까지 Robot과 같은 인스턴스를 생성할 때 `new Robot()`으로 인스턴스를 생성했다.

그러나 Robot 타입의 변수에 Robot의 자식 클래스를 할당할 것이 아니라면 뒤에 Robot()은 생략이 가능하다.  
컴파일러가 추론이 가능하기 때문이다.

```C#
Robot robot = new();
```

위와 같은 코딩이 가능하다. 꽤 편리하다.

## 제네릭

우리가 만든 클래스는 필드는 string 등으로 고정되어 있다. Robot 클래스의 필드는 이름이므로 string으로 고정해 놓아도 큰 문제가 되지 않는다.

그렇지만 Queue, Stack 같은 자료구조에 데이터를 저장할 때 int를 저장할 수도 있고 int[]를 저장할 수도 있고 다른 타입을 저장할 수도 있다.

그런데 데이터 타입마다 클래스를 구현하는 것은 비효율적이다. 데이터 저장 타입을 Object로 하면 가능은 하겠지만 앞서 설명했다시피 Object 타입으로 프로그래밍하는 것은 이상적이지 않다.

여기서 필요한 것이 제네릭이다.

```C#
public class Program
{
    static public void Main(string[] arg)
    {
        TestClass<string> str = new("Hello");       // 제네릭 타입으로 string 설정
        TestClass<int> integer = new(1);            // 제네릭 타입으로 int 설정
        TestClass<Robot> robot = new(new());        // 제네릭 타입으로 Robot 설정
        TestClass<Queue<int>> q = new(new());       // 제네릭 타입으로 Queue<int> 설정
                                                            // Queue의 제네릭 타입은 int
    }
}

class Robot { }

class TestClass<E>
{
    public TestClass(E e)
    {
        Data = e;
    }
    public E? Data { get; set; }
}
```

위 코드에서 TestClass(class이름) 옆에 `<E>`라는 표시가 있다. 이것이 바로 제네릭으로 여기에 타입을 명시하면 클래스 내부에 E 타입들이 전부 해당 타입으로 변환된다.

예를 들어 `TestClass<string>` 으로 선언하면

```C#
class TestClass<string>
{
    public TestClass(string e)
    {
        Data = e;
    }
    public string? Data { get; set; }
}
```

위 코드처럼 컴파일 과정에서 변환되어 사용할 수 있다.

# 대리자

C#의 고유 문법으로 대리자라는 것이 존재한다.

대리자는 메서드를 대리해서 실행하는 메서드라고 이해하면 쉽다.

대리자를 생성하는 방법은 마찬가지로 `delegate` 키워드를 이용하는 것이다.

```C#
public class Program
{
    static public void Main(string[] arg)
    {

    }

    delegate int Delegate(int i, int j); // <- 대리자
}
```

위 대리자는 매개변수로 정수 i,j를 받고 정수를 return 하는 메서드를 대리해서 실행할 수 있다.

```C#
public class Program
{
    static public void Main(string[] arg)
    {
        Delegate del = min;

        Console.WriteLine(del(1, 2));

        del = max;

        Console.WriteLine(del(1, 2));

        del = plus;

        Console.WriteLine(del(1, 2));
    }

    delegate int Delegate(int i, int j);

    static int min(int i, int j)            // 작은 값 return
    {
        return i < j ? i : j;
    }

    static int max(int i, int j)            // 큰 값 return
    {
        return i > j ? i : j;
    }

    static int plus(int i, int j)           // 두 수 합 return
    {
        return i + j;
    }
}
```

위 코드를 실행해 보면 같은 이름의 메서드(del)와 같은 매개변수(1,2)를 입력해서 세 번 출력했음에도 다르게 출력되는 것을 확인할 수 있다.

<img width="339" height="101" alt="10  대리자" src="https://github.com/user-attachments/assets/40158b97-7950-48cc-8482-404af732b0f3" />

대리자에 각자 min 메서드와 max 메서드, plus 메서드가 들어있기 때문이다.

대리자도 마찬가지로 제네릭 타입으로 선언할 수 있다.

```C#
public class Program
{
    static public void Main(string[] arg)
    {
        Delegate<int,int,int> del = min;        // <- int 타입을 return하고 매개변수로 각각 정수를 입력받음

        Console.WriteLine(del(1, 2));

        del = max;

        Console.WriteLine(del(1, 2));

        del = plus;

        Console.WriteLine(del(1, 2));
    }

    delegate T Delegate<T, A, B>(A a, B b);      // <- 매개변수를 제네릭으로 변경

    static int min(int i, int j)           
    {
        return i < j ? i : j;
    }

    static int max(int i, int j)          
    {
        return i > j ? i : j;
    }

    static int plus(int i, int j)         
    {
        return i + j;
    }
}
```

## 대리자 존재 이유

위 코드를 보면 그냥 `min` `miax` `plus` 메서드를 출력하면 될 것을 귀찮게 대리자에 할당해서 사용하는 이유를 알 수 없을 것이다.  
실제로 대리자는 저렇게 사용하지 않는다. 

대리자는 메서드를 매개변수로 받고 싶을 때 사용하는 것이다.

```C#
public class Program
{
    static public void Main(string[] arg)
    {
        int[] a = { 321, 4, 12, 45, 567, 23, 15, 589, 237, 69, 19, 3, 23 };

        for (int i = 0; i < a.Length; i++)
            Console.Write(a[i] + " ");

        Console.WriteLine("\n");

        Sort(a, Ascending);

        for (int i = 0; i < a.Length; i++)
            Console.Write(a[i] + " ");

        Console.WriteLine("\n");

        Sort(a, Descending);
        
        
        for (int i = 0; i < a.Length; i++)
            Console.Write(a[i] + " ");
    }

    static bool Ascending(int i, int j)
    {
        return i > j;
    }

    static bool Descending(int i, int j)
    {
        return i > j;
    }

    delegate bool Compare(int i, int j);

    static void Sort(int[] a, Compare comp)
    {
        for(int i = 0; i < a.Length; i++)
        {
            for(int j = i+1; j < a.Length; j++)
            {
                if (comp(a[i], a[j]))
                    (a[i], a[j]) = (a[j], a[i]);    // 스왑
            }
        }
    }
}
```

`Sort` 메서드는 버블 정렬 메서드이다. Compare이라는 이름의 대리자를 매개변수로 받아서 대리자에 따라 오름차순과 내림차순으로 정렬할 수 있다.

<img width="525" height="146" alt="11  버블 정렬" src="https://github.com/user-attachments/assets/3346bb9b-c4ec-4924-a0bd-596b09af1829" />

`Descending` 메서드를 `Sort`에 넣었을 때는 내림차순으로 `Ascending` 메서드를 `Sort`에 넣었을 때는 오름차순으로 정렬된 것을 확인할 수 있다.

## 익명 대리자

대리자를 매개변수로 받는 자리에 메서드를 넣으려면 `Ascending` `Descending` 처럼 메서드를 어딘가에 구현해 놓아야 한다.

메서드는 일종의 함수로 코드를 재사용 할 때 주로 사용하는 편인데 매개변수로 집어넣을 메서드를 단 한 번 사용할 때 메서드를 구현해 놓는 것보다 그 메서드를 구현해 놓는 것 외에도 익명 대리자를 생성하여 매개변수로 대입하는 것이 하나의 방법이 될 수 있다.  
그리고 이것이 우리가 .NET Core를 사용할  가장 자주 사용하는 방법일 것이다.

위 정렬 코드를 익명 대리자로 바꿔 보겠다.

```C#
public class Program
{
    static public void Main(string[] arg)
    {
        int[] a = { 321, 4, 12, 45, 567, 23, 15, 589, 237, 69, 19, 3, 23 };

        for (int i = 0; i < a.Length; i++)
            Console.Write(a[i] + " ");

        Console.WriteLine("\n");

        Sort(a, delegate(int i,int j)
        {
            return i > j;
        });

        for (int i = 0; i < a.Length; i++)
            Console.Write(a[i] + " ");

        Console.WriteLine("\n");

        Sort(a, delegate(int i, int j)
        {
            return i < j;
        });
        
        
        for (int i = 0; i < a.Length; i++)
            Console.Write(a[i] + " ");
    }

    //static bool Ascending(int i, int j)   사용 X
    //{
    //    return i > j;
    //}

    //static bool Descending(int i, int j)
    //{
    //    return i < j;
    //}

    delegate bool Compare(int i, int j);

    static void Sort(int[] a, Compare comp)
    {
        for(int i = 0; i < a.Length; i++)
        {
            for(int j = i+1; j < a.Length; j++)
            {
                if (comp(a[i], a[j]))
                    (a[i], a[j]) = (a[j], a[i]);
            }
        }
    }
}
```

위 코드처럼 `delegate(매개변수) {코드블럭}`으로 익명 대리자를 생성할 수 있다.  
완전히 같은 코드를 익명 대리자로 변환한 것일 뿐이므로 동일한 결과가 나타날 것이다.

<img width="525" height="146" alt="11  버블 정렬" src="https://github.com/user-attachments/assets/9f3a9435-c355-4649-ace8-f300ca2b3007" />

# 람다식

람다식은 익명 대리자를 좀 더 간단하게 구현할 수 있도록 도입된 문법이다.  
따라서 앞으로는 익명 대리자를 사용할 일은 없을 것이고 대부분 람다식으로 코딩할 것이다.

C#에서 람다식은 등호(`=`)와 비교연산자(`>`)를 조합한 화살표(`=>`)를 이용하여 구현할 수 있다.

```C#
public class Program
{
    static public void Main(string[] arg)
    {
        int[] a = { 321, 4, 12, 45, 567, 23, 15, 589, 237, 69, 19, 3, 23 };

        for (int i = 0; i < a.Length; i++)
            Console.Write(a[i] + " ");

        Console.WriteLine("\n");

        Sort(a, (i, j) => i > j);           // 람다식

        for (int i = 0; i < a.Length; i++)
            Console.Write(a[i] + " ");

        Console.WriteLine("\n");

        Sort(a, (i, j) => i < j);           // 람다식
        
        
        for (int i = 0; i < a.Length; i++)
            Console.Write(a[i] + " ");
    }

    //static bool Ascending(int i, int j)   사용 X
    //{
    //    return i < j;
    //}

    //static bool Descending(int i, int j)
    //{
    //    return i > j;
    //}

    delegate bool Compare(int i, int j);

    static void Sort(int[] a, Compare comp)
    {
        for(int i = 0; i < a.Length; i++)
        {
            for(int j = i+1; j < a.Length; j++)
            {
                if (comp(a[i], a[j]))
                    (a[i], a[j]) = (a[j], a[i]);
            }
        }
    }
}
```

위처럼 기존에 익명 대리자가 들어간 자리가 람다식으로 대체된 것을 보자. 매개변수로 두 개의 값이 필요하고 두 매개변수를 비교한 결과를 return한다.  
매개변수의 타입을 지정하지 않은 이유는 Sort에 지정된 매개변수 대리자에 타입이 정수로 설정되어 있기 때문이다.  
만약 대리자의 매개변수를 제네릭 타입으로 지정했을 경우 람다식 매개변수에도 타입을 명시해 주어야 한다.

## 람다식 문법

다음과 같은 경우가 있다고 알고 넘어가자

```C#
(i, j) => i - j;                // 람다식이 한 줄로 끝날 경우 {코드블럭} 생략 가능
i => Console.Write(i);       // 매개변수가 하나일 경우 괄호,() 생략 가능
(i, j) => { 코드블럭}           // 일반적인 람다식
```

## 람다식을 사용하려면

람다식을 사용하려면 반드시 대리자가 존재해야 한다. (위 버블 정렬 메서드에도 delegate Compare이라는 대리자가 구현되어 있는 것을 확인할 수 있다.)  
그러나 람다식을 사용하기 위해서 매번 대리자를 구현해야 하는 건 조금 귀찮을 수 있다. 
(매개변수 개수만큼 대리자를 생성해야 하기 때문)

그러므로 C#에서는 대리자를 내가 직접 생성하여 사용할 필요 없이 사용할 수 있도록 내장된 대리자들이 존재한다.

바로 <b>Action</b>과 <b>Func</b>이다.

Action은 void 타입의 대리자고 Func은 return 값이 존재하는 대리자라고 생각하면 된다.

0개부터 최대 16개의 매개변수를 입력받는 대리자들이고 17개 이상의 매개변수를 받는 람다식을 사용하고 싶으면 구현해야겠지만 그정도로 많은 매개변수가 필요할 일을 아마 없을 것이다.

이렇게만 알고 있자.

```C#
public class Program
{
    static public void Main(string[] arg)
    {
        int[] a = { 321, 4, 12, 45, 567, 23, 15, 589, 237, 69, 19, 3, 23 };

        for (int i = 0; i < a.Length; i++)
            Console.Write(a[i] + " ");

        Console.WriteLine("\n");

        Sort(a, (i, j) => i > j);           

        for (int i = 0; i < a.Length; i++)
            Console.Write(a[i] + " ");

        Console.WriteLine("\n");

        Sort(a, (i, j) => i < j);
        
        
        for (int i = 0; i < a.Length; i++)
            Console.Write(a[i] + " ");
    }

    //static bool Ascending(int i, int j)   사용 X
    //{
    //    return i < j;
    //}

    //static bool Descending(int i, int j)
    //{
    //    return i > j;
    //}

    //delegate T Compare<T, I, J>(I i, J j); 대리자 X

    static void Sort<T>(T[] a, Func<T, T, bool> comp)
    {
        for (int i = 0; i < a.Length; i++)
        {
            for (int j = i + 1; j < a.Length; j++)
            {
                if (comp(a[i], a[j]))
                    (a[i], a[j]) = (a[j], a[i]);
            }
        }
    }
}    
```
