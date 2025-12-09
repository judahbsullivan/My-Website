<template>
  <div class="flex flex-col">
    <div ref="titleWrapperRef" class="overflow-hidden">
      <UiTitle 
        ref="titleRef"
        as="h1" 
        size="5xl" 
        weight="bold" 
        align="left"
        class="uppercase leading-[0.9] tracking-tight font-bold text-left m-0 opacity-0"
      >
      {{ headerData.title }}
    </UiTitle>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted, watch, nextTick } from 'vue'
import { useNuxtApp } from '#app'
import { useIntroLoader } from '../../composables/useIntroLoader'
import { registerExitAnimation, unregisterExitAnimation } from '../../composables/usePageExitAnimations'

interface Props {
  headerData: {
    title: string
    tagline?: string
  }
}

defineProps<Props>()

const { isIntroLoaderComplete } = useIntroLoader()
const titleWrapperRef = ref<HTMLElement | null>(null)
const titleRef = ref<any>(null)

let hasAnimated = false
let storedTitleSplit: any = null
let storedElements: { titleWrapper: HTMLElement | null, title: HTMLElement | null } = {
  titleWrapper: null,
  title: null
}

function animateHeader() {
  if (import.meta.server || hasAnimated) return

  const nuxtApp = useNuxtApp()
  const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
  const SplitText = nuxtApp.$SplitText as any
  
  if (!gsap) {
    hasAnimated = true
    return
  }
  
  nextTick(() => {
    const titleWrapper = titleWrapperRef.value
    const titleComponent = titleRef.value as any
    const title = titleComponent?.el || titleComponent?.$el || (titleWrapper?.querySelector('h1, h2, h3, h4, h5, h6') as HTMLElement)
    
    if (!title) {
      setTimeout(() => animateHeader(), 50)
      return
    }
    
    // Split title text with mask (like Hero.vue)
    let titleSplit: any = null
    try {
      if (SplitText) {
        gsap.set(title, { lineHeight: '0.9' })
        
        titleSplit = new SplitText(title, { 
          type: 'chars',
          mask: 'chars',
          smartWrap: true,
          charsClass: 'char++',
        })
        if (titleSplit.chars && titleSplit.chars.length > 0) {
          titleSplit.chars.forEach((char: HTMLElement) => {
            gsap.set(char, {
              opacity: 0,
              yPercent: 120,
              rotationX: -90,
              lineHeight: '0.9'
            })
          })
        }
        storedTitleSplit = titleSplit
      }
    } catch (e) {
      // SplitText failed, continue without it
    }
    
    storedElements = { titleWrapper, title }
    
    // Create master timeline (matching Hero.vue pattern)
    const tl = gsap.timeline()
    
    // 1. Animate title characters with mask reveal and 3D rotation
    if (titleSplit && titleSplit.chars) {
      // Ensure parent becomes visible
      if (title) {
        tl.to(title, { opacity: 1, duration: 0.001 }, 0)
      }
      tl.to(titleSplit.chars, {
        opacity: 1,
        yPercent: 0,
        rotationX: 0,
        duration: 0.5,
        stagger: {
          amount: 0.4,
          from: 'start'
        },
        ease: 'power3.out'
      })
    } else {
      // Fallback if SplitText not available
      tl.fromTo(title,
        {
          opacity: 0,
          y: 20
        },
        {
          opacity: 1,
          y: 0,
          duration: 0.4,
          ease: 'power2.out'
        })
    }
    
    hasAnimated = true
  })
}

function animateExit(): Promise<void> {
  return new Promise((resolve) => {
    if (import.meta.server) return resolve()
    const nuxtApp = useNuxtApp()
    const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
    if (!gsap) return resolve()
    
    const titleWrapper = storedElements.titleWrapper || titleWrapperRef.value
    const titleComponent = titleRef.value as any
    const title = storedElements.title || titleComponent?.el || titleComponent?.$el || (titleWrapper?.querySelector('h1, h2, h3, h4, h5, h6') as HTMLElement)
    const titleSplit = storedTitleSplit
    
    if (!title) return resolve()
    
    const tl = gsap.timeline({
      onComplete: () => resolve()
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
    }
    if (title) {
      tl.to(title, { autoAlpha: 0, duration: contentDuration, ease: 'power2.in' }, 0)
    }
  })
}

// Watch for intro loader completion
watch(isIntroLoaderComplete, (complete) => {
  if (complete && !hasAnimated) {
    setTimeout(() => {
      animateHeader()
    }, 150)
  }
})

onMounted(() => {
  registerExitAnimation('aboutHeader', animateExit)
  
  nextTick(() => {
    if (!storedElements.title && titleWrapperRef.value) {
      const titleComponent = titleRef.value as any
      const title = titleComponent?.el || titleComponent?.$el || (titleWrapperRef.value.querySelector('h1, h2, h3, h4, h5, h6') as HTMLElement)
      storedElements = { titleWrapper: titleWrapperRef.value, title }
    }
  })
  
  if (isIntroLoaderComplete.value && !hasAnimated) {
    setTimeout(() => {
      animateHeader()
    }, 150)
  }
})

onUnmounted(() => {
  unregisterExitAnimation('aboutHeader')
  hasAnimated = false
  storedTitleSplit = null
  storedElements = { titleWrapper: null, title: null }
})
</script>

<style scoped>
/* SplitText mask styles - prevent line-height changes */
:deep(.char) {
  display: inline-block;
  overflow: hidden;
  vertical-align: baseline;
  transform-style: preserve-3d;
  line-height: 0.9;
  white-space: pre-wrap;
}

:deep(.line) {
  overflow: hidden;
}

/* Ensure title maintains line-height and prevents reflow */
:deep(h1) {
  line-height: 0.9 !important;
  display: block;
  min-height: 1.8em;
  margin: 0 !important;
}

/* Title wrapper should maintain height */
[ref="titleWrapperRef"] {
  min-height: fit-content;
}
</style>


