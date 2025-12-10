<template>
  <UiBentoBox
    ref="bentoBoxRef"
    container
    backdrop
    border
    shadow
    padding="lg"
    rounded="2xl"
    :className="`mb-6 ${interestsData.background} opacity-0 scale-90 -rotate-3`"
  >
    <div class="flex items-center justify-between mb-6">
      <h3 ref="titleRef" class="text-xl font-bold text-foreground opacity-0">{{ interestsData.title }}</h3>
      <Icon name="heroicons:sparkles" class="w-6 h-6 text-primary" />
    </div>
    <div class="grid grid-cols-2 md:grid-cols-4 gap-4">
      <UiBentoBox
        v-for="interest in interestsData.interests"
        :key="interest.name"
        :variant="interest.variant as 'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'default'"
        size="md"
        padding="md"
        :ref="el => { if (el) interestItemRefs.push(el as any) }"
        :className="`flex-col items-center justify-center gap-2 ${getInterestColor(interest.name)} opacity-0 scale-80 translate-y-4 -rotate-3`"
      >
        <UiBentoBoxIcon 
          v-if="interest.name !== 'Coding'"
          :icon="interest.icon" 
          :variant="interest.variant as 'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info' | 'default'" 
        />
        <div v-else class="w-8 h-8 rounded-lg flex items-center justify-center bg-purple-500/20">
          <Icon :name="interest.icon" class="w-4 h-4 text-purple-500" />
        </div>
        <p class="text-xs font-medium text-foreground text-center">{{ interest.name }}</p>
        <p v-if="interest.description" class="text-xs text-muted-foreground text-center mt-1">{{ interest.description }}</p>
      </UiBentoBox>
    </div>
  </UiBentoBox>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted, nextTick } from 'vue'
import { registerExitAnimation, unregisterExitAnimation } from '../../composables/usePageExitAnimations'
import { useGSAP } from '../../composables/useGSAP'

interface Props {
  interestsData: {
    title: string
    background: string
    interests: Array<{
      name: string
      icon: string
      variant: string
      description?: string
    }>
  }
}

const props = defineProps<Props>()

const bentoBoxRef = ref<any>(null)
const titleRef = ref<HTMLElement | null>(null)
const interestItemRefs = ref<(any | null)[]>([])

let scrollTriggers: any[] = []
let storedElements: { bentoBox: HTMLElement | null; title: HTMLElement | null; items: HTMLElement[] } = {
  bentoBox: null,
  title: null,
  items: []
}

const getInterestColor = (interestName: string): string => {
  const interestColors: Record<string, string> = {
    'Coding': 'bg-purple-500/10',
    'Guitar': 'bg-blue-500/10',
    'Basketball': 'bg-orange-500/10',
    'Family': 'bg-pink-500/10'
  }
  return interestColors[interestName] || ''
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
  const { gsap: readyGsap, ScrollTrigger: readyST, SplitText } = await waitForReady()
  if (!readyGsap || !readyST) {
    console.warn('GSAP or ScrollTrigger not available')
    return
  }

  await nextTick()

      const bentoBox = resolveEl(bentoBoxRef.value)
      const title = titleRef.value
      const items = interestItemRefs.value.map(resolveEl).filter(Boolean) as HTMLElement[]
      if (!bentoBox || !title) return

  readyGsap.set(bentoBox, { opacity: 0, scale: 0.9, rotation: -3 })
  readyGsap.set(title, { opacity: 0, y: 20 })
  if (items.length > 0) readyGsap.set(items, { opacity: 0, y: 20, scale: 0.9, rotation: -3 })

      let titleSplit: any = null
      try {
        if (SplitText) {
      readyGsap.set(title, { lineHeight: '0.9' })
          titleSplit = new SplitText(title, {
            type: 'chars',
            mask: 'chars',
            smartWrap: true,
            charsClass: 'char++'
          })
          if (titleSplit.chars && titleSplit.chars.length > 0) {
            titleSplit.chars.forEach((char: HTMLElement) => {
          readyGsap.set(char, { opacity: 0, yPercent: 120, rotationX: -90, lineHeight: '0.9' })
            })
          }
        }
      } catch (e) {
        titleSplit = null
      }

  const tl = readyGsap.timeline({
        scrollTrigger: {
          trigger: bentoBox,
          start: 'top 80%',
          once: true
        }
      })

      tl.to(bentoBox, {
        opacity: 1,
        scale: 1,
        rotation: 0,
        duration: 0.7,
        ease: 'back.out(1.4)'
      })

      if (titleSplit && titleSplit.chars) {
        tl.to(title, { opacity: 1, duration: 0.001 }, '-=0.35')
        tl.to(titleSplit.chars, {
          opacity: 1,
          yPercent: 0,
          rotationX: 0,
          duration: 0.5,
          stagger: { amount: 0.35, from: 'start' },
          ease: 'power3.out'
        }, '-=0.3')
      } else {
        tl.to(title, { opacity: 1, y: 0, duration: 0.45, ease: 'power2.out' }, '-=0.3')
      }

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
      storedElements = { bentoBox, title, items }
  setTimeout(() => readyST.refresh(), 100)
}

function animateExit(): Promise<void> {
  return new Promise((resolve) => {
    if (import.meta.server) return resolve()
    const { gsap: readyGsap } = useGSAP()
    if (!readyGsap) return resolve()

    const bentoBox = storedElements.bentoBox || resolveEl(bentoBoxRef.value)
    const title = storedElements.title || titleRef.value
    const items = storedElements.items && storedElements.items.length > 0
      ? storedElements.items
      : interestItemRefs.value.map(resolveEl).filter(Boolean) as HTMLElement[]

    if (!bentoBox) return resolve()

    const tl = readyGsap.timeline({ onComplete: () => resolve() })
    const contentDuration = 0.5

    if (title) tl.to(title, { y: -10, autoAlpha: 0, duration: contentDuration, ease: 'power2.in' }, 0)

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

onMounted(() => {
  if (import.meta.server) return
  registerExitAnimation('aboutInterests', animateExit)
  setupScrollAnimation()
})

onUnmounted(() => {
  if (import.meta.server) return
  unregisterExitAnimation('aboutInterests')
  scrollTriggers.forEach((st) => {
    if (st?.scrollTrigger) st.scrollTrigger.kill()
    if (st?.kill) st.kill()
  })
  scrollTriggers = []
  storedElements = { bentoBox: null, title: null, items: [] }
  interestItemRefs.value = []
})
</script>
