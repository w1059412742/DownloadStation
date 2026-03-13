<template>
  <header class="flex-shrink-0 z-50 bg-background/80 backdrop-blur-xl border-b border-border/40 transition-colors duration-300">
    <div class="max-w-7xl mx-auto px-6">
      <div class="flex items-center justify-between h-16">
        <!-- Logo -->
        <a href="#" @click.prevent="$router.push('/')" class="flex items-center gap-3 group">
          <div class="w-9 h-9 rounded-xl bg-primary flex items-center justify-center shadow-sm group-hover:shadow-md transition-shadow">
            <Download class="w-5 h-5 text-primary-foreground" />
          </div>
          <span class="text-lg font-semibold text-foreground tracking-tight">下载站</span>
        </a>

        <!-- 居中导航：仅登录后显示 -->
        <nav v-if="isLoggedIn" class="absolute left-1/2 -translate-x-1/2 flex items-center gap-6">
          <router-link
            to="/"
            class="nav-item text-sm font-medium transition-colors duration-200 px-3 py-1.5 rounded-lg"
            :class="isHome ? 'text-primary bg-primary/10' : 'text-muted-foreground hover:text-foreground'"
          >
            首页
          </router-link>
          <router-link
            to="/admin/dashboard"
            class="nav-item text-sm font-medium transition-colors duration-200 px-3 py-1.5 rounded-lg"
            :class="isAdmin ? 'text-primary bg-primary/10' : 'text-muted-foreground hover:text-foreground'"
          >
            后台
          </router-link>
        </nav>

        <!-- 右侧操作区 -->
        <div class="flex items-center gap-4">
          <button
            v-if="!isLoggedIn"
            @click="$router.push('/login')"
            class="hidden sm:flex items-center gap-2 px-4 py-2 text-sm font-medium text-primary-foreground bg-primary rounded-lg hover:bg-primary/90 transition-colors shadow-sm hover:shadow"
          >
            登录
          </button>
          <button
            v-else
            @click="logout"
            class="text-sm font-medium text-primary hover:text-danger flex items-center transition-colors"
          >
            退出
          </button>
        </div>
      </div>
    </div>
  </header>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { Download } from 'lucide-vue-next'

const route = useRoute()
const router = useRouter()

/// 登录状态
const isLoggedIn = ref(false)

/// 判断当前是否在首页（精确匹配根路径或软件详情页）
const isHome = computed(() => {
  return route.path === '/' || route.path.startsWith('/software/')
})

/// 判断当前是否在后台管理区域
const isAdmin = computed(() => {
  return route.path.startsWith('/admin')
})

onMounted(() => {
  isLoggedIn.value = !!localStorage.getItem('admin_token')
})

/// 退出登录，清除令牌并跳转首页
const logout = () => {
  localStorage.removeItem('admin_token')
  isLoggedIn.value = false
  router.push('/')
}
</script>
