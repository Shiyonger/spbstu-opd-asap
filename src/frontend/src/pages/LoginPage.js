import React from 'react';
import './LoginPage.css';

function LoginPage() {
    return (
        <div className="login-container">
            <div className="login-card">
                <form className="login-form">
                    <div className="input-group">
                        <label>Имя пользователя</label>
                        <input type="text" className="input-field" placeholder="Введите логин" />
                    </div>
                    <div className="input-group">
                        <label>Пароль</label>
                        <input type="password" className="input-field" placeholder="Введите пароль" />
                    </div>
                    <button type="submit" className="login-btn">Войти</button>
                </form>
            </div>
        </div>
    );
}

export default LoginPage;