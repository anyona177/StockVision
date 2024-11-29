<!-- src/components/Transactions/TransactionList.vue -->
<template>
    <div class="transactions">
        <h2>Транзакции</h2>
        <table>
            <thead>
                <tr>
                    <th>ID портфеля</th>
                    <th>ID актива</th>
                    <th>Дата транзакции</th>
                    <th>Количество</th>
                    <th>Цена за единицу</th>
                    <th>Тип транзакции</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="transaction in transactions" :key="transaction.transactionId">
                    <td>{{ transaction.portfolioId }}</td>
                    <td>{{ transaction.assetId }}</td>
                    <td>{{ new Date(transaction.transactionDate).toLocaleDateString() }}</td>
                    <td>{{ transaction.quantity }}</td>
                    <td>{{ transaction.pricePerUnit }}</td>
                    <td>{{ transaction.transactionType }}</td>
                </tr>
            </tbody>
        </table>
        <!-- Кнопка "Назад" -->
        <div class="back-button-container">
            <button @click="goBack" class="back-button">Назад</button>
        </div>
    </div>
</template>

<script>
    import axios from '@/settings/axios';

    export default {
        data() {
            return {
                transactions: [],
            };
        },
        mounted() {
            this.fetchTransactions();
        },
        methods: {
            async fetchTransactions() {
                try {
                    const response = await axios.get('http://localhost:5065/transactions');
                    this.transactions = response.data;
                } catch (error) {
                    console.error("Ошибка при получении транзакций:", error);
                }
            },
            // Метод для перехода на главную страницу
            goBack() {
                this.$router.push('/'); // Переход на главную страницу
            },
        },
    };
</script>

<style scoped>
    .transactions {
        background-color: black; /* Черный фон для всего компонента */
        color: white; /* Белый текст */
        padding: 20px; /* Отступы для удобства */
        height: 100vh; /* Устанавливаем высоту на всю высоту окна */
        display: flex;
        flex-direction: column; /* Вертикальная компоновка */
        justify-content: flex-start; /* Начинаем с верхней части */
    }

    table {
        width: 100%;
        border-collapse: collapse;
        margin-top: 20px; /* Отступ сверху для таблицы */
    }

    th, td {
        padding: 8px;
        text-align: left;
        border: 1px solid #ddd;
    }

    h2 {
        text-align: center; /* Центрируем текст заголовка */
        margin: 0; /* Убираем отступы для заголовка */
    }

    th {
        background-color: #333; /* Темный фон для заголовков */
        color: white; /* Белый текст для заголовков */
    }

    tr:nth-child(even) {
        background-color: #444; /* Темный фон для четных строк */
    }

    tr:hover {
        background-color: #555; /* Фон при наведении на строку */
    }
    /* Стиль для контейнера кнопки */
    .back-button-container {
        position: absolute;
        bottom: 10px; /* Располагаем кнопку внизу */
        left: 20px; /* Сдвигаем кнопку влево */
    }
    /* Стиль кнопки */
    .back-button {
        padding: 15px 30px;
        font-size: 18px;
        cursor: pointer;
        background-color: black; /* Черный фон */
        color: white; /* Белый текст */
        border: 2px solid white; /* Белая рамка */
        border-radius: 20px; /* Скругленные углы */
        transition: background-color 0.3s, color 0.3s;
    }

    .back-button:hover {
        background-color: white;
        color: black;
    }
</style>
