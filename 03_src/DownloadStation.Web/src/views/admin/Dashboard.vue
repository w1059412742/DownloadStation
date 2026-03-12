<template>
  <div class="space-y-6">
    <div class="flex items-center justify-between">
      <h1 class="text-2xl font-bold tracking-tight text-textPrimary">仪表盘</h1>
    </div>

    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
      <div v-for="metric in metrics" :key="metric.label" 
        class="bg-surface rounded-2xl p-6 border border-border shadow-soft flex items-center justify-between hover:border-primary/50 transition-colors">
        <div>
          <p class="text-sm font-medium text-textHint">{{ metric.label }}</p>
          <p class="text-3xl font-bold text-textPrimary mt-1">{{ metric.value }}</p>
        </div>
        <div class="p-3 bg-primary/10 rounded-xl">
          <component :is="metric.icon" class="w-6 h-6 text-primary" />
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { Box, Download, Layers, Monitor } from 'lucide-vue-next'
import http from '../../api/http'

const metrics = ref([
  { label: '软件总数', value: 0, icon: Box },
  { label: '总下载量', value: 0, icon: Download },
  { label: '分类数量', value: 0, icon: Layers },
  { label: '平台数量', value: 0, icon: Monitor },
])

onMounted(async () => {
  try {
    const res = await http.get('/api/admin/dashboard/statistics')
    if (res.data.code === 200) {
      metrics.value[0]!.value = res.data.data.softwareCount
      metrics.value[1]!.value = res.data.data.totalDownloads
      metrics.value[2]!.value = res.data.data.categoryCount
      metrics.value[3]!.value = res.data.data.platformCount
    }
  } catch (error) {
    console.error('获取统计数据失败', error)
  }
})
</script>
