<template>
  <section class="w-full">
    <div class="px-4 sm:px-6">
      <UiBentoBox
            ref="bentoBoxRef"
            container
            backdrop
            border
            shadow
            padding="lg"
            rounded="2xl"
            :className="`${homeData.sections.hero.background} opacity-0 scale-90 -rotate-3`"
          >
            <!-- Badge -->
            <div
              ref="badgeRef"
              class="inline-flex items-center gap-2 rounded-full border border-border/50 bg-card/40 px-3 py-1 text-xs font-medium text-muted-foreground mb-4 sm:mb-5 opacity-0 scale-80 translate-y-8 -rotate-5"
            >
              <span class="w-1.5 h-1.5 rounded-full bg-primary animate-pulse shrink-0" />
              <span>{{ homeData.sections.hero.badge.text }}</span>
            </div>

            <!-- Title -->
            <div ref="titleWrapperRef" class="mb-6 overflow-hidden">
              <UiTitle 
                ref="titleRef"
                as="h1"
                size="4xl"
                weight="bold"
                align="left"
                class="uppercase leading-[0.9] tracking-tight font-bold text-left opacity-0"
              >
                {{ homeData.sections.hero.title }}
              </UiTitle>
            </div>

            <!-- Paragraph -->
            <div ref="paragraphWrapperRef" class="overflow-hidden mb-8">
              <p 
                ref="paragraphRef"
                class="text-base sm:text-lg md:text-xl text-muted-foreground max-w-xl leading-relaxed opacity-0"
              >
                {{ homeData.sections.hero.paragraph }}
              </p>
            </div>

            <!-- CTA Buttons -->
            <div ref="ctaRef" class="flex flex-col md:flex-row w-full md:justify-between md:items-center gap-6">
              <div class='flex flex-wrap items-center gap-3 sm:gap-4'>
                <div ref="primaryBtnRef" class="opacity-0 scale-80 translate-y-8 -rotate-5">
                  <UiButton :to="homeData.sections.hero.primaryButton.to" variant="primary" size="lg" class="flex items-center gap-2 text-sm sm:text-base">
                    {{ homeData.sections.hero.primaryButton.text }}
                    <span aria-hidden="true">→</span>
                  </UiButton>
                </div>

                <div ref="secondaryBtnRef" class="opacity-0 scale-80 translate-y-8 -rotate-5">
                  <UiButton :to="homeData.sections.hero.secondaryButton.to" variant="outline" size="lg" class="border-border/60 bg-background/40 text-sm sm:text-base">
                    {{ homeData.sections.hero.secondaryButton.text }}
                  </UiButton>
                </div>
              </div>

              <div 
                ref="statusRef"
                class="flex items-center gap-3 shrink-0 opacity-0 translate-y-2.5"
              >
                <span class="w-2.5 h-2.5 rounded-full bg-success animate-pulse shrink-0" />
                <span class="text-xs sm:text-sm text-muted-foreground">
                  {{ homeData.sections.hero.status.text }}
                </span>
              </div>
            </div>

      </UiBentoBox>
    </div>
  </section>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted, onBeforeUnmount, watch, nextTick } from 'vue'
import { useNuxtApp } from '#app'
import { useIntroLoader } from '../../composables/useIntroLoader'
import { registerExitAnimation, unregisterExitAnimation } from '../../composables/usePageExitAnimations'
import homeData from '../../data/home.json'

const { isIntroLoaderComplete } = useIntroLoader()

const bentoBoxRef = ref<any>(null)
const badgeRef = ref<HTMLElement | null>(null)
const titleWrapperRef = ref<HTMLElement | null>(null)
const titleRef = ref<any>(null)
const paragraphWrapperRef = ref<HTMLElement | null>(null)
const paragraphRef = ref<HTMLElement | null>(null)
const primaryBtnRef = ref<HTMLElement | null>(null)
const secondaryBtnRef = ref<HTMLElement | null>(null)
const statusRef = ref<HTMLElement | null>(null)
const ctaRef = ref<HTMLElement | null>(null)

let hasAnimated = false
// Store SplitText instances for exit animation
let storedTitleSplit: any = null
let storedParagraphSplit: any = null

// Store actual DOM elements (not Vue refs) for exit animation
let storedElements: {
  bentoBox: HTMLElement | null
  badge: HTMLElement | null
  title: HTMLElement | null
  paragraph: HTMLElement | null
  primaryBtn: HTMLElement | null
  secondaryBtn: HTMLElement | null
  status: HTMLElement | null
} = {
  bentoBox: null,
  badge: null,
  title: null,
  paragraph: null,
  primaryBtn: null,
  secondaryBtn: null,
  status: null
}

function animateHero() {
  if (import.meta.server || hasAnimated) return

  const nuxtApp = useNuxtApp()
  const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
  const SplitText = nuxtApp.$SplitText as any
  
  if (!gsap) {
    hasAnimated = true
    return
  }
  
  nextTick(() => {
    const bentoBox = bentoBoxRef.value?.el || bentoBoxRef.value?.$el || bentoBoxRef.value
    const badge = badgeRef.value
    const titleWrapper = titleWrapperRef.value
    const titleComponent = titleRef.value as any
    const title = titleComponent?.el || titleComponent?.$el || (titleWrapper?.querySelector('h1, h2, h3, h4, h5, h6') as HTMLElement)
    const paragraphWrapper = paragraphWrapperRef.value
    const paragraph = paragraphRef.value
    const primaryBtn = primaryBtnRef.value
    const secondaryBtn = secondaryBtnRef.value
    const status = statusRef.value
    
    if (!bentoBox || !badge || !title || !paragraph) {
      setTimeout(() => animateHero(), 50)
      return
    }
    
    // Split title text with mask (like IntroLoader)
    let titleSplit: any = null
    try {
      if (SplitText) {
        // Ensure title maintains line-height before splitting
        gsap.set(title, { lineHeight: '0.9' })
        
        titleSplit = new SplitText(title, { 
          type: 'chars',
          mask: 'chars',
          smartWrap: true,
          charsClass: 'char++',
        })
        if (titleSplit.chars && titleSplit.chars.length > 0) {
          // For masked chars, use yPercent instead of y
          // Set line-height on each char to prevent changes
          titleSplit.chars.forEach((char: HTMLElement) => {
            gsap.set(char, {
              opacity: 0,
              yPercent: 120,
              rotationX: -90,
              lineHeight: '0.9'
            })
          })
        }
      }
    } catch (e) {
      // SplitText failed, continue without it
    }
    
    // Split paragraph into lines with mask
    let paragraphSplit: any = null
    try {
      if (SplitText && paragraph) {
        paragraphSplit = new SplitText(paragraph, { 
          type: 'lines',
          mask: 'lines',
          smartWrap: true,
          linesClass: 'line++',
        })
        if (paragraphSplit.lines && paragraphSplit.lines.length > 0) {
          // For masked lines, use yPercent
          gsap.set(paragraphSplit.lines, {
            opacity: 0,
            yPercent: 100
          })
        }
      }
    } catch (e) {
      // SplitText failed, continue without it
    }
    
    // Collect button boxes for animation
    const buttonBoxes = [
      primaryBtn,
      secondaryBtn
    ].filter(Boolean) as HTMLElement[]
    
    // Create master timeline (matching IntroLoader pattern)
    const tl = gsap.timeline()
    
    // 1. Container bounces in with scale and rotation (like IntroLoader bentos)
    gsap.set(bentoBox, {
      opacity: 0,
      scale: 0.9,
      rotation: -3
    })
    tl.to(bentoBox, {
        opacity: 1,
        scale: 1,
      rotation: 0,
      duration: 0.7,
      ease: 'back.out(1.4)'
    })
    
    // 2. Animate badge first (like IntroLoader name box) with bounce
    tl.fromTo(badge,
      {
        opacity: 0,
        scale: 0.8,
        y: 30,
        rotation: -5
      },
      {
        opacity: 1,
        scale: 1,
        y: 0,
        rotation: 0,
        duration: 0.6,
        ease: 'back.out(1.4)'
      }, '-=0.3')
    
    // 3. Animate title characters with mask reveal and 3D rotation (like IntroLoader name)
    if (titleSplit && titleSplit.chars) {
      // Make sure the parent title element becomes visible too
      if (title) {
        tl.to(title, { opacity: 1, duration: 0.001 }, '-=0.3')
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
      }, '-=0.3')
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
        }, '-=0.2')
    }
    
    // 4. Animate paragraph lines with mask reveal and stagger (like IntroLoader tagline)
    if (paragraphSplit && paragraphSplit.lines) {
      // Ensure paragraph parent is visible
      if (paragraph) {
        tl.to(paragraph, { opacity: 1, duration: 0.001 }, '-=0.25')
      }
      tl.to(paragraphSplit.lines, {
        opacity: 1,
        yPercent: 0,
        duration: 0.5,
        stagger: {
          amount: 0.3,
          from: 'start'
        },
        ease: 'power2.out'
      }, '-=0.3')
    } else if (paragraph) {
      // Fallback if SplitText not available
      tl.fromTo(paragraph,
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
    
    // 5. Animate button boxes with creative stagger (like IntroLoader other boxes)
    tl.fromTo(buttonBoxes,
      {
        opacity: 0,
        scale: 0.8,
        y: 30,
        rotation: -5
      },
      {
        opacity: 1,
        scale: 1,
        y: 0,
        rotation: 0,
        duration: 0.5,
        stagger: {
          amount: 0.4,
          from: 'start',
          ease: 'power2.out'
        },
        ease: 'back.out(1.2)'
      }, '-=0.3')
    
    // 6. Animate status with subtle delay
    if (status) {
      tl.fromTo(status,
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
    
    // Store split instances for exit animation (don't revert - we need them for exit)
    storedTitleSplit = titleSplit
    storedParagraphSplit = paragraphSplit
    
    // Store actual DOM elements for exit animation (Vue refs get cleaned up before onLeave)
    storedElements = {
      bentoBox,
      badge,
      title,
      paragraph,
      primaryBtn,
      secondaryBtn,
      status
    }
    console.log('[Hero] Stored DOM elements for exit')
  })
}

// Exit animation - mirrors the enter animation
function animateExit(): Promise<void> {
  console.log('[Hero] animateExit called')
  
  return new Promise((resolve) => {
    if (import.meta.server) {
      console.log('[Hero] SSR guard - resolving immediately')
      resolve()
      return
    }

    const nuxtApp = useNuxtApp()
    const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
    const SplitText = nuxtApp.$SplitText as any

    if (!gsap) {
      console.log('[Hero] No GSAP - resolving immediately')
      resolve()
      return
    }

    // Use stored DOM elements (Vue refs are already cleaned up by this point)
    const bentoBox = storedElements.bentoBox
    const badge = storedElements.badge
    const title = storedElements.title
    const paragraph = storedElements.paragraph
    const primaryBtn = storedElements.primaryBtn
    const secondaryBtn = storedElements.secondaryBtn
    const status = storedElements.status

    console.log('[Hero] Using stored elements:', { 
      bentoBox: !!bentoBox, 
      badge: !!badge, 
      title: !!title, 
      paragraph: !!paragraph,
      primaryBtn: !!primaryBtn,
      secondaryBtn: !!secondaryBtn,
      storedTitleSplit: !!storedTitleSplit,
      storedParagraphSplit: !!storedParagraphSplit
    })

    if (!bentoBox) {
      console.log('[Hero] No stored bentoBox - resolving immediately')
      resolve()
      return
    }

    const tl = gsap.timeline({ 
      onComplete: () => {
        console.log('[Hero] Exit animation timeline complete')
        resolve()
      }
    })

    // Collect buttons and icons
    const buttonBoxes = [primaryBtn, secondaryBtn].filter(Boolean) as HTMLElement[]

    // 1. Exit text first - mask away (text goes up through mask)
    // If we have stored splits, use them; otherwise create new ones
    let titleSplit = storedTitleSplit
    let paragraphSplit = storedParagraphSplit

    // Try to create splits if not stored
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

    if (!paragraphSplit && SplitText && paragraph) {
      try {
        paragraphSplit = new SplitText(paragraph, { 
          type: 'lines',
          mask: 'lines',
          smartWrap: true,
          linesClass: 'line++',
        })
      } catch (e) {
        paragraphSplit = null
      }
    }

    const contentDuration = 0.5
    const contentStagger = 0.3
    
    // Animate title chars out (mask away - goes up through mask)
    if (titleSplit && titleSplit.chars && titleSplit.chars.length > 0) {
      console.log('[Hero] Animating title with SplitText:', titleSplit.chars.length, 'chars')
      tl.to(titleSplit.chars, {
        yPercent: -120,
        rotationX: 90,
        autoAlpha: 0,
        duration: contentDuration,
        stagger: {
          amount: contentStagger,
          from: 'end'
        },
        ease: 'power2.in'
      }, 0)
    }
    // Always fade the title element too
    if (title) {
      tl.to(title, {
        autoAlpha: 0,
        duration: contentDuration,
        ease: 'power2.in'
      }, 0)
    }

    // Animate paragraph lines out (mask away)
    if (paragraphSplit && paragraphSplit.lines && paragraphSplit.lines.length > 0) {
      console.log('[Hero] Animating paragraph with SplitText:', paragraphSplit.lines.length, 'lines')
      tl.to(paragraphSplit.lines, {
        yPercent: -100,
        autoAlpha: 0,
        duration: contentDuration,
        stagger: {
          amount: 0.2,
          from: 'end'
        },
        ease: 'power2.in'
      }, 0)
    }
    // Always fade the paragraph element too
    if (paragraph) {
      tl.to(paragraph, {
        autoAlpha: 0,
        duration: contentDuration,
        ease: 'power2.in'
      }, 0)
    }

    // 2. Buttons and badge scale to 0 (simultaneously with text)
    if (buttonBoxes.length > 0) {
      console.log('[Hero] Animating buttons:', buttonBoxes.length)
      tl.to(buttonBoxes, {
        scale: 0,
        autoAlpha: 0,
        duration: contentDuration,
        stagger: 0.08,
        ease: 'back.in(1.7)'
      }, 0)
    }

    if (badge) {
      console.log('[Hero] Animating badge')
      tl.to(badge, {
        scale: 0,
        autoAlpha: 0,
        duration: contentDuration - 0.1,
        ease: 'back.in(1.7)'
      }, 0)
    }

    if (status) {
      console.log('[Hero] Animating status')
      tl.to(status, {
        y: -15,
        autoAlpha: 0,
        duration: contentDuration - 0.15,
        ease: 'power2.in'
      }, 0)
    }

    // 3. Container fades and scales - starts after content is mostly done
    const containerDelay = contentDuration * 0.85
    console.log('[Hero] Container will animate at:', containerDelay)
    
    tl.to(bentoBox, {
      autoAlpha: 0,
      scale: 0.85,
      y: -40,
      rotation: 5,
      duration: 0.5,
      ease: 'power2.in'
    }, containerDelay)

    // Cleanup splits after exit
    tl.call(() => {
      if (titleSplit && titleSplit.revert) {
        try { titleSplit.revert() } catch (e) {}
      }
      if (paragraphSplit && paragraphSplit.revert) {
        try { paragraphSplit.revert() } catch (e) {}
      }
      storedTitleSplit = null
      storedParagraphSplit = null
    })
  })
}

// Expose the exit animation method for page transitions
defineExpose({ animateExit })

// Watch for intro loader completion
watch(isIntroLoaderComplete, (complete) => {
  if (complete && !hasAnimated) {
    // Small delay after loader completes for smooth transition
    setTimeout(() => {
      animateHero()
    }, 150)
  }
})

onMounted(() => {
  // Register exit animation
  registerExitAnimation('hero', animateExit)
  
  // Store elements immediately as fallback
  nextTick(() => {
    if (!storedElements.bentoBox) {
      const bentoBox = bentoBoxRef.value?.el || bentoBoxRef.value?.$el || bentoBoxRef.value
      const badge = badgeRef.value
      const titleWrapper = titleWrapperRef.value
      const titleComponent = titleRef.value as any
      const title = titleComponent?.el || titleComponent?.$el || (titleWrapper?.querySelector('h1, h2, h3, h4, h5, h6') as HTMLElement)
      const paragraph = paragraphRef.value
      const primaryBtn = primaryBtnRef.value
      const secondaryBtn = secondaryBtnRef.value
      const status = statusRef.value
      
      if (bentoBox) {
        storedElements = { bentoBox, badge, title, paragraph, primaryBtn, secondaryBtn, status }
        console.log('[Hero] Stored elements on mount (fallback)')
      }
    }
  })
  
  // If loader is already complete, animate immediately
  if (isIntroLoaderComplete.value && !hasAnimated) {
    setTimeout(() => {
      animateHero()
    }, 150)
  }
})

onUnmounted(() => {
  unregisterExitAnimation('hero')
  
  // Reset state for clean re-mount
  hasAnimated = false
  storedTitleSplit = null
  storedParagraphSplit = null
  storedElements = {
    bentoBox: null,
    badge: null,
    title: null,
    paragraph: null,
    primaryBtn: null,
    secondaryBtn: null,
    status: null
  }
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
  min-height: 1.8em; /* Prevent height changes */
}

/* Title wrapper should maintain height */
[ref="titleWrapperRef"] {
  min-height: fit-content;
}
</style>
