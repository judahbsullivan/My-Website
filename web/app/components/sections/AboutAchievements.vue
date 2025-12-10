<template>
  <div ref="gridRef" class="grid grid-cols-1 md:grid-cols-3 gap-4 sm:gap-6 mb-6 opacity-0 scale-90 -rotate-3">
    <UiBentoBox
      v-for="achievement in achievementsData.achievements"
      :key="achievement.title"
      :variant="achievement.variant as 'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'default'"
      size="lg"
      padding="lg"
      :ref="el => { if (el) achievementRefs.push(el as any) }"
      :className="`flex-col items-start gap-3 ${getAchievementColor(achievement.title)} opacity-0 scale-80 translate-y-4 -rotate-3`"
    >
      <div class="flex items-center gap-3 w-full">
        <UiBentoBoxIcon :icon="achievement.icon" :variant="achievement.variant as 'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'default'" size="lg" />
        <h4 class="text-base font-semibold text-foreground">{{ achievement.title }}</h4>
      </div>
      <p class="text-sm text-muted-foreground">{{ achievement.description }}</p>
    </UiBentoBox>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted, nextTick } from 'vue'
import { registerExitAnimation, unregisterExitAnimation } from '../../composables/usePageExitAnimations'
import { useGSAP } from '../../composables/useGSAP'

interface Props {
  achievementsData: {
    achievements: Array<{
      title: string
      description: string
      icon: string
      variant: string
    }>
  }
}

const props = defineProps<Props>()

const gridRef = ref<HTMLElement | null>(null)
const achievementRefs = ref<(any | null)[]>([])

let scrollTriggers: any[] = []
let storedElements: { grid: HTMLElement | null; items: HTMLElement[] } = {
  grid: null,
  items: []
}

const getAchievementColor = (achievementTitle: string): string => {
  const achievementColors: Record<string, string> = {
    'Open Source': 'bg-amber-500/10',
    'Blog Writer': 'bg-green-500/10',
    'Community': 'bg-blue-500/10'
  }
  return achievementColors[achievementTitle] || ''
}

const resolveEl = (maybeEl: any): HTMLElement | null => {
  if (!maybeEl) return null
  if (maybeEl instanceof HTMLElement) return maybeEl
  if ((maybeEl as any)?.$el instanceof HTMLElement) return (maybeEl as any).$el
  if ((maybeEl as any)?.el instanceof HTMLElement) return (maybeEl as any).el
  return null
}

async function setupScrollAnimation() {
  if (import.meta.server) return

  const { waitForReady } = useGSAP()
  const { gsap: readyGsap, ScrollTrigger: readyST } = await waitForReady()
  if (!readyGsap || !readyST) {
    console.warn('GSAP or ScrollTrigger not available')
    return
  }

  await nextTick()

  const grid = gridRef.value
  const items = achievementRefs.value.map(resolveEl).filter(Boolean) as HTMLElement[]
  if (!grid) return

  readyGsap.set(grid, { opacity: 0, scale: 0.9, rotation: -3 })
  if (items.length > 0) readyGsap.set(items, { opacity: 0, y: 20, scale: 0.9, rotation: -3 })

  const tl = readyGsap.timeline({
    scrollTrigger: {
      trigger: grid,
      start: 'top 80%',
      once: true
    }
  })

  tl.to(grid, {
    opacity: 1,
    scale: 1,
    rotation: 0,
    duration: 0.7,
    ease: 'back.out(1.4)'
  })

  if (items.length > 0) {
    tl.to(items, {
      opacity: 1,
      y: 0,
      scale: 1,
      rotation: 0,
      duration: 0.45,
      stagger: 0.06,
      ease: 'power2.out'
    }, '-=0.35')
  }

  scrollTriggers.push(tl)
  storedElements = { grid, items }
  setTimeout(() => readyST.refresh(), 100)
}

function animateExit(): Promise<void> {
  return new Promise((resolve) => {
    if (import.meta.server) return resolve()
    const { gsap: readyGsap } = useGSAP()
    if (!readyGsap) return resolve()

    const grid = storedElements.grid || gridRef.value
    const items = storedElements.items && storedElements.items.length > 0
      ? storedElements.items
      : achievementRefs.value.map(resolveEl).filter(Boolean) as HTMLElement[]

    if (!grid) return resolve()

    const tl = readyGsap.timeline({ onComplete: () => resolve() })
    const contentDuration = 0.5

    if (items.length > 0) {
      tl.to(items, {
        y: -10,
        scale: 0,
        autoAlpha: 0,
        duration: contentDuration,
        stagger: 0.05,
        ease: 'back.in(1.7)'
      }, 0)
    }

    const containerDelay = contentDuration * 0.85
    tl.to(grid, {
      autoAlpha: 0,
      scale: 0.85,
      y: -30,
      rotation: 4,
      duration: 0.5,
      ease: 'power2.in'
    }, containerDelay)
  })
}

onMounted(() => {
  if (import.meta.server) return
  registerExitAnimation('aboutAchievements', animateExit)
  setupScrollAnimation()
})

onUnmounted(() => {
  if (import.meta.server) return
  unregisterExitAnimation('aboutAchievements')
  scrollTriggers.forEach((st) => {
    if (st?.scrollTrigger) st.scrollTrigger.kill()
    if (st?.kill) st.kill()
  })
  scrollTriggers = []
  storedElements = { grid: null, items: [] }
  achievementRefs.value = []
})
</script>
