//axios.js
import axios from 'axios';

// ������� ��������� axios
const api = axios.create({
    baseURL: 'http://localhost:5065',  // ������� URL ��� API
    headers: {
        'Content-Type': 'application/json'
    }
});

// ����������� ��� ���������� ������ � ������ ������
api.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem('authToken');
        if (token) {
            // ���� ����� ����, ��������� ��� � ���������
            config.headers['Authorization'] = `Bearer ${ token }`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

// ����������� ��� ��������� ���a���
api.interceptors.response.use(
    (response) => {
        return response;
    },
    (error) => {
        if (error.response && error.response.status === 401) {
            // ��������, ���� ������� ����� � ������� 401 (�������������), ����� ������������� �� �������� �����
            window.location.href = '/login';
        }
        return Promise.reject(error);
    }
);

export default api;