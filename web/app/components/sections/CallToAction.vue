<template>
  <UiBentoBox
    ref="bentoBoxRef"
    container
    backdrop
    border
    shadow
    padding="xl"
    rounded="2xl"
    :className="`text-center ${ctaData.background} text-background opacity-0 scale-90 -rotate-3`"
  >
    <div class="max-w-2xl mx-auto">
      <Icon :name="ctaData.icon" class="w-12 h-12 mx-auto mb-4 text-current opacity-0" />
      <h3 ref="titleRef" class="text-2xl font-bold mb-4 leading-tight text-background opacity-0">
        {{ ctaData.title }}
      </h3>
      <p ref="descRef" class="text-base leading-relaxed text-background/80 mb-6 opacity-0">
        {{ ctaData.description }}
      </p>
      <div class="flex flex-col sm:flex-row gap-4 justify-center">
        <UiButton
          :to="ctaData.primaryButton.to"
          variant="secondary"
          size="lg"
          class="bg-background text-foreground border-transparent hover:bg-background/90"
        >
          <Icon :name="ctaData.primaryButton.icon" class="w-5 h-5" />
          {{ ctaData.primaryButton.text }}
        </UiButton>
        <UiButton
          :to="ctaData.secondaryButton.to"
          variant="outline"
          size="lg"
          class="border-background text-background hover:bg-background/10"
        >
          <Icon :name="ctaData.secondaryButton.icon" class="w-5 h-5" />
          {{ ctaData.secondaryButton.text }}
        </UiButton>
      </div>
    </div>
  </UiBentoBox>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted, nextTick } from 'vue'
import { useNuxtApp } from '#app'
import { registerExitAnimation, unregisterExitAnimation } from '../../composables/usePageExitAnimations'

interface Props {
  ctaData: {
    icon: string
    title: string
    description: string
    background: string
    primaryButton: {
      text: string
      to: string
      icon: string
    }
    secondaryButton: {
      text: string
      to: string
      icon: string
    }
  }
}

const props = defineProps<Props>()

const bentoBoxRef = ref<any>(null)
const titleRef = ref<HTMLElement | null>(null)
const descRef = ref<HTMLElement | null>(null)
const buttonsRef = ref<HTMLElement | null>(null)

let scrollTriggers: any[] = []
let storedElements: { bento: HTMLElement | null; title: HTMLElement | null; desc: HTMLElement | null; buttons: HTMLElement | null } = {
  bento: null,
  title: null,
  desc: null,
  buttons: null
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
  const SplitText = nuxtApp.$SplitText as any
  if (!gsap) return

  import('gsap/ScrollTrigger').then((stModule) => {
    const ScrollTrigger = stModule.default || stModule
    if (ScrollTrigger && gsap.registerPlugin) gsap.registerPlugin(ScrollTrigger)

    nextTick(() => {
      const bentoBox = resolveEl(bentoBoxRef.value)
      const title = titleRef.value
      const desc = descRef.value
      const buttons = buttonsRef.value
      if (!bentoBox || !title) return

      gsap.set(bentoBox, { opacity: 0, scale: 0.9, rotation: -3 })
      gsap.set(title, { opacity: 0, y: 20 })
      if (desc) gsap.set(desc, { opacity: 0, y: 15 })
      if (buttons) gsap.set(buttons.children, { opacity: 0, scale: 0.8, y: 20, rotation: -3 })

      let titleSplit: any = null
      try {
        if (SplitText) {
          gsap.set(title, { lineHeight: '1' })
          titleSplit = new SplitText(title, { type: 'chars', mask: 'chars', smartWrap: true, charsClass: 'char++' })
          if (titleSplit.chars && titleSplit.chars.length > 0) {
            titleSplit.chars.forEach((char: HTMLElement) => {
              gsap.set(char, { opacity: 0, yPercent: 120, rotationX: -90, lineHeight: '1' })
            })
          }
        }
      } catch (e) {
        titleSplit = null
      }

      const tl = gsap.timeline({
        scrollTrigger: {
          trigger: bentoBox,
          start: 'top 80%',
          once: true
        }
      })

      tl.to(bentoBox, { opacity: 1, scale: 1, rotation: 0, duration: 0.7, ease: 'back.out(1.4)' })

      if (titleSplit && titleSplit.chars) {
        tl.to(title, { opacity: 1, duration: 0.001 }, '-=0.35')
        tl.to(titleSplit.chars, {
          opacity: 1,
          yPercent: 0,
          rotationX: 0,
          duration: 0.5,
          stagger: { amount: 0.3, from: 'start' },
          ease: 'power3.out'
        }, '-=0.3')
      } else {
        tl.to(title, { opacity: 1, y: 0, duration: 0.45, ease: 'power2.out' }, '-=0.3')
      }

      if (desc) {
        tl.to(desc, { opacity: 1, y: 0, duration: 0.45, ease: 'power2.out' }, '-=0.25')
      }

      if (buttons && buttons.children.length > 0) {
        tl.to(Array.from(buttons.children) as HTMLElement[], {
          opacity: 1,
          scale: 1,
          y: 0,
          rotation: 0,
          duration: 0.4,
          stagger: 0.08,
          ease: 'back.out(1.4)'
        }, '-=0.3')
      }

      scrollTriggers.push(tl)
      storedElements = { bento: bentoBox, title, desc, buttons }
    })
  }).catch(() => console.warn('ScrollTrigger not available'))
}

function animateExit(): Promise<void> {
  return new Promise((resolve) => {
    if (import.meta.server) return resolve()
    const nuxtApp = useNuxtApp()
    const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
    if (!gsap) return resolve()

    const bento = storedElements.bento || resolveEl(bentoBoxRef.value)
    const title = storedElements.title || titleRef.value
    const desc = storedElements.desc || descRef.value
    const buttons = storedElements.buttons || buttonsRef.value

    if (!bento) return resolve()

    const tl = gsap.timeline({ onComplete: () => resolve() })
    const contentDuration = 0.5

    if (title) tl.to(title, { y: -10, autoAlpha: 0, duration: contentDuration, ease: 'power2.in' }, 0)
    if (desc) tl.to(desc, { y: -8, autoAlpha: 0, duration: contentDuration - 0.05, ease: 'power2.in' }, 0)

    if (buttons && buttons.children.length > 0) {
      tl.to(Array.from(buttons.children) as HTMLElement[], {
        scale: 0,
        autoAlpha: 0,
        y: -10,
        duration: contentDuration,
        stagger: 0.06,
        ease: 'back.in(1.7)'
      }, 0)
    }

    const containerDelay = contentDuration * 0.85
    tl.to(bento, {
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
  registerExitAnimation('aboutCTA', animateExit)
  setupScrollAnimation()
})

onUnmounted(() => {
  if (import.meta.server) return
  unregisterExitAnimation('aboutCTA')
  scrollTriggers.forEach((st) => {
    if (st?.scrollTrigger) st.scrollTrigger.kill()
    if (st?.kill) st.kill()
  })
  scrollTriggers = []
  storedElements = { bento: null, title: null, desc: null, buttons: null }
})
</script>
