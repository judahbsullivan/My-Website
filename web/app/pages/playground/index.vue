<template>
  <div class="min-h-screen pt-24 pb-12">
    <div class="px-4 sm:px-6 lg:px-8 space-y-8 sm:space-y-10">
      <SectionsContentHeader :header-data="headerData" exit-animation-key="playgroundHeader" />

      <SectionsContentLoadingState 
        v-if="pending" 
        loading-text="Loading playground items..."
        exit-animation-key="playgroundLoadingState"
      />
      <SectionsContentErrorState 
        v-else-if="error"
        title="Something went wrong"
        message="Unable to load playground items right now. Please try again shortly."
        exit-animation-key="playgroundErrorState"
      />
      <template v-else>
        <SectionsPlaygroundFeatured
          v-if="hasFeaturedPlaygrounds"
          :playgrounds="featuredPlaygrounds"
        />

        <SectionsPlaygroundGrid
          v-if="hasRegularPlaygrounds"
          :playgrounds="regularPlaygrounds"
        />

        <SectionsContentEmptyState
          v-else-if="!hasFeaturedPlaygrounds && !hasRegularPlaygrounds"
          icon="heroicons:code-bracket"
          title="No playground items yet"
          message="Check back soon for new content!"
          exit-animation-key="playgroundEmptyState"
        />
      </template>

      <!-- Call to Action -->
      <SectionsCallToAction :cta-data="ctaData" exit-animation-key="playgroundCTA" />
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Playground } from '~/../../shared/types'
import { runAllExitAnimations } from '~/composables/usePageExitAnimations'

const config = useRuntimeConfig()
const apiBase = config.public.apiBase

useHead({
  title: 'Playground | Judah Sullivan',
})

definePageMeta({
  pageTransition: {
    name: 'playground-page',
    mode: 'out-in',
    onLeave: async (_el: Element, done: () => void) => {
      if (import.meta.server) return done()
      try {
        await runAllExitAnimations()
      } catch (e) {
        console.warn('[Playground] Exit animation error:', e)
      }
      done()
    },
    onEnter: async (_el: Element, done: () => void) => {
      if (import.meta.server) return done()
      
      // Scroll to top immediately before enter animation begins
      const nuxtApp = useNuxtApp()
      const smoother = (nuxtApp.$scrollSmoother as any) || (typeof window !== 'undefined' && (window as any).ScrollSmoother?.get())
      
      if (smoother) {
        smoother.scrollTo(0, true) // true = immediate scroll
      } else {
        window.scrollTo({ top: 0, left: 0, behavior: 'instant' })
      }
      
      // Refresh ScrollTrigger when page enters
      const { ScrollTrigger } = useGSAP()
        if (ScrollTrigger) {
        nextTick(() => {
            ScrollTrigger.refresh()
            setTimeout(() => ScrollTrigger.refresh(), 100)
            setTimeout(() => ScrollTrigger.refresh(), 300)
        })
        }
        done()
    }
  }
})

const headerData = {
  title: 'Animation Playground',
  description: 'Explore interactive components, animations, and code snippets. Copy, paste, and customize to your heart\'s content.'
}

// Fetch all playgrounds
const { data: playgrounds, pending, error } = await useFetch<Playground[]>(
  `${apiBase}/api/playground`,
  {
    default: () => []
  }
)

const featuredPlaygrounds = computed(() => {
  return playgrounds.value?.filter(playground => playground.isFeatured) || []
})

const regularPlaygrounds = computed(() => {
  return playgrounds.value?.filter(playground => !playground.isFeatured) || []
})

const hasFeaturedPlaygrounds = computed(() => featuredPlaygrounds.value.length > 0)
const hasRegularPlaygrounds = computed(() => regularPlaygrounds.value.length > 0)

const ctaData = {
  icon: 'heroicons:folder',
  title: 'Have a Project in Mind?',
  description: 'Let\'s work together to bring your ideas to life. I\'m always open to discussing new opportunities.',
  background: 'bg-foreground dark:bg-foreground/90 text-secondary dark:text-secondary',
  primaryButton: {
    text: 'View My Work',
    to: '/projects',
    icon: 'heroicons:folder'
  },
  secondaryButton: {
    text: 'Get in Touch',
    to: '/contact',
    icon: 'heroicons:envelope'
  }
}
</script>
