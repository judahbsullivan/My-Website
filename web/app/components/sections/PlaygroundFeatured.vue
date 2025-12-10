<template>
  <section class="space-y-4">
    <div ref="titleWrapperRef" class="overflow-hidden">
      <UiTitle
        ref="titleRef"
        as="h2"
        size="lg"
        weight="bold"
        class="text-2xl sm:text-3xl font-bold text-foreground"
      >
        Featured
      </UiTitle>
    </div>

    <UiBentoBox
      ref="sectionBoxRef"
      container
      backdrop
      border
      shadow
      padding="lg"
      rounded="2xl"
      :className="'opacity-0 scale-95 translate-y-6'"
    >
      <div ref="gridRef" class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4 sm:gap-6">
        <NuxtLink
          v-for="(playground, index) in playgrounds"
          :key="playground.id"
          :to="`/playground/${playground.slug}`"
          class="group block"
        >
          <UiBentoBox
            :ref="el => setCardRef(el, index)"
            :size="'lg'"
            :variant="getVariant(index)"
            :hover="true"
            :className="'flex-col items-start justify-between h-full opacity-0 scale-90 -rotate-3'"
          >
            <div class="w-full">
              <div v-if="playground.previewImageUrl" class="mb-4 rounded-lg overflow-hidden aspect-video bg-muted relative">
                <img
                  :src="getImageUrl(playground.previewImageUrl)"
                  :alt="playground.title"
                  class="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300"
                />
                <div class="absolute top-3 right-3 flex gap-2">
                  <span :class="[
                    'px-2 py-1 text-xs font-medium rounded-full',
                    playground.isVueComponent ? 'bg-primary/20 text-primary' : 'bg-secondary/20 text-blue-500'
                  ]">
                    {{ playground.codeType }}
                  </span>
                  <span v-if="playground.requiresSubscription" class="px-2 py-1 text-xs font-medium rounded-full bg-amber-500/20 text-amber-500 flex items-center gap-1">
                    <Icon name="heroicons:lock-closed" class="w-3 h-3" />
                    Premium
                  </span>
                </div>
              </div>
              <div v-else class="mb-4 rounded-lg aspect-video bg-muted flex items-center justify-center relative">
                <Icon name="heroicons:code-bracket" class="w-12 h-12 text-muted-foreground" />
                <div class="absolute top-3 right-3 flex gap-2">
                  <span :class="[
                    'px-2 py-1 text-xs font-medium rounded-full',
                    playground.isVueComponent ? 'bg-primary/20 text-primary' : 'bg-secondary/20 text-blue-500'
                  ]">
                    {{ playground.codeType }}
                  </span>
                  <span v-if="playground.requiresSubscription" class="px-2 py-1 text-xs font-medium rounded-full bg-amber-500/20 text-amber-500 flex items-center gap-1">
                    <Icon name="heroicons:lock-closed" class="w-3 h-3" />
                    Premium
                  </span>
                </div>
              </div>
              <div class="flex items-center gap-2 mb-3">
                <span class="px-2 py-1 text-xs font-medium rounded-full bg-primary/20 text-primary">
                  {{ playground.category }}
                </span>
                <span v-if="playground.isFeatured" class="px-2 py-1 text-xs font-medium rounded-full bg-amber-500/20 text-amber-500">
                  Featured
                </span>
              </div>
              <h3 class="text-xl font-bold text-foreground mb-2 group-hover:text-primary transition-colors line-clamp-2">
                {{ playground.title }}
              </h3>
              <p class="text-sm text-muted-foreground line-clamp-3 mb-4">
                {{ playground.description }}
              </p>
            </div>
            <div class="flex items-center justify-between w-full text-xs text-muted-foreground">
              <span class="flex items-center gap-1">
                <Icon name="heroicons:eye" class="w-4 h-4" />
                {{ playground.viewCount }}
              </span>
              <span class="flex items-center gap-1">
                <Icon name="heroicons:heart" class="w-4 h-4" />
                {{ playground.likeCount }}
              </span>
              <span v-if="playground.publishedAt" class="flex items-center gap-1">
                <Icon name="heroicons:calendar" class="w-4 h-4" />
                {{ formatDate(playground.publishedAt) }}
              </span>
            </div>
          </UiBentoBox>
        </NuxtLink>
      </div>
    </UiBentoBox>
  </section>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted, nextTick } from 'vue'
import { registerExitAnimation, unregisterExitAnimation } from '../../composables/usePageExitAnimations'
import { useGSAP } from '../../composables/useGSAP'
import type { Playground } from '~/../../shared/types'

interface Props {
  playgrounds: Playground[]
}

defineProps<Props>()

const config = useRuntimeConfig()
const apiBase = config.public.apiBase

const sectionBoxRef = ref<any>(null)
const titleWrapperRef = ref<HTMLElement | null>(null)
const titleRef = ref<any>(null)
const gridRef = ref<HTMLElement | null>(null)
const cardRefs = ref<(any | null)[]>([])

function setCardRef(el: any, index: number) {
  if (!el) return
  if (!cardRefs.value) cardRefs.value = []
  cardRefs.value[index] = el as any
}

let storedTitleSplit: any = null
let storedElements: {
  box: HTMLElement | null
  title: HTMLElement | null
  cards: HTMLElement[]
} = {
  box: null,
  title: null,
  cards: []
}

const resolveEl = (maybeEl: any): HTMLElement | null => {
  if (!maybeEl) return null
  if (maybeEl instanceof HTMLElement) return maybeEl
  if ((maybeEl as any)?.$el instanceof HTMLElement) return (maybeEl as any).$el
  if ((maybeEl as any)?.el instanceof HTMLElement) return (maybeEl as any).el
  return null
}

const getImageUrl = (url?: string | null): string => {
  if (!url) return ''
  if (url.startsWith('http://') || url.startsWith('https://')) {
    return url
  }
  return `${apiBase}${url}`
}

const formatDate = (dateString?: string): string => {
  if (!dateString) return ''
  const date = new Date(dateString)
  return date.toLocaleDateString('en-US', { year: 'numeric', month: 'short', day: 'numeric' })
}

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

async function setupScrollAnimation() {
  if (import.meta.server) return

  const { waitForReady } = useGSAP()
  const { gsap: readyGsap, ScrollTrigger: readyST, SplitText } = await waitForReady()

  if (!readyGsap || !readyST) {
    console.warn('GSAP or ScrollTrigger not available')
    return
  }

  await nextTick()

  const box = resolveEl(sectionBoxRef.value)
  const titleComponent = titleRef.value as any
  const title = titleComponent?.el || titleComponent?.$el || (titleWrapperRef.value?.querySelector('h2, h3, h4') as HTMLElement)
  const cards = cardRefs.value.map(resolveEl).filter(Boolean) as HTMLElement[]

  if (!box || !title) return

  let titleSplit: any = null
  try {
    if (SplitText) {
      titleSplit = new SplitText(title, {
        type: 'chars',
        mask: 'chars',
        smartWrap: true,
        charsClass: 'char++',
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

  // Check if element is already in viewport
  const rect = box.getBoundingClientRect()
  const isInViewport = rect.top < window.innerHeight * 0.8 && rect.bottom > 0

  readyGsap.set(box, { opacity: 0, scale: 0.95, y: 24 })
  if (cards.length > 0) readyGsap.set(cards, { opacity: 0, y: 20, scale: 0.9, rotation: -3 })

  storedElements = { box, title, cards }
  storedTitleSplit = titleSplit

  // Check if element is already in viewport
  if (isInViewport) {
    const immediateTl = readyGsap.timeline()
    immediateTl.to(box, {
      opacity: 1,
      scale: 1,
      y: 0,
      duration: 0.6,
      ease: 'power3.out'
    })

    if (titleSplit && titleSplit.chars) {
      immediateTl.to(titleSplit.chars, {
        opacity: 1,
        yPercent: 0,
        rotationX: 0,
        duration: 0.5,
        stagger: { amount: 0.35, from: 'start' },
        ease: 'power3.out'
      }, '-=0.35')
    } else {
      immediateTl.fromTo(title,
        { opacity: 0, y: 10 },
        { opacity: 1, y: 0, duration: 0.6, ease: 'power2.out' },
        '-=0.25'
      )
    }

    if (cards.length > 0) {
      immediateTl.to(cards, {
        opacity: 1,
        y: 0,
        scale: 1,
        rotation: 0,
        duration: 0.6,
        stagger: 0.06,
        ease: 'power2.out'
      }, '-=0.35')
    }
  } else {
    // Use ScrollTrigger
    const tl = readyGsap.timeline({
      scrollTrigger: {
        trigger: box,
        start: 'top 80%',
        once: true,
        invalidateOnRefresh: true
      }
    })

    tl.to(box, {
      opacity: 1,
      scale: 1,
      y: 0,
      duration: 0.6,
      ease: 'power3.out'
    })

    if (titleSplit && titleSplit.chars) {
      tl.to(titleSplit.chars, {
        opacity: 1,
        yPercent: 0,
        rotationX: 0,
        duration: 0.5,
        stagger: { amount: 0.35, from: 'start' },
        ease: 'power3.out'
      }, '-=0.35')
    } else {
      tl.fromTo(title,
        { opacity: 0, y: 10 },
        { opacity: 1, y: 0, duration: 0.6, ease: 'power2.out' },
        '-=0.25'
      )
    }

    if (cards.length > 0) {
      tl.to(cards, {
        opacity: 1,
        y: 0,
        scale: 1,
        rotation: 0,
        duration: 0.6,
        stagger: 0.06,
        ease: 'power2.out'
      }, '-=0.35')
    }
  }

  // Refresh ScrollTrigger after a short delay
  if (readyST) {
    setTimeout(() => {
      readyST.refresh()
    }, 100)
  }
}

function animateExit(): Promise<void> {
  return new Promise((resolve) => {
    if (import.meta.server) return resolve()
    const nuxtApp = useNuxtApp()
    const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
    const SplitText = nuxtApp.$SplitText as any
    if (!gsap) return resolve()

    const box = storedElements.box || resolveEl(sectionBoxRef.value)
    const titleComponent = titleRef.value as any
    const title = storedElements.title || titleComponent?.el || titleComponent?.$el || (titleWrapperRef.value?.querySelector('h2, h3, h4') as HTMLElement)
    const cards = storedElements.cards && storedElements.cards.length > 0
      ? storedElements.cards
      : (cardRefs.value.map(resolveEl).filter(Boolean) as HTMLElement[])

    if (!box) return resolve()

    let titleSplit = storedTitleSplit
    if (!titleSplit && SplitText && title) {
      try {
        titleSplit = new SplitText(title, {
          type: 'chars',
          mask: 'chars',
          smartWrap: true,
          charsClass: 'char++',
        })
      } catch (e) {
        titleSplit = null
      }
    }

    const tl = gsap.timeline({
      onComplete: () => {
        try { titleSplit?.revert?.() } catch (e) {}
        resolve()
      }
    })

    const contentDuration = 0.5

    if (titleSplit && titleSplit.chars && titleSplit.chars.length > 0) {
      tl.to(titleSplit.chars, {
        yPercent: -120,
        rotationX: 90,
        autoAlpha: 0,
        duration: contentDuration,
        stagger: { amount: 0.25, from: 'end' },
        ease: 'power2.in'
      }, 0)
    } else if (title) {
      tl.to(title, { y: -10, autoAlpha: 0, duration: contentDuration, ease: 'power2.in' }, 0)
    }

    if (cards.length > 0) {
      tl.to(cards, {
        y: -12,
        scale: 0.9,
        autoAlpha: 0,
        duration: contentDuration,
        stagger: 0.06,
        ease: 'back.in(1.6)'
      }, 0)
    }

    tl.to(box, {
      autoAlpha: 0,
      scale: 0.85,
      y: -28,
      rotation: 4,
      duration: 0.5,
      ease: 'power2.in'
    }, contentDuration * 0.85)
  })
}

onMounted(() => {
  registerExitAnimation('playgroundFeatured', animateExit)
  nextTick(() => {
    if (!storedElements.box) {
      const box = resolveEl(sectionBoxRef.value)
      const titleComponent = titleRef.value as any
      const title = titleComponent?.el || titleComponent?.$el || (titleWrapperRef.value?.querySelector('h2, h3, h4') as HTMLElement)
      storedElements = { box, title, cards: cardRefs.value.map(resolveEl).filter(Boolean) as HTMLElement[] }
    }
  })
  setupScrollAnimation()
})

onUnmounted(() => {
  unregisterExitAnimation('playgroundFeatured')
  storedTitleSplit = null
  storedElements = { box: null, title: null, cards: [] }
})
</script>

<style scoped>
::deep(.char) {
  display: inline-block;
  overflow: hidden;
  vertical-align: baseline;
  transform-style: preserve-3d;
  line-height: 0.9;
  white-space: pre-wrap;
}
</style>
