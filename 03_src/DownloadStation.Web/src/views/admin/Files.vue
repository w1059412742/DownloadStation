<template>
  <div class="space-y-6 animate-fade-in-up p-6 lg:p-10">
    <!-- Header Area -->
    <div class="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4 bg-surface p-6 rounded-2xl border border-border shadow-soft">
      <div>
        <h1 class="text-2xl font-bold tracking-tight text-textPrimary flex items-center gap-2">
          <FolderSearch class="w-6 h-6 text-primary" />
          NAS 文件扫描
        </h1>
        <p class="text-sm text-textSecondary mt-1">扫描 NAS 存储目录中尚未关联到软件版本的文件。</p>
      </div>
      <div class="flex items-center gap-4">
        <div class="relative min-w-[300px]">
          <input 
            v-model="scanPath" 
            type="text" 
            placeholder="自定义扫描路径 (留空使用默认)" 
            class="w-full px-4 py-2 bg-surface border border-border rounded-xl text-sm focus:outline-none focus:ring-2 focus:ring-primary/20 focus:border-primary transition-all"
          />
        </div>
        <button @click="scanFiles" :disabled="loading" class="flex items-center px-4 py-2 bg-primary text-white rounded-xl text-sm font-medium hover:bg-primaryHover transition-colors shadow-soft hover:shadow-hover disabled:opacity-50">
          <Radar class="w-4 h-4 mr-2" :class="{'animate-spin': loading}" />           {{ loading ? '扫描中...' : '开始扫描' }}
        </button>
      </div>
    </div>

    <!-- Results Card -->
    <div class="bg-surface rounded-2xl border border-border shadow-soft overflow-hidden">
      <div class="px-6 py-4 border-b border-border bg-black/5 flex justify-between items-center">
         <h2 class="font-semibold text-textPrimary">未关联的文件</h2>
         <span class="text-xs text-textHint">{{ files.length }} 个文件</span>
      </div>

      <div class="p-0">
        <div v-if="files.length === 0 && !loading" class="text-center py-16">
          <CheckCircle class="w-16 h-16 text-success/20 mx-auto mb-4" />
           <h3 class="text-lg font-medium text-textPrimary">暂无未关联文件</h3>
           <p class="text-textHint mt-1">所有文件均已关联到对应的软件版本。</p>
        </div>

        <ul class="divide-y divide-border" v-else>
          <li v-for="file in files" :key="file.filePath" class="p-6 hover:bg-black/5 transition-colors flex flex-col lg:flex-row lg:items-center justify-between gap-4">
            <div class="flex items-start space-x-4">
              <div class="mt-1 p-2 bg-textHint/10 rounded-lg">
                 <FileArchive class="w-6 h-6 text-textSecondary" />
              </div>
              <div>
                <h4 class="font-semibold text-textPrimary break-all">{{ file.fileName }}</h4>
                <p class="text-xs text-textHint mt-1 font-mono tracking-tight">{{ file.filePath }}</p>
                <div class="flex items-center space-x-3 mt-2">
                  <span class="text-xs font-medium text-textSecondary bg-black/5 px-2 py-1 rounded-md">
                    {{ (file.size / 1024 / 1024).toFixed(2) }} MB
                  </span>
                </div>
              </div>
            </div>
            
            <div class="flex lg:flex-shrink-0 lg:ml-4 border-t lg:border-t-0 border-border pt-4 lg:pt-0">
               <!-- 简化版：一键快捷绑定 Modal，此处需真实软件 ID 列表下拉 -->
               <button @click="openBindDialog(file)" class="px-4 py-2 bg-surface border border-border rounded-xl text-sm font-medium text-textPrimary hover:border-primary hover:text-primary transition-colors hover:shadow-soft">
                  关联到软件
               </button>
            </div>
          </li>
        </ul>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { FolderSearch, Radar, CheckCircle, FileArchive } from 'lucide-vue-next'
import http from '../../api/http'

const files = ref<any[]>([])
const loading = ref(false)
const scanPath = ref('')

const scanFiles = async () => {
  loading.value = true
  try {
    const res = await http.get('/api/admin/files/scan', {
      params: { path: scanPath.value }
    })
    if (res.data.code === 200) {
      files.value = res.data.data
    }
  } catch (error: any) {
    alert(error.response?.data?.message || '扫描失败，请检查路径是否正确及权限设置。')
  } finally {
    loading.value = false
  }
}

const openBindDialog = (file: any) => {
  alert(`将文件「${file.fileName}」关联到软件版本的功能尚在开发中。`)
}
</script>

<style scoped>
@keyframes fadeInUp {
  from { opacity: 0; transform: translateY(10px); }
  to { opacity: 1; transform: translateY(0); }
}
.animate-fade-in-up {
  animation: fadeInUp 0.4s ease-out forwards;
}
</style>
