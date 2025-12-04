<template>
  <div class="min-h-screen pt-24 pb-12">
    <div class="px-4 sm:px-6 lg:px-8">
      <!-- Header -->
      <div class="mb-8">
        <div class="flex items-center justify-between mb-4">
          <div>
            <UiTitle as="h1" size="4xl" weight="bold" align="left" class="text-3xl sm:text-4xl">
              Create Project
            </UiTitle>
            <p class="text-base sm:text-lg text-muted-foreground max-w-2xl leading-relaxed mt-2">
              Add a new project to your portfolio
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
            maxlength="200"
            placeholder="Enter project title"
            class="w-full px-4 py-2.5 bg-card border-2 border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground placeholder:text-muted-foreground"
          />
          <p class="mt-1 text-xs text-muted-foreground">{{ form.title.length }}/200</p>
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
            maxlength="100"
            placeholder="project-slug"
            class="w-full px-4 py-2.5 bg-card border-2 border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground placeholder:text-muted-foreground"
          />
          <p class="mt-1 text-xs text-muted-foreground">
            URL-friendly identifier (e.g., "my-awesome-project")
          </p>
        </div>

        <!-- Category, Year, Role Row -->
        <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
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
              placeholder="e.g., Web App, Mobile"
              class="w-full px-4 py-2.5 bg-card border-2 border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground placeholder:text-muted-foreground"
            />
          </div>

          <!-- Year -->
          <div>
            <label for="year" class="block text-sm font-medium text-foreground mb-2">
              Year
            </label>
            <input
              id="year"
              v-model="form.year"
              type="text"
              maxlength="4"
              placeholder="2024"
              class="w-full px-4 py-2.5 bg-card border-2 border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground placeholder:text-muted-foreground"
            />
          </div>

          <!-- Role -->
          <div>
            <label for="role" class="block text-sm font-medium text-foreground mb-2">
              Role
            </label>
            <input
              id="role"
              v-model="form.role"
              type="text"
              maxlength="100"
              placeholder="e.g., Full Stack Developer"
              class="w-full px-4 py-2.5 bg-card border-2 border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground placeholder:text-muted-foreground"
            />
          </div>
        </div>

        <!-- Description -->
        <div>
          <label for="description" class="block text-sm font-medium text-foreground mb-2">
            Description <span class="text-destructive">*</span>
          </label>
          <textarea
            id="description"
            v-model="form.description"
            rows="3"
            required
            maxlength="500"
            placeholder="Short description of the project"
            class="w-full px-4 py-2.5 bg-card border-2 border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground placeholder:text-muted-foreground resize-none"
          ></textarea>
          <p class="mt-1 text-xs text-muted-foreground">{{ form.description.length }}/500</p>
        </div>

        <!-- Overview -->
        <div>
          <label for="overview" class="block text-sm font-medium text-foreground mb-2">
            Overview
          </label>
          <textarea
            id="overview"
            v-model="form.overview"
            rows="6"
            placeholder="Detailed overview of the project..."
            class="w-full px-4 py-2.5 bg-card border-2 border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground placeholder:text-muted-foreground resize-none"
          ></textarea>
        </div>

        <!-- Image Upload -->
        <div>
          <label for="imageFile" class="block text-sm font-medium text-foreground mb-2">
            Featured Image
          </label>
          
          <!-- File Input with Thumbnail -->
          <div class="space-y-4">
            <div class="flex items-center gap-4">
              <label
                for="imageFile"
                class="flex items-center justify-center gap-2 px-6 py-3 bg-card border-2 border-border rounded-lg hover:border-primary/50 transition-colors cursor-pointer text-foreground"
              >
                <Icon name="heroicons:photo" class="w-5 h-5" />
                <span class="text-sm font-medium">
                  {{ selectedFile || form.imageUrl ? 'Change Image' : 'Choose Image' }}
                </span>
                <input
                  id="imageFile"
                  type="file"
                  accept="image/jpeg,image/jpg,image/png,image/gif,image/webp"
                  class="hidden"
                  @change="handleFileSelect"
                />
              </label>

              <!-- Small Thumbnail with Loader -->
              <div v-if="imagePreview || form.imageUrl || isUploading" class="relative">
                <div class="relative w-20 h-20 rounded-lg border-2 border-border overflow-hidden bg-muted flex items-center justify-center">
                  <!-- Show image if available -->
                  <img
                    v-if="imagePreview || form.imageUrl"
                    :src="imagePreview || getImageUrl(form.imageUrl)"
                    alt="Thumbnail"
                    class="w-full h-full object-cover"
                    @error="handleImageError"
                  />
                  <!-- Show placeholder while loading -->
                  <div v-else-if="isUploading" class="w-full h-full flex items-center justify-center">
                    <Icon name="heroicons:photo" class="w-8 h-8 text-muted-foreground" />
                  </div>
                  
                  <!-- Tiny Loader Overlay -->
                  <div
                    v-if="isUploading"
                    class="absolute inset-0 bg-black/70 flex items-center justify-center z-10"
                  >
                    <div class="flex flex-col items-center gap-1">
                      <Icon name="heroicons:arrow-path" class="w-4 h-4 text-white animate-spin" />
                      <span class="text-[10px] text-white">Uploading</span>
                    </div>
                  </div>
                  
                  <!-- Success Checkmark -->
                  <div
                    v-else-if="form.imageUrl && !isUploading && imagePreview"
                    class="absolute top-1 right-1 w-5 h-5 bg-success rounded-full flex items-center justify-center z-10 shadow-lg"
                  >
                    <Icon name="heroicons:check" class="w-3 h-3 text-white" />
                  </div>
                </div>
              </div>
              
              <button
                v-if="selectedFile || form.imageUrl"
                type="button"
                @click="clearImage"
                class="px-4 py-3 text-sm font-medium text-destructive hover:text-destructive/80 transition-colors border border-destructive/20 rounded-lg hover:border-destructive/40"
              >
                Remove
              </button>
            </div>

            <!-- Manual URL Input (Optional) -->
            <div class="pt-2 border-t border-border">
              <p class="text-xs text-muted-foreground mb-2">Or enter image URL manually:</p>
              <input
                v-model="form.imageUrl"
                type="url"
                maxlength="500"
                placeholder="https://example.com/image.jpg"
                class="w-full px-4 py-2.5 bg-card border-2 border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground placeholder:text-muted-foreground text-sm"
                @input="handleManualUrlInput"
              />
            </div>
          </div>
        </div>

        <!-- URLs Row -->
        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
          <!-- Live URL -->
          <div>
            <label for="liveUrl" class="block text-sm font-medium text-foreground mb-2">
              Live URL
            </label>
            <input
              id="liveUrl"
              v-model="form.liveUrl"
              type="url"
              maxlength="500"
              placeholder="https://example.com"
              class="w-full px-4 py-2.5 bg-card border-2 border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground placeholder:text-muted-foreground"
            />
          </div>

          <!-- GitHub URL -->
          <div>
            <label for="githubUrl" class="block text-sm font-medium text-foreground mb-2">
              GitHub URL
            </label>
            <input
              id="githubUrl"
              v-model="form.githubUrl"
              type="url"
              maxlength="500"
              placeholder="https://github.com/username/repo"
              class="w-full px-4 py-2.5 bg-card border-2 border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground placeholder:text-muted-foreground"
            />
          </div>
        </div>

        <!-- Tech Stack -->
        <div>
          <label for="techStack" class="block text-sm font-medium text-foreground mb-2">
            Tech Stack
          </label>
          <input
            id="techStack"
            v-model="techStackInput"
            type="text"
            placeholder="React, TypeScript, Node.js"
            class="w-full px-4 py-2.5 bg-card border-2 border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground placeholder:text-muted-foreground"
            @blur="updateTechStack"
          />
          <p class="mt-1 text-xs text-muted-foreground">
            Separate technologies with commas
          </p>
          <div v-if="form.techStack && form.techStack.length > 0" class="mt-2 flex flex-wrap gap-2">
            <span
              v-for="(tech, index) in form.techStack"
              :key="index"
              class="inline-flex items-center gap-1 px-2 py-1 bg-primary/20 text-primary text-xs rounded-full"
            >
              {{ tech }}
              <button
                type="button"
                @click="removeTechStack(index)"
                class="hover:text-destructive transition-colors"
              >
                <Icon name="heroicons:x-mark" class="w-3 h-3" />
              </button>
            </span>
          </div>
        </div>

        <!-- Features -->
        <div>
          <label class="block text-sm font-medium text-foreground mb-2">
            Features
          </label>
          <div class="space-y-4">
            <div
              v-for="(feature, index) in form.features"
              :key="index"
              class="p-4 bg-card border-2 border-border rounded-lg space-y-3"
            >
              <div class="flex items-center justify-between">
                <span class="text-sm font-medium text-foreground">Feature {{ index + 1 }}</span>
                <button
                  type="button"
                  @click="removeFeature(index)"
                  class="text-destructive hover:text-destructive/80 transition-colors"
                >
                  <Icon name="heroicons:trash" class="w-4 h-4" />
                </button>
              </div>
              <input
                v-model="feature.title"
                type="text"
                placeholder="Feature title"
                class="w-full px-4 py-2 bg-card border border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground placeholder:text-muted-foreground text-sm"
              />
              <textarea
                v-model="feature.description"
                rows="2"
                placeholder="Feature description"
                class="w-full px-4 py-2 bg-card border border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground placeholder:text-muted-foreground resize-none text-sm"
              ></textarea>
            </div>
            <button
              type="button"
              @click="addFeature"
              class="w-full px-4 py-2 border-2 border-dashed border-border rounded-lg text-foreground hover:border-primary/50 transition-colors text-sm font-medium"
            >
              + Add Feature
            </button>
          </div>
        </div>

        <!-- Gallery Images -->
        <div>
          <label class="block text-sm font-medium text-foreground mb-2">
            Gallery Images
          </label>
          <input
            v-model="galleryImagesInput"
            type="text"
            placeholder="https://example.com/image1.jpg, https://example.com/image2.jpg"
            class="w-full px-4 py-2.5 bg-card border-2 border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground placeholder:text-muted-foreground"
            @blur="updateGalleryImages"
          />
          <p class="mt-1 text-xs text-muted-foreground">
            Separate image URLs with commas
          </p>
          <div v-if="form.galleryImages && form.galleryImages.length > 0" class="mt-4 grid grid-cols-2 md:grid-cols-4 gap-4">
            <div
              v-for="(imageUrl, index) in form.galleryImages"
              :key="index"
              class="relative group"
            >
              <img
                :src="getImageUrl(imageUrl)"
                :alt="`Gallery image ${index + 1}`"
                class="w-full h-32 object-cover rounded-lg border-2 border-border"
              />
              <button
                type="button"
                @click="removeGalleryImage(index)"
                class="absolute top-2 right-2 p-1 bg-destructive/80 text-white rounded-full opacity-0 group-hover:opacity-100 transition-opacity"
              >
                <Icon name="heroicons:x-mark" class="w-4 h-4" />
              </button>
            </div>
          </div>
        </div>

        <!-- Options Row -->
        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
          <!-- Display Order -->
          <div>
            <label for="displayOrder" class="block text-sm font-medium text-foreground mb-2">
              Display Order
            </label>
            <input
              id="displayOrder"
              v-model.number="form.displayOrder"
              type="number"
              min="0"
              placeholder="0"
              class="w-full px-4 py-2.5 bg-card border-2 border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground placeholder:text-muted-foreground"
            />
            <p class="mt-1 text-xs text-muted-foreground">
              Lower numbers appear first
            </p>
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
            <span class="text-sm font-medium text-foreground">Featured Project</span>
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
            <span v-if="!isSubmitting">Create Project</span>
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
import type { CreateProjectRequest, ProjectFeature } from '~/../../shared/types'

definePageMeta({
  middleware: 'admin'
})

useHead({
  title: 'Create Project | Admin | Judah Sullivan',
})

const { authFetch, user, getAuthHeader } = useAuth()
const router = useRouter()
const config = useRuntimeConfig()
const apiBase = config.public.apiBase

const form = ref<CreateProjectRequest>({
  slug: '',
  title: '',
  category: '',
  year: '',
  role: '',
  description: '',
  overview: '',
  imageUrl: '',
  liveUrl: '',
  githubUrl: '',
  techStack: [],
  features: [],
  galleryImages: [],
  isFeatured: false,
  displayOrder: 0
})

const techStackInput = ref('')
const galleryImagesInput = ref('')
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

// Update tech stack from comma-separated input
const updateTechStack = () => {
  if (!techStackInput.value.trim()) {
    form.value.techStack = []
    return
  }
  form.value.techStack = techStackInput.value
    .split(',')
    .map(tech => tech.trim())
    .filter(tech => tech.length > 0)
}

// Remove tech from stack
const removeTechStack = (index: number) => {
  if (form.value.techStack) {
    form.value.techStack.splice(index, 1)
    techStackInput.value = form.value.techStack.join(', ')
  }
}

// Add feature
const addFeature = () => {
  if (!form.value.features) {
    form.value.features = []
  }
  form.value.features.push({ title: '', description: '' })
}

// Remove feature
const removeFeature = (index: number) => {
  if (form.value.features) {
    form.value.features.splice(index, 1)
  }
}

// Update gallery images from comma-separated input
const updateGalleryImages = () => {
  if (!galleryImagesInput.value.trim()) {
    form.value.galleryImages = []
    return
  }
  form.value.galleryImages = galleryImagesInput.value
    .split(',')
    .map(url => url.trim())
    .filter(url => url.length > 0)
}

// Remove gallery image
const removeGalleryImage = (index: number) => {
  if (form.value.galleryImages) {
    form.value.galleryImages.splice(index, 1)
    galleryImagesInput.value = form.value.galleryImages.join(', ')
  }
}

// Handle file selection
const handleFileSelect = async (event: Event) => {
  const target = event.target as HTMLInputElement
  const file = target.files?.[0]
  
  if (!file) {
    return
  }

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

    form.value.imageUrl = response.url
    imagePreview.value = getImageUrl(response.url)
    success.value = 'Image uploaded successfully!'
    setTimeout(() => {
      success.value = ''
    }, 3000)
  } catch (err: any) {
    console.error('Error uploading image:', err)
    let errorMessage = 'Failed to upload image. Please try again.'
    if (err.data?.message) {
      errorMessage = err.data.message
    } else if (err.message) {
      errorMessage = err.message
    }
    error.value = errorMessage
  } finally {
    isUploading.value = false
  }
}

// Clear image
const clearImage = () => {
  selectedFile.value = null
  imagePreview.value = null
  form.value.imageUrl = ''
  
  const fileInput = document.getElementById('imageFile') as HTMLInputElement
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
  if (form.value.imageUrl) {
    const img = new Image()
    img.onload = () => {
      imagePreview.value = getImageUrl(form.value.imageUrl)
    }
    img.onerror = () => {
      imagePreview.value = null
    }
    img.src = getImageUrl(form.value.imageUrl)
  }
}

// Handle image load errors
const handleImageError = () => {
  if (form.value.imageUrl && !imagePreview.value) {
    const img = new Image()
    img.onload = () => {
      imagePreview.value = getImageUrl(form.value.imageUrl)
    }
    img.src = getImageUrl(form.value.imageUrl)
  }
}

// Handle form submission
const handleSubmit = async () => {
  error.value = ''
  success.value = ''
  isSubmitting.value = true

  try {
    // Update tech stack and gallery images before submission
    updateTechStack()
    updateGalleryImages()

    // Create the project
    const response = await authFetch<{ slug: string }>('/api/projects', {
      method: 'POST',
      body: form.value
    })

    success.value = 'Project created successfully!'
    
    setTimeout(() => {
      router.push(`/projects/${response.slug}`)
    }, 1500)
  } catch (err: any) {
    console.error('Error creating project:', err)
    error.value = err.data?.message || err.message || 'Failed to create project. Please try again.'
  } finally {
    isSubmitting.value = false
  }
}
</script>

