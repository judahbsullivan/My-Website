/**
 * Plugin to automatically refresh ScrollTrigger on route changes
 * This ensures animations work correctly after navigation
 * Uses GSAP from our direct import plugin
 */
export default defineNuxtPlugin({
  name: 'scrolltrigger-refresh',
  setup(nuxtApp) {
    if (import.meta.server) return

    // Refresh ScrollTrigger after route changes
    nuxtApp.hook('page:finish', () => {
      const ScrollTrigger = nuxtApp.$ScrollTrigger as typeof import('gsap/ScrollTrigger').ScrollTrigger
      
      if (ScrollTrigger && typeof ScrollTrigger.refresh === 'function') {
        // Wait for next tick to ensure DOM is updated
        nextTick(() => {
          ScrollTrigger.refresh()
          // Additional refresh after a short delay to handle async content
          setTimeout(() => ScrollTrigger.refresh(), 100)
        })
      }
    })

    // Also refresh on app:mounted (initial load)
    nuxtApp.hook('app:mounted', () => {
      const ScrollTrigger = nuxtApp.$ScrollTrigger as typeof import('gsap/ScrollTrigger').ScrollTrigger
      
      if (ScrollTrigger && typeof ScrollTrigger.refresh === 'function') {
        nextTick(() => {
          ScrollTrigger.refresh()
        })
      }
    })
  }
})
