<template>
  <ClientOnly>
    <div class="min-h-screen flex items-center justify-center p-4">
      <div class="w-full max-w-md space-y-8">
        <!-- Reset Password Card -->
        <div class="bg-card/80 backdrop-blur-xl border-2 border-border rounded-2xl p-8 shadow-lg">
          <!-- Header -->
          <div class="text-center mb-8">
            <div class="w-16 h-16 rounded-full bg-primary/20 flex items-center justify-center mx-auto mb-4">
              <Icon name="heroicons:key" class="w-8 h-8 text-primary" />
            </div>
            <h1 class="text-3xl font-bold text-foreground mb-2">Reset Your Password</h1>
            <p class="text-muted-foreground">
              Enter your new password below
            </p>
          </div>

          <!-- Success Message -->
          <div v-if="successMessage" class="mb-6 p-4 bg-success/10 border border-success/20 rounded-lg">
            <div class="flex items-start gap-3">
              <Icon name="heroicons:check-circle" class="w-5 h-5 text-success shrink-0 mt-0.5" />
              <div class="flex-1">
                <p class="text-success text-sm font-medium mb-2">{{ successMessage }}</p>
                <NuxtLink
                  to="/auth"
                  class="inline-flex items-center gap-2 mt-3 text-sm text-primary hover:underline font-medium"
                >
                  Go to Login
                  <Icon name="heroicons:arrow-right" class="w-4 h-4" />
                </NuxtLink>
              </div>
            </div>
          </div>

          <!-- Error Message -->
          <div v-if="errorMessage" class="mb-6 p-4 bg-destructive/10 border border-destructive/20 rounded-lg">
            <div class="flex items-start gap-3">
              <Icon name="heroicons:exclamation-circle" class="w-5 h-5 text-destructive shrink-0 mt-0.5" />
              <div class="flex-1">
                <p class="text-destructive text-sm font-medium mb-1">{{ errorMessage }}</p>
                <p v-if="!hasValidToken" class="text-xs text-muted-foreground mt-2">
                  <NuxtLink to="/auth/forgot-password" class="text-primary hover:underline">
                    Request a new password reset link
                  </NuxtLink>
                </p>
              </div>
            </div>
          </div>

          <!-- Token Validation Loading -->
          <div v-if="isValidatingToken" class="mb-6 p-4 bg-primary/10 border border-primary/20 rounded-lg">
            <div class="flex items-center gap-3">
              <Icon name="heroicons:arrow-path" class="w-5 h-5 text-primary shrink-0 animate-spin" />
              <p class="text-primary text-sm font-medium">Validating reset token...</p>
            </div>
          </div>

          <!-- Invalid Token Message -->
          <div v-if="!isValidatingToken && (!hasValidToken || tokenValidationError) && !errorMessage" class="mb-6 p-4 bg-amber-500/10 border border-amber-500/20 rounded-lg">
            <div class="flex items-start gap-3">
              <Icon name="heroicons:exclamation-triangle" class="w-5 h-5 text-amber-500 shrink-0 mt-0.5" />
              <div class="flex-1">
                <p class="text-amber-500 text-sm font-medium mb-2">{{ tokenValidationError || 'Invalid or expired reset link' }}</p>
                <p class="text-xs text-muted-foreground mb-3">
                  This password reset link is invalid or has expired. Please request a new one.
                </p>
                <NuxtLink
                  to="/auth/forgot-password"
                  class="inline-flex items-center gap-2 text-sm text-primary hover:underline font-medium"
                >
                  <Icon name="heroicons:arrow-left" class="w-4 h-4" />
                  Request New Reset Link
                </NuxtLink>
              </div>
            </div>
          </div>

          <!-- Form -->
          <form v-if="!isValidatingToken && hasValidToken && !successMessage" @submit.prevent="handleSubmit" class="space-y-5">
            <!-- New Password -->
            <div>
              <label for="newPassword" class="block text-sm font-medium text-foreground mb-2">
                New Password
              </label>
              <input
                id="newPassword"
                v-model="form.newPassword"
                type="password"
                placeholder="Enter your new password"
                required
                class="w-full px-4 py-3 bg-background border border-border rounded-lg text-foreground placeholder-muted-foreground focus:outline-none focus:ring-2 focus:ring-primary focus:border-transparent transition-all"
              />
              <p class="mt-1 text-xs text-muted-foreground">
                Min 6 chars with uppercase, lowercase, number, and special char (!@#$)
              </p>
            </div>

            <!-- Confirm Password -->
            <div>
              <label for="confirmPassword" class="block text-sm font-medium text-foreground mb-2">
                Confirm Password
              </label>
              <input
                id="confirmPassword"
                v-model="form.confirmPassword"
                type="password"
                placeholder="Confirm your new password"
                required
                class="w-full px-4 py-3 bg-background border border-border rounded-lg text-foreground placeholder-muted-foreground focus:outline-none focus:ring-2 focus:ring-primary focus:border-transparent transition-all"
              />
              <p v-if="form.confirmPassword && form.newPassword !== form.confirmPassword" class="mt-1 text-xs text-destructive">
                Passwords do not match
              </p>
            </div>

            <!-- Submit Button -->
            <button
              type="submit"
              :disabled="isLoading || !isFormValid"
              class="w-full py-3 px-4 bg-primary hover:bg-primary/90 disabled:bg-primary/50 disabled:cursor-not-allowed text-primary-foreground font-semibold rounded-lg transition-colors duration-200 flex items-center justify-center gap-2"
            >
              <Icon v-if="isLoading" name="heroicons:arrow-path" class="w-5 h-5 animate-spin" />
              <Icon v-else name="heroicons:key" class="w-5 h-5" />
              {{ isLoading ? 'Resetting Password...' : 'Reset Password' }}
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
// Protect this route - redirect if no token
const route = useRoute()
const router = useRouter()

// Get userId and token from query params (decode URL-encoded token)
const userId = computed(() => {
  const id = route.query.userId as string
  return id ? decodeURIComponent(id) : ''
})
const token = computed(() => {
  const t = route.query.token as string
  return t ? decodeURIComponent(t) : ''
})

// Check if we have valid token in URL
const hasTokenInUrl = computed(() => {
  return !!(userId.value && token.value)
})

// Server-side validation state
const isValidatingToken = ref(false)
const hasValidToken = ref(false)
const tokenValidationError = ref('')

// Validate token with server on mount
onMounted(async () => {
  // First check if token exists in URL
  if (!hasTokenInUrl.value) {
    router.push({
      path: '/auth/forgot-password',
      query: {
        error: 'missing_token'
      }
    })
    return
  }

  // Validate token with server
  isValidatingToken.value = true
  try {
    const response = await $fetch(`${apiBase}/api/Auth/validate-reset-token`, {
      method: 'GET',
      params: {
        userId: userId.value,
        token: token.value
      }
    })

    if (response.isValid) {
      hasValidToken.value = true
    } else {
      tokenValidationError.value = response.message || 'Invalid or expired reset token'
      hasValidToken.value = false
    }
  } catch (error: any) {
    console.error('Token validation error:', error)
    tokenValidationError.value = 'Failed to validate reset token. Please request a new password reset link.'
    hasValidToken.value = false
  } finally {
    isValidatingToken.value = false
  }
})

const config = useRuntimeConfig()
const apiBase = config.public.apiBase

const isLoading = ref(false)
const errorMessage = ref('')
const successMessage = ref('')

const form = reactive({
  newPassword: '',
  confirmPassword: '',
})

// Validate form
const isFormValid = computed(() => {
  return (
    form.newPassword.length >= 6 &&
    form.newPassword === form.confirmPassword &&
    hasValidToken.value
  )
})

const handleSubmit = async () => {
  if (!isFormValid.value) {
    errorMessage.value = 'Please fill in all fields correctly.'
    return
  }

  if (form.newPassword !== form.confirmPassword) {
    errorMessage.value = 'Passwords do not match.'
    return
  }

  isLoading.value = true
  errorMessage.value = ''
  successMessage.value = ''

  try {
    console.log('Resetting password with:', {
      userId: userId.value,
      tokenLength: token.value?.length,
      hasToken: !!token.value,
    })
    
    const response = await $fetch(`${apiBase}/api/Auth/reset-password`, {
      method: 'POST',
      body: {
        userId: userId.value,
        token: token.value,
        newPassword: form.newPassword,
      },
    })
    
    console.log('Reset password response:', response)

    successMessage.value = 'Password has been reset successfully! You can now login with your new password.'

    // Clear form
    form.newPassword = ''
    form.confirmPassword = ''

    // Redirect to login after 3 seconds
    setTimeout(() => {
      router.push('/auth')
    }, 3000)
  } catch (error: any) {
    console.error('Reset password error:', error)
    console.error('Error details:', {
      status: error?.status,
      statusCode: error?.statusCode,
      statusText: error?.statusText,
      data: error?.data,
      message: error?.message,
    })
    
    const errorData = error?.data || error?.response?._data
    if (error?.statusCode === 500 || error?.status === 500) {
      errorMessage.value = 'Server error occurred. The reset token may be invalid or expired. Please request a new password reset link.'
    } else if (errorData?.message) {
      errorMessage.value = errorData.message
    } else if (Array.isArray(errorData)) {
      errorMessage.value = errorData.join(' ')
    } else if (error?.message) {
      errorMessage.value = error.message
    } else {
      errorMessage.value = 'An error occurred while resetting your password. Please try again or request a new reset link.'
    }
  } finally {
    isLoading.value = false
  }
}

definePageMeta({
  layout: false,
})

useHead({
  title: 'Reset Password | Judah Sullivan',
})
</script>

