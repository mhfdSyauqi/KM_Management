import { createRouter, createWebHistory } from 'vue-router'
import BaseLayout from '@/components/BaseLayout.vue'

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
      component: BaseLayout,
      children: [
        {
          path: '',
          name: 'content',
          component: () => import('@/views/content/BaseContent.vue')
        },
        {
          path: 'create',
          name: 'create-content',
          component: () => import('@/views/content/CreateContent.vue')
        },
        {
          path: ':id',
          name: 'edit-content',
          component: () => import('@/views/content/EditContent.vue')
        }
      ]
    },
    {
      path: '/article/:id',
      name: 'article',
      component: () => import('@/views/article/BaseArticle.vue')
    }
  ]
})

export default router