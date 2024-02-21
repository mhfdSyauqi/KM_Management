import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'dashboard',
      redirect: { name: 'content' }
    },
    {
      path: '/content',
      name: 'content',
      component: () => import('../views/content/BaseContent.vue')
    }
  ]
})

export default router
