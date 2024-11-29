<template>
    <div class="search-view">
        <!-- Заголовок "Поиск" -->
        <div class="content">
            <h2>Поиск</h2>

            <!-- Поле для ввода тикера -->
            <div class="input-group">
                <label for="ticker">Введите тикер</label>
                <input id="ticker" type="text" v-model="ticker" placeholder="Например, AAPL" />
            </div>

            <!-- Поля для ввода цены -->
            <div class="input-group">
                <label>Цена</label>
                <div class="price-inputs">
                    <input type="number" v-model="priceFrom" placeholder="от" />
                    <input type="number" v-model="priceTo" placeholder="до" />
                </div>
            </div>
        </div>

        <!-- Кнопка "Назад" -->
        <div class="back-button-container">
            <button @click="goTo('assets')" class="back-button">Назад</button>
        </div>

        <!-- Кнопка "Искать" -->
        <div class="search-button-container">
            <button @click="search" class="search-button">Искать</button>
        </div>

        <!-- Таблица результатов поиска -->
        <div v-if="searchResults.length > 0" class="search-results">
            <h3>Результаты поиска</h3>
            <table>
                <thead>
                    <tr>
                        <th>Тип актива</th>
                        <th>Символ</th>
                        <th>Текущая цена</th>
                        <th>Валюта</th>
                        <th>Биржа</th>
                        <th>Последнее обновление</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="asset in searchResults" :key="asset.assetId">
                        <td>{{ asset.assetType.trim() }}</td>
                        <td>{{ asset.symbol.trim() }}</td>
                        <td>{{ asset.currentPrice }}</td>
                        <td>{{ asset.currency.trim() }}</td>
                        <td>{{ asset.exchange.trim() }}</td>
                        <td>{{ asset.lastUpdated }}</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script>
    import axios from '@/settings/axios';

    export default {
        data() {
            return {
                ticker: '', // Поле для ввода тикера
                priceFrom: '', // Поле "от" для цены
                priceTo: '', // Поле "до" для цены
                searchResults: [], // Результаты поиска
            };
        },
        methods: {
            goTo(route) {
                this.$router.push(`/${route}`);
            },
            async search() {
                try {
                    const params = {};
                    if (this.ticker) params.symbol = this.ticker;
                    if (this.priceFrom) params.priceFrom = this.priceFrom;
                    if (this.priceTo) params.priceTo = this.priceTo;

                    const response = await axios.get('http://localhost:5065/assets/search', { params });
                    this.searchResults = response.data;
                } catch (error) {
                    console.error("Ошибка при выполнении поиска:", error);
                }
            },
        },
    };
</script>

<style scoped>
    .search-view {
        background-color: black;
        color: white;
        height: 100vh;
        display: flex;
        flex-direction: column;
        justify-content: flex-start;
        align-items: center;
        position: relative;
    }

    .content {
        margin-top: 20px;
        text-align: center;
    }

    h2 {
        font-size: 32px;
        margin-bottom: 20px;
    }

    .input-group {
        margin-bottom: 20px;
    }

    label {
        font-size: 18px;
        margin-bottom: 10px;
        display: block;
    }

    input[type="text"], input[type="number"] {
        padding: 10px;
        font-size: 16px;
        width: 200px;
        border: 1px solid #ccc;
        border-radius: 5px;
        background-color: #222;
        color: white;
        margin-top: 10px;
    }

    .price-inputs {
        display: flex;
        gap: 10px;
        justify-content: center;
    }

    .back-button-container {
        position: absolute;
        bottom: 10px;
        left: 20px;
    }

    .search-button-container {
        position: absolute;
        bottom: 10px;
        right: 20px;
    }

    .back-button,
    .search-button {
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
    .search-button:hover {
        background-color: white;
        color: black;
    }

    .search-results {
        margin-top: 20px;
        width: 100%;
    }

    table {
        width: 100%;
        border-collapse: collapse;
        margin-top: 10px;
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
</style>
