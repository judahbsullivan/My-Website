import gsap from 'gsap'
import { ScrollTrigger } from 'gsap/ScrollTrigger'

export default defineNuxtPlugin({
  name: 'gsap-init',
  setup(nuxtApp) {
    if (import.meta.server) return

    // Register GSAP plugins
    if (typeof gsap !== 'undefined') {
      // Register ScrollTrigger (always available)
      if (ScrollTrigger && gsap.registerPlugin) {
        gsap.registerPlugin(ScrollTrigger)
      }

      // Make GSAP and ScrollTrigger available immediately
      nuxtApp.provide('gsap', gsap)
      nuxtApp.provide('ScrollTrigger', ScrollTrigger || null)

      // Try to import and register SplitText (club plugin - may not be available)
      import('gsap/SplitText').then((splitTextModule) => {
        const SplitText = splitTextModule.SplitText || splitTextModule.default || null
        if (SplitText && gsap.registerPlugin) {
          gsap.registerPlugin(SplitText)
        }
        nuxtApp.provide('SplitText', SplitText || null)
      }).catch(() => {
        // SplitText is a club plugin, may not be available
        nuxtApp.provide('SplitText', null)
      })

      // Try to import and register ScrollSmoother (club plugin - may not be available)
      import('gsap/ScrollSmoother').then((smootherModule) => {
        const ScrollSmoother = smootherModule.ScrollSmoother || smootherModule.default || null
        if (ScrollSmoother && gsap.registerPlugin) {
          gsap.registerPlugin(ScrollSmoother)
        }
        nuxtApp.provide('ScrollSmoother', ScrollSmoother || null)
      }).catch(() => {
        // ScrollSmoother is a club plugin, may not be available
        nuxtApp.provide('ScrollSmoother', null)
      })

      console.log('GSAP initialized successfully')
    }
  }
})
