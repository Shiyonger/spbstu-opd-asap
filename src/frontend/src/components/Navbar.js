import { Link, useNavigate } from 'react-router-dom';
import '../App.css'; // Убедимся, что стили импортируются

function Navbar() {
    const navigate = useNavigate();

    const handleLogout = () => {
        // Здесь можно добавить логику для очистки токена или состояния авторизации
        // Например, очистка localStorage, если используется
        localStorage.removeItem('authToken'); // Пример, если используется токен
        navigate('/'); // Перенаправление на страницу логина
    };

    return (
        <header className="app-header">
            <nav>
                <Link to="/" className="logo">CoursesApp</Link>
                <div className="nav-links">
                    <Link to="/courses">All Courses</Link>
                    <button onClick={handleLogout} className="logout-btn">
                        Выйти
                    </button>
                </div>
            </nav>
        </header>
    );
}

export default Navbar;
