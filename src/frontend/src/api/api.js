import axios from 'axios';

const API_BASE_URL = 'http://localhost:5163/';

const api = axios.create({
    baseURL: API_BASE_URL,
    withCredentials: true, // если используешь куки для авторизации
});

axios.defaults.headers.common['Access-Control-Allow-Origin'] = '*';
axios.defaults.headers.get['Accepts'] = 'application/json'
axios.defaults.headers.post['Content-Type'] = 'application/json; charset=utf-8'

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
        const response = await api.get('/assignments?courseId=' + courseId);
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

export const Logout = async () => {
    try {
        await api.post('/auth/logout');
    } catch (error) {
        console.error('Ошибка при логауте:', error);
        throw error;
    }
};