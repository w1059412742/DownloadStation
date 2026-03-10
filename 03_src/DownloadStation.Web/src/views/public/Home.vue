<template>
  <div class="animate-fade-in-up">
    <!-- Hero Section -->
    <section class="pt-24 pb-16 px-6">
      <div class="max-w-4xl mx-auto text-center">
        <h1 class="text-4xl md:text-5xl lg:text-6xl font-bold tracking-tight text-foreground mb-6 animate-fade-in-up">
          <span class="text-balance">发现优质软件</span>
        </h1>
        <p class="text-lg md:text-xl text-muted-foreground max-w-2xl mx-auto mb-10 text-pretty animate-fade-in-up" style="animation-delay: 100ms">
          精心挑选的高品质软件合集，提升您的工作效率与创作体验
        </p>
        <!-- SearchBar -->
        <div class="relative max-w-xl mx-auto animate-fade-in-up" style="animation-delay: 200ms">
          <div class="absolute inset-y-0 left-0 pl-5 flex items-center pointer-events-none">
            <Search class="w-5 h-5 text-muted-foreground" />
          </div>
          <input
            type="text"
            v-model="filters.keyword"
            @keyup.enter="() => fetchSoftwares()"
            placeholder="搜索软件名称或功能..."
            class="w-full h-14 pl-14 pr-6 bg-card border border-border rounded-2xl text-foreground placeholder:text-muted-foreground/60 focus:outline-none focus:ring-2 focus:ring-primary/20 focus:border-primary transition-all shadow-card hover:shadow-card-hover"
          />
        </div>
      </div>
    </section>

    <!-- Software Section -->
    <section class="max-w-7xl mx-auto px-6 pb-20">
      
      <!-- Filters (Category & Platform combined for simplicity) -->
      <div class="space-y-6 mb-12">
        <!-- Category Filter -->
        <div class="flex items-center justify-center gap-2 flex-wrap">
          <button
            @click="setCategory('')"
            :class="[
              'inline-flex items-center gap-2 px-5 py-2.5 rounded-full text-sm font-medium transition-all duration-200',
              filters.categoryId === '' ? 'bg-foreground text-card shadow-sm' : 'bg-secondary text-secondary-foreground hover:bg-secondary/80'
            ]"
          >
            <span>全部</span>
          </button>
          <button
            v-for="cat in categories"
            :key="cat.id"
            @click="setCategory(cat.id)"
            :class="[
              'inline-flex items-center gap-2 px-5 py-2.5 rounded-full text-sm font-medium transition-all duration-200',
              filters.categoryId === cat.id ? 'bg-foreground text-card shadow-sm' : 'bg-secondary text-secondary-foreground hover:bg-secondary/80'
            ]"
          >
            <span>{{ cat.name }}</span>
          </button>
        </div>

        <!-- Platform Filter -->
        <div class="flex items-center justify-center gap-2 flex-wrap">
          <button
            @click="setPlatform('')"
            :class="[
              'inline-flex items-center gap-2 px-4 py-2 rounded-full text-xs font-semibold uppercase tracking-wider transition-all duration-200',
              filters.platformId === '' ? 'bg-foreground text-card shadow-sm' : 'bg-secondary text-secondary-foreground hover:bg-secondary/80'
            ]"
          >
            全部平台
          </button>
          <button
            v-for="p in platforms"
            :key="p.id"
            @click="setPlatform(p.id)"
            :class="[
              'inline-flex items-center gap-2 px-4 py-2 rounded-full text-xs font-semibold uppercase tracking-wider transition-all duration-200',
              filters.platformId === p.id ? 'bg-foreground text-card shadow-sm' : 'bg-secondary text-secondary-foreground hover:bg-secondary/80'
            ]"
          >
            {{ p.name }}
          </button>
        </div>
      </div>

      <!-- Grid Layout -->
      <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6 relative min-h-[400px]">
        <div v-if="loading" class="absolute inset-0 flex items-center justify-center -top-20">
          <Loader2 class="w-8 h-8 text-primary animate-spin" />
        </div>
        
        <div v-else-if="softwares.length === 0" class="col-span-full text-center py-20">
           <SearchX class="w-16 h-16 text-muted-foreground/50 mx-auto mb-4" />
           <p class="text-muted-foreground text-lg">未找到匹配的软件</p>
           <p class="text-muted-foreground/60 text-sm mt-2">尝试调整搜索关键词或筛选条件</p>
        </div>

        <article 
          v-for="sw in softwares" 
          :key="sw.id" 
          @click="$router.push(`/software/${sw.id}`)"
          class="group bg-card rounded-2xl border border-border/60 p-5 cursor-pointer flex flex-col transition-all duration-300 hover:shadow-card-hover hover:border-border hover:-translate-y-1 h-full"
        >
          <!-- Header -->
          <div class="flex items-start gap-4 mb-4">
            <div class="w-14 h-14 rounded-xl overflow-hidden bg-secondary flex-shrink-0 shadow-sm flex items-center justify-center">
              <img v-if="sw.iconPath" :src="sw.iconPath" alt="icon" class="w-full h-full object-cover" loading="lazy" />
              <Package v-else class="w-6 h-6 text-primary" />
            </div>
            <div class="flex-1 min-w-0">
              <h3 class="font-semibold text-foreground truncate group-hover:text-primary transition-colors">
                {{ sw.name }}
              </h3>
              <div class="flex items-center gap-2 mt-1">
                <span class="text-xs text-muted-foreground">{{ sw.categoryName || '未分类' }}</span>
                <template v-if="sw.version">
                   <span class="text-muted-foreground/30">|</span>
                   <span class="text-xs text-muted-foreground">v{{ sw.version }}</span>
                </template>
              </div>
            </div>
          </div>

          <!-- Description -->
          <p class="text-sm text-muted-foreground leading-relaxed mb-4 line-clamp-2 flex-grow min-h-[2.5rem]">
            {{ sw.summary || '暂无简述' }}
          </p>

          <!-- Footer -->
          <div class="flex items-center justify-between">
            <div class="flex items-center gap-1.5 text-xs text-muted-foreground">
              <component :is="getPlatformIcon(sw.platform?.name)" class="w-3.5 h-3.5" />
              <span>{{ sw.platform ? sw.platform.name : '通用' }}</span>
            </div>
            <div class="flex items-center gap-1 text-xs text-muted-foreground">
               <Download class="w-3.5 h-3.5" />
               <span>{{ sw.totalDownloads > 999 ? (sw.totalDownloads/1000).toFixed(1) + 'k' : sw.totalDownloads }}</span>
            </div>
          </div>
        </article>
      </div>

      <!-- Simple Load More -->
      <div class="text-center pb-8 pt-12" v-if="hasMore && !loading">
         <button @click="loadMore" class="px-6 py-3 bg-secondary text-secondary-foreground border border-border/60 rounded-full text-sm font-medium hover:bg-secondary/80 shadow-sm hover:shadow transition-all">
             加载更多
         </button>
      </div>

    </section>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { Search, Loader2, Package, Download, SearchX, Monitor, Apple, Laptop } from 'lucide-vue-next'
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

const getPlatformIcon = (platformName?: string) => {
  if (!platformName) return Laptop
  const name = platformName.toLowerCase()
  if (name.includes('windows')) return Monitor
  if (name.includes('mac')) return Apple
  return Laptop
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

.line-clamp-2 {
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}
</style>
