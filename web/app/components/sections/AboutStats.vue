<template>
  <UiBentoBox
    ref="bentoBoxRef"
    container
    backdrop
    border
    shadow
    padding="lg"
    rounded="2xl"
    :className="`${statsData.background} opacity-0 scale-80 translate-y-8 -rotate-5`"
  >
    <h3 
      ref="titleRef"
      class="text-lg font-semibold text-foreground mb-6 opacity-0 translate-y-2.5"
    >
      {{ statsData.title }}
    </h3>
    <div ref="statsRef" class="space-y-6">
      <div 
        v-for="(stat, index) in statsData.stats" 
        :key="stat.label" 
        :ref="el => { if (el) statItemRefs[index] = el as HTMLElement }"
        class="flex items-center justify-between opacity-0 translate-y-2.5"
      >
        <div class="flex items-center gap-3">
          <div :class="[
            'w-10 h-10 rounded-lg flex items-center justify-center',
            stat.color === 'primary' ? 'bg-primary/20' : 
            stat.color === 'secondary' ? 'bg-blue-500/20' : 
            stat.color === 'success' ? 'bg-success/20' : 
            'bg-amber-500/20'
          ]">
            <Icon :name="stat.icon" :class="[
              'w-5 h-5',
              stat.color === 'primary' ? 'text-primary' : 
              stat.color === 'secondary' ? 'text-blue-500' : 
              stat.color === 'success' ? 'text-success' : 
              'text-amber-500'
            ]" />
          </div>
          <span class="text-sm text-muted-foreground">{{ stat.label }}</span>
        </div>
        <p class="text-lg font-bold text-foreground">
          <template v-if="stat.label === 'GitHub Repos' && githubStats">
            {{ githubStats.public_repos }}+
          </template>
          <template v-else>
            {{ stat.value }}
          </template>
        </p>
      </div>
      <div 
        v-if="githubStats" 
        ref="githubStatsRef"
        class="pt-4 border-t border-border/50 opacity-0 translate-y-2.5"
      >
        <div class="flex items-center justify-between text-xs text-muted-foreground">
          <span>Followers</span>
          <span class="font-semibold text-foreground">{{ githubStats.followers }}</span>
        </div>
        <div class="flex items-center justify-between text-xs text-muted-foreground mt-2">
          <span>Following</span>
          <span class="font-semibold text-foreground">{{ githubStats.following }}</span>
        </div>
      </div>
    </div>
  </UiBentoBox>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted, watch, nextTick } from 'vue'
import { useNuxtApp } from '#app'
import { useIntroLoader } from '../../composables/useIntroLoader'
import { registerExitAnimation, unregisterExitAnimation } from '../../composables/usePageExitAnimations'

interface Props {
  statsData: {
    title: string
    background: string
    stats: Array<{
      label: string
      value: string
      icon: string
      color: string
    }>
  }
  githubStats?: {
    avatar_url: string
    public_repos: number
    followers: number
    following: number
  } | null
}

defineProps<Props>()

const { isIntroLoaderComplete } = useIntroLoader()
const bentoBoxRef = ref<any>(null)
const titleRef = ref<HTMLElement | null>(null)
const statsRef = ref<HTMLElement | null>(null)
const statItemRefs = ref<(HTMLElement | null)[]>([])
const githubStatsRef = ref<HTMLElement | null>(null)

let hasAnimated = false
let storedElements: {
  bentoBox: HTMLElement | null
  title: HTMLElement | null
  statItems: HTMLElement[]
  githubStats: HTMLElement | null
} = {
  bentoBox: null,
  title: null,
  statItems: [],
  githubStats: null
}

function animateStats() {
  if (import.meta.server || hasAnimated) return

  const nuxtApp = useNuxtApp()
  const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
  
  if (!gsap) {
    hasAnimated = true
    return
  }
  
  nextTick(() => {
    const bentoBox = bentoBoxRef.value?.el || bentoBoxRef.value?.$el || bentoBoxRef.value
    const title = titleRef.value
    const stats = statsRef.value
    
    if (!bentoBox || !title || !stats) {
      setTimeout(() => animateStats(), 50)
      return
    }
    
    // Create master timeline (matching IntroLoader pattern)
    const tl = gsap.timeline()
    
    // 1. Animate bento box with bounce (like IntroLoader nameBox)
    tl.fromTo(bentoBox,
      {
        opacity: 0,
        scale: 0.8,
        y: 30,
        rotation: -5
      },
      {
        opacity: 1,
        scale: 1,
        y: 0,
        rotation: 0,
        duration: 0.6,
        ease: 'back.out(1.4)'
      }
    )
    
    // 2. Animate title with subtle fade
    tl.fromTo(title,
      {
        opacity: 0,
        y: 10
      },
      {
        opacity: 1,
        y: 0,
        duration: 0.4,
        ease: 'power2.out'
      }, '-=0.3')
    
    // 3. Animate stat items with stagger (like IntroLoader text elements)
    const statItems = statItemRefs.value.filter(Boolean) as HTMLElement[]
    const allTextElements = [
      ...statItems,
      ...(githubStatsRef.value ? [githubStatsRef.value] : [])
    ].filter(Boolean) as HTMLElement[]
    
    storedElements = {
      bentoBox,
      title,
      statItems,
      githubStats: githubStatsRef.value
    }
    
    tl.fromTo(allTextElements,
      {
        opacity: 0,
        y: 10
      },
      {
        opacity: 1,
        y: 0,
        duration: 0.35,
        stagger: 0.06,
        ease: 'power2.out'
      }, '-=0.4')
    
    hasAnimated = true
  })
}

function animateExit(): Promise<void> {
  return new Promise((resolve) => {
    if (import.meta.server) return resolve()
    const nuxtApp = useNuxtApp()
    const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
    if (!gsap) return resolve()
    
    const bentoBox = storedElements.bentoBox || bentoBoxRef.value?.el || bentoBoxRef.value?.$el || bentoBoxRef.value
    const title = storedElements.title || titleRef.value
    const statItems = storedElements.statItems && storedElements.statItems.length > 0
      ? storedElements.statItems
      : (statItemRefs.value.filter(Boolean) as HTMLElement[])
    const githubStatsEl = storedElements.githubStats || githubStatsRef.value
    
    if (!bentoBox) return resolve()
    
    const tl = gsap.timeline({
      onComplete: () => resolve()
    })
    
    const contentDuration = 0.5
    
    // Title out
    if (title) {
      tl.to(title, { y: -10, autoAlpha: 0, duration: contentDuration, ease: 'power2.in' }, 0)
    }
    
    // Stats out
    const allTextElements = [
      ...statItems,
      ...(githubStatsEl ? [githubStatsEl] : [])
    ].filter(Boolean) as HTMLElement[]
    if (allTextElements.length > 0) {
      tl.to(allTextElements, {
        y: -12,
        autoAlpha: 0,
        duration: contentDuration,
        stagger: 0.06,
        ease: 'power2.in'
      }, 0)
    }
    
    // Container out after content
    const containerDelay = contentDuration * 0.85
    tl.to(bentoBox, {
      autoAlpha: 0,
      scale: 0.85,
      y: -30,
      rotation: 4,
      duration: 0.5,
      ease: 'power2.in'
    }, containerDelay)
  })
}

// Watch for intro loader completion
watch(isIntroLoaderComplete, (complete) => {
  if (complete && !hasAnimated) {
    setTimeout(() => {
      animateStats()
    }, 150)
  }
})

onMounted(() => {
  registerExitAnimation('aboutStats', animateExit)
  
  nextTick(() => {
    if (!storedElements.bentoBox) {
      const bentoBox = bentoBoxRef.value?.el || bentoBoxRef.value?.$el || bentoBoxRef.value
      storedElements = {
        bentoBox,
        title: titleRef.value,
        statItems: statItemRefs.value.filter(Boolean) as HTMLElement[],
        githubStats: githubStatsRef.value
      }
    }
  })
  
  if (isIntroLoaderComplete.value && !hasAnimated) {
    setTimeout(() => {
      animateStats()
    }, 150)
  }
})

onUnmounted(() => {
  unregisterExitAnimation('aboutStats')
  hasAnimated = false
  storedElements = {
    bentoBox: null,
    title: null,
    statItems: [],
    githubStats: null
  }
})
</script>


