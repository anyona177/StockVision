<template>
    <div class="assets">
        <h2>Активы</h2>
        <table>
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Тип актива</th>
                    <th>Символ</th>
                    <th>Текущая цена</th>
                    <th>Валюта</th>
                    <th>Биржа</th>
                    <th>Последнее обновление</th>
                    <th> </th>
                    <th> </th> <!-- Новый столбец для кнопки "карандаш" -->
                </tr>
            </thead>
            <tbody>
                <tr v-for="asset in assets" :key="asset.symbol">
                    <td>{{ asset.assetId }}</td>
                    <td>{{ asset.assetType.trim() }}</td>
                    <td>{{ asset.symbol.trim() }}</td>
                    <td>{{ asset.currentPrice }}</td>
                    <td>{{ asset.currency.trim() }}</td>
                    <td>{{ asset.exchange.trim() }}</td>
                    <td>{{ asset.lastUpdated }}</td>
                    <td>
                        <button class="action-button" @click="handleAction(asset.symbol)">
                            ✖
                        </button>
                    </td>
                    <td>
                        <!-- Кнопка "карандаш" -->
                        <button class="action-button" @click="editAsset(asset)">
                            ✏
                        </button>
                    </td>
                </tr>
            </tbody>
        </table>

        <div class="button-container">
            <button @click="goTo('portfolios')" class="back-button">Назад</button>
            <button @click="search" class="search-button">Поиск</button>
        </div>
    </div>
</template>

<script>
    import axios from '@/settings/axios';

    export default {
        data() {
            return {
                assets: [],
            };
        },
        mounted() {
            this.fetchAssets();
        },
        methods: {
            async fetchAssets() {
                try {
                    const response = await axios.get('http://localhost:5065/assets');
                    this.assets = response.data;
                } catch (error) {
                    console.error('Ошибка при получении активов:', error);
                }
            },
            goTo(route) {
                this.$router.push(`/${route}`);
            },
            search() {
                this.$router.push('/search');
            },
            async deleteAsset(symbol) {
                try {
                    const trimmedSymbol = symbol.trim();
                    await axios.delete(`http://localhost:5065/assets/symbol/${encodeURIComponent(trimmedSymbol)}`);
                    this.assets = this.assets.filter(asset => asset.symbol !== trimmedSymbol);
                    alert(`Актив с символом '${trimmedSymbol}' успешно удален.`);
                } catch (error) {
                    console.error(`Ошибка при удалении актива с символом '${symbol}':`, error.response?.data || error.message);
                    alert(`Не удалось удалить актив с символом '${symbol}'.`);
                }
            },
            handleAction(assetSymbol) {
                if (confirm(`Вы уверены, что хотите удалить актив с символом: ${assetSymbol}?`)) {
                    this.deleteAsset(assetSymbol);
                }
            },
            editAsset(asset) {
                this.$router.push(`/assets/edit/${encodeURIComponent(asset.symbol)}`);
            }

        },
    };
</script>

<style scoped>
    .assets {
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

    .button-container {
        position: absolute;
        bottom: 50px;
        left: 20px;
        right: 20px;
        display: flex;
        justify-content: space-between;
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

    .action-button {
        background-color: transparent;
        color: white; /* Белый текст */
        border: none;
        cursor: pointer;
        font-size: 20px; /* Размер символа */
        transition: color 0.3s;
    }

        .action-button:hover {
            color: black; /* Чёрный цвет при наведении */
        }

        .action-button:focus {
            outline: none;
        }
</style>
