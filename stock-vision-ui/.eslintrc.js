module.exports = {
    env: {
        browser: true,
        es2021: true,
        node: true, // �������� ��� ��� ��������� ���������� ���������� Node.js, ����� ��� `module` � `process`
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
