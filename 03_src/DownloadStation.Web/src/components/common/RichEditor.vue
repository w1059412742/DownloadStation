<template>
  <div class="rich-editor border border-slate-200 rounded-2xl overflow-hidden bg-white focus-within:ring-2 focus-within:ring-primary/20 transition-all relative">
    <!-- Toolbar -->
    <div v-if="editor" class="editor-toolbar flex flex-wrap items-center gap-1 p-2 bg-slate-50 border-b border-slate-100">
      <button 
        v-for="btn in toolbarButtons" 
        :key="btn.label"
        type="button"
        @click="btn.action"
        :class="[
          'p-1.5 rounded-lg transition-colors flex items-center justify-center hover:bg-white hover:shadow-sm',
          btn.isActive() ? 'bg-white shadow-sm text-primary' : 'text-slate-500'
        ]"
        :title="btn.label"
      >
        <component :is="btn.icon" class="w-4 h-4" />
      </button>

      <div class="w-px h-6 bg-slate-200 mx-1"></div>

      <!-- Image Upload Button -->
      <button 
        type="button" 
        @click="triggerImageUpload"
        class="p-1.5 rounded-lg text-slate-500 hover:bg-white hover:shadow-sm transition-all"
        title="上传图片"
      >
        <ImageIcon class="w-4 h-4" />
        <input type="file" ref="imageInput" class="hidden" accept="image/*" @change="onFileSelected" />
      </button>
    </div>

    <!-- Bubble Menu for Images -->
    <bubble-menu
      v-if="editor"
      :editor="editor"
      :should-show="shouldShowBubbleMenu"
      class="flex items-center gap-1 p-1 bg-slate-900 text-white rounded-xl shadow-xl border border-slate-700/50 backdrop-blur-md"
    >
      <!-- Sizes -->
      <button @click="setImageSize('25%')" class="p-1.5 hover:bg-white/10 rounded text-xs font-bold px-2">25%</button>
      <button @click="setImageSize('50%')" class="p-1.5 hover:bg-white/10 rounded text-xs font-bold px-2">50%</button>
      <button @click="setImageSize('100%')" class="p-1.5 hover:bg-white/10 rounded text-xs font-bold px-2">100%</button>
      
      <div class="w-px h-4 bg-white/20 mx-1"></div>
      
      <!-- Alignments -->
      <button @click="setImageAlign('left')" :class="['p-1.5 hover:bg-white/10 rounded', isImageAligned('left') ? 'bg-white/20' : '']" title="左浮动并排">
        <AlignLeft class="w-3.5 h-3.5" />
      </button>
      <button @click="setImageAlign('center')" :class="['p-1.5 hover:bg-white/10 rounded', isImageAligned('center') ? 'bg-white/20' : '']" title="居中展示">
        <AlignCenter class="w-3.5 h-3.5" />
      </button>
      <button @click="setImageAlign('right')" :class="['p-1.5 hover:bg-white/10 rounded', isImageAligned('right') ? 'bg-white/20' : '']" title="右浮动并排">
        <AlignRight class="w-3.5 h-3.5" />
      </button>
    </bubble-menu>

    <!-- Editor Content -->
    <editor-content :editor="editor" class="prose prose-slate max-w-none p-4 min-h-[300px] outline-none" />
  </div>
</template>

<script setup lang="ts">
import { ref, onBeforeUnmount, watch } from 'vue'
import { useEditor, EditorContent } from '@tiptap/vue-3'
import { BubbleMenu } from '@tiptap/vue-3/menus'
import BubbleMenuExtension from '@tiptap/extension-bubble-menu'
import StarterKit from '@tiptap/starter-kit'
import Image from '@tiptap/extension-image'
import Placeholder from '@tiptap/extension-placeholder'
import Underline from '@tiptap/extension-underline'
import TextAlign from '@tiptap/extension-text-align'
import { 
  Bold, Italic, List, ListOrdered, Heading1, Heading2, Heading3, 
  Quote, Undo, Redo, Image as ImageIcon, AlignLeft, AlignCenter, AlignRight, Underline as UnderlineIcon
} from 'lucide-vue-next'
import http from '../../api/http'

// Extend Image extension to support width and alignment
const CustomImage = Image.extend({
  addAttributes() {
    return {
      ...this.parent?.(),
      width: {
        default: '100%',
        renderHTML: attributes => ({
          style: `width: ${attributes.width};`,
        }),
      },
      align: {
        default: 'center',
        renderHTML: attributes => {
          if (attributes.align === 'center') {
            return {
              style: `display: block; margin-left: auto; margin-right: auto; float: none;`,
            }
          }
          return {
            style: `display: inline-block; float: ${attributes.align}; margin: 0.5rem;`,
          }
        },
      },
    }
  },
})

const props = defineProps<{
  modelValue: string
}>()

const emit = defineEmits(['update:modelValue'])

const imageInput = ref<HTMLInputElement | null>(null)

const editor = useEditor({
  content: props.modelValue,
  extensions: [
    StarterKit,
    BubbleMenuExtension,
    CustomImage.configure({
      HTMLAttributes: {
        class: 'rounded-2xl border border-slate-100 shadow-lg transition-all'
      }
    }),
    Placeholder.configure({
      placeholder: '在这里开始编写详细介绍... 支持直接粘贴剪贴板图片。'
    }),
    Underline,
    TextAlign.configure({
      types: ['heading', 'paragraph'],
    }),
  ],
  onUpdate: ({ editor }) => {
    emit('update:modelValue', editor.getHTML())
  },
  editorProps: {
    handlePaste(_view: any, event: ClipboardEvent) {
      const items = event.clipboardData?.items
      if (!items) return false

      for (let i = 0; i < items.length; i++) {
        const item = items[i]
        if (item && item.type.indexOf('image') === 0) {
          const file = item.getAsFile()
          if (file) {
            uploadAndInsertImage(file)
            return true
          }
        }
      }
      return false
    }
  }
})

// Bubble Menu logic
const shouldShowBubbleMenu = ({ editor }: any) => {
  return editor.isActive('image')
}

const setImageSize = (width: string) => {
  editor.value?.chain().focus().updateAttributes('image', { width }).run()
}

const setImageAlign = (align: string) => {
  editor.value?.chain().focus().updateAttributes('image', { align }).run()
}

const isImageAligned = (align: string) => {
  return editor.value?.getAttributes('image').align === align
}

// Toolbar buttons configuration
const toolbarButtons = [
  { icon: Undo, label: '撤销', action: () => editor.value?.chain().focus().undo().run(), isActive: () => false },
  { icon: Redo, label: '重做', action: () => editor.value?.chain().focus().redo().run(), isActive: () => false },
  { icon: Bold, label: '加粗', action: () => editor.value?.chain().focus().toggleBold().run(), isActive: () => editor.value?.isActive('bold') },
  { icon: Italic, label: '倾斜', action: () => editor.value?.chain().focus().toggleItalic().run(), isActive: () => editor.value?.isActive('italic') },
  { icon: UnderlineIcon, label: '下划线', action: () => editor.value?.chain().focus().toggleUnderline().run(), isActive: () => editor.value?.isActive('underline') },
  { icon: Heading1, label: '一级标题', action: () => editor.value?.chain().focus().toggleHeading({ level: 1 }).run(), isActive: () => editor.value?.isActive('heading', { level: 1 }) },
  { icon: Heading2, label: '二级标题', action: () => editor.value?.chain().focus().toggleHeading({ level: 2 }).run(), isActive: () => editor.value?.isActive('heading', { level: 2 }) },
  { icon: Heading3, label: '三级标题', action: () => editor.value?.chain().focus().toggleHeading({ level: 3 }).run(), isActive: () => editor.value?.isActive('heading', { level: 3 }) },
  { icon: List, label: '无序列表', action: () => editor.value?.chain().focus().toggleBulletList().run(), isActive: () => editor.value?.isActive('bulletList') },
  { icon: ListOrdered, label: '有序列表', action: () => editor.value?.chain().focus().toggleOrderedList().run(), isActive: () => editor.value?.isActive('orderedList') },
  { icon: Quote, label: '引用', action: () => editor.value?.chain().focus().toggleBlockquote().run(), isActive: () => editor.value?.isActive('blockquote') },
  { icon: AlignLeft, label: '内容居左', action: () => editor.value?.chain().focus().setTextAlign('left').run(), isActive: () => editor.value?.isActive({ textAlign: 'left' }) },
  { icon: AlignCenter, label: '内容居中', action: () => editor.value?.chain().focus().setTextAlign('center').run(), isActive: () => editor.value?.isActive({ textAlign: 'center' }) },
  { icon: AlignRight, label: '内容居右', action: () => editor.value?.chain().focus().setTextAlign('right').run(), isActive: () => editor.value?.isActive({ textAlign: 'right' }) },
]

const triggerImageUpload = () => {
  imageInput.value?.click()
}

const onFileSelected = (e: Event) => {
  const target = e.target as HTMLInputElement
  if (target.files && target.files[0]) {
    uploadAndInsertImage(target.files[0])
  }
}

const uploadAndInsertImage = async (file: File) => {
  const formData = new FormData()
  formData.append('file', file)
  try {
    const res = await http.post('/api/admin/softwares/upload-image', formData)
    if (res.data.code === 200) {
      const url = res.data.data.url
      editor.value?.chain().focus().setImage({ src: url }).run()
    }
  } catch (error) {
    alert('图片上传失败')
  }
}

watch(() => props.modelValue, (val) => {
  if (editor.value && editor.value.getHTML() !== val) {
    editor.value.commands.setContent(val)
  }
})

onBeforeUnmount(() => {
  editor.value?.destroy()
})
</script>

<style>
/* Tiptap Placeholder Style */
.rich-editor .tiptap p.is-editor-empty:first-child::before {
  content: attr(data-placeholder);
  float: left;
  color: #adb5bd;
  pointer-events: none;
  height: 0;
}

.rich-editor .tiptap {
  outline: none !important;
}

/* Ensure aligned images display correctly in editor */
.rich-editor .tiptap img[style*="float: left"] {
  float: left;
  margin-right: 1.5rem;
  margin-bottom: 1rem;
}

.rich-editor .tiptap img[style*="float: right"] {
  float: right;
  margin-left: 1.5rem;
  margin-bottom: 1rem;
}

.rich-editor .tiptap img[style*="margin-left: auto"] {
  margin-left: auto;
  margin-right: auto;
  display: block;
}

.rich-editor .tiptap::after {
  content: "";
  display: table;
  clear: both;
}
</style>
