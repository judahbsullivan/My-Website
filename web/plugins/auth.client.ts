export default defineNuxtPlugin(() => {
  const { initAuth } = useAuth()
  
  // Initialize auth state from localStorage on client
  initAuth()
})

