import React, { useState } from 'react';
import './LoginPage.css';
import '../App.css';
import { useNavigate } from 'react-router-dom';
import { Login } from '../api/api';

function LoginPage() {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault();
        const result = await Login(username, password);
        if (result===true)
            navigate('/courses');
         else {
        alert('Неверный логин или пароль');
    }
};
    return (
        <>
            <div className="login-container">
                <div className="login-card">
                    <form className="login-form" onSubmit={handleLogin}>
                        <div className="input-group">
                            <label>Имя пользователя</label>
                            <input type="text" className="input-field" placeholder="Введите логин"
                                   value={username} onChange={e => setUsername(e.target.value)}/>
                        </div>
                        <div className="input-group">
                            <label>Пароль</label>
                            <input type="password" className="input-field" placeholder="Введите пароль"
                                   value={password} onChange={e => setPassword(e.target.value)}/>
                        </div>
                        <button type="submit" className="login-btn">Войти</button>
                    </form>
                </div>
            </div>
        </>
    );
}

export default LoginPage;