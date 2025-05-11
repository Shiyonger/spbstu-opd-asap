import axios from 'axios';

const API_BASE_URL = 'https://your-backend-url.com/api';

const api = axios.create({
    baseURL: API_BASE_URL,
    withCredentials: true, // если используешь куки для авторизации
});

// Получение курсов
export const getCourses = async () => {
    try {
        const response = await api.get('/courses');
        return response.data;
    } catch (error) {
        console.error('Ошибка при получении курсов:', error);
        throw error;
    }
};

export const getCourseDetails = async (courseId) => {
    try {
        const response = await api.get('/courses/${courseId}');
        return response.data;
    } catch (error) {
        console.error('Ошибка при получении курсов:', error);
        throw error;
    }
};

// Запрос на логин
export const Login = async (login, password) => {
    try {
        const response = await api.post('/auth/login', {
            login,
            password,
        });
        return response.data;
    } catch (error) {
        console.error('Ошибка при логине:', error);
        throw error;
    }
};