export default defineNuxtPlugin({
  name: 'gsap-scrolltrigger',
  setup(nuxtApp) {
    if (import.meta.server) return

    // Wait for GSAP to be available from @hypernym/nuxt-gsap
    const initScrollTrigger = () => {
      const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
      
      if (!gsap) {
        // Retry if GSAP isn't ready yet
        setTimeout(initScrollTrigger, 50)
        return
      }

      // Import ScrollTrigger synchronously (it's already configured in nuxt.config)
      // The @hypernym/nuxt-gsap module should make it available, but we ensure it's registered
      import('gsap/ScrollTrigger').then((stModule) => {
        const ScrollTrigger = stModule.default || stModule.ScrollTrigger || stModule
        
        if (ScrollTrigger && gsap.registerPlugin) {
          // Register ScrollTrigger if not already registered
          if (!gsap.plugins?.scrollTrigger) {
            gsap.registerPlugin(ScrollTrigger)
          }
          
          // Make ScrollTrigger available on nuxtApp
          nuxtApp.provide('scrollTrigger', ScrollTrigger)
          
          console.log('ScrollTrigger registered successfully')
        }
      }).catch((error) => {
        console.warn('Failed to load ScrollTrigger:', error)
      })
    }

    // Initialize after DOM is ready
    if (typeof window !== 'undefined') {
      if (document.readyState === 'complete' || document.readyState === 'interactive') {
        initScrollTrigger()
      } else {
        window.addEventListener('DOMContentLoaded', initScrollTrigger)
      }
    }
  }
})
