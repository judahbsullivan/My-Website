import type { User, AuthResponse, LoginRequest, RegisterRequest } from '~/../../shared/types'

// State is kept outside the composable so it persists across component instances
const user = ref<User | null>(null)
const token = ref<string | null>(null)

export const useAuth = () => {
  const config = useRuntimeConfig()
  const apiBase = config.public.apiBase

  // Computed
  const isAuthenticated = computed(() => !!token.value && !!user.value)

  // Initialize from localStorage on client-side
  const initAuth = () => {
    if (import.meta.client) {
      const storedToken = localStorage.getItem('auth_token')
      const storedUser = localStorage.getItem('auth_user')

      if (storedToken && storedUser) {
        token.value = storedToken
        user.value = JSON.parse(storedUser)
      }
    }
  }

  // Register
  const register = async (request: RegisterRequest): Promise<AuthResponse> => {
    const response = await $fetch<AuthResponse>(`${apiBase}/api/Auth/register`, {
      method: 'POST',
      body: request,
    })

    // Store auth data
    token.value = response.token
    user.value = response.user

    if (import.meta.client) {
      localStorage.setItem('auth_token', response.token)
      localStorage.setItem('auth_user', JSON.stringify(response.user))
    }

    return response
  }

  // Login
  const login = async (request: LoginRequest): Promise<AuthResponse> => {
    const response = await $fetch<AuthResponse>(`${apiBase}/api/Auth/login`, {
      method: 'POST',
      body: request,
    })

    // Store auth data
    token.value = response.token
    user.value = response.user

    if (import.meta.client) {
      localStorage.setItem('auth_token', response.token)
      localStorage.setItem('auth_user', JSON.stringify(response.user))
    }

    return response
  }

  // Logout
  const logout = () => {
    token.value = null
    user.value = null

    if (import.meta.client) {
      localStorage.removeItem('auth_token')
      localStorage.removeItem('auth_user')
      // Redirect to auth page after logout
      navigateTo('/auth')
    }
  }

  // Get auth header for API requests
  const getAuthHeader = (): Record<string, string> => {
    if (!token.value) return {}
    return { Authorization: `Bearer ${token.value}` }
  }

  // Authenticated fetch helper
  const authFetch = async <T>(url: string, options: Parameters<typeof $fetch>[1] = {}): Promise<T> => {
    const headers = {
      ...(options.headers as Record<string, string> || {}),
      ...getAuthHeader(),
    }
    return $fetch<T>(`${apiBase}${url}`, {
      ...options,
      headers,
    })
  }

  return {
    // State
    user: readonly(user),
    token: readonly(token),
    isAuthenticated,

    // Methods
    initAuth,
    register,
    login,
    logout,
    getAuthHeader,
    authFetch,
  }
}

