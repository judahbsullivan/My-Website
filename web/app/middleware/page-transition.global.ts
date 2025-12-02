export default defineNuxtRouteMiddleware((to, from) => {
  if (import.meta.server || to.path === from?.path) return
  
  // Get GSAP from the app context
  const nuxtApp = useNuxtApp()
  const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
  
  if (!gsap) return
  
  // Set page transition with GSAP hooks as per Nuxt docs
  to.meta.pageTransition = {
    name: 'page',
    mode: 'out-in',
    onLeave: (el: Element, done: () => void) => {
      const overlay = document.querySelector('.page-transition-overlay') as HTMLElement
      if (!overlay) {
        done()
        return
      }
      
      // Animate overlay covering from bottom
      gsap.to(overlay, {
        scaleY: 1,
        transformOrigin: 'bottom center',
        duration: 1,
        ease: 'power3.inOut',
        onComplete: done
      })
    },
    onEnter: (el: Element, done: () => void) => {
      // Page is ready, call done immediately
      done()
      
      const overlay = document.querySelector('.page-transition-overlay') as HTMLElement
      if (!overlay) return
      
      // Small delay then reveal from top
      setTimeout(() => {
        gsap.to(overlay, {
          scaleY: 0,
          transformOrigin: 'top center',
          duration: 1,
          ease: 'power3.inOut'
        })
      }, 50)
    },
  }
})

