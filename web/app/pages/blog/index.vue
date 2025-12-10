<template>
  <div class="min-h-screen pt-24 pb-12">
    <div class="px-4 sm:px-6 lg:px-8 space-y-8 sm:space-y-10">
      <SectionsContentHeader :header-data="headerData" exit-animation-key="blogHeader" />

      <SectionsContentLoadingState 
        v-if="pending" 
        loading-text="Loading blog posts..."
        exit-animation-key="blogLoadingState"
      />
      <SectionsContentErrorState 
        v-else-if="error"
        title="Something went wrong"
        message="Unable to load blog posts right now. Please try again shortly."
        exit-animation-key="blogErrorState"
      />
      <template v-else>
        <SectionsBlogFeatured
          v-if="hasFeaturedPosts"
          :posts="featuredPosts"
        />

        <SectionsBlogGrid
          v-if="hasRegularPosts"
          :posts="regularPosts"
        />

        <SectionsContentEmptyState
          v-else-if="!hasFeaturedPosts && !hasRegularPosts"
          icon="heroicons:document-text"
          title="No blog posts yet"
          message="Check back soon for new content!"
          exit-animation-key="blogEmptyState"
        />
      </template>

      <!-- Call to Action -->
      <SectionsCallToAction :cta-data="ctaData" exit-animation-key="blogCTA" />
    </div>
  </div>
</template>

<script setup lang="ts">
import type { BlogPost } from '~/../../shared/types'
import { runAllExitAnimations } from '~/composables/usePageExitAnimations'

const config = useRuntimeConfig()
const apiBase = config.public.apiBase

useHead({
  title: 'Blog | Judah Sullivan',
})

definePageMeta({
  pageTransition: {
    name: 'blog-page',
    mode: 'out-in',
    onLeave: async (_el: Element, done: () => void) => {
      if (import.meta.server) return done()
      try {
        await runAllExitAnimations()
      } catch (e) {
        console.warn('[Blog] Exit animation error:', e)
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
  title: 'Blog',
  description: 'Thoughts, tutorials, and insights on web development, design, and technology.'
}

const { data: posts, pending, error } = await useFetch<BlogPost[]>(
  `${apiBase}/api/blog`,
  {
    default: () => []
  }
)

const featuredPosts = computed(() => {
  return posts.value?.filter(post => post.isFeatured) || []
})

const regularPosts = computed(() => {
  return posts.value?.filter(post => !post.isFeatured) || []
})

const hasFeaturedPosts = computed(() => featuredPosts.value.length > 0)
const hasRegularPosts = computed(() => regularPosts.value.length > 0)

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
