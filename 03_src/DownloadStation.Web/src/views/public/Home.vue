<template>
  <div class="animate-fade-in-up">
    <!-- Hero Section -->
    <section class="pt-4 pb-4 px-6 md:pt-6 md:pb-6">
      <div class="max-w-3xl mx-auto text-center">
        <h1 class="text-2xl md:text-3xl lg:text-4xl font-bold tracking-tight text-foreground mb-2 animate-fade-in-up">
          <span class="text-balance">收藏优质软件</span>
        </h1>
        <p class="text-sm md:text-base text-muted-foreground max-w-xl mx-auto mb-4 text-pretty animate-fade-in-up" style="animation-delay: 100ms">
          您的个人应用私藏馆，高效收纳与发现每一份优质工具
        </p>
        <!-- SearchBar -->
        <div class="relative max-w-xl mx-auto animate-fade-in-up" style="animation-delay: 200ms">
          <div class="absolute inset-y-0 left-0 pl-4 flex items-center pointer-events-none">
            <Search class="w-5 h-5 text-muted-foreground" />
          </div>
          <input
            type="text"
            v-model="filters.keyword"
            @keyup.enter="() => fetchSoftwares()"
            placeholder="搜寻您的私藏工具..."
            class="w-full h-11 pl-11 pr-5 bg-card border border-border rounded-xl text-foreground placeholder:text-muted-foreground/60 focus:outline-none focus:ring-2 focus:ring-primary/20 focus:border-primary transition-all shadow-sm hover:shadow-md"
          />
        </div>
      </div>
    </section>

    <!-- Software Section -->
    <section class="max-w-7xl mx-auto px-6 pb-12">
      
      <!-- Filters -->
      <div class="space-y-4 mb-8">
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

        <!-- Parent Category Filter -->
        <div class="flex items-center justify-center gap-2 flex-wrap pb-2 border-b border-border/40">
          <button
            @click="setParentCategory('')"
            :class="[
              'inline-flex items-center gap-2 px-4 py-2 rounded-full text-sm font-medium transition-all duration-200',
              selectedParentId === '' ? 'bg-foreground text-card shadow-sm' : 'bg-secondary text-secondary-foreground hover:bg-secondary/80'
            ]"
          >
            <span>全部类型</span>
          </button>
          <button
            v-for="cat in categories"
            :key="cat.id"
            @click="setParentCategory(cat.id)"
            :class="[
              'inline-flex items-center gap-2 px-4 py-2 rounded-full text-sm font-medium transition-all duration-200',
              selectedParentId === cat.id ? 'bg-foreground text-card shadow-sm' : 'bg-secondary text-secondary-foreground hover:bg-secondary/80'
            ]"
          >
            <span>{{ cat.name }}</span>
          </button>
        </div>

        <!-- Sub Category Filter (Second Line) -->
        <transition 
          enter-active-class="transition duration-300 ease-out"
          enter-from-class="transform -translate-y-2 opacity-0"
          enter-to-class="transform translate-y-0 opacity-100"
          leave-active-class="transition duration-200 ease-in"
          leave-from-class="transform translate-y-0 opacity-100"
          leave-to-class="transform -translate-y-2 opacity-0"
        >
          <div v-if="subCategories.length > 0" class="flex items-center justify-center gap-2 flex-wrap pt-2 animate-fade-in-down">
            <button
              @click="setSubCategory('')"
              :class="[
                'inline-flex items-center gap-2 px-3 py-1.5 rounded-full text-xs font-medium transition-all duration-200',
                filters.categoryId === selectedParentId ? 'bg-primary text-primary-foreground shadow-sm' : 'bg-primary/5 text-primary/80 hover:bg-primary/10'
              ]"
            >
              <span>不限子类</span>
            </button>
            <button
              v-for="sub in subCategories"
              :key="sub.id"
              @click="setSubCategory(sub.id)"
              :class="[
                'inline-flex items-center gap-2 px-3 py-1.5 rounded-full text-xs font-medium transition-all duration-200',
                filters.categoryId === sub.id ? 'bg-primary text-primary-foreground shadow-sm' : 'bg-primary/5 text-primary/80 hover:bg-primary/10'
              ]"
            >
              <span>{{ sub.name }}</span>
            </button>
          </div>
        </transition>
      </div>

      <!-- Grid Layout -->
      <div class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-3.5 relative min-h-[300px]">
        <div v-if="loading" class="absolute inset-0 flex items-center justify-center -top-10">
          <Loader2 class="w-8 h-8 text-primary animate-spin" />
        </div>
        
        <div v-else-if="softwares.length === 0" class="col-span-full text-center py-12">
           <SearchX class="w-16 h-16 text-muted-foreground/50 mx-auto mb-4" />
           <p class="text-muted-foreground text-lg">未找到匹配的软件</p>
           <p class="text-muted-foreground/60 text-sm mt-2">尝试调整搜索关键词或筛选条件</p>
        </div>

        <article 
          v-for="sw in softwares" 
          :key="sw.id" 
          @click="$router.push(`/software/${sw.id}`)"
          class="group bg-card rounded-xl border border-border px-3.5 py-3 cursor-pointer flex flex-col transition-all duration-300 hover:shadow-card-hover hover:border-primary/40 hover:-translate-y-0.5 aspect-[6/6]"
        >
          <!-- Header -->
          <div class="flex items-center gap-3 mb-2.5 shrink-0">
            <SoftwareIcon 
              :iconPath="sw.iconPath" 
              :platformName="sw.platform?.name" 
              size="base" 
            />
            <div class="flex-1 min-w-0">
              <h3 class="text-[15px] font-bold text-foreground line-clamp-2 group-hover:text-primary transition-colors leading-tight mb-1">
                {{ sw.name }}
              </h3>
              <div class="flex items-center gap-1.5">
                <span class="text-xs text-muted-foreground truncate">{{ sw.categoryName || '未分类' }}</span>
                <template v-if="sw.version">
                   <span class="text-muted-foreground/30 text-[10px] leading-none">|</span>
                   <span class="text-[11px] text-muted-foreground font-medium truncate">v{{ sw.version }}</span>
                </template>
              </div>
            </div>
          </div>

          <!-- Description -->
          <div class="flex-grow relative overflow-hidden mb-2.5 mt-0.5">
            <div class="absolute inset-0">
              <p v-if="sw.summary" class="text-[13px] text-muted-foreground/90 font-semibold mb-2.5 truncate">
                {{ sw.summary }}
              </p>
              <p class="text-[11px] text-muted-foreground/60 leading-[1.4] break-words">
                {{ sw.description?.replace(/<[^>]+>/g, '') || '暂无详细描述' }}
              </p>
            </div>
            <!-- 底部渐变遮盖，避免文字被生硬切断 -->
            <div class="absolute inset-x-0 bottom-0 h-6 bg-gradient-to-t from-card to-transparent pointer-events-none"></div>
          </div>

          <!-- Footer -->
          <div class="flex items-center justify-between pt-2 border-t border-border/15 shrink-0">
            <div class="flex items-center gap-1.5 text-xs text-muted-foreground font-medium">
              <component :is="getPlatformIcon(sw.platform?.name)" class="w-3.5 h-3.5" />
              <span>{{ sw.platform ? sw.platform.name : '通用' }}</span>
            </div>
            <div class="flex items-center gap-1 text-xs text-muted-foreground font-medium">
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
import { ref, onMounted, computed } from 'vue'
import { Search, Loader2, Download, SearchX, Monitor, Apple, Laptop } from 'lucide-vue-next'
import http from '../../api/http'
import SoftwareIcon from '../../components/common/SoftwareIcon.vue'

const softwares = ref<any[]>([])
const categories = ref<any[]>([])
const loading = ref(false)
const hasMore = ref(false)
const totalCount = ref(0)
const selectedParentId = ref('')

const filters = ref({
  page: 1,
  pageSize: 15,
  keyword: '',
  categoryId: '',
  platformId: ''
})

const platforms = ref<any[]>([])

const fetchCategories = async () => {
  try {
    const res = await http.get('/api/categories/tree')
    if (res.data.code === 200) {
      categories.value = res.data.data
    }
  } catch (e) { console.error('Failed to load categories') }
}

const subCategories = computed(() => {
  if (!selectedParentId.value) return []
  const parent = categories.value.find(c => c.id === selectedParentId.value)
  return parent?.children || []
})

const fetchPlatforms = async () => {
  try {
    const res = await http.get('/api/platforms')
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
    const res = await http.get('/api/softwares', { params: filters.value })
    if (res.data.code === 200) {
      const { items, totalCount: count } = res.data.data
      if (append) softwares.value.push(...items)
      else softwares.value = items
      
      totalCount.value = count
      hasMore.value = softwares.value.length < count
    }
  } catch (e) {
    console.error('Failed to load softwares')
  } finally {
    loading.value = false
  }
}

const setParentCategory = (id: string) => {
  selectedParentId.value = id
  filters.value.categoryId = id // 如果选了父类且没选子类，搜索父类下的所有
  fetchSoftwares()
}

const setSubCategory = (id: string) => {
  if (id === '') {
    filters.value.categoryId = selectedParentId.value
  } else {
    filters.value.categoryId = id
  }
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

.animate-fade-in-down {
  animation: fadeInDown 0.3s ease-out;
}

@keyframes fadeInDown {
  from { opacity: 0; transform: translateY(-10px); }
  to { opacity: 1; transform: translateY(0); }
}

.line-clamp-1 {
  display: -webkit-box;
  -webkit-line-clamp: 1;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

.line-clamp-2 {
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}
</style>
