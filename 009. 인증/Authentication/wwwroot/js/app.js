document.addEventListener('DOMContentLoaded', () => {
    const signupForm = document.getElementById('signup-form');
    const signinForm = document.getElementById('signin-form');
    const myNameButton = document.getElementById('myname-button');
    const signoutButton = document.getElementById('signout-button');
    const messageDiv = document.getElementById('message');

    // 메시지 표시 함수
    const showMessage = (text, type) => {
        messageDiv.textContent = text;
        messageDiv.className = type; // 'success' 또는 'error'
    };

    // 회원가입 폼 제출 이벤트
    if (signupForm) {
        signupForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            const userId = document.getElementById('reg-userid').value;
            const username = document.getElementById('reg-username').value;
            const email = document.getElementById('reg-email').value;
            const password = document.getElementById('reg-password').value;

            try {
                const response = await fetch('/auth/signup', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ userId, username, email, password })
                });

                if (response.ok) {
                    showMessage('회원가입 성공!', 'success');
                    signupForm.reset();
                } else {
                    const errorData = await response.json();
                    const firstError = errorData.errors ? Object.values(errorData.errors)[0][0] : (errorData.message || response.statusText);
                    showMessage(`회원가입 실패: ${firstError}`, 'error');
                }
            } catch (error) {
                showMessage('네트워크 오류가 발생했습니다.', 'error');
            }
        });
    }

    // 로그인 폼 제출 이벤트
    if (signinForm) {
        signinForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            const userId = document.getElementById('login-userid').value;
            const password = document.getElementById('login-password').value;

            try {
                const response = await fetch('/auth/signin', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ userId, password })
                });

                if (response.ok) {
                    showMessage('로그인 성공! 이제 버튼을 테스트해보세요.', 'success');
                } else {
                    showMessage('로그인 실패: ID 또는 비밀번호를 확인하세요.', 'error');
                }
            } catch (error) {
                showMessage('네트워크 오류가 발생했습니다.', 'error');
            }
        });
    }

    // 인증 테스트 버튼 클릭 이벤트
    if (myNameButton) {
        myNameButton.addEventListener('click', async () => {
            try {
                const response = await fetch('/myname', {
                    method: 'GET'
                });

                if (response.ok) {
                    const data = await response.text();
                    showMessage(data, 'success');
                } else {
                    showMessage(`인증 실패: ${response.status} (로그인이 필요합니다)`, 'error');
                }
            } catch (error) {
                showMessage('네트워크 오류가 발생했습니다.', 'error');
            }
        });
    }

    // 로그아웃 버튼 클릭 이벤트
    if (signoutButton) {
        signoutButton.addEventListener('click', async () => {
            try {
                const response = await fetch('/auth/signout', {
                    method: 'POST'
                });

                if (response.ok) {
                    showMessage('로그아웃 되었습니다.', 'success');
                } else {
                    showMessage('로그아웃 실패. (로그인 상태가 아닐 수 있습니다)', 'error');
                }
            } catch (error) {
                showMessage('네트워크 오류가 발생했습니다.', 'error');
            }
        });
    }
});