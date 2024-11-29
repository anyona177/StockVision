<template>
    <div class="auth-background">
        <div class="auth-container">
            <h2>Вход</h2>
            <form @submit.prevent="login">
                <div class="input-group">
                    <label for="email">Email</label>
                    <input v-model="email" type="email" id="email" required placeholder="Введите ваш email" />
                </div>

                <div class="input-group">
                    <label for="password">Пароль</label>
                    <input v-model="password" type="password" id="password" required placeholder="Введите пароль" />
                </div>

                <button type="submit" class="submit-button">Войти</button>
            </form>

            <p class="redirect-text">Нет аккаунта? <a @click="goToRegister">Зарегистрироваться</a></p>
        </div>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                email: "",
                password: "",
                errorMessage: "",
            };
        },
        methods: {
            async login() {
                try {
                    const response = await fetch("http://localhost:5065/login", {
                        method: "POST",
                        headers: { "Content-Type": "application/json" },
                        body: JSON.stringify({
                            email: this.email,
                            password: this.password,
                        }),
                    });

                    if (!response.ok) {
                        throw new Error("Проверьте логин и пароль.");
                    }

                    const responseJson = await response.json();

                    console.log("Ответ сервера:", responseJson);

                    //Проверяем наличие токена
                    const token = responseJson.token;
                    if (!token) {
                        throw new Error("Токен отсутствует в ответе сервера.");
                    }

                    //Сохраняем токен в localStorage
                    localStorage.setItem("authToken", token);
                    localStorage.setItem("email", this.email);
                    console.log("Токен успешно сохранён. Перенаправление на главную");
                    this.$router.push('/');
                } catch (error) {
                    console.error("Ошибка входа:", error);
                    this.errorMessage = error.message;
                }
            },
            goToRegister() {
                this.$router.push("/register");
            },
        },
    };
</script>

<style scoped>
    .auth-background {
        background-color: black;
        height: 100vh;
        display: flex;
        justify-content: center;
        align-items: center;
    }

    .auth-container {
        background-color: black;
        color: white;
        padding: 30px;
        width: 100%;
        max-width: 400px;
        border: 2px solid white; /* Белая каемка */
        border-radius: 10px;
        text-align: center;
    }

    h2 {
        margin-bottom: 20px;
    }

    .input-group {
        margin-bottom: 15px;
    }

    label {
        display: block;
        margin-bottom: 5px;
    }

    input {
        width: 100%;
        padding: 10px;
        border: 1px solid #ddd;
        border-radius: 5px;
        background-color: #222;
        color: white;
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
        margin-top: 20px;
    }

        button.submit-button:hover {
            background-color: white;
            color: black;
        }

    .redirect-text {
        margin-top: 20px;
    }

        .redirect-text a {
            color: white;
            text-decoration: underline;
            cursor: pointer;
        }
</style>
