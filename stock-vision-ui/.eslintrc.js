module.exports = {
    env: {
        browser: true,
        es2021: true,
        node: true, // Добавьте это для поддержки глобальных переменных Node.js, таких как `module` и `process`
    },
    extends: [
        'plugin:vue/vue3-essential',
        'eslint:recommended',
    ],
    parserOptions: {
        ecmaVersion: 12,
        sourceType: 'module',
    },
    rules: {},
};
