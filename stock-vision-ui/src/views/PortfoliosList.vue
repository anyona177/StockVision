<template>
    <div class="portfolio">
        <h2>Портфель</h2>
        <table>
            <thead>
                <tr>
                    <th>Название портфеля</th>
                    <th>Общая стоимость</th>
                    <th>Последнее обновление</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="portfolio in portfolios" :key="portfolio.portfolioId">
                    <td>{{ portfolio.portfolioName.trim() }}</td>
                    <td>{{ portfolio.totalValue }}</td>
                    <td>{{ portfolio.lastUpdated }}</td>
                </tr>
            </tbody>
        </table>

        <!-- Контейнер для кнопок -->
        <div class="button-container">
            <button @click="goBack" class="back-button">Назад</button>
            <button @click="goTo('assets')" class="assets-button">Активы</button>
        </div>
    </div>
</template>

<script>
    import axios from '@/settings/axios';

    export default {
        data() {
            return {
                portfolios: [],
            };
        },
        mounted() {
            this.fetchPortfolios();
        },
        methods: {
            async fetchPortfolios() {
                try {
                    const response = await axios.get('http://localhost:5065/portfolios');
                    this.portfolios = response.data;
                } catch (error) {
                    console.error("Ошибка при получении портфелей:", error);
                }
            },
            // Метод для перехода на предыдущую страницу
            goBack() {
                this.$router.push('/'); // Переход на главную страницу
            },
            goTo(route) {
                this.$router.push(`/${route}`); // Переход по маршруту
            },
        },
    };
</script>

<style scoped>
    .portfolio {
        background-color: black;
        color: white;
        padding: 20px;
        height: 100vh;
        display: flex;
        flex-direction: column;
        justify-content: flex-start;
        position: relative;
    }

    h2 {
        text-align: center;
        margin: 0;
    }

    table {
        width: 100%;
        border-collapse: collapse;
        margin-top: 20px;
    }

    th, td {
        border: 1px solid #ddd;
        padding: 8px;
        text-align: left;
    }

    th {
        background-color: #333;
        color: white;
    }

    tr:nth-child(even) {
        background-color: #444;
    }

    tr:hover {
        background-color: #555;
    }

    /* Контейнер для кнопок */
    .button-container {
        position: absolute;
        bottom: 50px;
        left: 20px;
        right: 20px;
        display: flex;
        justify-content: space-between;
    }

    /* Стиль кнопки "Назад" и "Активы" */
    .back-button,
    .assets-button {
        padding: 15px 30px;
        font-size: 18px;
        cursor: pointer;
        background-color: black;
        color: white;
        border: 2px solid white;
        border-radius: 20px;
        transition: background-color 0.3s, color 0.3s;
    }

    .back-button:hover,
    .assets-button:hover {
        background-color: white;
        color: black;
    }

    /* Размещение кнопок на одном уровне */
    .back-button {
        position: relative;
        bottom: 0;
    }

    .assets-button {
        position: relative;
        bottom: 0;
    }
</style>
