<template>
  <div class="h-screen flex flex-col overflow-hidden bg-background selection:bg-primary selection:text-white">
    <!-- 全局标题栏 -->
    <AppHeader />

    <!-- Scrollable Content -->
    <div class="flex-1 overflow-y-auto flex flex-col w-full">
      <main class="flex-1 w-full mx-auto container px-4 sm:px-6 lg:px-8 py-6">
        <router-view v-slot="{ Component }">
          <transition name="page-fade" mode="out-in">
            <component :is="Component" />
          </transition>
        </router-view>
      </main>

      <!-- Footer -->
      <footer class="border-t border-border bg-card/50 mt-auto flex-shrink-0">
        <div class="max-w-7xl mx-auto px-6 py-6">
          <div class="flex flex-col sm:flex-row items-center justify-center gap-4">
            <p class="text-sm text-muted-foreground">
              © {{ new Date().getFullYear() }} 下载站. All rights reserved.
            </p>
          </div>
        </div>
      </footer>
    </div>
  </div>
</template>

<script setup lang="ts">
import AppHeader from '../components/AppHeader.vue'
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
