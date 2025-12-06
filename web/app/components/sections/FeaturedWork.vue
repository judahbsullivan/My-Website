<template>
  <!-- Creative Grid Section -->
  <section class="w-full">
    <div class="px-4 sm:px-6">
      <UiBentoBox
        ref="sectionBoxRef"
        container
        backdrop
        border
        shadow
        padding="lg"
        rounded="2xl"
        :className="`${homepageData.sections.featuredWork.background} opacity-0 scale-95 translate-y-6`"
      >
        <!-- Header -->
        <div class="mb-6">
          <UiTitle ref="sectionTitleRef" as="h2" size="md" weight="semibold" class="mb-3">
            Featured Work
          </UiTitle>
          <p ref="sectionDescRef" class="text-sm text-muted-foreground max-w-2xl leading-relaxed">
            Explore my latest projects and creative solutions
          </p>
        </div>

        <!-- Creative Grid Layout -->
        <div v-if="pending" class="grid grid-cols-1 md:grid-cols-3 gap-4">
          <div v-for="i in 5" :key="i" class="aspect-video rounded-xl bg-muted animate-pulse" />
        </div>
        <div v-else-if="error" class="text-center py-12">
          <Icon name="heroicons:exclamation-triangle" class="w-16 h-16 text-muted-foreground mx-auto mb-4" />
          <p class="text-muted-foreground">Unable to load featured projects</p>
          <p class="text-xs text-muted-foreground mt-2">API may be unavailable</p>
        </div>
        <div v-else-if="featuredProjects && featuredProjects.length > 0" class="grid grid-cols-1 md:grid-cols-3 gap-4">
        <!-- Large Featured Card (First Project) -->
        <NuxtLink
          v-if="featuredProjects[0]"
          :to="`/projects/${featuredProjects[0].slug}`"
          class="md:col-span-2 aspect-video rounded-xl overflow-hidden group relative"
        >
          <UiBentoBox
            :variant="getVariant(0)"
            size="full"
            padding="none"
            rounded="xl"
            className="h-full flex-col items-start justify-end p-6 relative"
            hover
          >
            <div v-if="featuredProjects[0].imageUrl" class="absolute inset-0 z-0 rounded-xl overflow-hidden">
              <img
                :src="getImageUrl(featuredProjects[0].imageUrl)"
                :alt="featuredProjects[0].title"
                class="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300"
              />
              <div class="absolute inset-0 bg-gradient-to-t from-black/80 via-black/40 to-transparent" />
            </div>
            <div class="relative z-10 w-full">
              <div class="flex items-center gap-2 mb-2">
                <span class="px-2 py-1 text-xs font-medium rounded-full bg-primary/30 text-primary backdrop-blur-sm">
                  {{ featuredProjects[0].category }}
                </span>
                <span v-if="featuredProjects[0].isFeatured" class="px-2 py-1 text-xs font-medium rounded-full bg-amber-500/30 text-amber-300 backdrop-blur-sm">
                  Featured
                </span>
              </div>
              <h3 class="text-xl font-bold text-white mb-2 group-hover:text-primary transition-colors line-clamp-2">
                {{ featuredProjects[0].title }}
              </h3>
              <p class="text-sm text-white/80 line-clamp-2">
                {{ featuredProjects[0].description }}
              </p>
            </div>
          </UiBentoBox>
        </NuxtLink>

        <!-- Small Card 1 (Second Project) -->
        <NuxtLink
          v-if="featuredProjects[1]"
          :to="`/projects/${featuredProjects[1].slug}`"
          class="aspect-square rounded-xl overflow-hidden group relative"
        >
          <UiBentoBox
            :variant="getVariant(1)"
            size="full"
            padding="none"
            rounded="xl"
            className="h-full flex-col items-start justify-end p-4 relative"
            hover
          >
            <div v-if="featuredProjects[1].imageUrl" class="absolute inset-0 z-0 rounded-xl overflow-hidden">
              <img
                :src="getImageUrl(featuredProjects[1].imageUrl)"
                :alt="featuredProjects[1].title"
                class="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300"
              />
              <div class="absolute inset-0 bg-gradient-to-t from-black/80 via-black/40 to-transparent" />
            </div>
            <div class="relative z-10 w-full">
              <span class="px-2 py-1 text-xs font-medium rounded-full bg-primary/30 text-primary backdrop-blur-sm mb-2 inline-block">
                {{ featuredProjects[1].category }}
              </span>
              <h3 class="text-sm font-bold text-white mb-1.5 group-hover:text-primary transition-colors line-clamp-2">
                {{ featuredProjects[1].title }}
              </h3>
              <p class="text-xs text-white/70 line-clamp-2">
                {{ featuredProjects[1].description }}
              </p>
            </div>
          </UiBentoBox>
        </NuxtLink>

        <!-- Small Card 2 (Third Project) -->
        <NuxtLink
          v-if="featuredProjects[2]"
          :to="`/projects/${featuredProjects[2].slug}`"
          class="aspect-square rounded-xl overflow-hidden group relative"
        >
          <UiBentoBox
            :variant="getVariant(2)"
            size="full"
            padding="none"
            rounded="xl"
            className="h-full flex-col items-start justify-end p-4 relative"
            hover
          >
            <div v-if="featuredProjects[2].imageUrl" class="absolute inset-0 z-0 rounded-xl overflow-hidden">
              <img
                :src="getImageUrl(featuredProjects[2].imageUrl)"
                :alt="featuredProjects[2].title"
                class="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300"
              />
              <div class="absolute inset-0 bg-gradient-to-t from-black/80 via-black/40 to-transparent" />
            </div>
            <div class="relative z-10 w-full">
              <span class="px-2 py-1 text-xs font-medium rounded-full bg-primary/30 text-primary backdrop-blur-sm mb-2 inline-block">
                {{ featuredProjects[2].category }}
              </span>
              <h3 class="text-sm font-bold text-white mb-1.5 group-hover:text-primary transition-colors line-clamp-2">
                {{ featuredProjects[2].title }}
              </h3>
              <p class="text-xs text-white/70 line-clamp-2">
                {{ featuredProjects[2].description }}
              </p>
            </div>
          </UiBentoBox>
        </NuxtLink>

        <!-- Medium Card (Fourth Project) -->
        <NuxtLink
          v-if="featuredProjects[3]"
          :to="`/projects/${featuredProjects[3].slug}`"
          class="md:col-span-2 aspect-video rounded-xl overflow-hidden group relative"
        >
          <UiBentoBox
            :variant="getVariant(3)"
            size="full"
            padding="none"
            rounded="xl"
            className="h-full flex-col items-start justify-end p-6 relative"
            hover
          >
            <div v-if="featuredProjects[3].imageUrl" class="absolute inset-0 z-0 rounded-xl overflow-hidden">
              <img
                :src="getImageUrl(featuredProjects[3].imageUrl)"
                :alt="featuredProjects[3].title"
                class="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300"
              />
              <div class="absolute inset-0 bg-gradient-to-t from-black/80 via-black/40 to-transparent" />
            </div>
            <div class="relative z-10 w-full">
              <div class="flex items-center gap-2 mb-2">
                <span class="px-2 py-1 text-xs font-medium rounded-full bg-primary/30 text-primary backdrop-blur-sm">
                  {{ featuredProjects[3].category }}
                </span>
              </div>
              <h3 class="text-lg font-bold text-white mb-2 group-hover:text-primary transition-colors line-clamp-2">
                {{ featuredProjects[3].title }}
              </h3>
              <p class="text-sm text-white/80 line-clamp-2">
                {{ featuredProjects[3].description }}
              </p>
            </div>
          </UiBentoBox>
        </NuxtLink>

        <!-- Small Card 3 (Fifth Project) -->
        <NuxtLink
          v-if="featuredProjects[4]"
          :to="`/projects/${featuredProjects[4].slug}`"
          class="aspect-square rounded-xl overflow-hidden group relative"
        >
          <UiBentoBox
            :variant="getVariant(4)"
            size="full"
            padding="none"
            rounded="xl"
            className="h-full flex-col items-start justify-end p-4 relative"
            hover
          >
            <div v-if="featuredProjects[4].imageUrl" class="absolute inset-0 z-0 rounded-xl overflow-hidden">
              <img
                :src="getImageUrl(featuredProjects[4].imageUrl)"
                :alt="featuredProjects[4].title"
                class="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300"
              />
              <div class="absolute inset-0 bg-gradient-to-t from-black/80 via-black/40 to-transparent" />
            </div>
            <div class="relative z-10 w-full">
              <span class="px-2 py-1 text-xs font-medium rounded-full bg-primary/30 text-primary backdrop-blur-sm mb-2 inline-block">
                {{ featuredProjects[4].category }}
              </span>
              <h3 class="text-sm font-bold text-white mb-1.5 group-hover:text-primary transition-colors line-clamp-2">
                {{ featuredProjects[4].title }}
              </h3>
              <p class="text-xs text-white/70 line-clamp-2">
                {{ featuredProjects[4].description }}
              </p>
            </div>
          </UiBentoBox>
        </NuxtLink>
      </div>
        <div v-else-if="!pending && !error" class="text-center py-12">
          <Icon name="heroicons:folder" class="w-16 h-16 text-muted-foreground mx-auto mb-4" />
          <p class="text-muted-foreground">No featured projects yet</p>
        </div>
      </UiBentoBox>
    </div>
  </section>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted, nextTick } from 'vue'
import { useNuxtApp } from '#app'
import type { Project } from '~/../../shared/types'
import homepageData from '../../data/homepage.json'

const config = useRuntimeConfig()
const apiBase = config.public.apiBase
const sectionBoxRef = ref<any>(null)
const sectionTitleRef = ref<any>(null)
const sectionDescRef = ref<HTMLElement | null>(null)

// Fetch featured projects
const { data: projects, pending, error } = await useFetch<Project[]>(
  `${apiBase}/api/projects/featured`,
  {
    default: () => [],
    server: false, // Fetch on client-side only to avoid blocking SSR
    lazy: true // Don't block page rendering
  }
)

const featuredProjects = computed(() => {
  return projects.value?.slice(0, 5) || []
})

// Get full image URL
const getImageUrl = (url?: string | null): string => {
  if (!url) return ''
  if (url.startsWith('http://') || url.startsWith('https://')) {
    return url
  }
  return `${apiBase}${url}`
}

// Get variant based on index to cycle through different colors
const getVariant = (index: number): 'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'default' => {
  const variants: Array<'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'default'> = [
    'primary',
    'secondary',
    'success',
    'info',
    'warning',
    'default'
  ]
  return variants[index % variants.length] || 'default'
}

// ScrollTrigger animations matching Hero style
let scrollTriggers: any[] = []

const resolveEl = (maybeEl: any): HTMLElement | null => {
  if (!maybeEl) return null
  if (maybeEl instanceof HTMLElement) return maybeEl
  if ((maybeEl as any)?.$el instanceof HTMLElement) return (maybeEl as any).$el
  if ((maybeEl as any)?.el instanceof HTMLElement) return (maybeEl as any).el
  return null
}

function setupScrollAnimation() {
  if (import.meta.server) return
  
  const nuxtApp = useNuxtApp()
  const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
  const SplitText = nuxtApp.$SplitText as any
  
  if (!gsap) return
  
  // Import ScrollTrigger
  import('gsap/ScrollTrigger').then((stModule) => {
    const ScrollTrigger = stModule.default || stModule
    if (ScrollTrigger && gsap.registerPlugin) {
      gsap.registerPlugin(ScrollTrigger)
    }
    
    nextTick(() => {
      const box = resolveEl(sectionBoxRef.value)
      const titleComponent = sectionTitleRef.value as any
      const title = titleComponent?.el || titleComponent?.$el || (box?.querySelector('h2') as HTMLElement)
      const desc = sectionDescRef.value
      
      if (!box || !title) return
      
      // Split title with mask (matching Hero)
      let titleSplit: any = null
      try {
        if (SplitText) {
          gsap.set(title, { lineHeight: '0.9' })
          titleSplit = new SplitText(title, {
            type: 'chars',
            mask: 'chars',
            smartWrap: true,
            charsClass: 'char++',
          })
          if (titleSplit.chars && titleSplit.chars.length > 0) {
            titleSplit.chars.forEach((char: HTMLElement) => {
              gsap.set(char, {
                opacity: 0,
                yPercent: 120,
                rotationX: -90,
                lineHeight: '0.9'
              })
            })
          }
        }
      } catch (e) {
        titleSplit = null
      }
      
      // Split description with lines mask
      let descSplit: any = null
      try {
        if (SplitText && desc) {
          descSplit = new SplitText(desc, {
            type: 'lines',
            mask: 'lines',
            smartWrap: true,
            linesClass: 'line++',
          })
          if (descSplit.lines && descSplit.lines.length > 0) {
            gsap.set(descSplit.lines, {
              opacity: 0,
              yPercent: 100
            })
          }
        }
      } catch (e) {
        descSplit = null
      }
      
      // Create timeline with ScrollTrigger
      const tl = gsap.timeline({
        scrollTrigger: {
          trigger: box,
          start: 'top 80%',
          once: true
        }
      })
      
      // 1. Fade in container with scale (matching Hero/IntroLoader exactly)
      // Animate from Tailwind initial state (opacity-0 scale-95 translate-y-6)
      tl.fromTo(box, 
        {
          opacity: 0,
          scale: 0.95,
          y: 24
        },
        {
          opacity: 1,
          scale: 1,
          y: 0,
          duration: 0.6,
          ease: 'power3.out'
        }
      )
      
      // 2. Animate title chars with stagger and 3D rotation (matching Hero/IntroLoader exactly)
      if (titleSplit && titleSplit.chars) {
        tl.to(titleSplit.chars, {
          opacity: 1,
          yPercent: 0,
          rotationX: 0,
          duration: 0.5,
          stagger: {
            amount: 0.4,
            from: 'start'
          },
          ease: 'power3.out'
        }, '-=0.3')
      }
      
      // 3. Animate description lines (matching Hero paragraph exactly)
      if (descSplit && descSplit.lines) {
        tl.to(descSplit.lines, {
          opacity: 1,
          yPercent: 0,
          duration: 0.5,
          stagger: {
            amount: 0.3,
            from: 'start'
          },
          ease: 'power2.out'
        }, '-=0.3')
      } else if (desc) {
        tl.fromTo(desc,
          {
            opacity: 0,
            y: 10
          },
          {
            opacity: 1,
            y: 0,
            duration: 0.4,
            ease: 'power2.out'
          }, '-=0.2')
      }
      
      scrollTriggers.push(tl)
    })
  }).catch(() => {
    console.warn('ScrollTrigger not available')
  })
}

onMounted(() => {
  setupScrollAnimation()
})

onUnmounted(() => {
  // Cleanup ScrollTriggers
  scrollTriggers.forEach((st) => {
    if (st?.scrollTrigger) st.scrollTrigger.kill()
    if (st?.kill) st.kill()
  })
  scrollTriggers = []
})
</script>

<style scoped>
/* SplitText mask styles - prevent line-height changes */
:deep(.char) {
  display: inline-block;
  overflow: hidden;
  vertical-align: baseline;
  transform-style: preserve-3d;
  line-height: 0.9;
  white-space: pre-wrap;
}

:deep(.line) {
  overflow: hidden;
}

/* Ensure title maintains line-height */
:deep(h2) {
  line-height: 0.9 !important;
  display: block;
}
</style>

