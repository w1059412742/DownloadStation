<template>
  <div class="space-y-6 animate-fade-in-up p-6 lg:p-10">
    <!-- Header Area -->
    <div class="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4 bg-surface p-6 rounded-2xl border border-border shadow-soft">
      <div>
        <h1 class="text-2xl font-bold tracking-tight text-textPrimary flex items-center gap-2">
          <Box class="w-6 h-6 text-primary" />
          软件管理
        </h1>
        <p class="text-sm text-textSecondary mt-1">管理下载站中的所有软件条目，设置展示状态与详细信息。</p>
      </div>
      <div>
        <button @click="$router.push('/admin/softwares/new')" class="flex items-center px-4 py-2 bg-primary text-white rounded-xl text-sm font-medium hover:bg-primaryHover transition-colors shadow-soft hover:shadow-hover">
          <Plus class="w-4 h-4 mr-2" /> 新增软件
        </button>
      </div>
    </div>

    <!-- Filter & Table Card -->
    <div class="bg-surface rounded-2xl border border-border shadow-soft overflow-hidden">
      <!-- Top Filters -->
      <div class="p-6 border-b border-border bg-black/5 flex flex-col sm:flex-row gap-4 items-center justify-between">
        <div class="relative w-full sm:max-w-xs">
          <input v-model="filters.keyword" @keyup.enter="fetchData" type="text" placeholder="搜索软件名称..." class="w-full pl-10 pr-4 py-2 border border-border bg-surface rounded-xl text-sm focus:outline-none focus:ring-2 focus:ring-primary/50 transition-all text-textPrimary placeholder-textHint" />
          <div class="absolute left-3 top-2.5">
            <Search class="w-4 h-4 text-textHint" />
          </div>
        </div>
        
        <!-- Dropdown Filters -->
        <div class="flex-1 flex flex-col sm:flex-row gap-3">
          <select v-model="filters.platformId" @change="fetchData" class="w-full sm:w-32 px-3 py-2 bg-surface border border-border rounded-xl text-sm focus:outline-none focus:ring-2 focus:ring-primary/50 text-textPrimary appearance-none">
            <option value="">全部平台</option>
            <option v-for="p in platforms" :key="p.id" :value="p.id">{{ p.name }}</option>
          </select>
          
          <select v-model="filters.categoryId" @change="fetchData" class="w-full sm:w-36 px-3 py-2 bg-surface border border-border rounded-xl text-sm focus:outline-none focus:ring-2 focus:ring-primary/50 text-textPrimary appearance-none">
            <option value="">所有分类</option>
            <option v-for="c in flatCategories" :key="c.id" :value="c.id">{{ c.name }}</option>
          </select>
          
          <select v-model="filters.tagId" @change="fetchData" class="w-full sm:w-36 px-3 py-2 bg-surface border border-border rounded-xl text-sm focus:outline-none focus:ring-2 focus:ring-primary/50 text-textPrimary appearance-none">
            <option value="">所有标签</option>
            <option v-for="t in allTags" :key="t.id" :value="t.id">{{ t.name }}</option>
          </select>
        </div>

        <div>
          <button @click="fetchData" class="px-4 py-2 bg-white border border-border rounded-xl text-sm font-medium hover:bg-black/5 transition-colors">
            查询
          </button>
        </div>
      </div>

      <!-- Table Area -->
      <div class="overflow-x-auto relative min-h-[400px]">
        <div v-if="loading" class="absolute inset-0 bg-surface/50 backdrop-blur-sm z-10 flex items-center justify-center">
            <Loader2 class="w-8 h-8 text-primary animate-spin" />
        </div>
        <table class="w-full text-left border-collapse">
          <thead>
            <tr class="bg-black/5 text-xs text-textSecondary uppercase tracking-wider">
              <th class="px-6 py-4 font-semibold">软件信息</th>
              <th class="px-6 py-4 font-semibold">所属平台</th>
              <th class="px-6 py-4 font-semibold">所属分类</th>
              <th class="px-6 py-4 font-semibold">关联标签</th>
              <th class="px-6 py-4 font-semibold">展示状态</th>
              <th class="px-6 py-4 font-semibold">下载次数</th>
              <th class="px-6 py-4 font-semibold text-right">操作</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-border">
            <tr v-if="items.length === 0 && !loading">
              <td colspan="5" class="px-6 py-12 text-center text-textHint text-sm">
                暂无软件记录。
              </td>
            </tr>
            <tr v-for="item in items" :key="item.id" class="hover:bg-black/5 transition-colors group">
              <td class="px-6 py-4">
                <div @click="$router.push(`/admin/softwares/${item.id}`)" class="flex items-center space-x-3 cursor-pointer group/info">
                  <SoftwareIcon 
                    :iconPath="item.iconPath" 
                    :platformName="item.platform?.name" 
                    size="md" 
                  />
                  <div>
                    <h3 class="text-sm font-bold text-textPrimary group-hover/info:text-primary transition-colors">{{ item.name }}</h3>
                    <p class="text-xs text-textHint mt-0.5 line-clamp-1 w-48">{{ item.summary || '无简述内容...' }}</p>
                  </div>
                </div>
              </td>
              <td class="px-6 py-4">
                <span v-if="item.platform" class="inline-flex items-center px-2 py-1 rounded-md text-xs font-medium bg-black/5 text-textSecondary border border-border">
                  <span class="w-1.5 h-1.5 rounded-full mr-1.5" :style="{ backgroundColor: item.platform.colorHex || '#94a3b8' }"></span>
                  {{ item.platform.name }}
                </span>
                <span v-else class="text-xs text-textHint">未指定</span>
              </td>
              <td class="px-6 py-4 text-sm text-textSecondary">
                {{ item.categoryName || '未分配' }}
              </td>
              <td class="px-6 py-4">
                <div class="flex flex-wrap gap-1.5 max-w-[160px]">
                  <span v-for="tag in item.tags" :key="tag.id" 
                        class="inline-flex items-center px-2 py-0.5 rounded text-[10px] bg-black/5 text-textSecondary border border-border"
                        :title="tag.name">
                    <span class="w-1.5 h-1.5 rounded-full mr-1" :style="{ backgroundColor: tag.colorHex || '#94a3b8' }"></span>
                    {{ tag.name }}
                  </span>
                  <span v-if="!item.tags || item.tags.length === 0" class="text-[10px] text-textHint">无</span>
                </div>
              </td>
              <td class="px-6 py-4">
                <span class="inline-flex items-center px-2 py-1 rounded-md text-xs font-medium" 
                      :class="item.status === 1 ? 'bg-success/10 text-success' : 'bg-textHint/10 text-textSecondary'">
                  <span class="w-1.5 h-1.5 rounded-full mr-1.5" :class="item.status === 1 ? 'bg-success' : 'bg-textHint'"></span>
                  {{ item.status === 1 ? '已发布' : '未发布' }}
                </span>
              </td>
              <td class="px-6 py-4 text-sm text-textSecondary font-mono tracking-tight">
                {{ item.totalDownloads.toLocaleString() }} 次
              </td>
              <td class="px-6 py-4 text-right">
                <div class="flex justify-end space-x-2 opacity-0 group-hover:opacity-100 transition-opacity">
                  <button @click="previewSoftware(item.id)" class="p-1.5 bg-surface border border-border text-textSecondary hover:text-primary hover:border-primary/50 rounded-lg transition-colors" title="预览">
                    <ExternalLink class="w-4 h-4" />
                  </button>
                  <button @click="$router.push(`/admin/softwares/${item.id}`)" class="p-1.5 bg-surface border border-border text-textSecondary hover:text-primary hover:border-primary/50 rounded-lg transition-colors" title="编辑">
                    <Edit2 class="w-4 h-4" />
                  </button>
                  <button @click="toggleStatus(item)" class="p-1.5 bg-surface border border-border text-textSecondary hover:text-primary hover:border-primary/50 rounded-lg transition-colors" :title="item.status === 1 ? '下架' : '发布'">
                    <EyeOff v-if="item.status === 1" class="w-4 h-4" />
                    <Eye v-else class="w-4 h-4" />
                  </button>
                  <button @click="deleteSoftware(item.id)" class="p-1.5 bg-surface border border-border text-textSecondary hover:text-danger hover:border-danger/50 rounded-lg transition-colors" title="删除">
                    <Trash2 class="w-4 h-4" />
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      
      <!-- Pagination Placeholder -->
      <div class="p-4 border-t border-border flex justify-between items-center text-sm text-textSecondary">
        <span>共 {{ total }} 条记录，第 {{ filters.page }} / {{ Math.ceil(total / filters.pageSize) || 1 }} 页</span>
        <div class="flex space-x-2">
          <button :disabled="filters.page <= 1" @click="filters.page--; fetchData()" class="px-3 py-1 rounded-md border border-border hover:bg-black/5 disabled:opacity-30 disabled:hover:bg-transparent">上一页</button>
          <button :disabled="filters.page * filters.pageSize >= total" @click="filters.page++; fetchData()" class="px-3 py-1 rounded-md border border-border hover:bg-black/5 disabled:opacity-30 disabled:hover:bg-transparent">下一页</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { Box, Plus, Search, Loader2, Edit2, Eye, EyeOff, Trash2, ExternalLink } from 'lucide-vue-next'
import http from '../../api/http'
import SoftwareIcon from '../../components/common/SoftwareIcon.vue'

const items = ref<any[]>([])
const loading = ref(false)
const total = ref(0)
const filters = ref({
  page: 1,
  pageSize: 20,
  keyword: '',
  platformId: '',
  categoryId: '',
  tagId: ''
})

const platforms = ref<any[]>([])
const categories = ref<any[]>([])
const allTags = ref<any[]>([])

const flatCategories = computed(() => {
  const result: any[] = []
  const traverse = (nodes: any[], depth = 0) => {
    nodes.forEach(node => {
      const prefix = depth > 0 ? '　'.repeat(depth) + '├─ ' : ''
      result.push({ id: node.id, name: prefix + node.name })
      if (node.children && node.children.length > 0) {
        traverse(node.children, depth + 1)
      }
    })
  }
  traverse(categories.value)
  return result
})

const fetchData = async () => {
  loading.value = true
  try {
    const res = await http.get('/api/admin/softwares', {
      params: filters.value
    })
    if (res.data.code === 200) {
      items.value = res.data.data.items
      total.value = res.data.data.totalCount
    }
  } catch (error) {
    console.error('Data fetch failed', error)
  } finally {
    loading.value = false
  }
}

const previewSoftware = (id: string) => {
  window.open(`/software/${id}`, '_blank')
}

const toggleStatus = async (item: any) => {
  const newStatus = item.status === 1 ? 0 : 1
  try {
    const res = await http.patch(`/api/admin/softwares/${item.id}/status`, { status: newStatus })
    if (res.data.code === 200) {
      item.status = newStatus
    }
  } catch (error: any) {
    alert(error.response?.data?.message || '状态切换失败')
  }
}

const deleteSoftware = async (id: string) => {
  if (!confirm('确定删除该软件及其所有版本？')) return
  try {
    const res = await http.delete(`/api/admin/softwares/${id}`)
    if (res.data.code === 200) fetchData()
  } catch (error: any) {
    alert(error.response?.data?.message || '删除失败')
  }
}

const loadDictData = async () => {
  try {
    const [platRes, catRes, tagRes] = await Promise.all([
      http.get('/api/admin/platforms'),
      http.get('/api/admin/categories'),
      http.get('/api/admin/tags')
    ])
    if (platRes.data.code === 200) platforms.value = platRes.data.data
    if (catRes.data.code === 200) categories.value = catRes.data.data
    if (tagRes.data.code === 200) allTags.value = tagRes.data.data
  } catch (error) {
    console.error('Failed to load dictionary data', error)
  }
}

onMounted(async () => {
  await loadDictData()
  fetchData()
})
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
