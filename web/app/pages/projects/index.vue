<template>
  <div class="min-h-screen pt-24 pb-12">
    <div class="px-4 sm:px-6 lg:px-8 space-y-8 sm:space-y-10">
      <SectionsContentHeader :header-data="headerData" exit-animation-key="projectsHeader" />

      <SectionsContentLoadingState 
        v-if="pending" 
        loading-text="Loading projects..."
        exit-animation-key="projectsLoadingState"
      />
      <SectionsContentErrorState 
        v-else-if="error"
        title="Something went wrong"
        message="Unable to load projects right now. Please try again shortly."
        exit-animation-key="projectsErrorState"
      />
      <template v-else>
        <SectionsProjectsFeatured
          v-if="hasFeaturedProjects"
          :projects="featuredProjects"
        />

        <SectionsProjectsGrid
          v-if="hasRegularProjects"
          :projects="regularProjects"
        />

        <SectionsContentEmptyState
          v-else-if="!hasFeaturedProjects && !hasRegularProjects"
          icon="heroicons:folder"
          title="No projects yet"
          message="Check back soon for new projects!"
          exit-animation-key="projectsEmptyState"
        />
      </template>

      <!-- Call to Action -->
      <SectionsCallToAction :cta-data="ctaData" exit-animation-key="projectsCTA" />
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Project } from '~/../../shared/types'
import { runAllExitAnimations } from '~/composables/usePageExitAnimations'

const config = useRuntimeConfig()
const apiBase = config.public.apiBase

useHead({
  title: 'Projects | Judah Sullivan',
})

definePageMeta({
  pageTransition: {
    name: 'projects-page',
    mode: 'out-in',
    onLeave: async (_el: Element, done: () => void) => {
      if (import.meta.server) return done()
      try {
        await runAllExitAnimations()
      } catch (e) {
        console.warn('[Projects] Exit animation error:', e)
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
      import('gsap/ScrollTrigger').then((stModule) => {
        const ScrollTrigger = stModule.default || stModule
        if (ScrollTrigger) {
          setTimeout(() => {
            ScrollTrigger.refresh()
            setTimeout(() => ScrollTrigger.refresh(), 100)
            setTimeout(() => ScrollTrigger.refresh(), 300)
          }, 100)
        }
        done()
      }).catch(() => done())
    }
  }
})

const headerData = {
  title: 'Projects',
  description: 'A collection of my work, from web applications to design systems and everything in between.'
}

const { data: projects, pending, error } = await useFetch<Project[]>(
  `${apiBase}/api/projects`,
  {
    default: () => []
  }
)

const featuredProjects = computed(() => {
  return projects.value?.filter(project => project.isFeatured) || []
})

const regularProjects = computed(() => {
  return projects.value?.filter(project => !project.isFeatured) || []
})

const hasFeaturedProjects = computed(() => featuredProjects.value.length > 0)
const hasRegularProjects = computed(() => regularProjects.value.length > 0)

const ctaData = {
  icon: 'heroicons:document-text',
  title: 'Want to Read More?',
  description: 'Check out my latest articles on web development, design, and technology.',
  background: 'bg-foreground dark:bg-foreground/90 text-secondary dark:text-secondary',
  primaryButton: {
    text: 'Read My Blog',
    to: '/blog',
    icon: 'heroicons:document-text'
  },
  secondaryButton: {
    text: 'Get in Touch',
    to: '/contact',
    icon: 'heroicons:envelope'
  }
}
</script>
