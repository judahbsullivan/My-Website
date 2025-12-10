<template>
  <UiBentoBox
    ref="bentoBoxRef"
    container
    backdrop
    border
    shadow
    padding="lg"
    rounded="2xl"
    :className="`self-start ${educationData.background} opacity-0 scale-90 -rotate-3`"
  >
    <div class="flex items-center gap-3 mb-6">
      <div class="w-12 h-12 rounded-xl bg-secondary/20 flex items-center justify-center">
        <Icon name="heroicons:academic-cap" class="w-6 h-6 text-blue-500" />
      </div>
      <h3 ref="titleRef" class="text-xl font-bold text-foreground opacity-0">
        {{ educationData.title }}
      </h3>
    </div>
    <div class="space-y-6">
      <div
        v-for="(edu, index) in educationData.educations"
        :key="edu.degree"
        :ref="el => { if (el) eduItemRefs[index] = el as HTMLElement }"
        class="border-l-2 border-blue-500/30 pl-4"
      >
        <div class="flex items-start justify-between mb-2">
          <div>
            <h4 class="text-base font-semibold text-foreground">{{ edu.degree }}</h4>
            <p class="text-sm text-muted-foreground">{{ edu.institution }}</p>
          </div>
          <span class="text-xs text-muted-foreground whitespace-nowrap">{{ edu.year }}</span>
        </div>
        <p class="text-sm text-foreground">{{ edu.description }}</p>
      </div>
    </div>
  </UiBentoBox>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted, nextTick } from 'vue'
import { registerExitAnimation, unregisterExitAnimation } from '../../composables/usePageExitAnimations'
import { useGSAP } from '../../composables/useGSAP'

interface Props {
  educationData: {
    title: string
    background: string
    educations: Array<{
      degree: string
      institution: string
      year: string
      description: string
    }>
  }
}

const props = defineProps<Props>()

const bentoBoxRef = ref<any>(null)
const titleRef = ref<HTMLElement | null>(null)
const eduItemRefs = ref<(HTMLElement | null)[]>([])

let scrollTriggers: any[] = []
let storedElements: { bentoBox: HTMLElement | null; title: HTMLElement | null; items: HTMLElement[] } = {
  bentoBox: null,
  title: null,
  items: []
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
      const items = eduItemRefs.value.filter(Boolean) as HTMLElement[]

      if (!bentoBox || !title) return

  readyGsap.set(bentoBox, { opacity: 0, scale: 0.9, rotation: -3 })
  readyGsap.set(title, { opacity: 0, y: 20 })
  if (items.length > 0) readyGsap.set(items, { opacity: 0, y: 20 })

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
          duration: 0.45,
          stagger: 0.06,
          ease: 'power2.out'
        }, '-=0.3')
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
      : (eduItemRefs.value.filter(Boolean) as HTMLElement[])

    if (!bentoBox) return resolve()

    const tl = readyGsap.timeline({
      onComplete: () => resolve()
    })

    const contentDuration = 0.5

    if (title) {
      tl.to(title, { y: -10, autoAlpha: 0, duration: contentDuration, ease: 'power2.in' }, 0)
    }

    if (items.length > 0) {
      tl.to(items, {
        y: -10,
        autoAlpha: 0,
        duration: contentDuration,
        stagger: 0.05,
        ease: 'power2.in'
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
  registerExitAnimation('aboutEducation', animateExit)
  setupScrollAnimation()
})

onUnmounted(() => {
  if (import.meta.server) return
  unregisterExitAnimation('aboutEducation')
  scrollTriggers.forEach((st) => {
    if (st?.scrollTrigger) st.scrollTrigger.kill()
    if (st?.kill) st.kill()
  })
  scrollTriggers = []
  storedElements = { bentoBox: null, title: null, items: [] }
})
</script>
