<template>
  <div class="space-y-6 animate-fade-in-up">
    <!-- Header Area -->
    <div class="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4 bg-surface p-6 rounded-2xl border border-border shadow-soft">
      <div>
        <h1 class="text-2xl font-bold tracking-tight text-textPrimary flex items-center gap-2">
          <Layers class="w-6 h-6 text-primary" />
          分类与平台管理
        </h1>
        <p class="text-sm text-textSecondary mt-1">管理软件分类和支持的操作系统平台。</p>
      </div>
      <div class="flex space-x-3">
        <button @click="openCategoryModal()" class="flex items-center px-4 py-2 bg-primary text-white rounded-xl text-sm font-medium hover:bg-primaryHover transition-colors shadow-soft hover:shadow-hover">
          <Plus class="w-4 h-4 mr-2" /> 新增分类
        </button>
        <button @click="openPlatformModal()" class="flex items-center px-4 py-2 bg-black/5 text-textPrimary rounded-xl text-sm font-medium hover:bg-black/10 transition-colors">
          <Monitor class="w-4 h-4 mr-2" /> 新增平台
        </button>
      </div>
    </div>

    <div class="grid xl:grid-cols-2 gap-6">
      
      <!-- Categories Section -->
      <div class="bg-surface rounded-2xl border border-border overflow-hidden shadow-soft flex flex-col h-[600px]">
        <div class="px-6 py-4 border-b border-border bg-black/5 flex justify-between items-center">
          <h2 class="font-semibold text-textPrimary">软件分类</h2>
          <span class="text-xs text-textHint">{{ categories.length }} 个节点</span>
        </div>
        <div class="p-6 overflow-y-auto flex-1">
          <div v-if="loadingCategories" class="flex justify-center p-8">
            <Loader2 class="w-8 h-8 text-primary animate-spin" />
          </div>
          <div v-else-if="categories.length === 0" class="text-center py-12">
            <FolderTree class="w-12 h-12 text-border mx-auto mb-3" />
            <p class="text-textHint text-sm">暂无分类数据</p>
          </div>
          <ul v-else class="space-y-4">
             <li v-for="category in categories" :key="category.id" class="group bg-black/5 rounded-xl p-4 border border-transparent hover:border-border transition-colors">
                <div class="flex items-center justify-between">
                  <div class="flex items-center space-x-3">
                    <Folder class="w-5 h-5 text-textSecondary" />
                    <span class="font-medium text-textPrimary">{{ category.name }}</span>
                  </div>
                  <div class="opacity-0 group-hover:opacity-100 transition-opacity flex space-x-2">
                    <button @click="openCategoryModal(category)" class="p-1.5 text-textSecondary hover:text-primary rounded-lg hover:bg-primary/10 transition-colors">
                      <Edit2 class="w-4 h-4" />
                    </button>
                    <button @click="deleteCategory(category.id)" class="p-1.5 text-textSecondary hover:text-danger rounded-lg hover:bg-danger/10 transition-colors">
                      <Trash class="w-4 h-4" />
                    </button>
                  </div>
                </div>
                <!-- 预留子分类遍历 -->
                <ul v-if="category.children && category.children.length > 0" class="mt-3 pl-8 space-y-2 border-l-2 border-border ml-2">
                   <li v-for="child in category.children" :key="child.id" class="group/child flex items-center justify-between p-2 rounded-lg hover:bg-black/5 transition-colors">
                      <div class="flex items-center space-x-2">
                        <span class="font-medium text-sm text-textSecondary">{{ child.name }}</span>
                      </div>
                      <div class="opacity-0 group-hover/child:opacity-100 transition-opacity flex space-x-1">
                        <button @click="openCategoryModal(child)" class="p-1 text-textHint hover:text-primary transition-colors"><Edit2 class="w-3.5 h-3.5" /></button>
                        <button @click="deleteCategory(child.id)" class="p-1 text-textHint hover:text-danger transition-colors"><Trash class="w-3.5 h-3.5" /></button>
                      </div>
                   </li>
                </ul>
             </li>
          </ul>
        </div>
      </div>

      <!-- Platforms Section -->
      <div class="bg-surface rounded-2xl border border-border overflow-hidden shadow-soft flex flex-col h-[600px]">
        <div class="px-6 py-4 border-b border-border bg-black/5 flex justify-between items-center">
          <h2 class="font-semibold text-textPrimary">支持环境平台</h2>
          <span class="text-xs text-textHint">{{ platforms.length }} 个节点</span>
        </div>
        <div class="p-6 overflow-y-auto flex-1">
          <div v-if="loadingPlatforms" class="flex justify-center p-8">
            <Loader2 class="w-8 h-8 text-primary animate-spin" />
          </div>
          <div v-else-if="platforms.length === 0" class="text-center py-12">
            <MonitorOff class="w-12 h-12 text-border mx-auto mb-3" />
            <p class="text-textHint text-sm">未绑定任何环境平台</p>
          </div>
          <div v-else class="grid grid-cols-1 sm:grid-cols-2 gap-4">
             <div v-for="platform in platforms" :key="platform.id" class="group flex items-center justify-between p-4 rounded-xl border border-border hover:shadow-soft transition-all bg-surface">
                <div class="flex items-center space-x-3">
                  <div class="w-10 h-10 rounded-lg flex items-center justify-center text-white font-bold text-xl relative overflow-hidden" :style="{ backgroundColor: platform.colorHex || '#6B7280' }">
                     <Monitor class="w-5 h-5 absolute opacity-20" />
                     <span class="z-10 text-sm">{{ platform.name.charAt(0).toUpperCase() }}</span>
                  </div>
                  <div>
                    <h3 class="font-medium text-textPrimary text-sm">{{ platform.name }}</h3>
                    <p v-if="platform.iconClass" class="text-xs text-textHint">{{ platform.iconClass }}</p>
                  </div>
                </div>
                <div class="opacity-0 group-hover:opacity-100 transition-opacity flex flex-col space-y-1">
                    <button @click="openPlatformModal(platform)" class="p-1.5 text-textSecondary hover:text-primary rounded-lg hover:bg-black/5 transition-colors">
                      <Edit2 class="w-4 h-4" />
                    </button>
                    <button @click="deletePlatform(platform.id)" class="p-1.5 text-textSecondary hover:text-danger rounded-lg hover:bg-black/5 transition-colors">
                      <Trash class="w-4 h-4" />
                    </button>
                </div>
             </div>
          </div>
        </div>
      </div>

    </div>

    <!-- Modals would go here (Simplified for initial commit) -->
    
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { Layers, Plus, Monitor, Folder, Edit2, Trash, FolderTree, Loader2, MonitorOff } from 'lucide-vue-next'
import axios from 'axios'

const categories = ref<any[]>([])
const platforms = ref<any[]>([])
const loadingCategories = ref(true)
const loadingPlatforms = ref(true)

const token = localStorage.getItem('admin_token')
const axConfig = { headers: { Authorization: `Bearer ${token}` } }

const fetchCategories = async () => {
  try {
    loadingCategories.value = true
    const res = await axios.get('http://localhost:5000/api/admin/categories', axConfig)
    if (res.data.code === 200) categories.value = res.data.data
  } finally {
    loadingCategories.value = false
  }
}

const fetchPlatforms = async () => {
  try {
    loadingPlatforms.value = true
    const res = await axios.get('http://localhost:5000/api/admin/platforms', axConfig)
    if (res.data.code === 200) platforms.value = res.data.data
  } finally {
    loadingPlatforms.value = false
  }
}

onMounted(() => {
  fetchCategories()
  fetchPlatforms()
})

const openCategoryModal = (_category: any = null) => {
  alert('此处需要弹窗编辑分类资料，为保持演示原子性暂不展开')
}
const openPlatformModal = (_platform: any = null) => {
  alert('此处需要弹窗编辑平台资料，为保持演示原子性暂不展开')
}

const deleteCategory = async (id: string) => {
  if (!confirm('确定删除该分类？')) return
  try {
    const res = await axios.delete(`http://localhost:5000/api/admin/categories/${id}`, axConfig)
    if (res.data.code === 200) fetchCategories()
    else alert(res.data.message)
  } catch (e: any) { alert(e.response?.data?.message || '操作失败') }
}

const deletePlatform = async (id: string) => {
  if (!confirm('确定删除该平台？')) return
  try {
    const res = await axios.delete(`http://localhost:5000/api/admin/platforms/${id}`, axConfig)
    if (res.data.code === 200) fetchPlatforms()
    else alert(res.data.message)
  } catch (e: any) { alert(e.response?.data?.message || '操作失败') }
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
