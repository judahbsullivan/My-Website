/**
 * Centralized GSAP composable for accessing GSAP, ScrollTrigger, and SplitText
 * This ensures proper client-side access and prevents SSR issues
 * Uses direct GSAP imports instead of nuxt-gsap plugin
 */
export const useGSAP = () => {
  if (import.meta.server) {
    return {
      gsap: null,
      ScrollTrigger: null,
      SplitText: null,
      isReady: false,
      waitForReady: async () => ({ gsap: null, ScrollTrigger: null, SplitText: null, isReady: false })
    }
  }

  const nuxtApp = useNuxtApp()
  
  // Try to get from nuxtApp first (provided by our plugin), fallback to direct import
  const gsap = (nuxtApp.$gsap as typeof import('gsap').gsap) || null
  const ScrollTrigger = (nuxtApp.$ScrollTrigger as typeof import('gsap/ScrollTrigger').ScrollTrigger) || null
  const SplitText = (nuxtApp.$SplitText as any) || null

  const isReady = !!(gsap && ScrollTrigger)

  /**
   * Wait for ScrollTrigger to be ready (useful for components that initialize early)
   */
  const waitForReady = async (maxWait = 2000): Promise<{
    gsap: typeof import('gsap').gsap | null
    ScrollTrigger: typeof import('gsap/ScrollTrigger').ScrollTrigger | null
    SplitText: any
    isReady: boolean
  }> => {
    // If already ready, return immediately
    if (isReady && gsap && ScrollTrigger) {
      return { gsap, ScrollTrigger, SplitText, isReady: true }
    }

    // Wait for GSAP to be available via plugin or direct import
    return new Promise((resolve) => {
      const startTime = Date.now()
      const checkReady = () => {
        const nuxtApp = useNuxtApp()
        const currentGsap = (nuxtApp.$gsap as typeof import('gsap').gsap) || null
        const currentST = (nuxtApp.$ScrollTrigger as typeof import('gsap/ScrollTrigger').ScrollTrigger) || null
        const currentSplitText = (nuxtApp.$SplitText as any) || null
        
        if (currentGsap && currentST) {
          resolve({
            gsap: currentGsap,
            ScrollTrigger: currentST,
            SplitText: currentSplitText || null,
            isReady: true
          })
        } else if (Date.now() - startTime > maxWait) {
          // Timeout - try direct import as last resort
          import('gsap').then((gsapModule) => {
            import('gsap/ScrollTrigger').then((stModule) => {
              resolve({
                gsap: gsapModule.default,
                ScrollTrigger: stModule.ScrollTrigger || stModule.default || null,
                SplitText: currentSplitText || null,
                isReady: !!(gsapModule.default && (stModule.ScrollTrigger || stModule.default))
              })
            }).catch(() => {
              resolve({
                gsap: currentGsap || null,
                ScrollTrigger: currentST || null,
                SplitText: currentSplitText || null,
                isReady: false
              })
            })
          }).catch(() => {
            resolve({
              gsap: currentGsap || null,
              ScrollTrigger: currentST || null,
              SplitText: currentSplitText || null,
              isReady: false
            })
          })
        } else {
          setTimeout(checkReady, 50)
        }
      }
      checkReady()
    })
  }

  return {
    gsap: gsap || null,
    ScrollTrigger: ScrollTrigger || null,
    SplitText: SplitText || null,
    isReady,
    waitForReady
  }
}
