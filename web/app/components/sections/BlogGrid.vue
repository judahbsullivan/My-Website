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
        All Posts
      </UiTitle>
    </div>

    <div
      ref="gridWrapperRef"
      class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4 sm:gap-6 opacity-0 scale-95 translate-y-6"
    >
      <NuxtLink
        v-for="(post, index) in posts"
        :key="post.id"
        :to="`/blog/${post.slug}`"
        class="group block"
      >
        <UiBentoBox
          :ref="el => setCardRef(el, index)"
          :size="'md'"
          :variant="getVariant(index)"
          :hover="true"
          :className="'flex-col items-start justify-between h-full opacity-0 scale-90 -rotate-3'"
        >
          <div class="w-full">
            <div v-if="post.image" class="mb-3 rounded-lg overflow-hidden aspect-video bg-muted">
              <img
                :src="getImageUrl(post.image)"
                :alt="post.title"
                class="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300"
              />
            </div>
            <div v-else class="mb-3 rounded-lg aspect-video bg-muted flex items-center justify-center">
              <Icon name="heroicons:document-text" class="w-10 h-10 text-muted-foreground" />
            </div>
            <div class="flex items-center gap-2 mb-2">
              <span class="px-2 py-1 text-xs font-medium rounded-full bg-secondary/20 text-blue-500">
                {{ post.category }}
              </span>
            </div>
            <h3 class="text-lg font-bold text-foreground mb-2 group-hover:text-primary transition-colors line-clamp-2">
              {{ post.title }}
            </h3>
            <p class="text-sm text-muted-foreground line-clamp-2 mb-3">
              {{ post.excerpt }}
            </p>
          </div>
          <div class="flex items-center justify-between w-full text-xs text-muted-foreground">
            <span class="flex items-center gap-1">
              <Icon name="heroicons:clock" class="w-4 h-4" />
              {{ post.readTimeMinutes }} min
            </span>
            <span v-if="post.publishedAt" class="flex items-center gap-1">
              <Icon name="heroicons:calendar" class="w-4 h-4" />
              {{ formatDate(post.publishedAt) }}
            </span>
          </div>
        </UiBentoBox>
      </NuxtLink>
    </div>
  </section>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted, nextTick } from 'vue'
import { registerExitAnimation, unregisterExitAnimation } from '../../composables/usePageExitAnimations'
import { useGSAP } from '../../composables/useGSAP'
import type { BlogPost } from '~/../../shared/types'

interface Props {
  posts: BlogPost[]
}

defineProps<Props>()

const config = useRuntimeConfig()
const apiBase = config.public.apiBase

const titleWrapperRef = ref<HTMLElement | null>(null)
const titleRef = ref<any>(null)
const gridWrapperRef = ref<HTMLElement | null>(null)
const cardRefs = ref<(any | null)[]>([])

function setCardRef(el: any, index: number) {
  if (!el) return
  if (!cardRefs.value) cardRefs.value = []
  cardRefs.value[index] = el as any
}

let storedTitleSplit: any = null
let storedElements: {
  container: HTMLElement | null
  title: HTMLElement | null
  cards: HTMLElement[]
} = {
  container: null,
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

  const container = gridWrapperRef.value
  const titleComponent = titleRef.value as any
  const title = titleComponent?.el || titleComponent?.$el || (titleWrapperRef.value?.querySelector('h2, h3, h4') as HTMLElement)
  const cards = cardRefs.value.map(resolveEl).filter(Boolean) as HTMLElement[]

  if (!container || !title) return

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

  const rect = container.getBoundingClientRect()
  const isInViewport = rect.top < window.innerHeight * 0.8 && rect.bottom > 0

  readyGsap.set(container, { opacity: 0, scale: 0.95, y: 24 })
  if (cards.length > 0) readyGsap.set(cards, { opacity: 0, y: 18, scale: 0.9, rotation: -3 })

  storedElements = { container, title, cards }
  storedTitleSplit = titleSplit

  if (isInViewport) {
    const immediateTl = readyGsap.timeline()
    immediateTl.to(container, {
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
        stagger: 0.05,
        ease: 'power2.out'
      }, '-=0.35')
    }
  } else {
    const tl = readyGsap.timeline({
      scrollTrigger: {
        trigger: container,
        start: 'top 80%',
        once: true,
        invalidateOnRefresh: true
      }
    })

    tl.to(container, {
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
        stagger: 0.05,
        ease: 'power2.out'
      }, '-=0.35')
    }
  }

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

    const container = storedElements.container || gridWrapperRef.value
    const titleComponent = titleRef.value as any
    const title = storedElements.title || titleComponent?.el || titleComponent?.$el || (titleWrapperRef.value?.querySelector('h2, h3, h4') as HTMLElement)
    const cards = storedElements.cards && storedElements.cards.length > 0
      ? storedElements.cards
      : (cardRefs.value.map(resolveEl).filter(Boolean) as HTMLElement[])

    if (!container) return resolve()

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
        stagger: 0.05,
        ease: 'back.in(1.6)'
      }, 0)
    }

    tl.to(container, {
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
  registerExitAnimation('blogGrid', animateExit)
  nextTick(() => {
    if (!storedElements.container) {
      const container = gridWrapperRef.value
      const titleComponent = titleRef.value as any
      const title = titleComponent?.el || titleComponent?.$el || (titleWrapperRef.value?.querySelector('h2, h3, h4') as HTMLElement)
      storedElements = { container, title, cards: cardRefs.value.map(resolveEl).filter(Boolean) as HTMLElement[] }
    }
  })
  setupScrollAnimation()
})

onUnmounted(() => {
  unregisterExitAnimation('blogGrid')
  storedTitleSplit = null
  storedElements = { container: null, title: null, cards: [] }
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
