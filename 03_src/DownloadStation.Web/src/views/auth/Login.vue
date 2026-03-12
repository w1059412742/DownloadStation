<template>
  <div class="min-h-screen flex flex-col items-center justify-center p-6 bg-surface">
    <div class="max-w-md w-full animate-fade-in-up">
      <div class="text-center mb-10">
        <h2 class="text-3xl font-bold tracking-tight text-textPrimary">管理员登录</h2>
        <p class="mt-2 text-sm text-textSecondary">请输入管理员密码以访问后台。</p>
      </div>

      <form class="space-y-6" @submit.prevent="handleLogin">
        <div>
          <label for="password" class="sr-only">密码</label>
          <input 
            v-model="password" 
            id="password" 
            name="password" 
            type="password" 
            required 
            class="appearance-none rounded-xl relative block w-full px-4 py-3 border border-border placeholder-textHint text-textPrimary focus:outline-none focus:ring-2 focus:ring-primary focus:border-transparent transition-all duration-300" 
            placeholder="请输入密码" 
          />
        </div>

        <div v-if="errorMessage" class="text-danger text-sm text-center animate-shake">
          {{ errorMessage }}
        </div>

        <div>
          <button 
            type="submit" 
            :disabled="loading"
            class="group relative w-full flex justify-center py-3 px-4 border border-transparent text-sm font-medium rounded-xl text-white bg-primary hover:bg-primaryHover focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary transition-all duration-300 shadow-soft hover:shadow-hover disabled:opacity-50"
          >
            <span v-if="loading">登录中...</span>
            <span v-else>登录</span>
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import http from '../../api/http'

const password = ref('')
const loading = ref(false)
const errorMessage = ref('')
const router = useRouter()
const route = useRoute()

const handleLogin = async () => {
  if (!password.value) return

  loading.value = true
  errorMessage.value = ''

  try {
    const res = await http.post('/api/auth/login', {
      password: password.value
    })
    
    if (res.data.code === 200) {
      localStorage.setItem('admin_token', res.data.data.token)
      const redirect = route.query.redirect as string || '/admin/dashboard'
      router.push(redirect)
    } else {
      errorMessage.value = res.data.message || '密码错误'
    }
  } catch (error: any) {
    if (error.response?.data?.message) {
      errorMessage.value = error.response.data.message
    } else {
      errorMessage.value = '网络连接失败，请稍后重试。'
    }
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}
.animate-fade-in-up {
  animation: fadeInUp 0.6s cubic-bezier(0.16, 1, 0.3, 1) forwards;
}

@keyframes shake {
  0%, 100% { transform: translateX(0); }
  25% { transform: translateX(-5px); }
  75% { transform: translateX(5px); }
}
.animate-shake {
  animation: shake 0.4s ease-in-out;
}
</style>
