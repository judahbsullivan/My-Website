<template>
  <div ref="sectionRef" class="flex flex-col items-center justify-center py-12 opacity-0">
    <Icon 
      ref="iconRef"
      name="heroicons:arrow-path" 
      class="w-8 h-8 text-muted-foreground animate-spin mx-auto mb-4 opacity-0" 
    />
    <p 
      ref="textRef"
      class="text-muted-foreground opacity-0 translate-y-2.5"
    >
      {{ loadingText }}
    </p>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted, watch, nextTick } from 'vue'
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
const sectionRef = ref<HTMLElement | null>(null)
const iconRef = ref<HTMLElement | null>(null)
const textRef = ref<HTMLElement | null>(null)

let hasAnimated = false
let storedElements: {
  section: HTMLElement | null
  icon: HTMLElement | null
  text: HTMLElement | null
} = {
  section: null,
  icon: null,
  text: null
}

function animateLoading() {
  if (import.meta.server || hasAnimated) return

  const nuxtApp = useNuxtApp()
  const gsap = nuxtApp.$gsap as typeof import('gsap').gsap

  if (!gsap) {
    hasAnimated = true
    return
  }

  nextTick(() => {
    const section = sectionRef.value
    const icon = iconRef.value
    const text = textRef.value

    if (!section || !icon || !text) {
      setTimeout(() => animateLoading(), 50)
      return
    }

    storedElements = { section, icon, text }

    const tl = gsap.timeline()

    tl.fromTo(section,
      {
        opacity: 0,
        y: 20
      },
      {
        opacity: 1,
        y: 0,
        duration: 0.5,
        ease: 'power3.out'
      }
    )

    tl.fromTo(icon,
      {
        opacity: 0,
        scale: 0.8,
        y: 10
      },
      {
        opacity: 1,
        scale: 1,
        y: 0,
        duration: 0.4,
        ease: 'power2.out'
      }, '-=0.3'
    )

    tl.fromTo(text,
      {
        opacity: 0,
        y: 10
      },
      {
        opacity: 1,
        y: 0,
        duration: 0.4,
        ease: 'power2.out'
      }, '-=0.2'
    )

    hasAnimated = true
  })
}

function animateExit(): Promise<void> {
  return new Promise((resolve) => {
    if (import.meta.server) return resolve()
    const nuxtApp = useNuxtApp()
    const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
    if (!gsap) return resolve()

    const section = storedElements.section || sectionRef.value
    const icon = storedElements.icon || iconRef.value
    const text = storedElements.text || textRef.value

    if (!section) return resolve()

    const tl = gsap.timeline({
      onComplete: () => {
        resolve()
      }
    })

    const contentDuration = 0.35

    if (text) {
      tl.to(text, {
        opacity: 0,
        y: -10,
        duration: contentDuration,
        ease: 'power2.in'
      }, 0)
    }

    if (icon) {
      tl.to(icon, {
        opacity: 0,
        scale: 0.8,
        y: -10,
        duration: contentDuration,
        ease: 'power2.in'
      }, 0)
    }

    const containerDelay = contentDuration * 0.7
    tl.to(section, {
      opacity: 0,
      y: -20,
      duration: 0.4,
      ease: 'power2.in'
    }, containerDelay)
  })
}

watch(isIntroLoaderComplete, (complete) => {
  if (complete && !hasAnimated) {
    setTimeout(() => {
      animateLoading()
    }, 150)
  }
})

onMounted(() => {
  registerExitAnimation(props.exitAnimationKey, animateExit)

  nextTick(() => {
    if (!storedElements.section && sectionRef.value) {
      storedElements = {
        section: sectionRef.value,
        icon: iconRef.value,
        text: textRef.value
      }
    }
  })

  if (isIntroLoaderComplete.value && !hasAnimated) {
    setTimeout(() => {
      animateLoading()
    }, 150)
  }
})

onUnmounted(() => {
  unregisterExitAnimation(props.exitAnimationKey)
  hasAnimated = false
  storedElements = { section: null, icon: null, text: null }
})
</script>