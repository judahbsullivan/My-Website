<template>
  <div class="min-h-screen space-y-6 pt-18 pb-10">
    <div data-home-section="hero">
    <SectionsHero />
    </div>
    <div data-home-section="marquee">
    <SectionsMarqueeBanner />
    </div>
    <div data-home-section="what-i-do">
    <SectionsWhatIDoAndFacts />
    </div>
    <div data-home-section="featured">
    <SectionsFeaturedWork />
    </div>
  </div>
</template>

<script setup lang="ts">
import { runAllExitAnimations } from '../composables/usePageExitAnimations'

useHead({
  title: 'Home | Judah Sullivan',
})

// Page transition with component-level exit animations
definePageMeta({
  pageTransition: {
    name: 'home-page',
    mode: 'out-in',
    onLeave: async (_el: Element, done: () => void) => {
      // Client-only guard
      if (import.meta.server) return done()

      console.log('[PageTransition] onLeave triggered')
      
      try {
        await runAllExitAnimations()
        console.log('[PageTransition] Exit animations done, calling done()')
      } catch (e) {
        console.warn('[PageTransition] Exit animation error:', e)
      }
      
      done()
    },
    onEnter: async (_el: Element, done: () => void) => {
      if (import.meta.server) return done()
      
      // Scroll to top immediately before enter animation begins
      const nuxtApp = useNuxtApp()
      const smoother = (nuxtApp.$scrollSmoother as any) || (typeof window !== 'undefined' && (window as any).ScrollSmoother?.get())
      
      if (smoother && typeof smoother.scrollTop === 'function') {
        smoother.scrollTop(0) // Scroll to top
      } else {
        window.scrollTo({ top: 0, left: 0, behavior: 'instant' })
      }
      
      done()
    }
  }
})
</script>
