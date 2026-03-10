<template>
  <div class="min-h-screen flex flex-col bg-background selection:bg-primary selection:text-white">
    <!-- Header -->
    <header class="fixed top-0 left-0 right-0 z-50 bg-background/80 backdrop-blur-xl border-b border-border/40 transition-colors duration-300">
      <div class="max-w-7xl mx-auto px-6">
        <div class="flex items-center justify-between h-16">
          <!-- Logo -->
          <a href="#" @click.prevent="$router.push('/')" class="flex items-center gap-3 group">
            <div class="w-9 h-9 rounded-xl bg-primary flex items-center justify-center shadow-sm group-hover:shadow-md transition-shadow">
              <Download class="w-5 h-5 text-primary-foreground" />
            </div>
            <span class="text-lg font-semibold text-foreground tracking-tight">下载站</span>
          </a>

          <!-- Navigation -->
          <nav class="hidden md:flex items-center gap-8">
            <a href="#" @click.prevent="$router.push('/')" class="text-sm font-medium text-foreground hover:text-primary transition-colors">首页</a>
          </nav>

          <!-- Actions -->
          <div class="flex items-center gap-4">
            <button v-if="!isLoggedIn" @click="$router.push('/login')" class="hidden sm:flex items-center gap-2 px-4 py-2 text-sm font-medium text-primary-foreground bg-primary rounded-lg hover:bg-primary/90 transition-colors shadow-sm hover:shadow">
              登录
            </button>
            <div v-else class="flex items-center space-x-4">
              <button @click="$router.push('/admin/dashboard')" class="text-sm font-medium text-muted-foreground hover:text-foreground transition-colors">管理后台</button>
              <button @click="logout" class="text-sm font-medium text-primary hover:text-danger flex items-center transition-colors">退出</button>
            </div>
          </div>
        </div>
      </div>
    </header>

    <!-- Content -->
    <main class="flex-1 w-full mx-auto container pt-16 px-4 sm:px-6 lg:px-8 pb-12">
      <router-view v-slot="{ Component }">
        <transition name="page-fade" mode="out-in">
          <component :is="Component" />
        </transition>
      </router-view>
    </main>

    <!-- Footer -->
    <footer class="border-t border-border bg-card/50 mt-auto">
      <div class="max-w-7xl mx-auto px-6 py-6">
        <div class="flex flex-col sm:flex-row items-center justify-center gap-4">
          <p class="text-sm text-muted-foreground">
            © {{ new Date().getFullYear() }} 下载站. All rights reserved.
          </p>
        </div>
      </div>
    </footer>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { Download } from 'lucide-vue-next'

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
