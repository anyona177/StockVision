<template>
    <div class="edit-asset">
        <h2>Редактирование актива</h2>
        <form @submit.prevent="updateAsset">
            <div class="form-group">
                <label>ID:</label>
                <input type="text" v-model="asset.assetId" disabled />
            </div>
            <div class="form-group">
                <label>Тип актива:</label>
                <input type="text" v-model="asset.assetType" />
            </div>
            <div class="form-group">
                <label>Символ:</label>
                <input type="text" v-model="asset.symbol" />
            </div>
            <div class="form-group">
                <label>Текущая цена:</label>
                <input type="number" v-model.number="asset.currentPrice" step="0.01" />
            </div>
            <div class="form-group">
                <label>Валюта:</label>
                <input type="text" v-model="asset.currency" />
            </div>
            <div class="form-group">
                <label>Биржа:</label>
                <input type="text" v-model="asset.exchange" />
            </div>
            <div class="form-group">
                <label>Последнее обновление:</label>
                <input type="datetime-local" v-model="formattedLastUpdated" />
            </div>
            <div class="form-group">
                <label>Тип:</label>
                <input type="text" v-model="asset.type" />
            </div>
            <div class="form-group">
                <label>Дата обновления:</label>
                <input type="datetime-local" v-model="formattedUpdateDate" />
            </div>
            <div class="button-group">
                <button type="submit">Сохранить</button>
                <button type="button" @click="cancelEdit">Отмена</button>
            </div>
        </form>
    </div>
</template>

<script>
    import axios from '@/settings/axios';

    export default {
        data() {
            return {
                asset: {
                    assetId: null,
                    assetType: "",
                    symbol: "",
                    currentPrice: null,
                    currency: "",
                    exchange: "",
                    lastUpdated: "",
                    type: "",
                    updateDate: ""
                }
            };
        },
        computed: {
            formattedLastUpdated: {
                get() {
                    return this.asset.lastUpdated
                        ? new Date(this.asset.lastUpdated).toISOString().slice(0, 16)
                        : "";
                },
                set(value) {
                    this.asset.lastUpdated = new Date(value).toISOString();
                }
            },
            formattedUpdateDate: {
                get() {
                    return this.asset.updateDate
                        ? new Date(this.asset.updateDate).toISOString().slice(0, 16)
                        : "";
                },
                set(value) {
                    this.asset.updateDate = new Date(value).toISOString();
                }
            }
        },
        async mounted() {
            const { symbol } = this.$route.params;
            try {
                const response = await axios.get(`http://localhost:5065/assets/symbol/${encodeURIComponent(symbol)}`);
                this.asset = response.data;
            } catch (error) {
                console.error("Ошибка при загрузке данных актива:", error);
                alert("Не удалось загрузить данные актива.");
            }
        },
        methods: {
            async updateAsset() {
                try {
                    await axios.put(
                        `http://localhost:5065/assets/symbol/${encodeURIComponent(this.asset.symbol)}`,
                        this.asset
                    );
                    alert("Актив успешно обновлён!");
                    this.$router.push("/assets");
                } catch (error) {
                    console.error("Ошибка при обновлении актива:", error);
                    alert("Не удалось обновить актив.");
                }
            },
            cancelEdit() {
                this.$router.push("/assets");
            }
        }
    };
</script>

<style scoped>
    .edit-asset {
        background-color: black;
        border: 2px solid white;
        color: white;
        padding: 20px;
        max-width: 600px;
        margin: 20px auto;
        border-radius: 8px;
        box-shadow: 0 4px 10px rgba(0, 0, 0, 0.5);
    }

    h2 {
        text-align: center;
    }

    form {
        display: flex;
        flex-direction: column;
    }

    .form-group {
        margin-bottom: 15px;
    }

    label {
        display: block;
        margin-bottom: 5px;
        font-weight: bold;
    }

    input {
        width: 100%;
        padding: 9px;
        border: 1px solid #444;
        border-radius: 4px;
        background-color: #222;
        color: white;
    }

        input[disabled] {
            background-color: #333;
            color: #888;
        }

    .button-group {
        display: flex;
        justify-content: space-between;
    }

    button {
        padding: 10px 20px;
        font-size: 16px;
        cursor: pointer;
        border: none;
        border-radius: 4px;
        transition: background-color 0.3s, color 0.3s;
    }

        button[type="submit"] {
            background-color: #222;
            color: white;
        }

            button[type="submit"]:hover {
                background-color: white;
                color: black;
            }

        button[type="button"] {
            background-color: #222;
            color: white;
        }

            button[type="button"]:hover {
                background-color: white;
                color: black;
            }
</style>

<style>
    body {
        background-color: black; /* Чёрный фон для всей страницы */
        color: white;
    }
</style>
