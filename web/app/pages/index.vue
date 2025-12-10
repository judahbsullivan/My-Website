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
    }
  }
})
</script>
