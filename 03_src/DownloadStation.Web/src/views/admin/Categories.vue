<template>
  <div class="space-y-6 animate-fade-in-up p-6 lg:p-10">
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

    <!-- Category Modal -->
    <div v-if="showCategoryModal" class="fixed inset-0 z-50 flex items-center justify-center p-4">
      <div class="absolute inset-0 bg-black/50 backdrop-blur-sm" @click="showCategoryModal = false"></div>
      <div class="relative bg-surface w-full max-w-md rounded-2xl shadow-xl overflow-hidden animate-fade-in-up">
        <div class="px-6 py-4 border-b border-border flex justify-between items-center bg-black/5">
          <h3 class="font-bold text-textPrimary">{{ isEditingCategory ? '编辑分类' : '新增分类' }}</h3>
          <button @click="showCategoryModal = false" class="text-textHint hover:text-textPrimary transition-colors">
            <X class="w-5 h-5" />
          </button>
        </div>
        <form @submit.prevent="saveCategory" class="p-6 space-y-4">
          <div>
            <label class="block text-xs font-bold text-textSecondary uppercase mb-1.5">分类名称</label>
            <input v-model="categoryForm.name" type="text" required placeholder="例如：开发工具" class="w-full px-4 py-2 bg-black/5 border border-border rounded-xl focus:outline-none focus:ring-2 focus:ring-primary/50 text-textPrimary" />
          </div>
          <div>
            <label class="block text-xs font-bold text-textSecondary uppercase mb-1.5">上级分类</label>
            <select v-model="categoryForm.parentId" class="w-full px-4 py-2 bg-black/5 border border-border rounded-xl focus:outline-none focus:ring-2 focus:ring-primary/50 text-textPrimary">
              <option value="">无（顶级分类）</option>
              <option v-for="cat in flatCategories" :key="cat.id" :value="cat.id" :disabled="cat.id === categoryForm.id">
                {{ cat.name }}
              </option>
            </select>
          </div>
          <div>
            <label class="block text-xs font-bold text-textSecondary uppercase mb-1.5">显示排序</label>
            <input v-model.number="categoryForm.sortOrder" type="number" class="w-full px-4 py-2 bg-black/5 border border-border rounded-xl focus:outline-none focus:ring-2 focus:ring-primary/50 text-textPrimary" />
          </div>
          <div class="pt-2 flex space-x-3">
            <button type="button" @click="showCategoryModal = false" class="flex-1 px-4 py-2 bg-black/5 text-textPrimary rounded-xl font-medium hover:bg-black/10 transition-colors">取消</button>
            <button type="submit" :disabled="saving" class="flex-1 px-4 py-2 bg-primary text-white rounded-xl font-medium hover:bg-primaryHover transition-colors disabled:opacity-50">
              {{ saving ? '保存中...' : '确定' }}
            </button>
          </div>
        </form>
      </div>
    </div>

    <!-- Platform Modal -->
    <div v-if="showPlatformModal" class="fixed inset-0 z-50 flex items-center justify-center p-4">
      <div class="absolute inset-0 bg-black/50 backdrop-blur-sm" @click="showPlatformModal = false"></div>
      <div class="relative bg-surface w-full max-w-md rounded-2xl shadow-xl overflow-hidden animate-fade-in-up">
        <div class="px-6 py-4 border-b border-border flex justify-between items-center bg-black/5">
          <h3 class="font-bold text-textPrimary">{{ isEditingPlatform ? '编辑平台' : '新增平台' }}</h3>
          <button @click="showPlatformModal = false" class="text-textHint hover:text-textPrimary transition-colors">
            <X class="w-5 h-5" />
          </button>
        </div>
        <form @submit.prevent="savePlatform" class="p-6 space-y-4">
          <div>
            <label class="block text-xs font-bold text-textSecondary uppercase mb-1.5">平台名称</label>
            <input v-model="platformForm.name" type="text" required placeholder="例如：Windows" class="w-full px-4 py-2 bg-black/5 border border-border rounded-xl focus:outline-none focus:ring-2 focus:ring-primary/50 text-textPrimary" />
          </div>
          <div class="grid grid-cols-2 gap-4">
            <div>
              <label class="block text-xs font-bold text-textSecondary uppercase mb-1.5">图标名称</label>
              <input v-model="platformForm.iconClass" type="text" placeholder="例如：Monitor" class="w-full px-4 py-2 bg-black/5 border border-border rounded-xl focus:outline-none focus:ring-2 focus:ring-primary/50 text-textPrimary" />
            </div>
            <div>
              <label class="block text-xs font-bold text-textSecondary uppercase mb-1.5">主题色</label>
              <div class="relative group/color">
                <div @click="showColorPicker = !showColorPicker" class="w-full px-4 py-2 pl-10 bg-black/5 border border-border rounded-xl cursor-pointer flex items-center justify-between text-textPrimary hover:bg-black/10 transition-colors">
                  <span>{{ colorOptions.find(o => o.value === platformForm.colorHex)?.label || '选择颜色' }}</span>
                  <div class="absolute left-3 top-2.5 w-4 h-4 rounded-full border border-border transition-transform group-hover/color:scale-110" :style="{ backgroundColor: platformForm.colorHex }"></div>
                  <X v-if="showColorPicker" class="w-3.5 h-3.5 text-textHint" />
                  <div v-else class="w-0 h-0 border-l-[4px] border-l-transparent border-r-[4px] border-r-transparent border-t-[5px] border-t-textHint"></div>
                </div>
                
                <div v-if="showColorPicker" class="absolute z-[60] left-0 right-0 mt-2 p-2 bg-surface border border-border rounded-2xl shadow-xl animate-fade-in-up">
                  <div v-for="opt in colorOptions" :key="opt.value" 
                       @click="platformForm.colorHex = opt.value; showColorPicker = false"
                       class="flex items-center space-x-3 px-3 py-2 rounded-xl hover:bg-black/5 cursor-pointer transition-colors group/item">
                    <div class="w-4 h-4 rounded-full border border-border group-hover/item:scale-110 transition-transform" :style="{ backgroundColor: opt.value }"></div>
                    <span class="text-sm text-textPrimary">{{ opt.label }}</span>
                    <span class="text-[10px] text-textHint font-mono ml-auto opacity-0 group-hover/item:opacity-100">{{ opt.value }}</span>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div>
            <label class="block text-xs font-bold text-textSecondary uppercase mb-1.5">显示排序</label>
            <input v-model.number="platformForm.sortOrder" type="number" class="w-full px-4 py-2 bg-black/5 border border-border rounded-xl focus:outline-none focus:ring-2 focus:ring-primary/50 text-textPrimary" />
          </div>
          <div class="pt-2 flex space-x-3">
            <button type="button" @click="showPlatformModal = false" class="flex-1 px-4 py-2 bg-black/5 text-textPrimary rounded-xl font-medium hover:bg-black/10 transition-colors">取消</button>
            <button type="submit" :disabled="saving" class="flex-1 px-4 py-2 bg-primary text-white rounded-xl font-medium hover:bg-primaryHover transition-colors disabled:opacity-50">
              {{ saving ? '保存中...' : '确定' }}
            </button>
          </div>
        </form>
      </div>
    </div>
    
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { Layers, Plus, Monitor, Folder, Edit2, Trash, FolderTree, Loader2, MonitorOff, X } from 'lucide-vue-next'
import http from '../../api/http'

const categories = ref<any[]>([])
const platforms = ref<any[]>([])
const loadingCategories = ref(true)
const loadingPlatforms = ref(true)
const showColorPicker = ref(false)

const colorOptions = [
  { value: '#0078D6', label: '天空蓝' },
  { value: '#10B981', label: '翠绿' },
  { value: '#8B5CF6', label: '紫罗兰' },
  { value: '#F43F5E', label: '珊瑚红' },
  { value: '#F59E0B', label: '琥珀黄' },
  { value: '#6B7280', label: '石板灰' },
  { value: '#3B82F6', label: '亮湛蓝' },
  { value: '#EC4899', label: '魅惑粉' },
  { value: '#000000', label: '深邃黑' }
]

const fetchCategories = async () => {
  try {
    loadingCategories.value = true
    const res = await http.get('/api/admin/categories')
    if (res.data.code === 200) categories.value = res.data.data
  } finally {
    loadingCategories.value = false
  }
}

const fetchPlatforms = async () => {
  try {
    loadingPlatforms.value = true
    const res = await http.get('/api/admin/platforms')
    if (res.data.code === 200) platforms.value = res.data.data
  } finally {
    loadingPlatforms.value = false
  }
}

const flatCategories = computed(() => {
  const result: any[] = []
  const traverse = (nodes: any[]) => {
    nodes.forEach(node => {
      // Keep essential info for sortOrder calculation too
      result.push({ id: node.id, name: node.name, sortOrder: node.sortOrder })
      if (node.children && node.children.length > 0) {
        traverse(node.children)
      }
    })
  }
  traverse(categories.value)
  return result
})

onMounted(() => {
  fetchCategories()
  fetchPlatforms()
})

const saving = ref(false)

// Category Modal Logic
const showCategoryModal = ref(false)
const isEditingCategory = ref(false)
const categoryForm = ref({
  id: '',
  name: '',
  parentId: '',
  sortOrder: 0
})

const openCategoryModal = (category: any = null) => {
  if (category) {
    isEditingCategory.value = true
    categoryForm.value = { 
      id: category.id, 
      name: category.name, 
      parentId: category.parentId || '', 
      sortOrder: category.sortOrder || 0 
    }
  } else {
    isEditingCategory.value = false
    // Calculate max sortOrder + 1
    const maxSort = flatCategories.value.length > 0 
      ? Math.max(...flatCategories.value.map((c: any) => c.sortOrder || 0)) 
      : 0
    categoryForm.value = { id: '', name: '', parentId: '', sortOrder: maxSort + 1 }
  }
  showCategoryModal.value = true
}

const saveCategory = async () => {
  try {
    saving.value = true
    const url = '/api/admin/categories'
    const res = isEditingCategory.value 
      ? await http.put(`${url}/${categoryForm.value.id}`, categoryForm.value)
      : await http.post(url, categoryForm.value)
    
    if (res.data.code === 200) {
      showCategoryModal.value = false
      fetchCategories()
    } else {
      alert(res.data.message)
    }
  } catch (e: any) {
    alert(e.response?.data?.message || '保存失败')
  } finally {
    saving.value = false
  }
}

// Platform Modal Logic
const showPlatformModal = ref(false)
const isEditingPlatform = ref(false)
const platformForm = ref({
  id: '',
  name: '',
  iconClass: 'Monitor',
  colorHex: '#0078D6',
  sortOrder: 0
})

const openPlatformModal = (platform: any = null) => {
  if (platform) {
    isEditingPlatform.value = true
    platformForm.value = { 
      id: platform.id, 
      name: platform.name, 
      iconClass: platform.iconClass || 'Monitor', 
      colorHex: platform.colorHex || '#0078D6', 
      sortOrder: platform.sortOrder || 0 
    }
  } else {
    isEditingPlatform.value = false
    // Calculate max sortOrder + 1
    const maxSort = platforms.value.length > 0 
      ? Math.max(...platforms.value.map((p: any) => p.sortOrder || 0)) 
      : 0
    platformForm.value = { id: '', name: '', iconClass: 'Monitor', colorHex: '#0078D6', sortOrder: maxSort + 1 }
  }
  showPlatformModal.value = true
}

const savePlatform = async () => {
  try {
    saving.value = true
    const url = '/api/admin/platforms'
    const res = isEditingPlatform.value 
      ? await http.put(`${url}/${platformForm.value.id}`, platformForm.value)
      : await http.post(url, platformForm.value)
    
    if (res.data.code === 200) {
      showPlatformModal.value = false
      fetchPlatforms()
    } else {
      alert(res.data.message)
    }
  } catch (e: any) {
    alert(e.response?.data?.message || '保存失败')
  } finally {
    saving.value = false
  }
}

const deleteCategory = async (id: string) => {
  if (!confirm('确定删除该分类？')) return
  try {
    const res = await http.delete(`/api/admin/categories/${id}`)
    if (res.data.code === 200) fetchCategories()
    else alert(res.data.message)
  } catch (e: any) { alert(e.response?.data?.message || '操作失败') }
}

const deletePlatform = async (id: string) => {
  if (!confirm('确定删除该平台？')) return
  try {
    const res = await http.delete(`/api/admin/platforms/${id}`)
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
