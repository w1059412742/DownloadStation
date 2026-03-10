<template>
  <div class="space-y-6 animate-fade-in-up">
    <!-- Header Area -->
    <div class="flex items-center justify-between pb-4 border-b border-border">
      <div class="flex items-center space-x-4">
        <button @click="$router.push('/admin/softwares')" class="p-2 bg-surface border border-border rounded-xl text-textSecondary hover:text-primary transition-colors">
          <ArrowLeft class="w-5 h-5" />
        </button>
        <div>
          <h1 class="text-2xl font-bold tracking-tight text-textPrimary">
            {{ isNew ? '新增软件' : '编辑软件' }}
          </h1>
          <p class="text-sm text-textSecondary mt-1">
            {{ isNew ? '创建一条新的软件记录。' : '修改软件的基本信息和配置。' }}
          </p>
        </div>
      </div>
      <div>
         <button @click="save" :disabled="saving" class="px-6 py-2 bg-primary text-white rounded-xl text-sm font-medium hover:bg-primaryHover transition-all shadow-soft flex items-center disabled:opacity-50">
            <Save class="w-4 h-4 mr-2" /> {{ saving ? '保存中...' : '保存' }}
         </button>
      </div>
    </div>

    <div v-if="loading" class="flex justify-center py-20">
       <Loader2 class="w-10 h-10 text-primary animate-spin" />
    </div>

    <!-- Form Area -->
    <div v-else class="grid grid-cols-1 lg:grid-cols-3 gap-8">
      <!-- Main Content -->
      <div class="lg:col-span-2 space-y-6">
        <div class="bg-surface rounded-2xl p-6 border border-border shadow-soft space-y-5">
          <h3 class="text-lg font-bold text-textPrimary border-b border-border pb-3">基本信息</h3>
          
          <div>
            <label class="block text-sm font-medium text-textSecondary mb-1.5">软件名称 <span class="text-danger">*</span></label>
            <input v-model="form.name" type="text" class="w-full px-4 py-2 bg-black/5 border border-transparent focus:border-primary/50 rounded-xl text-sm focus:outline-none transition-colors text-textPrimary placeholder-textHint" placeholder="例如：IntelliJ IDEA Ultimate" />
          </div>

          <div>
            <label class="block text-sm font-medium text-textSecondary mb-1.5">简述</label>
            <input v-model="form.summary" type="text" class="w-full px-4 py-2 bg-black/5 border border-transparent focus:border-primary/50 rounded-xl text-sm focus:outline-none transition-colors text-textPrimary placeholder-textHint" placeholder="例如：领先的 Java 和 Kotlin IDE" />
          </div>

          <div>
            <label class="block text-sm font-medium text-textSecondary mb-1.5">详细描述</label>
            <textarea v-model="form.description" rows="10" class="w-full px-4 py-3 bg-black/5 border border-transparent focus:border-primary/50 rounded-xl text-sm focus:outline-none transition-colors text-textPrimary placeholder-textHint font-mono mt-1" placeholder="在这里输入详细描述，支持 Markdown 格式..."></textarea>
          </div>
        </div>

        <div v-if="!isNew" class="bg-surface rounded-2xl p-6 border border-border shadow-soft space-y-5">
           <div class="flex items-center justify-between border-b border-border pb-3">
              <h3 class="text-lg font-bold text-textPrimary flex items-center">
                <Terminal class="w-5 h-5 mr-2 text-primary" /> 版本列表
              </h3>
              <!-- 此处在真实应用中可以提供一个按钮直接弹出文件选择器 -->
           </div>
           
           <div v-if="versions.length === 0" class="text-center py-8 text-textHint text-sm border-2 border-dashed border-border rounded-xl">
              暂无版本记录。请前往「文件扫描」关联文件。
           </div>
           
           <div v-else class="space-y-3">
              <div v-for="ver in versions" :key="ver.id" class="p-4 bg-black/5 rounded-xl flex items-center justify-between group border border-transparent hover:border-border transition-colors">
                 <div>
                    <h4 class="font-bold text-sm text-textPrimary font-mono">v{{ ver.versionNumber }}</h4>
                    <p class="text-xs text-textSecondary mt-1 line-clamp-1">{{ ver.fileName }}</p>
                 </div>
                 <div class="flex items-center space-x-2 text-xs">
                    <span v-if="ver.hashStatus === 2" class="text-success flex items-center"><ShieldCheck class="w-3 h-3 mr-1" />校验完毕</span>
                    <span v-else-if="ver.hashStatus === 1" class="text-primary flex items-center"><Loader2 class="w-3 h-3 mr-1 animate-spin" />计算中</span>
                    <span v-else class="text-textHint flex items-center"><Clock class="w-3 h-3 mr-1" />等待中</span>
                 </div>
                 <div class="opacity-0 group-hover:opacity-100 transition-opacity">
                    <button @click="deleteVersion(ver.id)" class="p-1.5 text-textHint hover:text-danger rounded-lg transition-colors"><Trash class="w-4 h-4" /></button>
                 </div>
              </div>
           </div>
        </div>
      </div>

      <!-- Sidebar Content -->
      <div class="space-y-6">
        <div class="bg-surface rounded-2xl p-6 border border-border shadow-soft space-y-5">
          <h3 class="text-lg font-bold text-textPrimary border-b border-border pb-3">其他配置</h3>
          
          <div>
            <label class="block text-sm font-medium text-textSecondary mb-1.5">官方网址</label>
            <input v-model="form.officialUrl" type="url" class="w-full px-3 py-2 bg-black/5 border border-transparent focus:border-primary/50 rounded-xl text-sm focus:outline-none transition-colors text-textPrimary" placeholder="https://..." />
          </div>

          <div>
            <label class="block text-sm font-medium text-textSecondary mb-1.5">所属分类</label>
            <select v-model="form.categoryId" class="w-full px-3 py-2 bg-black/5 border border-transparent focus:border-primary/50 rounded-xl text-sm focus:outline-none transition-colors text-textPrimary appearance-none">
              <option value="">-- 未分类 --</option>
              <option v-for="cat in categories" :key="cat.id" :value="cat.id">{{ cat.name }}</option>
            </select>
          </div>

          <div>
            <label class="block text-sm font-medium text-textSecondary mb-1.5 line-clamp-1">运行平台 <span class="text-danger">*</span></label>
            <div class="grid grid-cols-2 gap-2 mt-2">
               <label v-for="p in platforms" :key="p.id" :class="['flex items-center cursor-pointer px-3 py-2 rounded-xl border transition-all', form.platformId === p.id ? 'bg-white border-transparent shadow-soft ring-1 ring-black/5' : 'bg-black/5 border-transparent hover:bg-black/10']">
                  <input type="radio" :value="p.id" v-model="form.platformId" class="sr-only" />
                  <span class="w-2.5 h-2.5 rounded-full mr-2 shrink-0" :style="{ backgroundColor: p.colorHex || '#9CA3AF' }"></span>
                  <span :class="['text-xs font-bold truncate', form.platformId === p.id ? 'text-textPrimary' : 'text-textSecondary']">{{ p.name }}</span>
               </label>
            </div>
            <p v-if="!form.platformId" class="text-[10px] text-danger mt-2 italic">* 请选择软件所属平台</p>
          </div>
        </div>

        <!-- NEW: Upload Package Card -->
        <div v-if="!isNew" class="bg-surface rounded-2xl p-6 border border-border shadow-soft space-y-5">
           <h3 class="text-lg font-bold text-textPrimary border-b border-border pb-3 flex items-center">
             <UploadCloud class="w-5 h-5 mr-2 text-primary" /> 上传新版本
           </h3>
           
           <div class="space-y-4">
              <div class="grid grid-cols-2 gap-4">
                <div>
                  <label class="block text-[10px] font-bold text-textHint uppercase mb-1">版本号</label>
                  <input v-model="uploadForm.versionNumber" type="text" class="w-full px-3 py-2 bg-black/5 border border-transparent focus:border-primary/50 rounded-lg text-sm focus:outline-none" placeholder="1.0.0" />
                </div>
                <div>
                  <label class="block text-[10px] font-bold text-textHint uppercase mb-1">安装包文件</label>
                  <label class="flex items-center justify-center px-3 py-2 bg-black/5 hover:bg-black/10 rounded-lg cursor-pointer transition-colors border border-dashed border-border group">
                    <input type="file" class="sr-only" @change="onFileChange" />
                    <Paperclip v-if="!uploadFile" class="w-4 h-4 text-textHint group-hover:text-primary transition-colors" />
                    <span class="text-xs text-textSecondary truncate max-w-[80px] ml-1">{{ uploadFile ? uploadFile.name : '选择文件' }}</span>
                  </label>
                </div>
              </div>
              
              <div>
                <label class="block text-[10px] font-bold text-textHint uppercase mb-1">更新日志</label>
                <textarea v-model="uploadForm.changelog" rows="3" class="w-full px-3 py-2 bg-black/5 border border-transparent focus:border-primary/50 rounded-lg text-sm focus:outline-none resize-none" placeholder="输入更新亮点..."></textarea>
              </div>

              <div v-if="uploading" class="h-1.5 w-full bg-black/5 rounded-full overflow-hidden">
                 <div class="h-full bg-primary transition-all duration-300" :style="{ width: uploadProgress + '%' }"></div>
              </div>

              <button @click="handleUpload" :disabled="uploading || !uploadFile || !uploadForm.versionNumber" class="w-full py-2.5 bg-textPrimary text-white rounded-xl text-xs font-bold hover:bg-black transition-all flex items-center justify-center disabled:opacity-30">
                 <UploadCloud class="w-4 h-4 mr-2" /> {{ uploading ? `上传中 ${uploadProgress}%` : '开始上传安装包' }}
              </button>
           </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed, reactive } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { ArrowLeft, Save, Loader2, Terminal, ShieldCheck, Clock, Trash, UploadCloud, Paperclip } from 'lucide-vue-next'
import axios from 'axios'

const route = useRoute()
const router = useRouter()
const isNew = computed(() => route.params.id === 'new')

const loading = ref(false)
const saving = ref(false)

const form = ref({
  name: '',
  summary: '',
  description: '',
  officialUrl: '',
  categoryId: '',
  platformId: ''
})

const uploadForm = reactive({
  versionNumber: '',
  changelog: ''
})
const uploadFile = ref<File | null>(null)
const uploading = ref(false)
const uploadProgress = ref(0)

const categories = ref<any[]>([])
const platforms = ref<any[]>([])
const versions = ref<any[]>([])

const getToken = () => ({ headers: { Authorization: `Bearer ${localStorage.getItem('admin_token')}` } })

const fetchData = async () => {
  loading.value = true
  try {
    // 拉取字典数据
    const [catRes, platRes] = await Promise.all([
      axios.get('http://localhost:5000/api/admin/categories', getToken()),
      axios.get('http://localhost:5000/api/admin/platforms', getToken())
    ])
    if (catRes.data.code === 200) categories.value = catRes.data.data
    if (platRes.data.code === 200) platforms.value = platRes.data.data

    if (!isNew.value) {
      // 拉取详情
      const detailRes = await axios.get(`http://localhost:5000/api/admin/softwares/${route.params.id}`, getToken())
      if (detailRes.data.code === 200) {
        const d = detailRes.data.data
        form.value = {
          name: d.name,
          summary: d.summary || '',
          description: d.description || '',
          officialUrl: d.officialUrl || '',
          categoryId: d.categoryId || '',
          platformId: d.platform?.id || ''
        }
      }

      // 拉取版本信息用于附属面板
      const verRes = await axios.get(`http://localhost:5000/api/admin/versions/software/${route.params.id}`, getToken())
      if (verRes.data.code === 200) {
         versions.value = verRes.data.data
      }
    }
  } catch (error) {
    console.error(error)
    alert('数据加载失败，请稍后重试。')
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  fetchData()
})

const save = async () => {
  if (!form.value.name) return alert('软件名称为必填项')
  
  saving.value = true
  try {
    if (isNew.value) {
      await axios.post('http://localhost:5000/api/admin/softwares', form.value, getToken())
      alert('软件创建成功！')
      router.push('/admin/softwares')
    } else {
      await axios.put(`http://localhost:5000/api/admin/softwares/${route.params.id}`, form.value, getToken())
      alert('保存成功！')
    }
  } catch (error: any) {
    alert(error.response?.data?.message || '操作失败')
  } finally {
    saving.value = false
  }
}

const deleteVersion = async (id: string) => {
   if (!confirm('确定删除该版本及其关联文件？')) return
   try {
     const res = await axios.delete(`http://localhost:5000/api/admin/versions/${id}?physicalDelete=true`, getToken())
     if (res.data.code === 200) {
        versions.value = versions.value.filter(v => v.id !== id)
     }
   } catch(e) { alert('删除失败') }
}

const onFileChange = (e: any) => {
  const file = e.target.files[0]
  if (file) uploadFile.value = file
}

const handleUpload = async () => {
  if (!uploadFile.value || !uploadForm.versionNumber) return

  uploading.value = true
  uploadProgress.value = 0
  
  const formData = new FormData()
  formData.append('file', uploadFile.value)
  formData.append('softwareId', route.params.id as string)
  formData.append('versionNumber', uploadForm.versionNumber)
  formData.append('changelog', uploadForm.changelog)

  try {
    const res = await axios.post('http://localhost:5000/api/admin/versions/upload', formData, {
      ...getToken(),
      onUploadProgress: (progressEvent) => {
        if (progressEvent.total) {
          uploadProgress.value = Math.round((progressEvent.loaded * 100) / progressEvent.total)
        }
      }
    })

    if (res.data.code === 200) {
       // 重置上传表单
       uploadFile.value = null
       uploadForm.versionNumber = ''
       uploadForm.changelog = ''
       // 刷新版本列表
       const verRes = await axios.get(`http://localhost:5000/api/admin/versions/software/${route.params.id}`, getToken())
       if (verRes.data.code === 200) versions.value = verRes.data.data
       alert('安装包上传成功！')
    }
  } catch (error: any) {
    alert(error.response?.data?.message || '上传失败')
  } finally {
    uploading.value = false
    uploadProgress.value = 0
  }
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
