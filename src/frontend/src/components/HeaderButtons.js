import { useNavigate } from 'react-router-dom';
import '../App.css';
import {Logout} from "../api/api"; // Убедимся, что стили импортируются

function HeaderButtons() {
    const navigate = useNavigate();

    const handleLogout = async () => {
        await Logout();
        navigate('/');
    };

    return (
        <div className="header-buttons">
            <button onClick={handleLogout} className="logout-btn">Выйти</button>
        </div>
    );
}

export default HeaderButtons;
