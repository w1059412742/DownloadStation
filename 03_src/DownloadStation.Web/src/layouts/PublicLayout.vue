<template>
  <div class="min-h-screen bg-background selection:bg-primary selection:text-white flex flex-col">
    <!-- Header -->
    <header class="sticky top-0 z-40 w-full backdrop-blur-md bg-surface/80 border-b border-border transition-colors duration-300">
      <div class="container mx-auto px-4 sm:px-6 lg:px-8">
        <div class="flex items-center justify-between h-16">
          <div class="flex items-center cursor-pointer group" @click="$router.push('/')">
            <div class="w-8 h-8 rounded-lg bg-primary flex items-center justify-center text-white shadow-soft group-hover:scale-105 transition-transform">
              <span class="font-bold text-lg">D</span>
            </div>
            <span class="ml-3 font-semibold text-lg text-textPrimary tracking-tight">下载站</span>
          </div>

          <div class="hidden md:flex items-center space-x-8">
            <router-link to="/" class="text-sm font-medium text-textPrimary hover:text-primary transition-colors">首页</router-link>
            <!-- 预留平台横向筛选等 -->
          </div>

          <div class="flex items-center px-4 space-x-4">
             <!-- Placeholder for Search -->
             <div class="relative hidden sm:block w-full max-w-xs">
              <input type="text" placeholder="搜索软件..." class="w-full pl-10 pr-4 py-2 border-none bg-black/5 rounded-full text-sm focus:outline-none focus:ring-2 focus:ring-primary/50 transition-all text-textPrimary placeholder-textHint" />
              <div class="absolute left-3 top-2.5">
                <Search class="w-4 h-4 text-textHint" />
              </div>
             </div>
             
             <!-- User Auth State -->
             <div class="flex items-center">
                <button v-if="!isLoggedIn" @click="$router.push('/login')" class="flex items-center text-sm font-medium text-textSecondary hover:text-primary transition-colors">
                   登录
                </button>
                <div v-else class="flex items-center space-x-3">
                   <button @click="$router.push('/admin/dashboard')" class="text-sm font-medium text-textSecondary hover:text-primary transition-colors">管理后台</button>
                   <button @click="logout" class="text-sm font-medium text-primary hover:text-danger transition-colors">退出</button>
                </div>
             </div>
          </div>
        </div>
      </div>
    </header>

    <!-- Content -->
    <main class="flex-1 w-full mx-auto container px-4 sm:px-6 lg:px-8 py-8 lg:py-12">
      <router-view v-slot="{ Component }">
        <transition name="page-fade" mode="out-in">
          <component :is="Component" />
        </transition>
      </router-view>
    </main>

    <!-- Footer -->
    <footer class="border-t border-border bg-surface mt-auto">
      <div class="container mx-auto px-4 py-8">
        <p class="text-center text-sm text-textHint">
          © {{ new Date().getFullYear() }} 下载站. All rights reserved.
        </p>
      </div>
    </footer>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { Search } from 'lucide-vue-next'

const router = useRouter()
const isLoggedIn = ref(false)

onMounted(() => {
  isLoggedIn.value = !!localStorage.getItem('admin_token')
})

const logout = () => {
  localStorage.removeItem('admin_token')
  isLoggedIn.value = false
  router.push('/')
}
</script>

<style scoped>
.page-fade-enter-active,
.page-fade-leave-active {
  transition: opacity 0.4s ease;
}

.page-fade-enter-from,
.page-fade-leave-to {
  opacity: 0;
}
</style>
