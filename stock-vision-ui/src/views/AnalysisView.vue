<!-- src/components/Analysis/Analysis.vue -->
<template>
    <div class="analysis">
        <h2>Анализ активов</h2>
        <table>
            <thead>
                <tr>
                    <th>Символ актива</th>
                    <th>Биржа</th>
                    <th>Дата</th>
                    <th>Закрывающая цена</th>
                    <th>Максимальная цена</th>
                    <th>Минимальная цена</th>
                    <th>Объем</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="data in historicalData" :key="data.historicalDataId">
                    <td>{{ data.asset.symbol }}</td>
                    <td>{{ data.asset.exchange }}</td>
                    <td>{{ new Date(data.date).toLocaleDateString() }}</td>
                    <td>{{ data.closingPrice }}</td>
                    <td>{{ data.maxPrice }}</td>
                    <td>{{ data.minPrice }}</td>
                    <td>{{ data.volume }}</td>
                </tr>
            </tbody>
        </table>
        <div class="back-button-container">
            <button @click="goTo('filtr')" class="back-button">Назад</button>
        </div>
    </div>
</template>




<script>
import axios from '@/settings/axios';

export default {
    data() {
        return {
            historicalData: [],
        };
    },
    async mounted() {
        await this.fetchHistoricalData();
    },
    methods: {
        async fetchHistoricalData() {
            const { ticker, exchange, priceFrom, priceTo, volumeFrom, volumeTo } = this.$route.query;

            try {
                const response = await axios.get('http://localhost:5065/historicaldata/filter', {
                    params: {
                        ticker: ticker || '',
                        exchange: exchange || '', // Фильтр по бирже из query параметров
                        priceFrom: priceFrom || null,
                        priceTo: priceTo || null,
                        volumeFrom: volumeFrom || null,
                        volumeTo: volumeTo || null
                    }
                });
                this.historicalData = response.data;
            } catch (error) {
                console.error("Ошибка при получении данных:", error);
            }
        },
        goTo(route) {
            this.$router.push(`/${route}`);
        },
    },
};
</script>




<style scoped>
.analysis {
    background-color: black;
    color: white;
    padding: 20px;
    height: 100vh;
    display: flex;
    flex-direction: column;
    justify-content: flex-start;
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

.back-button-container {
    position: absolute;
    bottom: 10px;
    left: 20px;
}

.back-button {
    padding: 15px 30px;
    font-size: 18px;
    cursor: pointer;
    background-color: black;
    color: white;
    border: 2px solid white;
    border-radius: 20px;
    transition: background-color 0.3s, color 0.3s;
}

.back-button:hover {
    background-color: white;
    color: black;
}
</style>


