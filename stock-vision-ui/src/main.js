import { createApp } from 'vue';
import App from './App.vue';
import router from './router';
import './assets/styles.css'; // Импортируйте файл со стилями

createApp(App).use(router).mount('#app');
