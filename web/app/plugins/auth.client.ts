export default defineNuxtPlugin(() => {
  // Initialize auth state from localStorage on client-side
  const { initAuth } = useAuth()
  initAuth()
})

