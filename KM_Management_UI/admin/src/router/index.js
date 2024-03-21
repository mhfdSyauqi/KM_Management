import { createRouter, createWebHistory } from 'vue-router'
import BaseLayout from '@/components/BaseLayout.vue'
import { GetUserRole } from '@/api/reqRole.js'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      redirect: { name: 'analytic-dashboard' }
    },
    {
      path: '/dashboard',
      component: BaseLayout,
      children: [
        {
          path: 'analytic',
          name: 'analytic-dashboard',
          component: () => import('@/views/dashboard/analytic/BaseAnalytic.vue')
        },
        {
          path: 'rate',
          name: 'rate-dashboard',
          component: () => import('@/views/dashboard/rate/BaseRate.vue')
        }
      ]
    },
    {
      path: '/setup',
      component: BaseLayout,
      meta: {
        roles: ['super']
      },
      children: [
        {
          path: 'profile',
          name: 'profile-setup',
          component: () => import('@/views/setup/profile/BaseProfile.vue')
        },
        {
          path: 'role',
          name: 'role-setup',
          component: () => import('@/views/setup/role/BaseRole.vue')
        },
        {
          path: 'message',
          name: 'message-setup',
          component: () => import('@/views/setup/message/BaseMessages.vue')
        },
        {
          path: 'general',
          name: 'general-setup',
          component: () => import('@/views/setup/general/BaseGeneral.vue')
        }
      ]
    },
    {
      path: '/categories',
      component: BaseLayout,
      meta: {
        roles: ['super', 'admin']
      },
      children: [
        {
          path: '',
          name: 'categories',
          component: () => import('@/views/categories/list/BaseListCategories.vue')
        },
        {
          path: ':secondLayer',
          name: 'categories-second-layer',
          component: () => import('@/views/categories/list/ListCategoriesSecondLayer.vue')
        },
        {
          path: ':secondLayer/:thirdLayer',
          name: 'categories-third-layer',
          component: () => import('@/views/categories/list/ListCategoriesThirdLayer.vue')
        },
        {
          path: 'top',
          name: 'top-categories',
          component: () => import('@/views/categories/top/BaseTopCategories.vue')
        }
      ]
    },
    {
      path: '/content',
      component: BaseLayout,
      meta: {
        roles: ['super', 'admin']
      },
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
    },
    {
      path: '/error/:code',
      name: 'error-page',
      component: () => import('@/views/errors/BaseError.vue')
    },
    {
      path: '/:pathMatch(.*)*',
      name: 'not-found',
      redirect: {
        name: 'error-page',
        params: { code: 404 }
      }
    }
  ]
})

router.beforeEach(async (to) => {
  if (to.name === 'error-page') return

  const userInfo = await GetUserRole()

  if (!userInfo.is_success) {
    return { name: 'error-page', params: { code: 500 }, meta: userInfo.error }
  }
  const userRole = userInfo.user_role.role.toLowerCase()

  to.meta.userInfo = {
    userName: userInfo.user_role.user_name,
    role: userRole
  }

  if (to.meta.roles && !to.meta.roles.includes(userRole)) {
    return { name: 'error-page', params: { code: 401 }, meta: userInfo.error }
  }
})

export default router