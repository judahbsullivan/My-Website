export default defineNuxtPlugin({
  name: 'gsap-scrolltrigger',
  setup(nuxtApp) {
    if (import.meta.server) return

    // @hypernym/nuxt-gsap already provides ScrollTrigger as $ScrollTrigger
    // Just ensure it's registered if needed
    const initScrollTrigger = () => {
      const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
      const ScrollTrigger = nuxtApp.$ScrollTrigger as typeof import('gsap/ScrollTrigger').ScrollTrigger
      
      if (!gsap || !ScrollTrigger) {
        // Retry if not ready yet
        setTimeout(initScrollTrigger, 50)
        return
      }

      // Register ScrollTrigger if not already registered
      if (gsap.registerPlugin && !gsap.plugins?.scrollTrigger) {
        gsap.registerPlugin(ScrollTrigger)
        console.log('ScrollTrigger registered successfully')
      }
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
