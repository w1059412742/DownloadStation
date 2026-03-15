<template>
  <div class="max-w-6xl mx-auto px-4 py-6 sm:py-8 animate-fade-in-up">
    <!-- 顶部返回 -->
    <div class="mb-4">
      <button @click="$router.push('/')" class="flex items-center text-sm font-medium text-textSecondary hover:text-primary transition-colors group">
        <ArrowLeft class="w-4 h-4 mr-1 group-hover:-translate-x-1 transition-transform" /> 返回首页
      </button>
    </div>

    <div v-if="loading" class="flex justify-center py-20">
       <Loader2 class="w-10 h-10 text-primary animate-spin" />
    </div>
    
    <div v-else-if="software" class="space-y-6">
      <!-- 1. 顶部概览卡片 - 采用超大圆角、细腻阴影和微弱渐变背景 -->
      <section class="bg-gradient-to-b from-white to-blue-50/20 rounded-[2.5rem] p-8 sm:p-10 border border-slate-100 shadow-[0_20px_50px_rgba(0,0,0,0.03)] flex flex-col md:flex-row items-center gap-10 relative overflow-hidden">
        <!-- 装饰性渐变底纹 -->
        <div class="absolute inset-x-0 top-0 h-40 bg-gradient-to-b from-blue-50/50 to-transparent pointer-events-none"></div>
        
        <!-- 左部：图标 - 尺寸从 2xl 减小到 xl -->
        <div class="relative z-10 flex-shrink-0">
          <SoftwareIcon 
            :iconPath="software.iconPath" 
            :platformName="software.platform?.name" 
            size="xl" 
            class="shadow-lg border border-white"
          />
        </div>
        
        <!-- 中部：核心信息 -->
        <div class="flex-1 text-center md:text-left z-10 space-y-3">
          <h1 class="text-3xl sm:text-4xl font-bold text-slate-900 tracking-tight">{{ software.name }}</h1>
          <p class="text-textSecondary text-base sm:text-lg leading-relaxed max-w-2xl">{{ software.summary }}</p>
          
          <div class="flex flex-wrap items-center justify-center md:justify-start gap-2 pt-2">
            <!-- 分类标签 -->
            <span v-if="software.categoryName" class="px-3 py-1 bg-blue-50 text-blue-600 text-xs font-semibold rounded-full border border-blue-100/50">
              {{ software.categoryName }}
            </span>
            <!-- 平台标签 - 移除“专用” -->
            <span v-if="software.platform" class="px-3 py-1 bg-slate-50 text-slate-500 text-xs font-semibold rounded-full border border-slate-100 flex items-center gap-1.5">
              <Monitor class="w-3.5 h-3.5" /> {{ software.platform.name }}
            </span>
            <!-- 业务标签 - 增加显示且提高清晰度 -->
            <template v-if="software.tags && software.tags.length > 0">
              <span v-for="tag in software.tags" :key="tag.id" class="px-2.5 py-1 text-slate-500 text-[11px] font-bold border border-slate-200/60 rounded-lg bg-slate-50/80 shadow-sm">
                #{{ tag.name }}
              </span>
            </template>
          </div>
        </div>

        <!-- 右部：下载动作区 -->
        <div class="flex flex-col items-center md:items-end gap-3 z-10 min-w-[200px]">
          <button @click="triggerLatestDownload" class="w-full sm:w-auto px-10 py-4 bg-gradient-to-r from-blue-600 to-blue-500 hover:from-blue-700 hover:to-blue-600 text-white rounded-2xl text-base font-bold transition-all shadow-lg shadow-blue-200 flex items-center justify-center gap-2 group transform hover:-translate-y-0.5 active:scale-95">
            <Download class="w-5 h-5 group-hover:animate-bounce" /> 立即下载
          </button>
          
          <div class="flex items-center gap-6 text-sm text-slate-400 font-medium pt-1">
            <span class="flex items-center gap-1.5 cursor-default hover:text-slate-600 transition-colors">
              <Download class="w-4 h-4" /> {{ software.totalDownloads }} 次下载
            </span>
            <a v-if="software.officialUrl" :href="software.officialUrl" target="_blank" rel="noopener noreferrer" class="flex items-center gap-1.5 hover:text-blue-500 transition-colors group">
              <ExternalLink class="w-4 h-4" /> 访问原始网站
            </a>
          </div>
        </div>
      </section>

      <!-- 2. 详细介绍区域 - 按照设计图样式排版 -->
      <section class="bg-white rounded-[2rem] border border-slate-100 shadow-sm overflow-hidden min-h-[500px]">
        <div class="p-8 sm:p-10 space-y-8">
          <div class="flex items-center gap-2 mb-2">
            <div class="w-1.5 h-6 bg-blue-600 rounded-full"></div>
            <h3 class="text-xl font-bold text-slate-900">详细介绍</h3>
          </div>

          <div class="prose prose-slate max-w-none prose-img:rounded-3xl prose-img:shadow-xl prose-img:transition-all">
             <!-- 这里的 description 现在存储为 HTML，包含内联样式控制图片 -->
             <div v-html="software.description" class="rich-content-display text-slate-600 leading-relaxed text-lg clear-both overflow-hidden"></div>
          </div>
        </div>
      </section>

      <!-- 3. 历史版本列表 - 紧凑且功能完善 -->
      <section id="versions-area" class="bg-white rounded-[2rem] border border-slate-100 shadow-sm overflow-hidden">
        <div class="p-8 sm:p-10">
          <div class="flex flex-col sm:flex-row sm:items-center justify-between gap-4 mb-8">
            <div class="flex items-center gap-3">
              <div class="p-2.5 bg-blue-50 rounded-xl">
                <Clock class="w-5 h-5 text-blue-600" />
              </div>
              <h3 class="text-xl font-bold text-slate-900">历史版本</h3>
            </div>
            <span class="text-sm font-medium text-slate-400 bg-slate-50 px-4 py-1.5 rounded-full">
              共 {{ versions.length }} 个版本
            </span>
          </div>
          
          <div v-if="versions.length === 0" class="py-16 text-center">
            <div class="inline-flex p-6 bg-slate-50 rounded-full mb-4">
              <Package class="w-12 h-12 text-slate-200" />
            </div>
            <p class="text-slate-400 font-medium">暂无可下载的历史版本</p>
          </div>
          
          <div class="divide-y divide-slate-100">
            <div v-for="ver in versions" :key="ver.id" class="py-6 sm:py-8 first:pt-0 last:pb-0 group">
              <div class="flex flex-col md:flex-row gap-6">
                <!-- 版本元数据 -->
                <div class="flex-1 space-y-4">
                  <div class="flex items-center gap-4">
                    <h4 class="text-xl font-extrabold text-slate-900 group-hover:text-blue-600 transition-colors flex items-center gap-1">
                      v{{ ver.versionNumber }}
                    </h4>
                    <span class="text-sm font-medium text-slate-400">{{ new Date(ver.createdAt).toLocaleDateString() }}</span>
                    <span v-if="ver.hashStatus === 2" class="inline-flex items-center gap-1.5 px-2.5 py-1 bg-green-50 text-green-600 text-[11px] font-bold rounded-lg border border-green-100/50">
                      <ShieldCheck class="w-3.5 h-3.5" /> 已验证
                    </span>
                  </div>
                  
                  <p v-if="ver.changelog" class="text-slate-600 leading-relaxed font-medium">
                    {{ ver.changelog }}
                  </p>
                  
                  <div class="flex flex-wrap items-center gap-x-6 gap-y-2 text-xs font-bold text-slate-400 uppercase tracking-wider">
                    <span class="flex items-center gap-1.5">{{ (ver.fileSize / 1024 / 1024).toFixed(2) }} MB</span>
                    <span v-if="ver.hashSHA256" class="flex items-center gap-1.5 font-mono">
                      SHA256: <span class="text-slate-300 font-normal">{{ ver.hashSHA256.substring(0,8) }}...</span>
                    </span>
                    <span class="flex items-center gap-1.5">
                      <Download class="w-3.5 h-3.5" /> {{ ver.downloadCount }} 次下载
                    </span>
                  </div>
                </div>
                
                <!-- 操作区 -->
                <div class="flex items-center">
                   <button @click="triggerDownload(ver)" class="w-full md:w-auto px-8 py-3 bg-slate-50 hover:bg-blue-600 text-slate-600 hover:text-white rounded-xl text-sm font-bold transition-all flex items-center justify-center gap-2 group/btn border border-slate-100 hover:border-blue-600 shadow-sm active:scale-95">
                      <Download class="w-4 h-4 group-hover/btn:animate-bounce" /> 下载
                   </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>

    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { 
  ArrowLeft, Loader2, Package, Download, 
  ExternalLink, Clock, ShieldCheck, Monitor 
} from 'lucide-vue-next'
import http from '../../api/http'
import SoftwareIcon from '../../components/common/SoftwareIcon.vue'

const route = useRoute()
const software = ref<any>(null)
const versions = ref<any[]>([])
const loading = ref(true)

const fetchData = async () => {
  try {
    const detailRes = await http.get(`/api/softwares/${route.params.id}`)
    if (detailRes.data.code === 200) software.value = detailRes.data.data

    const versionRes = await http.get(`/api/softwares/${route.params.id}/versions`)
    if (versionRes.data.code === 200) {
      // 按照版本创建时间倒序排列，确保最新版本在前
      versions.value = versionRes.data.data.sort((a: any, b: any) => 
        new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()
      )
    }
  } catch (err) {
    console.error('Failed to load detail', err)
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  fetchData()
})

const triggerLatestDownload = () => {
  if (versions.value.length > 0) {
    triggerDownload(versions.value[0])
  }
}

const triggerDownload = (ver: any) => {
  const url = `${http.defaults.baseURL}/api/softwares/${software.value.id}/versions/${ver.id}/download`
  window.open(url, '_blank')
  
  // 乐观更新 UI
  ver.downloadCount++
  if (software.value) software.value.totalDownloads++
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

/* 富文本内容展示优化：支持图片对齐与并排 */
:deep(.rich-content-display img[style*="float: left"]) {
  float: left;
  margin-right: 2rem;
  margin-bottom: 1.5rem;
  max-width: 100%;
}

:deep(.rich-content-display img[style*="float: right"]) {
  float: right;
  margin-left: 2rem;
  margin-bottom: 1.5rem;
  max-width: 100%;
}

:deep(.rich-content-display img[style*="margin-left: auto"]) {
  margin-left: auto;
  margin-right: auto;
  display: block;
  float: none;
}

:deep(.rich-content-display p) {
  margin-bottom: 1.25rem;
}

:deep(.rich-content-display::after) {
  content: "";
  display: table;
  clear: both;
}

/* 隐藏滚动条但保留功能 */
.no-scrollbar::-webkit-scrollbar {
  display: none;
}
.no-scrollbar {
  -ms-overflow-style: none;
  scrollbar-width: none;
}
</style>
