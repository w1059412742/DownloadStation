<template>
  <div class="space-y-6 animate-fade-in-up">
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
              <th class="px-6 py-4 font-semibold">所属分类</th>
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
                <div class="flex items-center space-x-3">
                  <div class="w-10 h-10 rounded-xl bg-gradient-to-br from-primary/20 to-primary/5 flex items-center justify-center flex-shrink-0 overflow-hidden">
                    <img v-if="item.iconPath" :src="`${apiUrl}${item.iconPath}`" alt="" class="w-full h-full object-cover" />
                    <Package v-else class="w-5 h-5 text-primary" />
                  </div>
                  <div>
                    <h3 class="text-sm font-bold text-textPrimary">{{ item.name }}</h3>
                    <p class="text-xs text-textHint mt-0.5 line-clamp-1 w-48">{{ item.summary || '无简述内容...' }}</p>
                  </div>
                </div>
              </td>
              <td class="px-6 py-4 text-sm text-textSecondary">
                {{ item.categoryName || '未分配' }}
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
                  <button @click="$router.push(`/admin/softwares/${item.id}`)" class="p-1.5 bg-surface border border-border text-textSecondary hover:text-primary hover:border-primary/50 rounded-lg transition-colors" title="编辑">
                    <Edit2 class="w-4 h-4" />
                  </button>
                  <button @click="toggleStatus(item)" class="p-1.5 bg-surface border border-border text-textSecondary hover:text-primary hover:border-primary/50 rounded-lg transition-colors" :title="item.status === 1 ? '下架' : '发布'">
                    <Power class="w-4 h-4" />
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
import { ref, onMounted } from 'vue'
import { Box, Plus, Search, Loader2, Package, Edit2, Power, Trash2 } from 'lucide-vue-next'
import http from '../../api/http'

const apiUrl = import.meta.env.VITE_API_URL || 'http://localhost:5186'

const items = ref<any[]>([])
const loading = ref(false)
const total = ref(0)
const filters = ref({
  page: 1,
  pageSize: 20,
  keyword: ''
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

const toggleStatus = async (item: any) => {
  const newStatus = item.status === 1 ? 0 : 1
  try {
    const res = await http.patch(`/api/admin/softwares/${item.id}/status`, newStatus)
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

onMounted(() => {
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
