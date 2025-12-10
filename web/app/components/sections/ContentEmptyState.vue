<template>
  <UiBentoBox
    ref="boxRef"
    container
    backdrop
    border
    shadow
    padding="lg"
    rounded="2xl"
    :className="'flex-col items-center text-center gap-3 opacity-0 scale-90 -rotate-3'"
  >
    <div ref="iconRef" class="w-14 h-14 rounded-full bg-muted flex items-center justify-center opacity-0 scale-90">
      <Icon :name="icon" class="w-8 h-8 text-muted-foreground" />
    </div>
    <h2 ref="titleRef" class="text-2xl font-bold text-foreground opacity-0 translate-y-2">
      {{ title }}
    </h2>
    <p ref="textRef" class="text-muted-foreground text-base opacity-0 translate-y-2">
      {{ message }}
    </p>
  </UiBentoBox>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue'
import { useNuxtApp } from '#app'
import { registerExitAnimation, unregisterExitAnimation } from '../../composables/usePageExitAnimations'

interface Props {
  icon?: string
  title?: string
  message?: string
  exitAnimationKey?: string
}

const props = withDefaults(defineProps<Props>(), {
  icon: 'heroicons:folder',
  title: 'No content yet',
  message: 'Check back soon for new content!',
  exitAnimationKey: 'contentEmptyState'
})

const boxRef = ref<any>(null)
const iconRef = ref<HTMLElement | null>(null)
const titleRef = ref<HTMLElement | null>(null)
const textRef = ref<HTMLElement | null>(null)

let matchMedia: any = null
let storedElements: {
  box: HTMLElement | null
  icon: HTMLElement | null
  title: HTMLElement | null
  text: HTMLElement | null
} = {
  box: null,
  icon: null,
  title: null,
  text: null
}

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
  if (!gsap) return

  import('gsap/ScrollTrigger').then((stModule) => {
    const ScrollTrigger = stModule.default || stModule
    if (ScrollTrigger && gsap.registerPlugin) gsap.registerPlugin(ScrollTrigger)

    const box = resolveEl(boxRef.value)
    const icon = iconRef.value
    const title = titleRef.value
    const text = textRef.value

    if (!box) return

    // Check if element is already in viewport
    const rect = box.getBoundingClientRect()
    const isInViewport = rect.top < window.innerHeight * 0.85 && rect.bottom > 0

    gsap.set(box, { opacity: 0, scale: 0.9, rotation: -3 })
    if (icon) gsap.set(icon, { opacity: 0, scale: 0.9, y: 10 })
    if (title) gsap.set(title, { opacity: 0, y: 12 })
    if (text) gsap.set(text, { opacity: 0, y: 12 })

    storedElements = { box, icon, title, text }

    // Use gsap.matchMedia() for responsive and accessible animations
    matchMedia = gsap.matchMedia()

    matchMedia.add(
      {
        isDesktop: '(min-width: 768px)',
        isMobile: '(max-width: 767px)',
        reduceMotion: '(prefers-reduced-motion: reduce)'
      },
      (context: any) => {
        const { isDesktop, isMobile, reduceMotion } = context.conditions
        const duration = reduceMotion ? 0 : (isMobile ? 0.5 : 0.6)
        const startPosition = isMobile ? 'top 90%' : 'top 85%'

        // If already in viewport, animate immediately
        if (isInViewport) {
          const immediateTl = gsap.timeline()
          immediateTl.to(box, {
            opacity: 1,
            scale: 1,
            rotation: 0,
            duration: duration || 0.01,
            ease: 'back.out(1.4)'
          })

          if (icon) {
            immediateTl.to(icon, {
              opacity: 1,
              scale: 1,
              y: 0,
              duration: duration || 0.01,
              ease: 'back.out(1.4)'
            }, '-=0.3')
          }

          immediateTl.to([title, text].filter(Boolean), {
            opacity: 1,
            y: 0,
            duration: duration || 0.01,
            stagger: reduceMotion ? 0 : 0.08,
            ease: 'power2.out'
          }, '-=0.3')
          return
        }

        // Otherwise, use ScrollTrigger
        const tl = gsap.timeline({
          scrollTrigger: {
            trigger: box,
            start: startPosition,
            once: true,
            invalidateOnRefresh: true
          }
        })

        tl.to(box, {
          opacity: 1,
          scale: 1,
          rotation: 0,
          duration,
          ease: 'back.out(1.4)'
        })

        if (icon) {
          tl.to(icon, {
            opacity: 1,
            scale: 1,
            y: 0,
            duration: duration || 0.01,
            ease: 'back.out(1.4)'
          }, '-=0.3')
        }

        tl.to([title, text].filter(Boolean), {
          opacity: 1,
          y: 0,
          duration: duration || 0.01,
          stagger: reduceMotion ? 0 : 0.08,
          ease: 'power2.out'
        }, '-=0.3')
      }
    )

    // Refresh ScrollTrigger after a short delay
    if (ScrollTrigger) {
      setTimeout(() => {
        ScrollTrigger.refresh()
      }, 100)
    }
  }).catch(() => {
    console.warn('ScrollTrigger not available')
  })
}

function animateExit(): Promise<void> {
  return new Promise((resolve) => {
    if (import.meta.server) return resolve()
    const nuxtApp = useNuxtApp()
    const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
    if (!gsap) return resolve()

    const box = storedElements.box || resolveEl(boxRef.value)
    const icon = storedElements.icon || iconRef.value
    const title = storedElements.title || titleRef.value
    const text = storedElements.text || textRef.value

    if (!box) return resolve()

    const tl = gsap.timeline({ onComplete: () => resolve() })
    const contentDuration = 0.5

    tl.to([icon, title, text].filter(Boolean), {
      y: -10,
      autoAlpha: 0,
      scale: 0.9,
      duration: contentDuration,
      stagger: 0.07,
      ease: 'power2.in'
    }, 0)

    tl.to(box, {
      autoAlpha: 0,
      scale: 0.85,
      y: -24,
      rotation: 4,
      duration: 0.5,
      ease: 'power2.in'
    }, contentDuration * 0.85)
  })
}

onMounted(() => {
  registerExitAnimation(props.exitAnimationKey, animateExit)
  setupScrollAnimation()
})

onUnmounted(() => {
  unregisterExitAnimation(props.exitAnimationKey)
  if (matchMedia) {
    matchMedia.revert()
    matchMedia = null
  }
  storedElements = { box: null, icon: null, title: null, text: null }
})
</script>

