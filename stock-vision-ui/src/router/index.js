import { createRouter, createWebHistory } from 'vue-router';

import Home from '../views/HomeView.vue';
import AssetList from '../views/AssetList.vue';
import PortfolioList from '../views/PortfoliosList.vue';
import TransactionList from '../views/TransactionsList.vue';
import Analysis from '../views/AnalysisView.vue';
import SearchView from '../views/SearchView.vue';
import FiltrView from '../views/FiltrView.vue';
import SettingsVeiw from '../views/SettingsVeiw.vue';
import EditAsset from '../views/EditAssetList.vue';
import RegisterView from '../views/RegisterList.vue';
import LoginView from '../views/LoginList.vue';

const routes = [
    { path: '/', component: Home },
    { path: '/assets', component: AssetList, meta: { requiresAuth: true } },
    { path: '/portfolios', component: PortfolioList, meta: { requiresAuth: true } },
    { path: '/transactions', component: TransactionList, meta: { requiresAuth: true } },
    { path: '/analysis', component: Analysis, meta: { requiresAuth: true } },
    { path: '/search', component: SearchView, meta: { requiresAuth: true } },
    { path: '/filtr', component: FiltrView, meta: { requiresAuth: true } },
    { path: '/settings', component: SettingsVeiw, meta: { requiresAuth: true } },
    { path: '/assets/edit/:symbol', component: EditAsset, meta: { requiresAuth: true } },
    { path: '/register', component: RegisterView },
    { path: '/login', component: LoginView },
];

const router = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    routes,
});

// Глобальный перехватчик навигации для авторизации
router.beforeEach((to, from, next) => {
    const token = localStorage.getItem('authToken');

    if (to.matched.some(record => record.meta.requiresAuth)) {
        if (!token) {
            next('/login');
        } else {
            next();
        }
    } else {
        next();
    }
});


export default router;