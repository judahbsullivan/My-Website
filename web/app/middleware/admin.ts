export default defineNuxtRouteMiddleware((to, from) => {
  // Only run on client-side (server-side will redirect to auth)
  if (import.meta.server) {
    // On server, always redirect to auth (will check client-side after hydration)
    return navigateTo({
      path: '/auth',
      query: {
        redirect: to.fullPath
      }
    })
  }

  // Initialize auth state first (in case plugin hasn't run yet)
  const { isAuthenticated, initAuth } = useAuth()
  
  // Ensure auth is initialized
  initAuth()
  
  // Check if user is authenticated
  if (!isAuthenticated.value) {
    // Redirect to auth page with return URL
    return navigateTo({
      path: '/auth',
      query: {
        redirect: to.fullPath
      }
    })
  }
  
  // If accessing /admin without a specific route, redirect to dashboard
  if (to.path === '/admin' || to.path === '/admin/') {
    return navigateTo('/admin/dashboard')
  }
})

