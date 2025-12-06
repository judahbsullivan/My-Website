export default defineNuxtPlugin({
  name: 'scroll-smoother',
  setup() {
    if (import.meta.server) return

    const nuxtApp = useNuxtApp()
    const { isIntroLoaderComplete } = useIntroLoader()
    
    // Wait for DOM and intro loader to be ready
    const initScrollSmoother = () => {
      const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
      const ScrollSmoother = (nuxtApp.$ScrollSmoother as any) || (typeof window !== 'undefined' && (window as any).ScrollSmoother)

      if (!gsap || !ScrollSmoother) {
        console.warn('GSAP or ScrollSmoother not available')
        return
      }

      // Check if wrapper and content exist
      const wrapper = document.getElementById('smooth-wrapper')
      const content = document.getElementById('smooth-content')
      
      if (!wrapper || !content) {
        // Retry if elements don't exist yet
        setTimeout(initScrollSmoother, 100)
        return
      }

      try {
        // Register ScrollSmoother if not already registered
        if (gsap.registerPlugin && !gsap.plugins?.scrollSmoother) {
          gsap.registerPlugin(ScrollSmoother)
        }

        // Check if ScrollSmoother is already initialized
        if (ScrollSmoother.get()) {
          console.log('ScrollSmoother already initialized')
          return
        }

        // Initialize ScrollSmoother
        const smoother = ScrollSmoother.create({
          wrapper: '#smooth-wrapper',
          content: '#smooth-content',
          smooth: 1.2, // Smooth scroll speed (1-2 is recommended)
          effects: true, // Enable effects for parallax
          smoothTouch: 0.1, // Smooth scroll on touch devices (0 = disabled)
          normalizeScroll: true, // Normalize scroll across browsers
          ignoreMobileResize: true, // Prevent issues on mobile resize
        })
        
        if (smoother) {
          console.log('ScrollSmoother initialized successfully')
          // Store smoother instance
          nuxtApp.$scrollSmoother = smoother
          // Add class to enable ScrollSmoother styles
          document.documentElement.classList.add('scroll-smoother-active')
        } else {
          console.warn('ScrollSmoother returned null - using normal scroll')
        }
      } catch (error) {
        console.warn('Failed to initialize ScrollSmoother:', error)
        console.log('Falling back to normal scrolling')
      }
    }

    // Wait for intro loader to complete before initializing
    const checkAndInit = () => {
      if (isIntroLoaderComplete.value) {
        // Small delay to ensure layout is ready
        setTimeout(initScrollSmoother, 300)
      } else {
        // Check again after a short delay
        setTimeout(checkAndInit, 100)
      }
    }

    // Start checking after a small delay to ensure DOM is ready
    if (typeof window !== 'undefined') {
      setTimeout(checkAndInit, 500)
    }

    // Cleanup on app close
    return {
      provide: {
        scrollSmoother: () => nuxtApp.$scrollSmoother
      }
    }
  },
  hooks: {
    'app:beforeMount'() {
      // Additional initialization if needed
    }
  }
})

