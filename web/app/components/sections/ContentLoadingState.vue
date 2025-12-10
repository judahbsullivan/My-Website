<template>
  <div ref="wrapperRef" class="text-center py-12 opacity-0 translate-y-4">
    <div ref="iconRef" class="w-12 h-12 mx-auto mb-3 text-muted-foreground opacity-0 scale-90">
      <Icon name="heroicons:arrow-path" class="w-12 h-12 animate-spin" />
    </div>
    <p ref="textRef" class="text-muted-foreground text-base opacity-0 translate-y-2">
      {{ loadingText }}
    </p>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted, watch } from 'vue'
import { useNuxtApp } from '#app'
import { useIntroLoader } from '../../composables/useIntroLoader'
import { registerExitAnimation, unregisterExitAnimation } from '../../composables/usePageExitAnimations'

interface Props {
  loadingText?: string
  exitAnimationKey?: string
}

const props = withDefaults(defineProps<Props>(), {
  loadingText: 'Loading...',
  exitAnimationKey: 'contentLoadingState'
})

const { isIntroLoaderComplete } = useIntroLoader()
const wrapperRef = ref<HTMLElement | null>(null)
const iconRef = ref<HTMLElement | null>(null)
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
  const text = textRef.value

  if (!wrapper || !icon || !text) return

  const tl = gsap.timeline()
  tl.fromTo(wrapper, { opacity: 0, y: 16 }, { opacity: 1, y: 0, duration: 0.45, ease: 'power2.out' })
  tl.fromTo(icon, { opacity: 0, scale: 0.9 }, { opacity: 1, scale: 1, duration: 0.5, ease: 'back.out(1.4)' }, '-=0.25')
  tl.fromTo(text, { opacity: 0, y: 8 }, { opacity: 1, y: 0, duration: 0.4, ease: 'power2.out' }, '-=0.25')

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
    const text = textRef.value

    if (!wrapper) return resolve()

    const tl = gsap.timeline({ onComplete: () => resolve() })
    const contentDuration = 0.4

    if (icon) tl.to(icon, { y: -10, autoAlpha: 0, scale: 0.85, duration: contentDuration, ease: 'power2.in' }, 0)
    if (text) tl.to(text, { y: -10, autoAlpha: 0, duration: contentDuration, ease: 'power2.in' }, 0)

    tl.to(wrapper, {
      autoAlpha: 0,
      scale: 0.9,
      y: -16,
      duration: 0.45,
      ease: 'power2.in'
    }, contentDuration * 0.8)
  })
}

watch(isIntroLoaderComplete, (complete) => {
  if (complete && !hasAnimated) {
    setTimeout(() => animateEnter(), 100)
  }
})

onMounted(() => {
  registerExitAnimation(props.exitAnimationKey, animateExit)
  if (isIntroLoaderComplete.value && !hasAnimated) {
    setTimeout(() => animateEnter(), 100)
  }
})

onUnmounted(() => {
  unregisterExitAnimation(props.exitAnimationKey)
  hasAnimated = false
})
</script>

