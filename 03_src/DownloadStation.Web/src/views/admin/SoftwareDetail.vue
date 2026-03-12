<template>
  <div class="animate-fade-in-up">
    <!-- Header Area: 粘性定位，使用负边距并内补填充抵消父容器 padding，避免空白间隙 -->
    <div class="flex items-center justify-between pb-3 pt-3 border-b border-border sticky top-0 z-30 bg-surface backdrop-blur-none -mx-6 lg:-mx-10 px-6 lg:px-10">
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
            <Save class="w-4 h-4 mr-2" /> 
            <span v-if="saving && uploadProgress > 0 && uploadProgress < 100">上传中 {{ uploadProgress }}%</span>
            <span v-else-if="saving">保存中...</span>
            <span v-else>{{ (isNew && uploadFile) ? '保存并上传' : '保存' }}</span>
         </button>
      </div>
    </div>

    <div class="mt-6 space-y-6">
    <div v-if="loading" class="flex justify-center py-20">
       <Loader2 class="w-10 h-10 text-primary animate-spin" />
    </div>

    <!-- Form Area -->
    <div v-else class="grid grid-cols-1 lg:grid-cols-3 gap-8">
      <!-- Main Content -->
      <div class="lg:col-span-2 space-y-6">
        <div class="bg-surface rounded-2xl p-6 border border-border shadow-soft space-y-5">
          <h3 class="text-lg font-bold text-textPrimary border-b border-border pb-3">基本信息</h3>
          
          <div class="flex items-center space-x-6">
             <div class="relative group cursor-pointer w-20 h-20 shrink-0">
                <input type="file" class="hidden" accept="image/*" id="iconUpload" @change="onIconChange" />
                <label for="iconUpload" class="w-full h-full rounded-2xl overflow-hidden shadow-soft border-2 border-dashed border-border flex items-center justify-center bg-black/5 hover:border-primary transition-all cursor-pointer">
                   <img v-if="form.iconPath" :src="`${apiUrl}${form.iconPath}`" class="w-full h-full object-cover" />
                   <div v-else-if="form.platformId" class="w-full h-full flex items-center justify-center text-white text-3xl font-bold" :style="{ backgroundColor: defaultIconColor }">
                      {{ defaultIcon }}
                   </div>
                   <div v-else class="w-full h-full flex items-center justify-center text-textHint flex-col">
                      <Image class="w-6 h-6 mb-1" />
                      <span class="text-[10px]">上传图标</span>
                   </div>
                   
                   <div v-if="isUploadingIcon" class="absolute inset-0 bg-black/50 flex items-center justify-center backdrop-blur-sm">
                      <Loader2 class="w-5 h-5 text-white animate-spin" />
                   </div>
                   <div v-else class="absolute inset-0 bg-black/40 flex items-center justify-center opacity-0 group-hover:opacity-100 transition-opacity backdrop-blur-sm text-white text-xs font-medium content-center text-center">更换</div>
                </label>
             </div>
             
             <div class="flex-1">
               <label class="block text-sm font-medium text-textSecondary mb-1.5">软件名称 <span class="text-danger">*</span></label>
               <input v-model="form.name" type="text" class="w-full px-4 py-2 bg-black/5 border border-transparent focus:border-primary/50 rounded-xl text-sm focus:outline-none transition-colors text-textPrimary placeholder-textHint" placeholder="例如：IntelliJ IDEA Ultimate" />
             </div>
          </div>

          <div>
            <label class="block text-sm font-medium text-textSecondary mb-1.5">简述</label>
            <input v-model="form.summary" type="text" class="w-full px-4 py-2 bg-black/5 border border-transparent focus:border-primary/50 rounded-xl text-sm focus:outline-none transition-colors text-textPrimary placeholder-textHint" placeholder="例如：领先的 Java 和 Kotlin IDE" />
          </div>

          <div class="relative">
            <label class="block text-sm font-medium text-textSecondary mb-1.5 flex items-center justify-between">
               <span>详细描述</span>
               <span v-if="isPastingImage" class="text-xs text-primary flex items-center animate-pulse"><Loader2 class="w-3 h-3 mr-1 animate-spin" />图片上传中...</span>
            </label>
            <textarea v-model="form.description" @paste="onPasteImage" rows="10" class="w-full px-4 py-3 bg-black/5 border border-transparent focus:border-primary/50 rounded-xl text-sm focus:outline-none transition-colors text-textPrimary placeholder-textHint font-mono mt-1" placeholder="在这里输入详细描述，支持 Markdown 格式...可以直接 Ctrl+V 粘贴截图上传"></textarea>
          </div>
        </div>

        <div class="bg-surface rounded-2xl p-6 border border-border shadow-soft space-y-5">
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
              <div v-for="ver in versions" :key="ver.id" class="relative p-4 bg-black/5 rounded-xl flex items-center justify-between group border border-transparent hover:border-border transition-colors">
                 <div class="flex-1 min-w-0 pr-4">
                    <div class="flex items-center space-x-2">
                       <h4 class="font-bold text-sm text-textPrimary font-mono">v{{ ver.versionNumber }}</h4>
                       <span v-if="ver.isVisible === 1" class="px-1.5 py-0.5 rounded text-[10px] bg-success/10 text-success">可见</span>
                       <span v-else class="px-1.5 py-0.5 rounded text-[10px] bg-textHint/10 text-textSecondary">隐藏</span>
                    </div>
                    <p class="text-xs text-textSecondary mt-1 line-clamp-1" :title="ver.fileName">{{ ver.fileName }}</p>
                 </div>
                 <div class="flex items-center space-x-2 text-xs flex-shrink-0 mr-4">
                    <span v-if="ver.hashStatus === 2" class="text-success flex items-center"><ShieldCheck class="w-3 h-3 mr-1" />校验完毕</span>
                    <span v-else-if="ver.hashStatus === 1" class="text-primary flex items-center"><Loader2 class="w-3 h-3 mr-1 animate-spin" />计算中</span>
                    <span v-else class="text-textHint flex items-center"><Clock class="w-3 h-3 mr-1" />等待中</span>
                 </div>
                 <div class="opacity-0 group-hover:opacity-100 transition-opacity flex space-x-1 flex-shrink-0">
                    <button @click="toggleVersionVisibility(ver)" class="p-1.5 text-textHint hover:text-primary rounded-lg transition-colors" :title="ver.isVisible === 1 ? '隐藏' : '显示'"><Eye class="w-4 h-4" v-if="ver.isVisible !== 1"/><EyeOff class="w-4 h-4" v-else /></button>
                    <button @click="openEditVersion(ver)" class="p-1.5 text-textHint hover:text-primary rounded-lg transition-colors" title="编辑"><Edit2 class="w-4 h-4" /></button>
                    <button @click="deleteVersion(ver.id)" class="p-1.5 text-textHint hover:text-danger rounded-lg transition-colors" title="删除"><Trash class="w-4 h-4" /></button>
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

        <!-- Upload Package Card -->
        <div class="bg-surface rounded-2xl p-6 border border-border shadow-soft space-y-5">
           <h3 class="text-lg font-bold text-textPrimary border-b border-border pb-3 flex items-center">
             <UploadCloud class="w-5 h-5 mr-2 text-primary" /> 上传新版本
           </h3>

           <!-- 编辑及新增模式下皆显示上传表单 -->
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

              <div v-if="uploading || (saving && uploadProgress > 0)" class="h-1.5 w-full bg-black/5 rounded-full overflow-hidden">
                 <div class="h-full bg-primary transition-all duration-300" :style="{ width: uploadProgress + '%' }"></div>
              </div>

              <div v-if="isNew" class="text-xs text-textHint text-center mt-2 italic px-2">
                 * 提醒：请在页面上方点击「保存并上传」按钮，系统将自动录入软件并绑定此安装包。
              </div>
              <button v-else @click="handleUpload" :disabled="uploading || !uploadFile || !uploadForm.versionNumber" class="w-full py-2.5 bg-textPrimary text-white rounded-xl text-xs font-bold hover:bg-black transition-all flex items-center justify-center disabled:opacity-30">
                 <UploadCloud class="w-4 h-4 mr-2" /> {{ uploading ? `上传中 ${uploadProgress}%` : '开始上传安装包' }}
              </button>
           </div>
        </div>
      </div>
    </div>

    <!-- Edit Version Modal -->
    <div v-if="editingVersion" class="fixed inset-0 z-50 flex items-center justify-center bg-black/50 backdrop-blur-sm animate-fade-in-up">
      <div class="bg-surface w-full max-w-md p-6 rounded-2xl shadow-xl flex flex-col space-y-4">
        <h3 class="text-lg font-bold text-textPrimary">修改版本信息</h3>
        <div>
           <label class="block text-sm font-medium text-textSecondary mb-1">版本号</label>
           <input v-model="editVersionForm.versionNumber" type="text" class="w-full px-3 py-2 bg-black/5 border border-transparent focus:border-primary/50 rounded-lg text-sm focus:outline-none" />
        </div>
        <div>
           <label class="block text-sm font-medium text-textSecondary mb-1">更新日志</label>
           <textarea v-model="editVersionForm.changelog" rows="4" class="w-full px-3 py-2 bg-black/5 border border-transparent focus:border-primary/50 rounded-lg text-sm focus:outline-none"></textarea>
        </div>
        <div class="flex justify-end space-x-3 pt-4 border-t border-border">
           <button @click="editingVersion = null" class="px-4 py-2 bg-black/5 hover:bg-black/10 rounded-xl text-sm font-medium text-textSecondary transition-colors">取消</button>
           <button @click="submitEditVersion" :disabled="savingVersion" class="px-4 py-2 bg-primary hover:bg-primaryHover text-white rounded-xl text-sm font-medium transition-colors disabled:opacity-50">保存</button>
        </div>
      </div>
    </div>

    </div><!-- end mt-6 space-y-6 wrapper -->
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed, reactive } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { ArrowLeft, Save, Loader2, Terminal, ShieldCheck, Clock, Trash, UploadCloud, Paperclip, Eye, EyeOff, Edit2, Image } from 'lucide-vue-next'
import http from '../../api/http'

const route = useRoute()
const router = useRouter()
const isNew = computed(() => route.params.id === 'new')

const loading = ref(false)
const saving = ref(false)

const apiUrl = import.meta.env.VITE_API_URL || 'http://localhost:5186'

const form = ref({
  name: '',
  summary: '',
  description: '',
  iconPath: '',
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
const isPastingImage = ref(false)
const isUploadingIcon = ref(false)

const defaultIcon = computed(() => {
   if (form.value.iconPath) return ''
   const p = platforms.value.find(x => x.id === form.value.platformId)
   if (!p) return ''
   return p.name.substring(0, 1).toUpperCase()
})

const defaultIconColor = computed(() => {
   const p = platforms.value.find(x => x.id === form.value.platformId)
   return p?.colorHex || '#9CA3AF'
})

const onIconChange = async (e: any) => {
   const file = e.target.files[0]
   if (!file) return
   isUploadingIcon.value = true
   const formData = new FormData()
   formData.append('file', file)
   try {
      const res = await http.post('/api/admin/softwares/upload-image', formData)
      if (res.data.code === 200) {
         form.value.iconPath = res.data.data.url
      }
   } catch (error) {
      alert('图标上传失败')
   } finally {
      isUploadingIcon.value = false
   }
}

const categories = ref<any[]>([])
const platforms = ref<any[]>([])
const versions = ref<any[]>([])

const fetchData = async () => {
  loading.value = true
  try {
    // 拉取字典数据
    const [catRes, platRes] = await Promise.all([
      http.get('/api/admin/categories'),
      http.get('/api/admin/platforms')
    ])
    if (catRes.data.code === 200) categories.value = catRes.data.data
    if (platRes.data.code === 200) platforms.value = platRes.data.data

    if (!isNew.value) {
      // 拉取详情
      const detailRes = await http.get(`/api/admin/softwares/${route.params.id}`)
      if (detailRes.data.code === 200) {
        const d = detailRes.data.data
        form.value = {
          name: d.name,
          summary: d.summary || '',
          description: d.description || '',
          iconPath: d.iconPath || '',
          officialUrl: d.officialUrl || '',
          categoryId: d.categoryId || '',
          platformId: d.platform?.id || ''
        }
      }

      // 拉取版本信息用于附属面板
      await loadVersions()
    }
  } catch (error) {
    console.error(error)
    alert('数据加载失败，请稍后重试。')
  } finally {
    loading.value = false
  }
}

const loadVersions = async () => {
   const verRes = await http.get(`/api/admin/versions/software/${route.params.id}`)
   if (verRes.data.code === 200) {
      versions.value = verRes.data.data
   }
}

onMounted(() => {
  fetchData()
})

const save = async () => {
  if (!form.value.name) return alert('软件名称为必填项')
  if (!form.value.platformId) return alert('请选择软件所属平台')
  if (isNew.value && uploadFile.value && !uploadForm.versionNumber) return alert('请为上传的安装包填写版本号')

  // 校验同平台下是否已存在同名软件
  try {
    const checkRes = await http.get('/api/admin/softwares/check-name', {
      params: {
        name: form.value.name,
        platformId: form.value.platformId,
        excludeId: isNew.value ? '' : (route.params.id as string)
      }
    })
    if (checkRes.data.code === 200 && checkRes.data.data?.exists) {
      return alert('在相同平台下已存在同名软件，请修改名称或更换平台。')
    }
  } catch (err) {
    // 若接口不存在则忽略校验，继续保存
  }
  
  saving.value = true
  uploadProgress.value = 0
  try {
    if (isNew.value) {
      // 1. 先保存软件本体
      const createRes = await http.post('/api/admin/softwares', form.value)
      const newId = createRes.data?.data?.id
      if (!newId) throw new Error('未能获取新建软件 ID')

      // 2. 串行上传安装包（独立捕获异常，防止上传失败阻断进入编辑页路由）
      if (uploadFile.value) {
        const formData = new FormData()
        formData.append('file', uploadFile.value)
        formData.append('softwareId', newId)
        formData.append('versionNumber', uploadForm.versionNumber)
        formData.append('changelog', uploadForm.changelog)

        try {
          await http.post('/api/admin/versions/upload', formData, {
            onUploadProgress: (progressEvent) => {
              if (progressEvent.total) {
                uploadProgress.value = Math.round((progressEvent.loaded * 100) / progressEvent.total)
              }
            }
          })
          alert('软件档案与首个版本安装包上传成功！')
        } catch (upErr: any) {
          alert('软件已成功创建，但安装包上传失败：' + (upErr.response?.data?.message || upErr.message))
        }
      }
      
      // 保存成功后返回列表页
      router.push('/admin/softwares')
    } else {
      await http.put(`/api/admin/softwares/${route.params.id}`, form.value)
      alert('保存成功！')
      router.push('/admin/softwares')
    }
  } catch (error: any) {
    alert(error.response?.data?.message || '操作失败')
  } finally {
    saving.value = false
  }
}

const deleteVersion = async (id: string) => {
   if (!confirm('确定删除该版本及其物理文件？')) return
   try {
     const res = await http.delete(`/api/admin/versions/${id}?physicalDelete=true`)
     if (res.data.code === 200) {
        versions.value = versions.value.filter(v => v.id !== id)
     }
   } catch(e: any) { alert(e.response?.data?.message || '删除失败') }
}

const editingVersion = ref<any>(null)
const editVersionForm = reactive({ versionNumber: '', changelog: '' })
const savingVersion = ref(false)

const openEditVersion = (ver: any) => {
    editingVersion.value = ver
    editVersionForm.versionNumber = ver.versionNumber
    editVersionForm.changelog = ver.changelog || ''
}

const submitEditVersion = async () => {
    if (!editVersionForm.versionNumber) return alert('版本号必填')
    savingVersion.value = true
    try {
        const res = await http.put(`/api/admin/versions/${editingVersion.value.id}`, editVersionForm)
        if (res.data.code === 200) {
            await loadVersions()
            editingVersion.value = null
            alert('版本信息修改成功！')
        }
    } catch (error: any) {
        alert(error.response?.data?.message || '修改失败')
    } finally {
        savingVersion.value = false
    }
}

const toggleVersionVisibility = async (ver: any) => {
    const newStatus = ver.isVisible === 1 ? 0 : 1
    try {
        const res = await http.patch(`/api/admin/versions/${ver.id}/visibility`, newStatus, {
            headers: { 'Content-Type': 'application/json' }
        })
        if (res.data.code === 200) {
            ver.isVisible = newStatus
        }
    } catch (error: any) {
        alert(error.response?.data?.message || '切换可见性失败')
    }
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
    const res = await http.post('/api/admin/versions/upload', formData, {
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
       const verRes = await http.get(`/api/admin/versions/software/${route.params.id}`)
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

const onPasteImage = async (e: ClipboardEvent) => {
  const items = e.clipboardData?.items
  if (!items) return

  let imageFile: File | null = null
  for (let i = 0; i < items.length; i++) {
    if (items[i].type.indexOf('image') === 0) {
      imageFile = items[i].getAsFile()
      break
    }
  }

  if (imageFile) {
    e.preventDefault()
    isPastingImage.value = true
    const formData = new FormData()
    formData.append('file', imageFile)
    
    try {
      const res = await http.post('/api/admin/softwares/upload-image', formData)
      if (res.data.code === 200) {
         const url = `${import.meta.env.VITE_API_URL || 'http://localhost:5186'}${res.data.data.url}`
         const mdImage = `\n![图片说明](${url})\n`
         
         const textarea = e.target as HTMLTextAreaElement
         const start = textarea.selectionStart
         const end = textarea.selectionEnd
         const text = form.value.description || ''
         
         form.value.description = text.substring(0, start) + mdImage + text.substring(end)
         
         // 焦点还原至插入后的位置
         setTimeout(() => {
            textarea.selectionStart = textarea.selectionEnd = start + mdImage.length
            textarea.focus()
         }, 0)
      }
    } catch (error) {
      alert('图片上传失败，请检查网络或配置。')
    } finally {
      isPastingImage.value = false
    }
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
