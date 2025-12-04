<template>
  <div class="min-h-screen pt-24 pb-12">
    <div class="px-4 sm:px-6 lg:px-8 ">
      <!-- Header -->
      <div class="mb-8">
        <div class="flex items-center justify-between mb-4">
          <div>
            <UiTitle as="h1" size="4xl" weight="bold" align="left" class="text-3xl sm:text-4xl">
              Create Blog Post
            </UiTitle>
            <p class="text-base sm:text-lg text-muted-foreground max-w-2xl leading-relaxed mt-2">
              Write and publish a new blog post
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
            placeholder="Enter blog post title"
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
            placeholder="blog-post-slug"
            class="w-full px-4 py-2.5 bg-card border-2 border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground placeholder:text-muted-foreground"
          />
          <p class="mt-1 text-xs text-muted-foreground">
            URL-friendly identifier (e.g., "my-awesome-post")
          </p>
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
            placeholder="e.g., Technology, Design, Tutorial"
            class="w-full px-4 py-2.5 bg-card border-2 border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground placeholder:text-muted-foreground"
          />
        </div>

        <!-- Excerpt -->
        <div>
          <label for="excerpt" class="block text-sm font-medium text-foreground mb-2">
            Excerpt
          </label>
          <textarea
            id="excerpt"
            v-model="form.excerpt"
            rows="3"
            maxlength="500"
            placeholder="Short description of the blog post"
            class="w-full px-4 py-2.5 bg-card border-2 border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground placeholder:text-muted-foreground resize-none"
          ></textarea>
          <p class="mt-1 text-xs text-muted-foreground">{{ form.excerpt.length }}/500</p>
        </div>

        <!-- Content -->
        <div>
          <label for="content" class="block text-sm font-medium text-foreground mb-2">
            Content <span class="text-destructive">*</span>
          </label>
          <textarea
            id="content"
            v-model="form.content"
            rows="15"
            required
            placeholder="Write your blog post content here..."
            class="w-full px-4 py-2.5 bg-card border-2 border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground placeholder:text-muted-foreground resize-none font-mono text-sm"
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
                  {{ selectedFile || form.image ? 'Change Image' : 'Choose Image' }}
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
              <div v-if="imagePreview || form.image || isUploading" class="relative">
                <div class="relative w-20 h-20 rounded-lg border-2 border-border overflow-hidden bg-muted flex items-center justify-center">
                  <!-- Show image if available -->
                  <img
                    v-if="imagePreview || form.image"
                    :src="imagePreview || getImageUrl(form.image)"
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
                    v-else-if="form.image && !isUploading && imagePreview"
                    class="absolute top-1 right-1 w-5 h-5 bg-success rounded-full flex items-center justify-center z-10 shadow-lg"
                  >
                    <Icon name="heroicons:check" class="w-3 h-3 text-white" />
                  </div>
                </div>
              </div>
              
              <button
                v-if="selectedFile || form.image"
                type="button"
                @click="clearImage"
                class="px-4 py-3 text-sm font-medium text-destructive hover:text-destructive/80 transition-colors border border-destructive/20 rounded-lg hover:border-destructive/40"
              >
                Remove
              </button>
            </div>

            <!-- Full Size Preview (Optional - can be expanded) -->
            <div v-if="(imagePreview || form.image) && showFullPreview" class="relative">
              <img
                :src="imagePreview || getImageUrl(form.image)"
                alt="Preview"
                class="w-full max-w-md h-64 object-cover rounded-lg border-2 border-border"
              />
            </div>

            <!-- Manual URL Input (Optional) -->
            <div class="pt-2 border-t border-border">
              <p class="text-xs text-muted-foreground mb-2">Or enter image URL manually:</p>
              <input
                v-model="form.image"
                type="url"
                maxlength="500"
                placeholder="https://example.com/image.jpg"
                class="w-full px-4 py-2.5 bg-card border-2 border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground placeholder:text-muted-foreground text-sm"
                @input="handleManualUrlInput"
              />
            </div>
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
            placeholder="tag1, tag2, tag3"
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

        <!-- Read Time & Options -->
        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
          <!-- Read Time -->
          <div>
            <label for="readTimeMinutes" class="block text-sm font-medium text-foreground mb-2">
              Read Time (minutes)
            </label>
            <input
              id="readTimeMinutes"
              v-model.number="form.readTimeMinutes"
              type="number"
              min="1"
              placeholder="5"
              class="w-full px-4 py-2.5 bg-card border-2 border-border rounded-lg focus:outline-none focus:border-primary transition-colors text-foreground placeholder:text-muted-foreground"
            />
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
            <span class="text-sm font-medium text-foreground">Featured Post</span>
          </label>
          <label class="flex items-center gap-3 cursor-pointer">
            <input
              v-model="form.isPublished"
              type="checkbox"
              class="w-5 h-5 rounded border-2 border-border text-primary focus:ring-primary focus:ring-2"
            />
            <span class="text-sm font-medium text-foreground">Publish Immediately</span>
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
            <span v-if="!isSubmitting">Create Post</span>
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
import type { CreateBlogPostRequest } from '~/../../shared/types'

definePageMeta({
  middleware: 'admin'
})

useHead({
  title: 'Create Blog Post | Admin | Judah Sullivan',
})

const { authFetch, user, getAuthHeader } = useAuth()
const router = useRouter()
const config = useRuntimeConfig()
const apiBase = config.public.apiBase

const form = ref<CreateBlogPostRequest>({
  slug: '',
  title: '',
  category: '',
  excerpt: '',
  content: '',
  image: '',
  tags: [],
  readTimeMinutes: 5,
  isFeatured: false,
  isPublished: false
})

const tagsInput = ref('')
const isSubmitting = ref(false)
const error = ref('')
const success = ref('')
const selectedFile = ref<File | null>(null)
const imagePreview = ref<string | null>(null)
const isUploading = ref(false)
const showFullPreview = ref(false)

// Auto-generate slug from title
watch(() => form.value.title, (newTitle) => {
  if (newTitle && !form.value.slug) {
    form.value.slug = newTitle
      .toLowerCase()
      .replace(/[^a-z0-9]+/g, '-')
      .replace(/(^-|-$)/g, '')
  }
})

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
  
  if (!file) {
    return
  }

  // Clear previous errors
  error.value = ''
  success.value = ''

  // Validate file type
  const allowedTypes = ['image/jpeg', 'image/jpg', 'image/png', 'image/gif', 'image/webp']
  if (!allowedTypes.includes(file.type)) {
    error.value = 'Invalid file type. Please select a JPEG, PNG, GIF, or WebP image.'
    // Reset file input
    if (target) target.value = ''
    return
  }

  // Validate file size (5MB)
  const maxSize = 5 * 1024 * 1024
  if (file.size > maxSize) {
    error.value = 'File size exceeds 5MB limit.'
    // Reset file input
    if (target) target.value = ''
    return
  }

  // Set file immediately
  selectedFile.value = file

  // Create preview FIRST - wait for it to load before uploading
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

  // Only upload if preview was created successfully
  if (imagePreview.value) {
    await uploadImage(file)
  }
}

// Upload image to server
const uploadImage = async (file: File) => {
  // Ensure we have a preview before starting upload
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
    // Don't set Content-Type, let browser set it with boundary for multipart/form-data
    const headers: Record<string, string> = { ...authHeaders }
    // Explicitly remove Content-Type to let browser set it
    if ('Content-Type' in headers) {
      delete headers['Content-Type']
    }

    const response = await $fetch<{ url: string; fileName: string }>(`${apiBase}/api/upload/image`, {
      method: 'POST',
      body: formData,
      headers
    })

    // Set the image URL (relative path from server)
    form.value.image = response.url
    
    // Update preview to use the uploaded image URL (but keep local preview as fallback)
    const uploadedImageUrl = getImageUrl(response.url)
    
    // Preload the uploaded image to ensure it's available
    await new Promise<void>((resolve, reject) => {
      const img = new Image()
      img.onload = () => {
        // Optionally switch to server URL, but keep local preview for now
        // imagePreview.value = uploadedImageUrl
        resolve()
      }
      img.onerror = () => {
        // If server image fails to load, keep local preview
        console.warn('Server image failed to load, keeping local preview')
        resolve()
      }
      img.src = uploadedImageUrl
    })

    success.value = 'Image uploaded successfully!'
    
    // Clear success message after 3 seconds
    setTimeout(() => {
      success.value = ''
    }, 3000)
  } catch (err: any) {
    console.error('Error uploading image:', err)
    
    // More detailed error handling
    let errorMessage = 'Failed to upload image. Please try again.'
    if (err.data?.message) {
      errorMessage = err.data.message
    } else if (err.message) {
      errorMessage = err.message
    } else if (err.status === 401) {
      errorMessage = 'Authentication required. Please log in again.'
    } else if (err.status === 413) {
      errorMessage = 'File is too large. Maximum size is 5MB.'
    }
    
    error.value = errorMessage
    
    // Don't clear the preview on error - let user see what they selected
    // They can try uploading again or remove it manually
  } finally {
    isUploading.value = false
  }
}

// Clear image
const clearImage = () => {
  selectedFile.value = null
  imagePreview.value = null
  form.value.image = ''
  
  // Reset file input
  const fileInput = document.getElementById('imageFile') as HTMLInputElement
  if (fileInput) {
    fileInput.value = ''
  }
}

// Get full image URL for preview
const getImageUrl = (url?: string | null): string => {
  if (!url) return ''
  // If it's already a full URL, return it
  if (url.startsWith('http://') || url.startsWith('https://')) {
    return url
  }
  // Otherwise, prepend API base URL
  return `${apiBase}${url}`
}

// Handle manual URL input
const handleManualUrlInput = () => {
  // Clear file selection when manually entering URL
  selectedFile.value = null
  imagePreview.value = null
  // If URL is entered, try to show preview
  if (form.value.image) {
    // Create a preview from the URL
    const img = new Image()
    img.onload = () => {
      imagePreview.value = getImageUrl(form.value.image)
    }
    img.onerror = () => {
      // If image fails to load, that's okay - user might be entering URL
      imagePreview.value = null
    }
    img.src = getImageUrl(form.value.image)
  }
}

// Handle image load errors
const handleImageError = () => {
  // If image fails to load, try to use the form URL
  if (form.value.image && !imagePreview.value) {
    const img = new Image()
    img.onload = () => {
      imagePreview.value = getImageUrl(form.value.image)
    }
    img.src = getImageUrl(form.value.image)
  }
}

// Handle form submission
const handleSubmit = async () => {
  error.value = ''
  success.value = ''
  isSubmitting.value = true

  try {
    // Set author ID from current user
    if (user.value) {
      form.value.authorId = user.value.id
    }

    // Update tags before submission
    updateTags()

    // Create the blog post
    const response = await authFetch<{ slug: string }>('/api/blog', {
      method: 'POST',
      body: form.value
    })

    success.value = 'Blog post created successfully!'
    
    // Redirect to the blog post after a short delay
    setTimeout(() => {
      router.push(`/blog/${response.slug}`)
    }, 1500)
  } catch (err: any) {
    console.error('Error creating blog post:', err)
    error.value = err.data?.message || err.message || 'Failed to create blog post. Please try again.'
  } finally {
    isSubmitting.value = false
  }
}
</script>

