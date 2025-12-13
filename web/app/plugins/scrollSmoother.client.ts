export default defineNuxtPlugin({
  name: 'scroll-smoother',
  setup() {
    if (import.meta.server) return

    const nuxtApp = useNuxtApp()
    const { isIntroLoaderComplete } = useIntroLoader()
    
    // Flags to prevent multiple initializations
    let isInitializing = false
    let hasInitialized = false
    
    // Wait for DOM and intro loader to be ready
    const initScrollSmoother = () => {
      // Early return if already initialized
      if (hasInitialized) {
        return
      }

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
        // Retry if elements don't exist yet (but only if not already initialized)
        if (!hasInitialized) {
          setTimeout(initScrollSmoother, 100)
        }
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
          hasInitialized = true
          return
        }

        // Detect mobile device
        const isMobile = /iPhone|iPad|iPod|Android/i.test(navigator.userAgent) || window.innerWidth <= 768
        
        // Initialize ScrollSmoother with mobile-optimized settings
        const smoother = ScrollSmoother.create({
          wrapper: '#smooth-wrapper',
          content: '#smooth-content',
          smooth: isMobile ? 0.5 : 1.2, // Faster response on mobile for less lag
          effects: !isMobile, // Disable parallax effects on mobile for better performance
          smoothTouch: isMobile ? 0.3 : 0.1, // Moderate smoothing on mobile to reduce jumpiness without feeling disconnected (0.2-0.4 recommended)
          normalizeScroll: true, // Normalize scroll across browsers - prevents address bar issues
          ignoreMobileResize: true, // Prevent issues on mobile resize
        })
        
        if (smoother) {
          console.log('ScrollSmoother initialized successfully')
          // Store smoother instance - check if it already exists first
          try {
            if (!(nuxtApp as any).$scrollSmoother) {
              nuxtApp.provide('scrollSmoother', smoother)
            } else {
              console.log('ScrollSmoother already provided, using existing instance')
            }
          } catch (e) {
            // If provide fails, just use the smoother directly
            console.warn('Could not provide scrollSmoother, but smoother is initialized')
          }
          // Add class to enable ScrollSmoother styles
          document.documentElement.classList.add('scroll-smoother-active')
          hasInitialized = true
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
      // Don't initialize if already initialized or currently initializing
      if (hasInitialized || isInitializing) {
        return
      }

      if (isIntroLoaderComplete.value) {
        isInitializing = true
        // Small delay to ensure layout is ready
        setTimeout(() => {
          initScrollSmoother()
          // hasInitialized is set inside initScrollSmoother if successful
          isInitializing = false
        }, 300)
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

