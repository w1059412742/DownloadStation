import { createRouter, createWebHistory } from 'vue-router'
import type { RouteRecordRaw } from 'vue-router'

const routes: Array<RouteRecordRaw> = [
  // 公开前台展示
  {
    path: '/',
    component: () => import('../layouts/PublicLayout.vue'),
    children: [
      {
        path: '',
        name: 'Home',
        component: () => import('../views/public/Home.vue'),
        meta: { title: '首页' }
      },
      {
        path: 'software/:id',
        name: 'SoftwareDetail',
        component: () => import('../views/public/SoftwareDetail.vue'),
        meta: { title: '软件详情' }
      },
      {
        path: 's/:id',
        name: 'SoftwareShare',
        component: () => import('../views/public/SoftwareDetail.vue'),
        meta: { title: '软件分享' }
      }
    ]
  },
  // 独立登录页
  {
    path: '/login',
    name: 'Login',
    component: () => import('../views/auth/Login.vue'),
    meta: { title: '管理员登录' }
  },
  // 后台管理面板
  {
    path: '/admin',
    component: () => import('../layouts/AdminLayout.vue'),
    meta: { requiresAuth: true },
    children: [
      {
        path: 'dashboard',
        name: 'Dashboard',
        component: () => import('../views/admin/Dashboard.vue'),
        meta: { title: '仪表盘' }
      },
      {
        path: 'categories',
        name: 'AdminCategories',
        component: () => import('../views/admin/Categories.vue'),
        meta: { title: '分类与平台' }
      },
      {
        path: 'tags',
        name: 'AdminTags',
        component: () => import('../views/admin/Tags.vue'),
        meta: { title: '标签管理' }
      },

      {
        path: 'softwares',
        name: 'AdminSoftwares',
        component: () => import('../views/admin/Softwares.vue'),
        meta: { title: '软件管理' }
      },
      {
        path: 'softwares/:id',
        name: 'AdminSoftwareDetail',
        component: () => import('../views/admin/SoftwareDetail.vue'),
        meta: { title: '编辑软件' }
      },
      {
        path: 'files',
        name: 'AdminFiles',
        component: () => import('../views/admin/Files.vue'),
        meta: { title: '文件扫描' }
      }
    ]
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

// 简单前置路由守卫
router.beforeEach((to, _from, next) => {
  document.title = `${to.meta.title} - 应用私藏馆`

  const isAuthenticated = !!localStorage.getItem('admin_token')

  if (to.matched.some(record => record.meta.requiresAuth)) {
    if (!isAuthenticated) {
      next({ name: 'Login', query: { redirect: to.fullPath } })
    } else {
      next()
    }
  } else {
    // 已登录用户禁止反向进入 Login 重新看动画
    if (to.name === 'Login' && isAuthenticated) {
      next({ name: 'Dashboard' })
    } else {
      next()
    }
  }
})

export default router
