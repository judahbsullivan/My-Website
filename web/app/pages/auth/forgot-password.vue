<template>
  <ClientOnly>
    <div class="min-h-screen flex items-center justify-center p-4">
      <div class="w-full max-w-md space-y-8">
        <!-- Forgot Password Card -->
        <div class="bg-card/80 backdrop-blur-xl border-2 border-border rounded-2xl p-8 shadow-lg">
          <!-- Header -->
          <div class="text-center mb-8">
          <h1 class="text-3xl font-bold text-foreground mb-2">Forgot Password?</h1>
            <p class="text-muted-foreground">
              Enter your email and we'll send you a password reset link
            </p>
          </div>

          <!-- Success Message -->
          <div v-if="successMessage" class="mb-6 p-4 bg-success/10 border border-success/20 rounded-lg">
            <div class="flex items-start gap-3">
              <Icon name="heroicons:check-circle" class="w-5 h-5 text-success shrink-0 mt-0.5" />
              <div class="flex-1">
                <p class="text-success text-sm font-medium mb-2">{{ successMessage }}</p>
                <p class="text-xs text-muted-foreground">
                  Please check your email inbox (and spam folder) for the password reset link. The link will expire in 1 hour.
                </p>
              </div>
            </div>
          </div>

          <!-- Error Message -->
          <div v-if="errorMessage" class="mb-6 p-4 bg-destructive/10 border border-destructive/20 rounded-lg">
            <p class="text-destructive text-sm">{{ errorMessage }}</p>
          </div>

          <!-- Form -->
          <form @submit.prevent="handleSubmit" class="space-y-5">
            <!-- Email -->
            <div>
              <label for="email" class="block text-sm font-medium text-foreground mb-2">
                Email
              </label>
              <input
                id="email"
                v-model="form.email"
                type="email"
                placeholder="you@example.com"
                required
                class="w-full px-4 py-3 bg-background border border-border rounded-lg text-foreground placeholder-muted-foreground focus:outline-none focus:ring-2 focus:ring-primary focus:border-transparent transition-all"
              />
            </div>

            <!-- Submit Button -->
            <button
              type="submit"
              :disabled="isLoading"
              class="w-full py-3 px-4 bg-primary hover:bg-primary/90 disabled:bg-primary/50 disabled:cursor-not-allowed text-primary-foreground font-semibold rounded-lg transition-colors duration-200 flex items-center justify-center gap-2"
            >
              <svg v-if="isLoading" class="animate-spin h-5 w-5" viewBox="0 0 24 24">
                <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" fill="none" />
                <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4z" />
              </svg>
              {{ isLoading ? 'Sending...' : 'Send Reset Link' }}
            </button>
          </form>

          <!-- Back to Login -->
          <div class="mt-6 text-center space-y-3">
            <NuxtLink to="/auth" class="inline-flex items-center gap-2 text-sm text-primary hover:text-primary/80 font-medium transition-colors">
              <Icon name="heroicons:arrow-left" class="w-4 h-4" />
              Back to Login
            </NuxtLink>
            <div>
              <NuxtLink to="/" class="inline-flex items-center gap-2 text-sm text-muted-foreground hover:text-foreground transition-colors">
                <Icon name="heroicons:home" class="w-4 h-4" />
                Back to Site
              </NuxtLink>
            </div>
          </div>
        </div>
      </div>
    </div>
    <template #fallback>
      <div class="min-h-screen flex items-center justify-center">
        <p class="text-muted-foreground">Loading...</p>
      </div>
    </template>
  </ClientOnly>
</template>

<script setup lang="ts">
const config = useRuntimeConfig()
const apiBase = config.public.apiBase

const route = useRoute()

// Check if redirected from reset-password due to missing token
onMounted(() => {
  if (route.query.error === 'missing_token') {
    errorMessage.value = 'Please request a password reset link first.'
  }
})

const isLoading = ref(false)
const errorMessage = ref('')
const successMessage = ref('')

const form = reactive({
  email: '',
})

const handleSubmit = async () => {
  console.log('Forgot password form submitted', { email: form.email, apiBase })
  
  if (!form.email) {
    errorMessage.value = 'Please enter your email address.'
    return
  }

  isLoading.value = true
  errorMessage.value = ''
  successMessage.value = ''

  try {
    console.log('Calling forgot password API:', `${apiBase}/api/Auth/forgot-password`)
    
    const response = await $fetch<{
      message: string
    }>(`${apiBase}/api/Auth/forgot-password`, {
      method: 'POST',
      body: {
        email: form.email,
      },
    })

    console.log('Forgot password response:', response)
    successMessage.value = response.message
    
    // Clear email field after successful submission
    form.email = ''
  } catch (error: any) {
    console.error('Forgot password error:', error)
    console.error('Error details:', {
      status: error?.status,
      statusCode: error?.statusCode,
      statusText: error?.statusText,
      data: error?.data,
      message: error?.message,
    })
    
    // Handle network errors
    if (error?.statusCode === 404 || error?.status === 404) {
      errorMessage.value = 'API endpoint not found. Please ensure the API server is running at ' + apiBase
    } else if (error?.statusCode >= 500 || error?.status >= 500) {
      errorMessage.value = 'Server error. Please try again later.'
    } else if (error?.statusCode === 0 || error?.message?.includes('fetch')) {
      errorMessage.value = 'Cannot connect to API server. Please ensure the API is running at ' + apiBase
    } else {
      const errorData = error?.data || error?.response?._data
      if (errorData?.message) {
        errorMessage.value = errorData.message
      } else if (error?.message) {
        errorMessage.value = error.message
      } else {
        errorMessage.value = 'An error occurred. Please check your connection and try again. Error: ' + (error?.statusCode || error?.status || 'Unknown')
      }
    }
  } finally {
    isLoading.value = false
  }
}

definePageMeta({
  layout: false,
})

useHead({
  title: 'Forgot Password | Judah Sullivan',
})
</script>

