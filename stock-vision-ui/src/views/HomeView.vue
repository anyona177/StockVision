<template>
    <div class="home">
        <div class="overlay"></div> <!-- Полупрозрачный слой для затемнения -->
        <img :src="logoSrc" alt="Логотип" class="logo" /> <!-- Логотип в верхней части -->
        <h1 class="title">StockVision</h1> <!-- Заголовок сразу под логотипом -->
        <div class="divider"></div> <!-- Линия под заголовком -->
        <div class="content">
            <button @click="goTo('portfolios')">Ваш Портфель</button>
            <button @click="goTo('transactions')">Транзакции</button>
            <button @click="goTo('filtr')">Анализ</button>
            <button @click="goTo('login')">Выход</button>
        </div>

        <!-- Кнопка в правом верхнем углу -->
        <button class="settings-button" @click="goTo('settings')">
            <img :src="settingsLogo" alt="Settings" class="settings-icon" />
        </button>
    </div>
</template>

<script>
    import Logo from '@/assets/logo.png'; // Импорт логотипа как модуля
    import SettingsLogo from '@/assets/setting.png'; // Импорт картинки для кнопки

    export default {
        data() {
            return {
                logoSrc: Logo,
                settingsLogo: SettingsLogo,
                userRole: null, // Для хранения роли пользователя
            };
        },
        mounted() {
            this.userRole = localStorage.getItem("role"); // Получаем роль из localStorage
        },
        methods: {
            goTo(route) {
                this.$router.push(`/${route}`);
            },
        },
    };

</script>

<style scoped>
    .home {
        position: relative;
        display: flex;
        flex-direction: column;
        align-items: center;
        height: 100vh;
        background-image: url('@/assets/fon.jpg'); /* Укажите путь к изображению фона */
        background-size: cover;
        background-position: center;
        color: white;
        overflow: hidden;
    }

    .overlay {
        position: absolute;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background-color: rgba(0, 0, 0, 0.65);
        z-index: 1;
    }

    .logo {
        margin-top: -90px;
        width: 300px; /* Увеличенный логотип */
        height: auto;
        z-index: 2;
    }

    .title {
        margin-top: -80px; /* Отступ между логотипом и заголовком */
        font-size: 62px;
        font-family: 'Courier New', Courier, monospace;
        font-weight: 100;
        z-index: 2;
    }

    .divider {
        width: 100%; /* Линия на всю ширину */
        height: 1px; /* Толщина линии */
        background-color: white; /* Цвет линии */
        margin: -25px 0; /* Отступы сверху и снизу */
        z-index: 2;
    }

    .content {
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        margin-top: -90px;
        flex-grow: 1;
        z-index: 2;
    }

    button {
        margin: 25px;
        padding: 30px 60px;
        font-size: 20px;
        cursor: pointer;
        background-color: black; /* Черный фон */
        color: white; /* Белый текст */
        border: 2px solid white; /* Белый контур */
        border-radius: 20px; /* Скругленные углы */
        position: relative;
        transition: background-color 0.3s, color 0.3s;
    }

    button:hover {
        background-color: white; /* Инвертировать цвета при наведении */
        color: black;
    }

    /* Стили для кнопки с изображением в правом верхнем углу */
    .settings-button {
        position: absolute;
        top: 1px; /* Отступ сверху */
        right: -10px; /* Отступ справа */
        background-color: #dcdcdc; /* Темно-белый фон */
        border: 2px solid white; /* Белый контур */
        border-radius: 50%; /* Круглая форма */
        padding: 10px; /* Внутренний отступ */
        display: flex;
        justify-content: center;
        align-items: center;
        cursor: pointer;
        transition: background-color 0.3s, transform 0.3s;
        z-index: 3; /* Обеспечиваем, что кнопка находится выше всех элементов */
    }

    .settings-button:hover {
        background-color: white; /* Белый фон при наведении */
        transform: scale(1.1); /* Увеличение кнопки при наведении */
    }

    .settings-icon {
        width: 24px; /* Размер иконки */
        height: 24px;
    }
</style>
