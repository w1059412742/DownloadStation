<template>
  <div class="max-w-4xl mx-auto px-4 py-8 sm:py-12 animate-fade-in-up">
    <!-- Back Button -->
    <button @click="$router.push('/')" class="flex items-center text-sm font-medium text-textSecondary hover:text-primary transition-colors mb-8 group">
      <ArrowLeft class="w-4 h-4 mr-1 group-hover:-translate-x-1 transition-transform" /> 返回首页
    </button>

    <div v-if="loading" class="flex justify-center py-20">
       <Loader2 class="w-10 h-10 text-primary animate-spin" />
    </div>
    
    <div v-else-if="software" class="space-y-12">
      
      <!-- Top Overview Card -->
      <section class="bg-surface rounded-[2rem] p-6 sm:p-10 border border-border shadow-soft flex flex-col md:flex-row gap-8 items-center md:items-start relative overflow-hidden">
        <div class="absolute top-0 right-0 p-8 opacity-5 transform translate-x-1/4 -translate-y-1/4 pointer-events-none">
           <Layers class="w-64 h-64 text-primary" />
        </div>
        
        <div class="w-32 h-32 sm:w-40 sm:h-40 shrink-0 bg-gradient-to-br from-black/5 to-transparent rounded-3xl p-4 flex items-center justify-center border border-border/50 shadow-inner">
           <img v-if="software.iconPath" :src="software.iconPath" class="w-full h-full object-contain drop-shadow-md" />
           <Package v-else class="w-16 h-16 text-textHint" />
        </div>
        
        <div class="flex-1 text-center md:text-left z-10">
           <div class="flex items-center justify-center md:justify-start gap-3 mb-2">
             <span class="px-2.5 py-1 bg-black/5 text-textSecondary text-xs rounded-lg font-medium">{{ software.categoryName || '未分类' }}</span>
             <span class="flex items-center text-xs text-textHint font-medium"><Download class="w-3 h-3 mr-1"/> {{ software.totalDownloads.toLocaleString() }}</span>
           </div>
           
           <h1 class="text-3xl sm:text-4xl font-extrabold text-textPrimary tracking-tight">{{ software.name }}</h1>
           <p class="mt-4 text-textSecondary text-base sm:text-lg leading-relaxed">{{ software.summary }}</p>
           
           <div class="mt-6 flex justify-center md:justify-start">
             <div v-if="software.platform" class="px-4 py-2 rounded-2xl border border-transparent text-sm font-bold text-white flex items-center shadow-soft" :style="{ backgroundColor: software.platform.colorHex || '#9CA3AF' }">
                {{ software.platform.name.toUpperCase() }} 专用安装包
             </div>
             <div v-else class="px-4 py-2 rounded-2xl border border-border bg-surface text-sm font-medium text-textHint italic">
                通用运行平台
             </div>
           </div>
           
           <div class="mt-8 flex flex-col sm:flex-row gap-4 justify-center md:justify-start">
             <a v-if="software.officialUrl" :href="software.officialUrl" target="_blank" rel="noopener noreferrer" class="inline-flex items-center justify-center px-6 py-3 border border-border rounded-xl text-sm font-medium bg-surface hover:bg-black/5 text-textPrimary transition-colors shadow-sm">
                访问官网 <ExternalLink class="w-4 h-4 ml-2 text-textHint" />
             </a>
             <button @click="scrollToVersions" class="inline-flex items-center justify-center px-6 py-3 rounded-xl text-sm font-medium bg-primary text-white hover:bg-primaryHover transition-colors shadow-soft hover:shadow-hover">
                查看下载版本
             </button>
           </div>
        </div>
      </section>

      <!-- Description Area -->
      <section v-if="software.description" class="prose prose-slate max-w-none text-textSecondary px-4 sm:px-0">
         <h3 class="text-xl font-bold text-textPrimary mb-4">详细介绍</h3>
         <!-- 在真实项目中这里需接入 Marked 或者 Markdown 渲染器 -->
         <div class="bg-surface rounded-2xl p-6 sm:p-8 border border-border whitespace-pre-line leading-relaxed">
            {{ software.description }}
         </div>
      </section>

      <!-- Versions Extract Array -->
      <section id="versions-area" class="scroll-mt-24 px-4 sm:px-0">
        <h3 class="text-xl font-bold text-textPrimary mb-6 flex items-center gap-2">
          <Terminal class="w-5 h-5 text-primary" />
          历史版本 ({{ versions.length }})
        </h3>
        
        <div v-if="versions.length === 0" class="bg-surface border border-border border-dashed rounded-2xl p-12 text-center text-textHint">
          暂无可下载的版本。
        </div>
        
        <div class="space-y-4">
          <div v-for="ver in versions" :key="ver.id" class="bg-surface rounded-2xl p-5 sm:p-6 border border-border shadow-sm hover:shadow-soft transition-shadow flex flex-col md:flex-row gap-6 md:items-center">
             <div class="flex-1">
               <div class="flex items-center gap-3">
                 <h4 class="text-lg font-bold font-mono tracking-tight text-textPrimary">v{{ ver.versionNumber }}</h4>
                 <span class="text-xs text-textHint">{{ new Date(ver.createdAt).toLocaleDateString() }}</span>
                 <span v-if="ver.hashStatus === 2" class="px-2 py-0.5 bg-success/10 text-success text-[10px] rounded flex items-center" title="SHA256 校验通过"><ShieldCheck class="w-3 h-3 mr-0.5" /> 已验证</span>
               </div>
               <p v-if="ver.changelog" class="mt-2 text-sm text-textSecondary line-clamp-2">{{ ver.changelog }}</p>
               <div class="mt-3 flex items-center gap-4 text-xs font-mono text-textHint">
                 <span>{{ (ver.fileSize / 1024 / 1024).toFixed(2) }} MB</span>
                 <span v-if="ver.hashSHA256" class="truncate max-w-[120px] sm:max-w-xs" :title="ver.hashSHA256">SHA256: {{ ver.hashSHA256.substring(0,8) }}...</span>
                 <span class="flex items-center"><Download class="w-3 h-3 justify-center mr-1" /> {{ ver.downloadCount }}</span>
               </div>
             </div>
             <div>
                <button @click="triggerDownload(ver)" class="w-full md:w-auto px-5 py-2.5 bg-black/5 hover:bg-primary hover:text-white rounded-xl text-sm font-medium transition-colors flex items-center justify-center text-textPrimary group">
                   <Download class="w-4 h-4 mr-2 group-hover:animate-bounce" /> 下载
                </button>
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
import { ArrowLeft, Loader2, Package, Download, ExternalLink, Layers, Terminal, ShieldCheck } from 'lucide-vue-next'
import axios from 'axios'

const route = useRoute()
const software = ref<any>(null)
const versions = ref<any[]>([])
const loading = ref(true)

const fetchData = async () => {
  try {
    const detailRes = await axios.get(`http://localhost:5000/api/softwares/${route.params.id}`)
    if (detailRes.data.code === 200) software.value = detailRes.data.data

    const versionRes = await axios.get(`http://localhost:5000/api/softwares/${route.params.id}/versions`)
    if (versionRes.data.code === 200) versions.value = versionRes.data.data
  } catch (err) {
    console.error('Failed to load detail', err)
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  fetchData()
})

const scrollToVersions = () => {
  document.getElementById('versions-area')?.scrollIntoView({ behavior: 'smooth' })
}

const triggerDownload = (ver: any) => {
  // 直接利用浏览器的 a 标签下载以获取下载流，并在成功调用接口时更新本体次数。此处简化。
  const url = `http://localhost:5000/api/softwares/${software.value.id}/versions/${ver.id}/download`
  window.open(url, '_blank')
  
  // 假装界面上直接 +1 感官更好
  ver.downloadCount++
  if (software.value) software.value.totalDownloads++
}

</script>

<style scoped>
@keyframes fadeInUp {
  from { opacity: 0; transform: translateY(15px); }
  to { opacity: 1; transform: translateY(0); }
}
.animate-fade-in-up {
  opacity: 0;
  animation: fadeInUp 0.6s cubic-bezier(0.16, 1, 0.3, 1) forwards;
}
</style>
