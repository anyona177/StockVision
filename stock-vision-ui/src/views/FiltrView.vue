<template>
    <div class="filter-container">
        <h1 class="header">Анализ активов</h1>

        <div class="form-content">
            <div class="button-container">
                <button @click="goBack" class="back-button">Назад</button>
            </div>

            <h2>Введите тикер</h2>
            <input type="text" v-model="ticker" placeholder="Тикер актива" />

            <h2>Введите биржу</h2>
            <input type="text" v-model="exchange" placeholder="Биржа" />

            <h2>Диапазон цены</h2>
            <div class="price-range">
                <input type="number" v-model="priceFrom" placeholder="от" />
                <input type="number" v-model="priceTo" placeholder="до" />
            </div>

            <h2>Диапазон объема торгов</h2>
            <div class="volume-range">
                <input type="number" v-model="volumeFrom" placeholder="от" />
                <input type="number" v-model="volumeTo" placeholder="до" />
            </div>
        </div>

        <div class="apply-button-container">
            <button @click="applyFilters" class="apply-button">Применить фильтры</button>
        </div>
    </div>
</template>

<script>
export default {
    data() {
        return {
            ticker: '',
            exchange: '',
            priceFrom: null,
            priceTo: null,
            volumeFrom: null,
            volumeTo: null
        };
    },
    methods: {
        goBack() {
            this.$router.push('/');
        },
        applyFilters() {
            this.$router.push({
                path: '/analysis',
                query: {
                    ticker: this.ticker,
                    exchange: this.exchange, // Передаем значение биржи
                    priceFrom: this.priceFrom,
                    priceTo: this.priceTo,
                    volumeFrom: this.volumeFrom,
                    volumeTo: this.volumeTo
                }
            });
        },
    }
};
</script>



<style scoped>
    .filter-container {
        display: flex;
        flex-direction: column;
        align-items: center;
        height: 100vh;
        background-color: black;
        color: white;
        padding: 20px;
    }

    .header {
        margin-top: 20px;
        font-size: 44px;
        color: white;
        text-align: center;
    }

    .form-content {
        margin-top: -100px;
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        flex-grow: 1;
    }

    h2 {
        margin: 10px 0;
    }

    input {
        width: 200px;
        padding: 10px;
        margin-bottom: 20px;
        font-size: 16px;
        border-radius: 5px;
        border: 1px solid #ddd;
        background-color: #333;
        color: white;
    }

    .price-range,
    .volume-range {
        display: flex;
        gap: 10px;
    }

    .price-range input,
    .volume-range input {
        width: 90px;
    }

    .button-container {
        position: absolute;
        bottom: 20px;
        left: 20px;
    }

    .apply-button-container {
        position: absolute;
        bottom: 20px;
        right: 20px;
    }

    .back-button,
    .apply-button {
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
    .apply-button:hover {
        background-color: white;
        color: black;
    }
</style>
