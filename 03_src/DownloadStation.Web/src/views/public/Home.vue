<template>
  <div class="space-y-12 animate-fade-in-up">
    <!-- Hero Section -->
    <section class="text-center space-y-4 pt-10 pb-6">
      <h1 class="text-4xl md:text-5xl font-extrabold tracking-tight text-textPrimary animate-fade-in-up">
        发现. 下载. <span class="text-primary">掌握.</span>
      </h1>
      <p class="text-lg text-textSecondary max-w-2xl mx-auto animate-fade-in-up" style="animation-delay: 100ms">
        专属软件下载站，汇聚常用工具与安装包，随时随地快速获取。
      </p>
    </section>

    <!-- Content Placeholder / Filter -->
    <section class="max-w-5xl mx-auto space-y-6 mb-10 px-4">
      <!-- Category Filter -->
      <div class="flex items-center space-x-2 overflow-x-auto pb-2 scrollbar-hide">
         <button @click="setCategory('')" :class="['px-4 py-2 rounded-full text-sm font-medium whitespace-nowrap transition-colors', filters.categoryId === '' ? 'bg-primary text-white shadow-soft' : 'bg-surface border border-border text-textSecondary hover:bg-black/5']">
             全部
         </button>
         <button v-for="cat in categories" :key="cat.id" @click="setCategory(cat.id)" :class="['px-4 py-2 rounded-full text-sm font-medium whitespace-nowrap transition-colors', filters.categoryId === cat.id ? 'bg-primary text-white shadow-soft' : 'bg-surface border border-border text-textSecondary hover:bg-black/5']">
            {{ cat.name }}
         </button>
      </div>

      <!-- Platform Filter -->
      <div class="flex flex-wrap items-center justify-between gap-4">
        <div class="flex items-center space-x-2 overflow-x-auto pb-1 scrollbar-hide">
           <button @click="setPlatform('')" :class="['px-4 py-2 rounded-xl text-xs font-bold uppercase tracking-wider transition-all', filters.platformId === '' ? 'bg-textPrimary text-white shadow-soft' : 'bg-surface border border-border text-textHint hover:bg-black/5']">
               全部平台
           </button>
           <button v-for="p in platforms" :key="p.id" @click="setPlatform(p.id)" :class="['px-4 py-2 rounded-xl text-xs font-bold uppercase tracking-wider transition-all border flex items-center', filters.platformId === p.id ? 'bg-white shadow-soft border-transparent' : 'bg-surface border-border text-textSecondary hover:bg-black/5']" :style="filters.platformId === p.id ? { color: p.colorHex || 'var(--color-primary)', borderColor: p.colorHex || 'var(--color-primary)' } : {}">
              <span class="w-2 h-2 rounded-full mr-2" :style="{ backgroundColor: p.colorHex || '#9CA3AF' }"></span>
              {{ p.name }}
           </button>
        </div>
        <div class="relative w-full sm:w-64">
          <input v-model="filters.keyword" @keyup.enter="() => fetchSoftwares()" type="text" placeholder="搜索软件..." class="w-full pl-10 pr-4 py-2 border-none bg-black/5 rounded-full text-sm focus:outline-none focus:ring-2 focus:ring-primary/50 transition-all text-textPrimary placeholder-textHint" />
          <Search class="absolute left-3 top-2.5 w-4 h-4 text-textHint" />
        </div>
      </div>
    </section>

    <!-- Grid Layout -->
    <section class="max-w-7xl mx-auto px-4 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6 relative min-h-[400px]">
      <div v-if="loading" class="absolute inset-0 flex items-center justify-center -top-20">
        <Loader2 class="w-8 h-8 text-primary animate-spin" />
      </div>
      
      <div v-else-if="softwares.length === 0" class="col-span-full text-center py-20">
         <SearchX class="w-16 h-16 text-border mx-auto mb-4" />
         <h3 class="text-xl font-bold text-textPrimary">暂无软件</h3>
         <p class="text-textHint mt-2">试试其他关键词或切换分类。</p>
      </div>

      <div v-for="sw in softwares" :key="sw.id" @click="$router.push(`/software/${sw.id}`)" class="group bg-surface rounded-2xl shadow-soft p-6 border border-border hover:shadow-hover hover:border-primary/30 transition-all duration-300 transform hover:-translate-y-1 cursor-pointer flex flex-col h-full">
        <div class="flex items-center space-x-4 mb-4">
          <div class="w-12 h-12 bg-gradient-to-br from-primary/10 to-transparent rounded-xl flex items-center justify-center overflow-hidden border border-black/5 group-hover:border-primary/20 transition-colors">
            <img v-if="sw.iconPath" :src="sw.iconPath" alt="icon" class="w-8 h-8 object-contain" />
            <Package v-else class="w-6 h-6 text-primary" />
          </div>
          <div class="flex-1 overflow-hidden">
            <h3 class="font-bold text-lg text-textPrimary truncate group-hover:text-primary transition-colors">{{ sw.name }}</h3>
            <p class="text-xs text-textHint truncate mt-0.5">{{ sw.categoryName || '未分类' }}</p>
          </div>
        </div>
        
        <p class="text-sm text-textSecondary line-clamp-2 mt-2 flex-grow min-h-[2.5rem] leading-relaxed">
          {{ sw.summary || '暂无简述' }}
        </p>
        
        <div class="mt-6 flex items-center justify-between pt-4 border-t border-border group-hover:border-primary/20 transition-colors">
          <div v-if="sw.platform" class="flex items-center">
            <div class="px-2.5 py-1 rounded-lg text-[10px] font-bold text-white shadow-sm flex items-center" :style="{ backgroundColor: sw.platform.colorHex || '#9CA3AF' }">
               {{ sw.platform.name.toUpperCase() }}
            </div>
          </div>
          <div v-else class="text-[10px] text-textHint italic">通用</div>
          <div class="flex items-center text-xs text-textHint font-medium gap-1">
            <Download class="w-3.5 h-3.5" />
            <span>{{ sw.totalDownloads > 999 ? (sw.totalDownloads/1000).toFixed(1) + 'k' : sw.totalDownloads }}</span>
          </div>
        </div>
      </div>
    </section>

    <!-- Simple Load More -->
    <div class="text-center pb-20 pt-8" v-if="hasMore && !loading">
       <button @click="loadMore" class="px-6 py-3 bg-surface border border-border rounded-full text-sm font-medium text-textPrimary hover:bg-black/5 shadow-soft transition-colors">
           加载更多
       </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { Search, Loader2, Package, Download, SearchX } from 'lucide-vue-next'
import axios from 'axios'

const softwares = ref<any[]>([])
const categories = ref<any[]>([])
const loading = ref(false)
const hasMore = ref(false)

const filters = ref({
  page: 1,
  pageSize: 12,
  keyword: '',
  categoryId: '',
  platformId: ''
})

const platforms = ref<any[]>([])

const fetchCategories = async () => {
  try {
    const res = await axios.get('http://localhost:5000/api/categories/tree')
    if (res.data.code === 200) {
      categories.value = res.data.data
    }
  } catch (e) { console.error('Failed to load categories') }
}

const fetchPlatforms = async () => {
  try {
    const res = await axios.get('http://localhost:5000/api/platforms')
    if (res.data.code === 200) {
      platforms.value = res.data.data
    }
  } catch (e) { console.error('Failed to load platforms') }
}

const fetchSoftwares = async (append = false) => {
  if (!append) {
    loading.value = true
    filters.value.page = 1
    softwares.value = []
  }
  
  try {
    const res = await axios.get('http://localhost:5000/api/softwares', { params: filters.value })
    if (res.data.code === 200) {
      const { items, totalCount } = res.data.data
      if (append) softwares.value.push(...items)
      else softwares.value = items
      
      hasMore.value = softwares.value.length < totalCount
    }
  } catch (e) {
    console.error('Failed to load softwares')
  } finally {
    loading.value = false
  }
}

const setCategory = (id: string) => {
  filters.value.categoryId = id
  fetchSoftwares()
}

const setPlatform = (id: string) => {
  filters.value.platformId = id
  fetchSoftwares()
}

const loadMore = () => {
  filters.value.page++
  fetchSoftwares(true)
}

onMounted(() => {
  fetchCategories()
  fetchPlatforms()
  fetchSoftwares()
})
</script>

<style scoped>
@keyframes fadeInUp {
  from { opacity: 0; transform: translateY(20px); }
  to { opacity: 1; transform: translateY(0); }
}
.animate-fade-in-up {
  opacity: 0;
  animation: fadeInUp 0.8s cubic-bezier(0.16, 1, 0.3, 1) forwards;
}
</style>
