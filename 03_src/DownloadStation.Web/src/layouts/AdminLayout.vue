<template>
  <div class="h-screen bg-background flex flex-col overflow-hidden selection:bg-primary selection:text-white">
    <!-- 全局标题栏 -->
    <AppHeader />

    <!-- 下方：侧边栏 + 主内容区 -->
    <div class="flex-1 flex overflow-hidden">
      <!-- Sidebar -->
      <div class="w-64 bg-surface border-r border-border flex flex-col transition-all duration-300">
        <div class="h-12 flex items-center px-6 border-b border-border">
          <Monitor class="w-5 h-5 text-primary mr-2" />
          <span class="font-bold text-base tracking-tight">管理后台</span>
        </div>
        <nav class="flex-1 overflow-y-auto py-4 space-y-1 px-3">
          <router-link 
            v-for="item in navItems" 
            :key="item.path"
            :to="item.path"
            class="nav-link flex items-center px-3 py-2 rounded-xl text-sm font-medium transition-colors duration-200"
            :class="[ route.path.startsWith(item.path) ? 'bg-primary/10 text-primary' : 'text-textSecondary hover:bg-black/5 hover:text-textPrimary' ]"
          >
            <component :is="item.icon" class="w-5 h-5 mr-3 shrink-0" 
              :class="[ route.path.startsWith(item.path) ? 'text-primary' : 'text-textHint' ]" 
            />
            {{ item.name }}
          </router-link>
        </nav>
        <div class="p-4 border-t border-border">
          <button @click="logout" class="flex w-full items-center px-3 py-2 text-sm font-medium rounded-xl text-textSecondary hover:bg-danger/10 hover:text-danger transition-colors duration-200">
            <LogOut class="w-5 h-5 mr-3 shrink-0 text-textHint group-hover:text-danger" />
            退出登录
          </button>
        </div>
      </div>

      <!-- Main Content -->
      <div class="flex-1 flex flex-col min-w-0 overflow-hidden">
        <main class="flex-1 overflow-y-auto relative">
          <router-view v-slot="{ Component }">
            <transition name="fade-slide" mode="out-in">
              <component :is="Component" />
            </transition>
          </router-view>
        </main>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useRoute, useRouter } from 'vue-router'
import { LayoutDashboard, Layers, Tag as TagIcon, Box, FolderSearch, LogOut, Monitor } from 'lucide-vue-next'
import AppHeader from '../components/AppHeader.vue'

const route = useRoute()
const router = useRouter()

const navItems = [
  { name: '仪表盘', path: '/admin/dashboard', icon: LayoutDashboard },
  { name: '分类与平台', path: '/admin/categories', icon: Layers },
  { name: '标签管理', path: '/admin/tags', icon: TagIcon },
  { name: '软件管理', path: '/admin/softwares', icon: Box },
  { name: '文件扫描', path: '/admin/files', icon: FolderSearch },
]


const logout = () => {
  localStorage.removeItem('admin_token')
  router.push('/login')
}
</script>

<style scoped>
.fade-slide-enter-active,
.fade-slide-leave-active {
  transition: all 0.3s cubic-bezier(0.16, 1, 0.3, 1);
}

.fade-slide-enter-from {
  opacity: 0;
  transform: translateY(10px);
}

.fade-slide-leave-to {
  opacity: 0;
  transform: translateY(-10px);
}
</style>
