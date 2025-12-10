<template>
  <UiBentoBox
    ref="bentoBoxRef"
    container
    backdrop
    border
    shadow
    padding="lg"
    rounded="2xl"
    :className="`text-center ${ctaData.background} opacity-0 scale-95 translate-y-6`"
  >
    <div class="flex flex-col gap-12 max-w-2xl mx-auto">
      <div class="flex flex-col items-center gap-6">
  <Icon :name="ctaData.icon" class="w-12 h-12 mx-auto mb-6 text-current opacity-0" />
      <UiTitle ref="titleRef" as="h2" size="md" weight="semibold" class="mb-3">

        {{ ctaData.title }}
          </UiTitle>    
      <p ref="descRef" class="text-base sm:text-lg leading-relaxed text-current/90 mb-8 opacity-0">
        {{ ctaData.description }}
      </p>

      </div>
      
      <!-- Newsletter Form -->
      <div ref="newsletterRef" class="w-full opacity-0 translate-y-4">
        <form @submit.prevent="handleNewsletterSubmit" class="flex flex-col sm:flex-row gap-3 w-full max-w-md mx-auto">
          <input
            v-model="newsletterEmail"
            type="email"
            required
            placeholder="Enter your email"
            class="flex-1 px-4 py-3 bg-background/50 dark:bg-background/50 backdrop-blur-sm border border-background/50 rounded-xl text-foreground placeholder:text-muted-foreground/50 focus:outline-none focus:ring-2 focus:ring-background/20 focus:border-background/50 transition-all"
          />
          <UiButton
            type="submit"
            variant="secondary"
            size="lg"
            :disabled="isSubmittingNewsletter"
            overlay-color="bg-gray-900 dark:bg-white"
            class="cta-newsletter-button bg-white dark:bg-gray-900 text-gray-900 dark:text-white border-transparent hover:bg-white/90 dark:hover:bg-gray-800 whitespace-nowrap"
          >
            <svg v-if="isSubmittingNewsletter" class="animate-spin w-5 h-5" fill="none" viewBox="0 0 24 24">
              <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" />
              <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4z" />
            </svg>
            <span v-else>Subscribe</span>
          </UiButton>
        </form>
        
        <!-- Success Message -->
        <div v-if="newsletterSuccess" class="mt-4 p-4 rounded-xl bg-success/10 dark:bg-success/10 border border-success/20 text-center">
          <div class="flex items-center justify-center gap-3">
            <span class="w-2 h-2 rounded-full bg-success" />
            <p class="text-success text-sm">{{ newsletterSuccess }}</p>
          </div>
        </div>
        
        <!-- Error Message -->
        <div v-if="newsletterError" class="mt-4 p-4 rounded-xl bg-destructive/10 dark:bg-destructive/10 border border-destructive/20 text-center">
          <div class="flex items-center justify-center gap-3">
            <span class="w-2 h-2 rounded-full bg-destructive" />
            <p class="text-destructive text-sm">{{ newsletterError }}</p>
          </div>
        </div>
      </div>
      
      <div ref="buttonsRef" class="flex flex-col sm:flex-row gap-4 justify-center">
        <UiButton
          :to="ctaData.primaryButton.to"
          variant="secondary"
          size="lg"
          overlay-color="bg-gray-900 dark:bg-white"
          class="cta-primary-button bg-white dark:bg-gray-900 text-gray-900 dark:text-white border-transparent hover:bg-white/90 dark:hover:bg-gray-800"
        >
          <Icon :name="ctaData.primaryButton.icon" class="w-5 h-5" />
          {{ ctaData.primaryButton.text }}
        </UiButton>
        <UiButton
          :to="ctaData.secondaryButton.to"
          variant="outline"
          size="lg"
          overlay-color="bg-gray-900 dark:bg-white"
          class="cta-outline-button bg-white/10 dark:bg-gray-900/20 border-2 border-white dark:border-gray-900 text-white dark:text-gray-900 hover:bg-white/20 dark:hover:bg-gray-900/30"
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
import { registerExitAnimation, unregisterExitAnimation } from '../../composables/usePageExitAnimations'
import { useGSAP } from '../../composables/useGSAP'

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
  exitAnimationKey?: string
}

const props = withDefaults(defineProps<Props>(), {
  exitAnimationKey: 'cta'
})

const config = useRuntimeConfig()
const apiBase = config.public.apiBase

const bentoBoxRef = ref<any>(null)
const titleRef = ref<HTMLElement | null>(null)
const descRef = ref<HTMLElement | null>(null)
const newsletterRef = ref<HTMLElement | null>(null)
const buttonsRef = ref<HTMLElement | null>(null)

const newsletterEmail = ref('')
const isSubmittingNewsletter = ref(false)
const newsletterSuccess = ref('')
const newsletterError = ref('')

let storedElements: { bento: HTMLElement | null; title: HTMLElement | null; desc: HTMLElement | null; newsletter: HTMLElement | null; buttons: HTMLElement | null } = {
  bento: null,
  title: null,
  desc: null,
  newsletter: null,
  buttons: null
}

const resolveEl = (maybeEl: any): HTMLElement | null => {
  if (!maybeEl) return null
  if (maybeEl instanceof HTMLElement) return maybeEl
  if ((maybeEl as any)?.$el instanceof HTMLElement) return (maybeEl as any).$el
  if ((maybeEl as any)?.el instanceof HTMLElement) return (maybeEl as any).el
  return null
}

// Newsletter form handler
const handleNewsletterSubmit = async () => {
  isSubmittingNewsletter.value = true
  newsletterError.value = ''
  newsletterSuccess.value = ''

  try {
    // TODO: Replace with actual newsletter API endpoint when available
    // For now, using contact endpoint as placeholder
    await $fetch(`${apiBase}/api/contact`, {
      method: 'POST',
      body: {
        name: 'Newsletter Subscriber',
        email: newsletterEmail.value,
        subject: 'newsletter',
        message: 'Newsletter subscription request'
      },
    })

    newsletterSuccess.value = 'Thanks for subscribing! Check your email for confirmation.'
    newsletterEmail.value = ''
    
    // Clear success message after 5 seconds
    setTimeout(() => {
      newsletterSuccess.value = ''
    }, 5000)
  } catch (error: any) {
    newsletterError.value = error?.data?.message || 'Failed to subscribe. Please try again.'
    
    // Clear error message after 5 seconds
    setTimeout(() => {
      newsletterError.value = ''
    }, 5000)
  } finally {
    isSubmittingNewsletter.value = false
  }
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
  
  const bentoBox = resolveEl(bentoBoxRef.value)
  const title = titleRef.value
  const desc = descRef.value
  const newsletter = newsletterRef.value
  const buttons = buttonsRef.value
  if (!bentoBox || !title) return

  // Check if element is already in viewport
  const rect = bentoBox.getBoundingClientRect()
  const isInViewport = rect.top < window.innerHeight * 0.8 && rect.bottom > 0

  readyGsap.set(bentoBox, { opacity: 0, scale: 0.95, y: 24 })
  readyGsap.set(title, { opacity: 0, y: 20 })
  if (desc) readyGsap.set(desc, { opacity: 0, y: 15 })
  if (newsletter) readyGsap.set(newsletter, { opacity: 0, y: 16 })
  if (buttons) readyGsap.set(buttons.children, { opacity: 0, scale: 0.8, y: 20, rotation: -3 })

  let titleSplit: any = null
  try {
    if (SplitText) {
      readyGsap.set(title, { lineHeight: '1' })
      titleSplit = new SplitText(title, { type: 'chars', mask: 'chars', smartWrap: true, charsClass: 'char++' })
      if (titleSplit.chars && titleSplit.chars.length > 0) {
        titleSplit.chars.forEach((char: HTMLElement) => {
          readyGsap.set(char, { opacity: 0, yPercent: 120, rotationX: -90, lineHeight: '1' })
        })
      }
    }
  } catch (e) {
    titleSplit = null
  }

  storedElements = { bento: bentoBox, title, desc, newsletter, buttons }

  // Check if element is already in viewport
  if (isInViewport) {
    const immediateTl = readyGsap.timeline()
    immediateTl.to(bentoBox, { opacity: 1, scale: 1, y: 0, duration: 0.6, ease: 'power3.out' })

    if (titleSplit && titleSplit.chars) {
      immediateTl.to(title, { opacity: 1, duration: 0.001 }, '-=0.35')
      immediateTl.to(titleSplit.chars, {
        opacity: 1,
        yPercent: 0,
        rotationX: 0,
        duration: 0.5,
        stagger: { amount: 0.3, from: 'start' },
        ease: 'power3.out'
      }, '-=0.3')
    } else {
      immediateTl.to(title, { opacity: 1, y: 0, duration: 0.6, ease: 'power2.out' }, '-=0.3')
    }

    if (desc) {
      immediateTl.to(desc, { opacity: 1, y: 0, duration: 0.6, ease: 'power2.out' }, '-=0.25')
    }

    if (newsletter) {
      immediateTl.to(newsletter, { opacity: 1, y: 0, duration: 0.6, ease: 'power2.out' }, '-=0.2')
    }

    if (buttons && buttons.children.length > 0) {
      immediateTl.to(Array.from(buttons.children) as HTMLElement[], {
        opacity: 1,
        scale: 1,
        y: 0,
        rotation: 0,
        duration: 0.6,
        stagger: 0.08,
        ease: 'back.out(1.4)'
      }, '-=0.3')
    }
  } else {
    // Use ScrollTrigger
    const tl = readyGsap.timeline({
      scrollTrigger: {
        trigger: bentoBox,
        start: 'top 80%',
        once: true,
        invalidateOnRefresh: true
      }
    })

    tl.to(bentoBox, { opacity: 1, scale: 1, y: 0, duration: 0.6, ease: 'power3.out' })

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
      tl.to(title, { opacity: 1, y: 0, duration: 0.6, ease: 'power2.out' }, '-=0.3')
    }

    if (desc) {
      tl.to(desc, { opacity: 1, y: 0, duration: 0.6, ease: 'power2.out' }, '-=0.25')
    }

    if (newsletter) {
      tl.to(newsletter, { opacity: 1, y: 0, duration: 0.6, ease: 'power2.out' }, '-=0.2')
    }

    if (buttons && buttons.children.length > 0) {
      tl.to(Array.from(buttons.children) as HTMLElement[], {
        opacity: 1,
        scale: 1,
        y: 0,
        rotation: 0,
        duration: 0.6,
        stagger: 0.08,
        ease: 'back.out(1.4)'
      }, '-=0.3')
    }
  }

  // Refresh ScrollTrigger after a short delay to ensure proper initialization
  if (readyST) {
    setTimeout(() => {
      readyST.refresh()
    }, 100)
  }
}

async function animateExit(): Promise<void> {
  if (import.meta.server) return
  
  const { gsap: readyGsap } = useGSAP()
  if (!readyGsap) return

  const bento = storedElements.bento || resolveEl(bentoBoxRef.value)
  const title = storedElements.title || titleRef.value
  const desc = storedElements.desc || descRef.value
  const newsletter = storedElements.newsletter || newsletterRef.value
  const buttons = storedElements.buttons || buttonsRef.value

  if (!bento) return

  return new Promise((resolve) => {
    const tl = readyGsap.timeline({ onComplete: () => resolve() })
    const contentDuration = 0.5

    if (title) tl.to(title, { y: -10, autoAlpha: 0, duration: contentDuration, ease: 'power2.in' }, 0)
    if (desc) tl.to(desc, { y: -8, autoAlpha: 0, duration: contentDuration - 0.05, ease: 'power2.in' }, 0)
    if (newsletter) tl.to(newsletter, { y: -8, autoAlpha: 0, duration: contentDuration - 0.05, ease: 'power2.in' }, 0)

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
  registerExitAnimation(props.exitAnimationKey, animateExit)
  setupScrollAnimation()
})

onUnmounted(() => {
  if (import.meta.server) return
  unregisterExitAnimation(props.exitAnimationKey)
  storedElements = { bento: null, title: null, desc: null, newsletter: null, buttons: null }
})
</script>

<style scoped>
/* Override button hover text colors for CTA */
/* Primary and Newsletter buttons - white text on dark overlay (light mode), dark text on white overlay (dark mode) */
:deep(.cta-primary-button.group:hover span),
:deep(.cta-newsletter-button.group:hover span) {
  color: rgb(255 255 255) !important; /* white for light mode - white text on dark overlay */
}

:deep(.dark .cta-primary-button.group:hover span),
:deep(.dark .cta-newsletter-button.group:hover span) {
  color: rgb(17 24 39) !important; /* gray-900 for dark mode - dark text on white overlay */
}

/* Outline button - dark text on white overlay (light mode), white text on dark overlay (dark mode) */
:deep(.cta-outline-button.group:hover span) {
  color: rgb(17 24 39) !important; /* gray-900 for light mode - dark text on white overlay */
}

:deep(.dark .cta-outline-button.group:hover span) {
  color: rgb(255 255 255) !important; /* white for dark mode - white text on dark overlay */
}
</style>
