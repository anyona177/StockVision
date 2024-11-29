<template>
  <div class="settings-view">
      <div class="form-container">
          <h1>Добавить актив</h1>

          <div v-if="errorMessage" class="error-message">{{ errorMessage }}</div>
          <div v-if="successMessage" class="success-message">{{ successMessage }}</div>

          <div class="form-group">
              <label for="assetType">Тип актива:</label>
              <input type="text" id="assetType" v-model="form.assetType" placeholder="Например, Stock" />
          </div>

          <div class="form-group">
              <label for="symbol">Символ:</label>
              <input type="text" id="symbol" v-model="form.symbol" placeholder="Например, YNDX" />
          </div>

          <div class="form-group">
              <label for="currentPrice">Текущая цена:</label>
              <input type="number" id="currentPrice" v-model="form.currentPrice" placeholder="Например, 500" />
          </div>

          <div class="form-group">
              <label for="currency">Валюта:</label>
              <input type="text" id="currency" v-model="form.currency" placeholder="Например, USD" />
          </div>

          <div class="form-group">
              <label for="exchange">Биржа:</label>
              <input type="text" id="exchange" v-model="form.exchange" placeholder="Например, NASDAQ" />
          </div>

          <div class="form-group">
              <label for="lastUpdated">Дата последнего обновления:</label>
              <input type="datetime-local" id="lastUpdated" v-model="form.lastUpdated" />
          </div>

          <div class="form-group">
              <label for="type">Тип актива:</label>
              <input type="text" id="type" v-model="form.type" placeholder="Например, Technology" />
          </div>

          <div class="button-container">
              <button class="back-button" @click="goBack">Назад</button>
              <button class="add-button" @click="addAsset">Добавить</button>
          </div>
      </div>
  </div>
</template>

<script>
import axios from '@/settings/axios';

export default {
  name: 'AddAsset',
  data() {
    return {
      form: {
        assetType: '',
        symbol: '',
        currentPrice: '',
        currency: '',
        exchange: '',
        lastUpdated: '', // Дата для ввода
        type: '',
      },
      successMessage: '',
      errorMessage: '',
    };
  },
  methods: {
    goBack() {
        this.$router.push('/'); // Переход на главную страницу
    },
    async addAsset() {
      // Проверка заполненности обязательных полей
      if (!this.form.assetType.trim() || !this.form.symbol.trim()) {
        this.errorMessage = 'Пожалуйста, заполните все обязательные поля: Тип актива и Символ.';
        return;
      }

      // Очистка сообщений об ошибке и успехе
      this.errorMessage = '';
      this.successMessage = '';

      // Проверка правильности значения цены
      if (isNaN(this.form.currentPrice) || this.form.currentPrice <= 0) {
        this.errorMessage = 'Пожалуйста, введите корректную цену.';
        return;
      }

      // Преобразование даты в нужный формат
      const formatDate = (date) => {
        // Преобразуем в строку с датой в формате yyyy-MM-ddThh:mm:ssZ (включая временную зону)
        return new Date(date).toISOString(); // Это автоматически добавит 'Z' для временной зоны UTC
      };

      const lastUpdatedFormatted = formatDate(this.form.lastUpdated);
      const updateDateFormatted = formatDate(this.form.lastUpdated); // Используем ту же дату для updateDate

      // Подготовка данных для отправки
      const newAsset = {
        assetType: this.form.assetType,
        symbol: this.form.symbol,
        currentPrice: parseFloat(this.form.currentPrice),
        currency: this.form.currency,
        exchange: this.form.exchange,
        lastUpdated: lastUpdatedFormatted,  // Отформатированная дата
        type: this.form.type,
        updateDate: updateDateFormatted,  // Дата обновления передается как updateDate
      };

      try {
        // Отправка POST-запроса на API
        await axios.post('http://localhost:5065/assets', newAsset);

        // Сообщение об успешном добавлении
        this.successMessage = 'Актив успешно добавлен!';
        this.clearForm();
      } catch (error) {
        this.errorMessage = 'Ошибка при добавлении актива. Проверьте данные и попробуйте снова.';
        console.error('Ошибка при добавлении актива:', error);
      }
    },

    clearForm() {
      this.form = {
        assetType: '',
        symbol: '',
        currentPrice: '',
        currency: '',
        exchange: '',
        lastUpdated: '',
        type: '',
      };
      this.successMessage = '';
      this.errorMessage = '';
    },
  },
};
</script>

<style scoped>
.settings-view {
  background-color: black;
  color: white;
  padding: 20px;
  display: flex;
  flex-direction: column;
  align-items: center;
  height: 100vh;
}

.form-container {
  background-color: #333;
  padding: 30px;
  border-radius: 10px;
  width: 80%;
  max-width: 600px;
}

h1 {
  text-align: center;
  font-size: 28px;
  margin-bottom: 20px;
}

.form-group {
  display: flex;
  flex-direction: row;
  margin-bottom: 15px;
  justify-content: space-between;
  align-items: center;
}

label {
  font-size: 18px;
  width: 150px;
}

input {
  padding: 10px;
  font-size: 16px;
  width: 200px;
  border-radius: 5px;
  border: 1px solid white;
  background-color: #444;
  color: white;
}

button.add-button {
  margin-top: 20px;
  padding: 15px 30px;
  font-size: 18px;
  background-color: black;
  color: white;
  border: 2px solid white;
  border-radius: 10px;
  min-width: 200px; /* Задаём минимальную ширину */
  height: 60px; /* Устанавливаем одинаковую высоту */
  cursor: pointer;
  transition: background-color 0.3s, color 0.3s;
}


button.add-button:hover {
  background-color: white;
  color: black;
}

.button-container {
  display: flex;
  justify-content: space-between;
  margin-top: 20px;
}

.success-message {
  color: #4caf50;
  margin-bottom: 20px;
}

.error-message {
  color: #f44336;
  margin-bottom: 20px;
}

button.back-button {
  margin-top: 20px;
  padding: 15px 30px; /* Увеличиваем отступы для соответствия кнопке "Добавить" */
  font-size: 18px;
  background-color: black;
  color: white;
  border: 2px solid white;
  border-radius: 10px;
  width: auto; /* Убираем фиксированную ширину */
  min-width: 200px; /* Задаём минимальную ширину */
  height: 60px; /* Устанавливаем одинаковую высоту */
  cursor: pointer;
  transition: background-color 0.3s, color 0.3s;
}

button.back-button:hover {
  background-color: white;
  color: black;
}
</style>
