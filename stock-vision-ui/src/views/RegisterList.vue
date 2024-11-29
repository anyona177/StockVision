<template>
    <div class="auth-container">
        <h2>Регистрация</h2>
        <form @submit.prevent="register">
            <div class="input-group">
                <label for="name">Имя</label>
                <input v-model="name" type="text" id="name" required placeholder="Введите ваше имя" />
            </div>

            <div class="input-group">
                <label for="email">Email</label>
                <input v-model="email" type="email" id="email" required placeholder="Введите ваш email" />
            </div>

            <div class="input-group">
                <label for="password">Пароль</label>
                <input v-model="password" type="password" id="password" required placeholder="Введите пароль" />
            </div>

            <div class="input-group">
                <label for="role">Роль</label>
                <select v-model="role" id="role" required>
                    <option value="User">User</option>
                    <option value="Admin">Admin</option>
                </select>
            </div>

            <button type="submit" class="submit-button">Зарегистрироваться</button>
        </form>

        <p class="redirect-text">Уже есть аккаунт? <a @click="goToLogin">Войти</a></p>
    </div>
</template>

<script>
    import axios from '@/settings/axios';

    export default {
        data() {
            return {
                name: "",
                email: "",
                password: "",
                role: "User", // Устанавливаем роль по умолчанию
            };
        },
        methods: {
            async register() {
                try {
                    const response = await axios.post("http://localhost:5065/register", {
                        Name: this.name,
                        Email: this.email,
                        Password: this.password,
                        Role: this.role,
                    });
                    alert(response.data.message || "Регистрация прошла успешно!");
                    this.$router.push("/login");
                } catch (error) {
                    console.error("Ошибка при регистрации:", error);
                    alert(error.response?.data || "Ошибка при регистрации!");
                }
            },
            goToLogin() {
                this.$router.push("/login");
            },
        },
    };
</script>

<style scoped>
    .auth-container {
        background-color: black;
        color: white;
        padding: 20px;
        height: 100vh;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
    }

    h2 {
        text-align: center;
        margin-bottom: 30px;
    }

    .input-group {
        margin-bottom: 15px;
        width: 100%;
        max-width: 300px;
    }

    label {
        display: block;
        margin-bottom: 5px;
    }

    input, select {
        width: 100%;
        padding: 10px;
        border: 1px solid #ddd;
        border-radius: 5px;
        background-color: #222;
        color: white;
        font-size: 16px; /* Чтобы текст был одинаковым */
        box-sizing: border-box; /* Для выравнивания */
    }

    select {
        appearance: none; /* Убирает стандартные стрелки на некоторых браузерах */
    }

    button.submit-button {
        padding: 15px 30px;
        background-color: black;
        color: white;
        border: 2px solid white;
        border-radius: 20px;
        font-size: 18px;
        cursor: pointer;
        transition: background-color 0.3s, color 0.3s;
        display: block;
        margin: 20px auto 0;
    }

        button.submit-button:hover {
            background-color: white;
            color: black;
        }

    .redirect-text {
        margin-top: 20px;
        text-align: center;
    }

        .redirect-text a {
            color: white;
            text-decoration: underline;
            cursor: pointer;
        }
</style>
