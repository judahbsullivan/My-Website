<template>
  <div class="min-h-screen flex items-center justify-center p-4">
    <div class="w-full max-w-md space-y-8">
      <!-- Auth Card -->
      <div class="bg-neutral-900 border border-neutral-800 rounded-2xl p-8 shadow-2xl">
        <!-- Header -->
        <div class="text-center mb-8">
          <h1 class="text-3xl font-bold text-white mb-2">
            {{ isLogin ? 'Welcome Back' : 'Create Account' }}
          </h1>
          <p class="text-neutral-400">
            {{ isLogin ? 'Sign in to your account' : 'Sign up for a new account' }}
          </p>
        </div>

        <!-- Success Message -->
        <div v-if="successMessage" class="mb-6 p-4 bg-emerald-500/10 border border-emerald-500/20 rounded-lg">
          <p class="text-emerald-400 text-sm">{{ successMessage }}</p>
        </div>

        <!-- Error Message -->
        <div v-if="errorMessage" class="mb-6 p-4 bg-red-500/10 border border-red-500/20 rounded-lg">
          <p class="text-red-400 text-sm">{{ errorMessage }}</p>
        </div>

        <!-- Form -->
        <form @submit.prevent="handleSubmit" class="space-y-5">
          <!-- Display Name (Register only) -->
          <div v-if="!isLogin">
            <label for="displayName" class="block text-sm font-medium text-neutral-300 mb-2">
              Display Name
            </label>
            <input
              id="displayName"
              v-model="form.displayName"
              type="text"
              placeholder="Your name"
              class="w-full px-4 py-3 bg-neutral-800 border border-neutral-700 rounded-lg text-white placeholder-neutral-500 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all"
            />
          </div>

          <!-- Email -->
          <div>
            <label for="email" class="block text-sm font-medium text-neutral-300 mb-2">
              Email
            </label>
            <input
              id="email"
              v-model="form.email"
              type="email"
              placeholder="you@example.com"
              required
              class="w-full px-4 py-3 bg-neutral-800 border border-neutral-700 rounded-lg text-white placeholder-neutral-500 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all"
            />
          </div>

          <!-- Password -->
          <div>
            <label for="password" class="block text-sm font-medium text-neutral-300 mb-2">
              Password
            </label>
            <input
              id="password"
              v-model="form.password"
              type="password"
              placeholder="••••••••"
              required
              class="w-full px-4 py-3 bg-neutral-800 border border-neutral-700 rounded-lg text-white placeholder-neutral-500 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all"
            />
            <p v-if="!isLogin" class="mt-1 text-xs text-neutral-500">
              Min 6 chars with uppercase, lowercase, number, and special char (!@#$)
            </p>
          </div>

          <!-- Submit Button -->
          <button
            type="submit"
            :disabled="isLoading"
            class="w-full py-3 px-4 bg-blue-600 hover:bg-blue-700 disabled:bg-blue-600/50 disabled:cursor-not-allowed text-white font-semibold rounded-lg transition-colors duration-200 flex items-center justify-center gap-2"
          >
            <svg v-if="isLoading" class="animate-spin h-5 w-5" viewBox="0 0 24 24">
              <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" fill="none" />
              <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4z" />
            </svg>
            {{ isLoading ? 'Please wait...' : (isLogin ? 'Sign In' : 'Create Account') }}
          </button>
        </form>

        <!-- Toggle Mode -->
        <div class="mt-6 text-center">
          <p class="text-neutral-400">
            {{ isLogin ? "Don't have an account?" : 'Already have an account?' }}
            <button
              @click="toggleMode"
              class="text-blue-400 hover:text-blue-300 font-medium ml-1 transition-colors"
            >
              {{ isLogin ? 'Sign up' : 'Sign in' }}
            </button>
          </p>
        </div>
      </div>

      <!-- User Info (when logged in) -->
      <div v-if="isAuthenticated" class="bg-neutral-900 border border-neutral-800 rounded-2xl p-6">
        <h2 class="text-xl font-semibold text-white mb-4">Logged in as:</h2>
        <div class="space-y-2 text-neutral-300">
          <p><span class="text-neutral-500">Name:</span> {{ user?.displayName }}</p>
          <p><span class="text-neutral-500">Email:</span> {{ user?.email }}</p>
          <p><span class="text-neutral-500">ID:</span> {{ user?.id }}</p>
        </div>
        <button
          @click="handleLogout"
          class="mt-4 w-full py-2 px-4 bg-red-600/20 hover:bg-red-600/30 text-red-400 font-medium rounded-lg transition-colors"
        >
          Sign Out
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
// definePageMeta({
//   layout: 'auth',
// })

const { user, isAuthenticated, login, register, logout } = useAuth()

const isLogin = ref(true)
const isLoading = ref(false)
const errorMessage = ref('')
const successMessage = ref('')

const form = reactive({
  email: '',
  password: '',
  displayName: '',
})

const toggleMode = () => {
  isLogin.value = !isLogin.value
  errorMessage.value = ''
  successMessage.value = ''
}

const handleSubmit = async () => {
  isLoading.value = true
  errorMessage.value = ''
  successMessage.value = ''

  try {
    if (isLogin.value) {
      await login({
        email: form.email,
        password: form.password,
      })
      successMessage.value = 'Successfully logged in!'
    } else {
      await register({
        email: form.email,
        password: form.password,
        displayName: form.displayName,
      })
      successMessage.value = 'Account created successfully!'
    }

    // Clear form
    form.email = ''
    form.password = ''
    form.displayName = ''
  } catch (error: any) {
    console.error('Auth error:', error)
    // Handle different error response formats
    const errorData = error?.data
    if (errorData?.message) {
      errorMessage.value = errorData.message
    } else if (Array.isArray(errorData)) {
      errorMessage.value = errorData.join(' ')
    } else if (error?.message) {
      errorMessage.value = error.message
    } else {
      errorMessage.value = 'An error occurred. Please try again.'
    }
  } finally {
    isLoading.value = false
  }
}

const handleLogout = () => {
  logout()
  successMessage.value = 'Successfully logged out!'
}
</script>

