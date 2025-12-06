<template>
  <div class="min-h-screen pt-24 pb-12">
    <div class="px-4 sm:px-6 lg:px-8">
      <!-- Header -->
      <div class="mb-8">
        <div class="flex items-center justify-between mb-4">
          <div>
            <UiTitle as="h1" size="4xl" weight="bold" align="left" class="text-3xl sm:text-4xl">
              Create Playground Item
            </UiTitle>
            <p class="text-base sm:text-lg text-muted-foreground max-w-2xl leading-relaxed mt-2">
              Create an interactive component or code snippet for the playground
            </p>
          </div>
          <NuxtLink
            to="/admin/dashboard"
            class="inline-flex items-center gap-2 px-4 py-2 text-sm font-medium text-foreground hover:text-primary transition-colors border border-border rounded-lg hover:border-primary/50"
          >
            <Icon name="heroicons:arrow-left" class="w-4 h-4" />
            Back to Dashboard
          </NuxtLink>
        </div>
      </div>

      <!-- Form -->
      <form @submit.prevent="handleSubmit" class="space-y-6">
        <!-- Title -->
        <div>
          <label for="title" class="block text-sm font-medium text-foreground mb-2">
            Title <span class="text-destructive">*</span>
          </label>
          <input
            id="title"
            v-model="form.title"
            type="text"
            required
            maxlength="300"
            placeholder="Enter playground item title"
            class="w-full px-4 py-2.5 bg-card border-2 border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground placeholder:text-muted-foreground"
          />
          <p class="mt-1 text-xs text-muted-foreground">{{ form.title.length }}/300</p>
        </div>

        <!-- Slug -->
        <div>
          <label for="slug" class="block text-sm font-medium text-foreground mb-2">
            Slug <span class="text-destructive">*</span>
          </label>
          <input
            id="slug"
            v-model="form.slug"
            type="text"
            required
            maxlength="200"
            placeholder="playground-item-slug"
            class="w-full px-4 py-2.5 bg-card border-2 border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground placeholder:text-muted-foreground"
          />
          <p class="mt-1 text-xs text-muted-foreground">
            URL-friendly identifier
          </p>
        </div>

        <!-- Description & Category Row -->
        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
          <!-- Description -->
          <div>
            <label for="description" class="block text-sm font-medium text-foreground mb-2">
              Description
            </label>
            <textarea
              id="description"
              v-model="form.description"
              rows="3"
              maxlength="500"
              placeholder="Describe what this playground item does"
              class="w-full px-4 py-2.5 bg-card border-2 border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground placeholder:text-muted-foreground resize-none"
            ></textarea>
            <p class="mt-1 text-xs text-muted-foreground">{{ form.description.length }}/500</p>
          </div>

          <!-- Category -->
          <div>
            <label for="category" class="block text-sm font-medium text-foreground mb-2">
              Category
            </label>
            <input
              id="category"
              v-model="form.category"
              type="text"
              maxlength="100"
              placeholder="e.g., Animation, Component, Effect"
              class="w-full px-4 py-2.5 bg-card border-2 border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground placeholder:text-muted-foreground"
            />
          </div>
        </div>

        <!-- Code Type & Options -->
        <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
          <!-- Code Type -->
          <div>
            <label for="codeType" class="block text-sm font-medium text-foreground mb-2">
              Code Type <span class="text-destructive">*</span>
            </label>
            <select
              id="codeType"
              v-model="form.codeType"
              required
              class="w-full px-4 py-2.5 bg-card border-2 border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground"
            >
              <option value="vue">Vue Component</option>
              <option value="html">HTML</option>
              <option value="css">CSS</option>
              <option value="js">JavaScript</option>
              <option value="animation">Animation</option>
              <option value="effect">Effect</option>
            </select>
          </div>

          <!-- Is Vue Component -->
          <div class="flex items-end">
            <label class="flex items-center gap-3 cursor-pointer">
              <input
                v-model="form.isVueComponent"
                type="checkbox"
                class="w-5 h-5 rounded border-2 border-border text-primary focus:ring-primary focus:ring-2"
              />
              <span class="text-sm font-medium text-foreground">Vue Component</span>
            </label>
          </div>

          <!-- Is Interactive -->
          <div class="flex items-end">
            <label class="flex items-center gap-3 cursor-pointer">
              <input
                v-model="form.isInteractive"
                type="checkbox"
                class="w-5 h-5 rounded border-2 border-border text-primary focus:ring-primary focus:ring-2"
              />
              <span class="text-sm font-medium text-foreground">Interactive</span>
            </label>
          </div>
        </div>

        <!-- Main Code -->
        <div>
          <label for="code" class="block text-sm font-medium text-foreground mb-2">
            Code <span class="text-destructive">*</span>
          </label>
          <textarea
            id="code"
            v-model="form.code"
            rows="15"
            required
            placeholder="Paste your code here (HTML, Vue component, CSS, JS, etc.)"
            class="w-full px-4 py-2.5 bg-card border-2 border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground placeholder:text-muted-foreground resize-none font-mono text-sm"
          ></textarea>
          <p class="mt-1 text-xs text-muted-foreground">
            Main code content (HTML template, Vue component, etc.)
          </p>
        </div>

        <!-- Additional CSS -->
        <div>
          <label for="css" class="block text-sm font-medium text-foreground mb-2">
            Additional CSS (Optional)
          </label>
          <textarea
            id="css"
            v-model="form.css"
            rows="8"
            placeholder="Additional CSS styles if needed"
            class="w-full px-4 py-2.5 bg-card border-2 border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground placeholder:text-muted-foreground resize-none font-mono text-sm"
          ></textarea>
        </div>

        <!-- Additional JavaScript -->
        <div>
          <label for="javascript" class="block text-sm font-medium text-foreground mb-2">
            Additional JavaScript (Optional)
          </label>
          <textarea
            id="javascript"
            v-model="form.javascript"
            rows="8"
            placeholder="Additional JavaScript if needed"
            class="w-full px-4 py-2.5 bg-card border-2 border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground placeholder:text-muted-foreground resize-none font-mono text-sm"
          ></textarea>
        </div>

        <!-- Dependencies -->
        <div>
          <label for="dependencies" class="block text-sm font-medium text-foreground mb-2">
            Dependencies
          </label>
          <input
            id="dependencies"
            v-model="dependenciesInput"
            type="text"
            placeholder="vue, gsap, tailwindcss"
            class="w-full px-4 py-2.5 bg-card border-2 border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground placeholder:text-muted-foreground"
            @blur="updateDependencies"
          />
          <p class="mt-1 text-xs text-muted-foreground">
            Separate dependencies with commas
          </p>
          <div v-if="form.dependencies && form.dependencies.length > 0" class="mt-2 flex flex-wrap gap-2">
            <span
              v-for="(dep, index) in form.dependencies"
              :key="index"
              class="inline-flex items-center gap-1 px-2 py-1 bg-primary/20 text-primary text-xs rounded-full"
            >
              {{ dep }}
              <button
                type="button"
                @click="removeDependency(index)"
                class="hover:text-destructive transition-colors"
              >
                <Icon name="heroicons:x-mark" class="w-3 h-3" />
              </button>
            </span>
          </div>
        </div>

        <!-- Tags -->
        <div>
          <label for="tags" class="block text-sm font-medium text-foreground mb-2">
            Tags
          </label>
          <input
            id="tags"
            v-model="tagsInput"
            type="text"
            placeholder="animation, vue, interactive"
            class="w-full px-4 py-2.5 bg-card border-2 border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground placeholder:text-muted-foreground"
            @blur="updateTags"
          />
          <p class="mt-1 text-xs text-muted-foreground">
            Separate tags with commas
          </p>
          <div v-if="form.tags && form.tags.length > 0" class="mt-2 flex flex-wrap gap-2">
            <span
              v-for="(tag, index) in form.tags"
              :key="index"
              class="inline-flex items-center gap-1 px-2 py-1 bg-primary/20 text-primary text-xs rounded-full"
            >
              {{ tag }}
              <button
                type="button"
                @click="removeTag(index)"
                class="hover:text-destructive transition-colors"
              >
                <Icon name="heroicons:x-mark" class="w-3 h-3" />
              </button>
            </span>
          </div>
        </div>

        <!-- Preview Image Upload -->
        <div>
          <label for="previewImageFile" class="block text-sm font-medium text-foreground mb-2">
            Preview Image
          </label>
          
          <div class="space-y-4">
            <div class="flex items-center gap-4">
              <label
                for="previewImageFile"
                class="flex items-center justify-center gap-2 px-6 py-3 bg-card border-2 border-border rounded-lg hover:border-primary/50 transition-colors cursor-pointer text-foreground"
              >
                <Icon name="heroicons:photo" class="w-5 h-5" />
                <span class="text-sm font-medium">
                  {{ selectedFile || form.previewImageUrl ? 'Change Image' : 'Choose Image' }}
                </span>
                <input
                  id="previewImageFile"
                  type="file"
                  accept="image/jpeg,image/jpg,image/png,image/gif,image/webp"
                  class="hidden"
                  @change="handleFileSelect"
                />
              </label>

              <div v-if="imagePreview || form.previewImageUrl || isUploading" class="relative">
                <div class="relative w-20 h-20 rounded-lg border-2 border-border overflow-hidden bg-muted flex items-center justify-center">
                  <img
                    v-if="imagePreview || form.previewImageUrl"
                    :src="imagePreview || getImageUrl(form.previewImageUrl)"
                    alt="Thumbnail"
                    class="w-full h-full object-cover"
                    @error="handleImageError"
                  />
                  <div v-else-if="isUploading" class="w-full h-full flex items-center justify-center">
                    <Icon name="heroicons:photo" class="w-8 h-8 text-muted-foreground" />
                  </div>
                  
                  <div
                    v-if="isUploading"
                    class="absolute inset-0 bg-black/70 flex items-center justify-center z-10"
                  >
                    <div class="flex flex-col items-center gap-1">
                      <Icon name="heroicons:arrow-path" class="w-4 h-4 text-white animate-spin" />
                      <span class="text-[10px] text-white">Uploading</span>
                    </div>
                  </div>
                  
                  <div
                    v-else-if="form.previewImageUrl && !isUploading && imagePreview"
                    class="absolute top-1 right-1 w-5 h-5 bg-success rounded-full flex items-center justify-center z-10 shadow-lg"
                  >
                    <Icon name="heroicons:check" class="w-3 h-3 text-white" />
                  </div>
                </div>
              </div>
              
              <button
                v-if="selectedFile || form.previewImageUrl"
                type="button"
                @click="clearImage"
                class="px-4 py-3 text-sm font-medium text-destructive hover:text-destructive/80 transition-colors border border-destructive/20 rounded-lg hover:border-destructive/40"
              >
                Remove
              </button>
            </div>

            <div class="pt-2 border-t border-border">
              <p class="text-xs text-muted-foreground mb-2">Or enter image URL manually:</p>
              <input
                v-model="form.previewImageUrl"
                type="url"
                maxlength="500"
                placeholder="https://example.com/image.jpg"
                class="w-full px-4 py-2.5 bg-card border-2 border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground placeholder:text-muted-foreground text-sm"
                @input="handleManualUrlInput"
              />
            </div>
          </div>
        </div>

        <!-- Checkboxes -->
        <div class="space-y-3">
          <label class="flex items-center gap-3 cursor-pointer">
            <input
              v-model="form.isFeatured"
              type="checkbox"
              class="w-5 h-5 rounded border-2 border-border text-primary focus:ring-primary focus:ring-2"
            />
            <span class="text-sm font-medium text-foreground">Featured Item</span>
          </label>
          <label class="flex items-center gap-3 cursor-pointer">
            <input
              v-model="form.isPublished"
              type="checkbox"
              class="w-5 h-5 rounded border-2 border-border text-primary focus:ring-primary focus:ring-2"
            />
            <span class="text-sm font-medium text-foreground">Publish Immediately</span>
          </label>
          <label class="flex items-center gap-3 cursor-pointer">
            <input
              v-model="form.requiresSubscription"
              type="checkbox"
              class="w-5 h-5 rounded border-2 border-border text-primary focus:ring-primary focus:ring-2"
            />
            <span class="text-sm font-medium text-foreground flex items-center gap-2">
              Requires Subscription
              <span class="px-2 py-0.5 text-xs font-medium rounded-full bg-amber-500/20 text-amber-500">Premium</span>
            </span>
          </label>
        </div>

        <!-- Error Message -->
        <div v-if="error" class="p-4 bg-destructive/10 border border-destructive/20 rounded-lg">
          <p class="text-sm text-destructive">{{ error }}</p>
        </div>

        <!-- Success Message -->
        <div v-if="success" class="p-4 bg-success/10 border border-success/20 rounded-lg">
          <p class="text-sm text-success">{{ success }}</p>
        </div>

        <!-- Submit Button -->
        <div class="flex items-center gap-4 pt-4">
          <UiButton
            type="submit"
            :disabled="isSubmitting"
            class="min-w-[120px]"
          >
            <span v-if="!isSubmitting">Create Item</span>
            <span v-else class="flex items-center gap-2">
              <Icon name="heroicons:arrow-path" class="w-4 h-4 animate-spin" />
              Creating...
            </span>
          </UiButton>
          <NuxtLink
            to="/admin/dashboard"
            class="text-sm text-muted-foreground hover:text-foreground transition-colors"
          >
            Cancel
          </NuxtLink>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { CreatePlaygroundRequest } from '~/../../shared/types'

definePageMeta({
  middleware: 'admin'
})

useHead({
  title: 'Create Playground Item | Admin | Judah Sullivan',
})

const { authFetch, user, getAuthHeader } = useAuth()
const router = useRouter()
const config = useRuntimeConfig()
const apiBase = config.public.apiBase

const form = ref<CreatePlaygroundRequest>({
  slug: '',
  title: '',
  description: '',
  category: '',
  code: '',
  codeType: 'vue',
  css: '',
  javascript: '',
  dependencies: [],
  tags: [],
  previewImageUrl: '',
  isVueComponent: false,
  isInteractive: false,
  isFeatured: false,
  isPublished: false,
  requiresSubscription: false
})

const dependenciesInput = ref('')
const tagsInput = ref('')
const isSubmitting = ref(false)
const error = ref('')
const success = ref('')
const selectedFile = ref<File | null>(null)
const imagePreview = ref<string | null>(null)
const isUploading = ref(false)

// Auto-generate slug from title
watch(() => form.value.title, (newTitle) => {
  if (newTitle && !form.value.slug) {
    form.value.slug = newTitle
      .toLowerCase()
      .replace(/[^a-z0-9]+/g, '-')
      .replace(/(^-|-$)/g, '')
  }
})

// Update dependencies from comma-separated input
const updateDependencies = () => {
  if (!dependenciesInput.value.trim()) {
    form.value.dependencies = []
    return
  }
  form.value.dependencies = dependenciesInput.value
    .split(',')
    .map(dep => dep.trim())
    .filter(dep => dep.length > 0)
}

// Remove dependency
const removeDependency = (index: number) => {
  if (form.value.dependencies) {
    form.value.dependencies.splice(index, 1)
    dependenciesInput.value = form.value.dependencies.join(', ')
  }
}

// Update tags from comma-separated input
const updateTags = () => {
  if (!tagsInput.value.trim()) {
    form.value.tags = []
    return
  }
  form.value.tags = tagsInput.value
    .split(',')
    .map(tag => tag.trim())
    .filter(tag => tag.length > 0)
}

// Remove tag
const removeTag = (index: number) => {
  if (form.value.tags) {
    form.value.tags.splice(index, 1)
    tagsInput.value = form.value.tags.join(', ')
  }
}

// Handle file selection
const handleFileSelect = async (event: Event) => {
  const target = event.target as HTMLInputElement
  const file = target.files?.[0]
  
  if (!file) return

  error.value = ''
  success.value = ''

  const allowedTypes = ['image/jpeg', 'image/jpg', 'image/png', 'image/gif', 'image/webp']
  if (!allowedTypes.includes(file.type)) {
    error.value = 'Invalid file type. Please select a JPEG, PNG, GIF, or WebP image.'
    if (target) target.value = ''
    return
  }

  const maxSize = 5 * 1024 * 1024
  if (file.size > maxSize) {
    error.value = 'File size exceeds 5MB limit.'
    if (target) target.value = ''
    return
  }

  selectedFile.value = file

  await new Promise<void>((resolve) => {
    const reader = new FileReader()
    reader.onload = (e) => {
      imagePreview.value = e.target?.result as string
      resolve()
    }
    reader.onerror = () => {
      error.value = 'Failed to read image file.'
      selectedFile.value = null
      if (target) target.value = ''
      resolve()
    }
    reader.readAsDataURL(file)
  })

  if (imagePreview.value) {
    await uploadImage(file)
  }
}

// Upload image to server
const uploadImage = async (file: File) => {
  if (!imagePreview.value) {
    error.value = 'Failed to load image preview. Please try again.'
    return
  }

  isUploading.value = true
  error.value = ''
  success.value = ''

  try {
    const formData = new FormData()
    formData.append('file', file)

    const authHeaders = getAuthHeader()
    const headers: Record<string, string> = { ...authHeaders }
    if ('Content-Type' in headers) {
      delete headers['Content-Type']
    }

    const response = await $fetch<{ url: string; fileName: string }>(`${apiBase}/api/upload/image`, {
      method: 'POST',
      body: formData,
      headers
    })

    form.value.previewImageUrl = response.url
    imagePreview.value = getImageUrl(response.url)
    success.value = 'Image uploaded successfully!'
    setTimeout(() => {
      success.value = ''
    }, 3000)
  } catch (err: any) {
    error.value = err.data?.message || err.message || 'Failed to upload image. Please try again.'
  } finally {
    isUploading.value = false
  }
}

// Clear image
const clearImage = () => {
  selectedFile.value = null
  imagePreview.value = null
  form.value.previewImageUrl = ''
  
  const fileInput = document.getElementById('previewImageFile') as HTMLInputElement
  if (fileInput) {
    fileInput.value = ''
  }
}

// Get full image URL for preview
const getImageUrl = (url?: string | null): string => {
  if (!url) return ''
  if (url.startsWith('http://') || url.startsWith('https://')) {
    return url
  }
  return `${apiBase}${url}`
}

// Handle manual URL input
const handleManualUrlInput = () => {
  selectedFile.value = null
  imagePreview.value = null
  if (form.value.previewImageUrl) {
    const img = new Image()
    img.onload = () => {
      imagePreview.value = getImageUrl(form.value.previewImageUrl)
    }
    img.onerror = () => {
      imagePreview.value = null
    }
    img.src = getImageUrl(form.value.previewImageUrl)
  }
}

// Handle image load errors
const handleImageError = () => {
  if (form.value.previewImageUrl && !imagePreview.value) {
    const img = new Image()
    img.onload = () => {
      imagePreview.value = getImageUrl(form.value.previewImageUrl)
    }
    img.src = getImageUrl(form.value.previewImageUrl)
  }
}

// Handle form submission
const handleSubmit = async () => {
  error.value = ''
  success.value = ''
  isSubmitting.value = true

  try {
    // Update dependencies and tags before submission
    updateDependencies()
    updateTags()

    // Set author ID from current user
    if (user.value) {
      form.value.authorId = user.value.id
    }

    // Create the playground item
    const response = await authFetch<{ slug: string }>('/api/playground', {
      method: 'POST',
      body: form.value
    })

    success.value = 'Playground item created successfully!'
    
    setTimeout(() => {
      router.push(`/playground/${response.slug}`)
    }, 1500)
  } catch (err: any) {
    error.value = err.data?.message || err.message || 'Failed to create playground item. Please try again.'
  } finally {
    isSubmitting.value = false
  }
}
</script>

