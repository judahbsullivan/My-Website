<template>
  <div ref="wrapperRef" class="text-center py-12 opacity-0 translate-y-4">
    <div ref="iconRef" class="w-16 h-16 mx-auto mb-4 text-danger opacity-0 scale-90">
      <Icon name="heroicons:exclamation-triangle" class="w-16 h-16" />
    </div>
    <h2 ref="titleRef" class="text-2xl font-bold text-foreground opacity-0 translate-y-2">
      {{ title }}
    </h2>
    <p ref="textRef" class="text-muted-foreground text-base opacity-0 translate-y-2">
      {{ message }}
    </p>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted, watch } from 'vue'
import { useNuxtApp } from '#app'
import { useIntroLoader } from '../../composables/useIntroLoader'
import { registerExitAnimation, unregisterExitAnimation } from '../../composables/usePageExitAnimations'

interface Props {
  title?: string
  message?: string
  exitAnimationKey?: string
}

const props = withDefaults(defineProps<Props>(), {
  title: 'Something went wrong',
  message: 'Unable to load content right now. Please try again shortly.',
  exitAnimationKey: 'contentErrorState'
})

const { isIntroLoaderComplete } = useIntroLoader()
const wrapperRef = ref<HTMLElement | null>(null)
const iconRef = ref<HTMLElement | null>(null)
const titleRef = ref<HTMLElement | null>(null)
const textRef = ref<HTMLElement | null>(null)

let hasAnimated = false

function animateEnter() {
  if (import.meta.server || hasAnimated) return

  const nuxtApp = useNuxtApp()
  const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
  if (!gsap) {
    hasAnimated = true
    return
  }

  const wrapper = wrapperRef.value
  const icon = iconRef.value
  const title = titleRef.value
  const text = textRef.value

  if (!wrapper || !icon || !title || !text) return

  const tl = gsap.timeline()
  tl.fromTo(wrapper, { opacity: 0, y: 18 }, { opacity: 1, y: 0, duration: 0.5, ease: 'power2.out' })
  tl.fromTo(icon, { opacity: 0, scale: 0.9 }, { opacity: 1, scale: 1, duration: 0.5, ease: 'back.out(1.4)' }, '-=0.3')
  tl.fromTo([title, text],
    { opacity: 0, y: 10 },
    { opacity: 1, y: 0, duration: 0.45, stagger: 0.08, ease: 'power2.out' },
    '-=0.35'
  )

  hasAnimated = true
}

function animateExit(): Promise<void> {
  return new Promise((resolve) => {
    if (import.meta.server) return resolve()
    const nuxtApp = useNuxtApp()
    const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
    if (!gsap) return resolve()

    const wrapper = wrapperRef.value
    const icon = iconRef.value
    const title = titleRef.value
    const text = textRef.value

    if (!wrapper) return resolve()

    const tl = gsap.timeline({ onComplete: () => resolve() })
    const contentDuration = 0.45

    tl.to([icon, title, text].filter(Boolean), {
      y: -12,
      autoAlpha: 0,
      duration: contentDuration,
      stagger: 0.06,
      ease: 'power2.in'
    }, 0)

    tl.to(wrapper, {
      autoAlpha: 0,
      scale: 0.9,
      y: -18,
      duration: 0.45,
      ease: 'power2.in'
    }, contentDuration * 0.8)
  })
}

watch(isIntroLoaderComplete, (complete) => {
  if (complete && !hasAnimated) {
    setTimeout(() => animateEnter(), 120)
  }
})

onMounted(() => {
  registerExitAnimation(props.exitAnimationKey, animateExit)
  if (isIntroLoaderComplete.value && !hasAnimated) {
    setTimeout(() => animateEnter(), 120)
  }
})

onUnmounted(() => {
  unregisterExitAnimation(props.exitAnimationKey)
  hasAnimated = false
})
</script>

