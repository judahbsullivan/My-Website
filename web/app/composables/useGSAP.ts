/**
 * Centralized GSAP composable for accessing GSAP, ScrollTrigger, and SplitText
 * This ensures proper client-side access and prevents SSR issues
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
  
  const gsap = nuxtApp.$gsap as typeof import('gsap').gsap | undefined
  const ScrollTrigger = (nuxtApp.$scrollTrigger as typeof import('gsap/ScrollTrigger').ScrollTrigger) || null
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
    if (isReady) {
      return { gsap: gsap || null, ScrollTrigger, SplitText, isReady: true }
    }

    // Wait for ScrollTrigger to be available
    return new Promise((resolve) => {
      const startTime = Date.now()
      const checkReady = () => {
        const currentGsap = nuxtApp.$gsap as typeof import('gsap').gsap | undefined
        const currentST = (nuxtApp.$scrollTrigger as typeof import('gsap/ScrollTrigger').ScrollTrigger) || null
        
        if (currentGsap && currentST) {
          resolve({
            gsap: currentGsap,
            ScrollTrigger: currentST,
            SplitText: (nuxtApp.$SplitText as any) || null,
            isReady: true
          })
        } else if (Date.now() - startTime > maxWait) {
          // Timeout - return what we have
          resolve({
            gsap: currentGsap || null,
            ScrollTrigger: currentST,
            SplitText: (nuxtApp.$SplitText as any) || null,
            isReady: false
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
    ScrollTrigger,
    SplitText,
    isReady,
    waitForReady
  }
}
