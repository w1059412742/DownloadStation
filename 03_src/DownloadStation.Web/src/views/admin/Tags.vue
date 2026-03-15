<template>
  <div class="space-y-6 p-6 lg:p-10 font-sans">
    <!-- 页面标题与操作 -->
    <div class="flex flex-col md:flex-row md:items-center justify-between gap-4">
      <div>
        <h1 class="text-2xl font-bold bg-gradient-to-r from-textPrimary to-textSecondary bg-clip-text text-transparent">
          标签管理
        </h1>
        <p class="text-textSecondary text-sm mt-1">管理软件关联的多维度标签，支持自定义颜色区分。</p>
      </div>
      <button 
        @click="openTagModal()"
        class="flex items-center px-4 py-2 bg-primary text-white rounded-xl hover:bg-primary/90 transition-all duration-300 shadow-lg shadow-primary/20 hover:-translate-y-0.5"
      >
        <Plus class="w-4 h-4 mr-2" />
        新增标签
      </button>
    </div>

    <!-- 标签列表 -->
    <div v-if="loading" class="flex flex-col items-center justify-center py-20 space-y-4">
      <div class="w-10 h-10 border-4 border-primary/20 border-t-primary rounded-full animate-spin"></div>
      <p class="text-textSecondary animate-pulse">正在加载标签数据...</p>
    </div>

    <div v-else-if="tags.length === 0" class="flex flex-col items-center justify-center py-20 bg-surface rounded-3xl border border-border border-dashed">
      <div class="w-16 h-16 bg-black/5 rounded-full flex items-center justify-center mb-4">
        <TagIcon class="w-8 h-8 text-textHint" />
      </div>
      <p class="text-textSecondary">暂无标签项</p>
      <button @click="openTagModal()" class="mt-4 text-primary font-medium hover:underline">立即创建第一个标签</button>
    </div>

    <div v-else class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4">
      <div 
        v-for="tag in tags" 
        :key="tag.id"
        class="group bg-surface p-4 rounded-2xl border border-border hover:border-primary/30 transition-all duration-300 hover:shadow-xl hover:shadow-primary/5 flex items-center justify-between"
      >
        <div class="flex items-center space-x-3">
          <div 
            class="w-3 h-3 rounded-full shadow-sm"
            :style="{ backgroundColor: tag.colorHex || '#94a3b8' }"
          ></div>
          <span class="font-medium text-textPrimary">{{ tag.name }}</span>
        </div>
        
        <div class="flex items-center opacity-0 group-hover:opacity-100 transition-opacity space-x-1">
          <button 
            @click="openTagModal(tag)"
            class="p-2 text-textSecondary hover:text-primary hover:bg-primary/10 rounded-lg transition-colors"
            title="编辑"
          >
            <Pencil class="w-4 h-4" />
          </button>
          <button 
            @click="deleteTag(tag)"
            class="p-2 text-textSecondary hover:text-danger hover:bg-danger/10 rounded-lg transition-colors"
            title="删除"
          >
            <Trash2 class="w-4 h-4" />
          </button>
        </div>
      </div>
    </div>

    <!-- 标签编辑弹窗 -->
    <div v-if="showModal" class="fixed inset-0 z-50 flex items-center justify-center p-4">
      <div class="absolute inset-0 bg-black/40 backdrop-blur-sm" @click="showModal = false"></div>
      <div class="bg-surface w-full max-w-md rounded-3xl border border-border shadow-2xl relative overflow-hidden animate-in fade-in zoom-in duration-300">
        <div class="px-6 py-4 border-b border-border flex items-center justify-between bg-black/[0.02]">
          <h3 class="text-lg font-bold">{{ isEditing ? '编辑标签' : '新增标签' }}</h3>
          <button @click="showModal = false" class="p-2 hover:bg-black/5 rounded-full transition-colors text-textHint">
            <X class="w-5 h-5" />
          </button>
        </div>

        <form @submit.prevent="saveTag" class="p-6 space-y-4">
          <div>
            <label class="block text-xs font-bold text-textSecondary uppercase mb-1.5">标签名称</label>
            <input 
              v-model="tagForm.name"
              type="text"
              placeholder="例如：开源、热门"
              class="w-full px-4 py-2 bg-black/5 border border-border rounded-xl focus:outline-none focus:ring-2 focus:ring-primary/50 text-textPrimary"
              required
            />
          </div>

          <div>
            <label class="block text-xs font-bold text-textSecondary uppercase mb-1.5">颜色标识</label>
            <div class="flex items-center space-x-3">
              <input 
                v-model="tagForm.colorHex"
                type="color"
                class="w-10 h-10 p-0 border-0 bg-transparent cursor-pointer rounded-lg overflow-hidden"
              />
              <input 
                v-model="tagForm.colorHex"
                type="text"
                placeholder="#000000"
                class="flex-1 px-4 py-2 bg-black/5 border border-border rounded-xl focus:outline-none focus:ring-2 focus:ring-primary/50 text-textPrimary font-mono uppercase"
              />
            </div>
          </div>

          <div class="flex justify-end space-x-3 pt-4">
            <button 
              type="button"
              @click="showModal = false"
              class="px-5 py-2 text-sm font-medium text-textSecondary hover:bg-black/5 rounded-xl transition-colors"
            >
              取消
            </button>
            <button 
              type="submit"
              :disabled="saving"
              class="flex items-center px-6 py-2 bg-primary text-white text-sm font-bold rounded-xl hover:bg-primary/90 transition-all disabled:opacity-50"
            >
              <Loader2 v-if="saving" class="w-4 h-4 mr-2 animate-spin" />
              {{ saving ? '保存中...' : '提交确认' }}
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { Plus, Tag as TagIcon, Pencil, Trash2, X, Loader2 } from 'lucide-vue-next'
import http from '../../api/http'

interface Tag {
  id: string
  name: string
  colorHex: string | null
}

const tags = ref<Tag[]>([])
const loading = ref(true)
const saving = ref(false)
const showModal = ref(false)
const isEditing = ref(false)

const tagForm = ref({
  id: '',
  name: '',
  colorHex: '#3b82f6'
})

const fetchTags = async () => {
  loading.value = true

  try {
    const res = await http.get('/api/admin/tags')
    if (res.data.code === 200) {
      tags.value = res.data.data
    }
  } catch (error) {
    console.error('Fetch tags failed:', error)
  } finally {
    loading.value = false
  }
}

const openTagModal = (tag?: Tag) => {
  if (tag) {
    isEditing.value = true
    tagForm.value = { 
      id: tag.id, 
      name: tag.name, 
      colorHex: tag.colorHex || '#3b82f6' 
    }
  } else {
    isEditing.value = false
    tagForm.value = { id: '', name: '', colorHex: '#3b82f6' }
  }
  showModal.value = true
}

const saveTag = async () => {
  saving.value = true
  try {
    const url = isEditing.value ? `/api/admin/tags/${tagForm.value.id}` : '/api/admin/tags'
    const method = isEditing.value ? 'put' : 'post'
    
    const res = await http[method](url, {
      name: tagForm.value.name,
      colorHex: tagForm.value.colorHex
    })

    if (res.data.code === 200) {
      showModal.value = false
      await fetchTags()
    } else {
      alert(res.data.message || '操作失败')
    }
  } catch (error: any) {
    alert(error.response?.data?.message || '网络错误，保存失败')
  } finally {
    saving.value = false
  }
}

const deleteTag = async (tag: Tag) => {
  if (!confirm(`确定要彻底删除标签 "${tag.name}" 吗？该操作无法撤销。`)) return
  
  try {
    const res = await http.delete(`/api/admin/tags/${tag.id}`)
    if (res.data.code === 200) {
      await fetchTags()
    }
  } catch (error) {
    alert('删除失败，请稍后重试')
  }
}

onMounted(() => {
  fetchTags()
})
</script>
