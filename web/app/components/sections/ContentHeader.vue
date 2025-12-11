<template>
  <div ref="sectionRef" class="flex flex-col gap-4 opacity-0">
    <div ref="titleWrapperRef" class="overflow-hidden">
      <UiTitle ref="titleRef" as="h1" size="4xl" weight="bold" align="left"
        class="uppercase leading-[0.9] tracking-tight font-bold text-left m-0 opacity-0">
        {{ headerData.title }}
      </UiTitle>
    </div>
    <div ref="descriptionWrapperRef" class="overflow-hidden max-w-3xl">
      <p ref="descriptionRef"
        class="text-base sm:text-lg text-muted-foreground leading-relaxed opacity-0 translate-y-2.5">
        {{ headerData.description }}
      </p>
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
    description: string
  }
  exitAnimationKey?: string
}

const props = withDefaults(defineProps<Props>(), {
  exitAnimationKey: 'contentHeader'
})

const { isIntroLoaderComplete } = useIntroLoader()
const sectionRef = ref<HTMLElement | null>(null)
const titleWrapperRef = ref<HTMLElement | null>(null)
const titleRef = ref<any>(null)
const descriptionWrapperRef = ref<HTMLElement | null>(null)
const descriptionRef = ref<HTMLElement | null>(null)

let hasAnimated = false
let storedTitleSplit: any = null
let storedDescriptionSplit: any = null
let storedElements: {
  section: HTMLElement | null
  title: HTMLElement | null
  description: HTMLElement | null
} = {
  section: null,
  title: null,
  description: null
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
    const section = sectionRef.value
    const titleComponent = titleRef.value as any
    const title = titleComponent?.el || titleComponent?.$el || (titleWrapperRef.value?.querySelector('h1, h2, h3, h4, h5, h6') as HTMLElement)
    const description = descriptionRef.value

    if (!section || !title || !description) {
      setTimeout(() => animateHeader(), 50)
      return
    }

    let titleSplit: any = null
    let descriptionSplit: any = null

    try {
      if (SplitText && title) {
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

    try {
      if (SplitText && description) {
        descriptionSplit = new SplitText(description, {
          type: 'lines',
          mask: 'lines',
          smartWrap: true,
          linesClass: 'line++',
        })
        if (descriptionSplit.lines && descriptionSplit.lines.length > 0) {
          gsap.set(descriptionSplit.lines, {
            opacity: 0,
            yPercent: 100
          })
        }
        storedDescriptionSplit = descriptionSplit
      }
    } catch (e) {
      // SplitText failed, continue without it
    }

    storedElements = { section, title, description }

    const tl = gsap.timeline()

    tl.fromTo(section,
      {
        opacity: 0,
        y: 20
      },
      {
        opacity: 1,
        y: 0,
        duration: 0.55,
        ease: 'power3.out'
      }
    )

    if (titleSplit && titleSplit.chars) {
      tl.to(title, { opacity: 1, duration: 0.001 }, '-=0.35')
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
      }, '-=0.25')
    } else if (title) {
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
        }, '-=0.2')
    }

    if (descriptionSplit && descriptionSplit.lines) {
      tl.to(description, { opacity: 1, duration: 0.001 }, '-=0.3')
      tl.to(descriptionSplit.lines, {
        opacity: 1,
        yPercent: 0,
        duration: 0.5,
        stagger: {
          amount: 0.3,
          from: 'start'
        },
        ease: 'power2.out'
      }, '-=0.25')
    } else if (description) {
      tl.fromTo(description,
        {
          opacity: 0,
          y: 10
        },
        {
          opacity: 1,
          y: 0,
          duration: 0.4,
          ease: 'power2.out'
        }, '-=0.2')
    }

    hasAnimated = true
  })
}

function animateExit(): Promise<void> {
  return new Promise((resolve) => {
    if (import.meta.server) return resolve()
    const nuxtApp = useNuxtApp()
    const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
    const SplitText = nuxtApp.$SplitText as any
    if (!gsap) return resolve()

    const section = storedElements.section || sectionRef.value
    const titleComponent = titleRef.value as any
    const title = storedElements.title || titleComponent?.el || titleComponent?.$el || (titleWrapperRef.value?.querySelector('h1, h2, h3, h4, h5, h6') as HTMLElement)
    const description = storedElements.description || descriptionRef.value

    if (!section) return resolve()

    let titleSplit = storedTitleSplit
    let descriptionSplit = storedDescriptionSplit

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

    if (!descriptionSplit && SplitText && description) {
      try {
        descriptionSplit = new SplitText(description, {
          type: 'lines',
          mask: 'lines',
          smartWrap: true,
          linesClass: 'line++',
        })
      } catch (e) {
        descriptionSplit = null
      }
    }

    const tl = gsap.timeline({
      onComplete: () => {
        try { titleSplit?.revert?.() } catch (e) { }
        try { descriptionSplit?.revert?.() } catch (e) { }
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
    }
    if (title) {
      tl.to(title, { autoAlpha: 0, duration: contentDuration, ease: 'power2.in' }, 0)
    }

    if (descriptionSplit && descriptionSplit.lines && descriptionSplit.lines.length > 0) {
      tl.to(descriptionSplit.lines, {
        yPercent: -100,
        autoAlpha: 0,
        duration: contentDuration,
        stagger: { amount: 0.2, from: 'end' },
        ease: 'power2.in'
      }, 0)
    }
    if (description) {
      tl.to(description, { autoAlpha: 0, duration: contentDuration, ease: 'power2.in' }, 0)
    }

    const containerDelay = contentDuration * 0.85
    tl.to(section, {
      autoAlpha: 0,
      scale: 0.9,
      y: -20,
      rotation: 2,
      duration: 0.45,
      ease: 'power2.in'
    }, containerDelay)
  })
}

watch(isIntroLoaderComplete, (complete) => {
  if (complete && !hasAnimated) {
    setTimeout(() => {
      animateHeader()
    }, 150)
  }
})

onMounted(() => {
  registerExitAnimation(props.exitAnimationKey, animateExit)

  nextTick(() => {
    if (!storedElements.section && sectionRef.value) {
      const titleComponent = titleRef.value as any
      const title = titleComponent?.el || titleComponent?.$el || (titleWrapperRef.value?.querySelector('h1, h2, h3, h4, h5, h6') as HTMLElement)
      storedElements = {
        section: sectionRef.value,
        title,
        description: descriptionRef.value
      }
    }
  })

  if (isIntroLoaderComplete.value && !hasAnimated) {
    setTimeout(() => {
      animateHeader()
    }, 150)
  }
})

onUnmounted(() => {
  unregisterExitAnimation(props.exitAnimationKey)
  hasAnimated = false
  storedTitleSplit = null
  storedDescriptionSplit = null
  storedElements = { section: null, title: null, description: null }
})
</script>

<style scoped>
/* SplitText mask styles */
::deep(.char) {
  display: inline-block;
  overflow: hidden;
  vertical-align: baseline;
  transform-style: preserve-3d;
  line-height: 0.9;
  white-space: pre-wrap;
}

::deep(.line) {
  overflow: hidden;
}

::deep(h1) {
  line-height: 0.9 !important;
  display: block;
  min-height: 1.8em;
  margin: 0 !important;
}
</style>
