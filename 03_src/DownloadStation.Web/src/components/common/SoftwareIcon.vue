<template>
  <div :class="['rounded-xl overflow-hidden flex items-center justify-center shrink-0 border animate-fade-in', sizeClass, bgClass]">
    <img v-if="iconPath" :src="fullIconUrl" alt="icon" class="w-full h-full object-cover" loading="lazy" @error="onIconError" />
    <component v-else :is="platformIcon" :class="iconSizeClass" :style="{ color: platformColor }" />
  </div>
</template>

<script setup lang="ts">
import { computed, ref, watch } from 'vue'
import { Package, Monitor, Apple, Laptop, Smartphone, HelpCircle } from 'lucide-vue-next'

interface Props {
  iconPath?: string | null
  platformName?: string | null
  size?: 'sm' | 'base' | 'md' | 'lg' | 'xl' | '2xl'
}

const props = withDefaults(defineProps<Props>(), {
  iconPath: null,
  platformName: null,
  size: 'md'
})

const apiUrl = ''
const hasError = ref(false)

// 当 iconPath 改变时重置错误状态
watch(() => props.iconPath, () => {
  hasError.value = false
})

const onIconError = () => {
  hasError.value = true
}

const fullIconUrl = computed(() => props.iconPath || '')

const platformIcon = computed(() => {
  if (!props.platformName) return Package
  const name = props.platformName.toLowerCase()
  if (name.includes('windows')) return Monitor
  if (name.includes('mac') || name.includes('ios')) return Apple
  if (name.includes('linux')) return Laptop
  if (name.includes('android')) return Smartphone
  return HelpCircle
})

const platformColor = computed(() => {
  if (!props.platformName) return 'currentColor'
  const name = props.platformName.toLowerCase()
  if (name.includes('windows')) return '#0078D4' // Windows Blue
  if (name.includes('mac') || name.includes('ios')) return '#555555' // Apple Grey
  if (name.includes('linux')) return '#FCC624' // Linux Yellow
  if (name.includes('android')) return '#3DDC84' // Android Green
  return 'currentColor'
})

const sizeClass = computed(() => {
  switch (props.size) {
    case 'sm': return 'w-8 h-8'
    case 'base': return 'w-12 h-12'
    case 'md': return 'w-10 h-10'
    case 'lg': return 'w-14 h-14'
    case 'xl': return 'w-20 h-20'
    case '2xl': return 'w-32 h-32 sm:w-40 sm:h-40'
    default: return 'w-10 h-10'
  }
})

const iconSizeClass = computed(() => {
  switch (props.size) {
    case 'sm': return 'w-4 h-4'
    case 'base': return 'w-6 h-6'
    case 'md': return 'w-5 h-5'
    case 'lg': return 'w-6 h-6'
    case 'xl': return 'w-10 h-10'
    case '2xl': return 'w-16 h-16 sm:w-20 sm:h-20'
    default: return 'w-5 h-5'
  }
})

const bgClass = computed(() => {
  return props.iconPath ? 'bg-transparent border-transparent' : 'bg-black/5 border-border/50'
})
</script>

<style scoped>
@keyframes fadeIn {
  from { opacity: 0; }
  to { opacity: 1; }
}
.animate-fade-in {
  animation: fadeIn 0.3s ease-out forwards;
}
</style>
