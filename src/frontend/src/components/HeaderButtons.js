import { useNavigate } from 'react-router-dom';
import '../App.css'; // Убедимся, что стили импортируются

function HeaderButtons() {
    const navigate = useNavigate();

    const handleLogout = () => {
        // Здесь можно добавить логику для очистки токена или состояния авторизации
        // Например, очистка localStorage, если используется
        localStorage.removeItem('authToken'); // Пример, если используется токен
        navigate('/'); // Перенаправление на страницу логина
    };

    return (
        <div className="header-buttons">
            <button onClick={handleLogout} className="logout-btn">Выйти</button>
        </div>
    );
}

export default HeaderButtons;
