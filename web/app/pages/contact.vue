<template>
  <div class="min-h-screen pt-32 pb-20">
    <!-- Contact Content -->
    <section>
      <div class=" px-4 sm:px-6">
        <div ref="headerRef" class="mb-16 sm:mb-20 opacity-0">
          <div ref="titleWrapperRef" class="overflow-hidden">
            <UiTitle 
              ref="titleRef"
              as="h1" 
              size="5xl" 
              weight="bold" 
              align="left" 
              class="mb-6 opacity-0"
            >
            Get in Touch
          </UiTitle>
          </div>
          <div ref="descriptionWrapperRef" class="overflow-hidden">
            <p 
              ref="descriptionRef"
              class="text-lg sm:text-xl text-muted-foreground max-w-2xl leading-relaxed opacity-0 translate-y-2.5"
            >
            Have a project in mind? Let's work together to bring your ideas to life.
          </p>
          </div>
        </div>

        <div class="grid lg:grid-cols-2 gap-8 lg:gap-12">
          <!-- Contact Form -->
          <div 
            ref="formRef"
            class="bg-card/50 backdrop-blur-xl rounded-2xl p-6 sm:p-8 lg:p-10 border border-border/50 shadow-sm opacity-0 translate-y-6"
          >
            <div ref="formTitleWrapperRef" class="overflow-hidden mb-8">
              <UiTitle 
                ref="formTitleRef"
                as="h2" 
                size="md" 
                weight="semibold"
                class="opacity-0"
              >
                Send a Message
              </UiTitle>
            </div>
            <form @submit.prevent="handleSubmit" class="space-y-6">
              <div>
                <label for="name" class="block text-sm font-medium text-muted-foreground mb-3">
                  Name
                </label>
                <input id="name" v-model="form.name" type="text" required placeholder="Your name"
                  class="w-full px-4 py-3 bg-background/50 border border-border/50 rounded-xl text-foreground placeholder:text-muted-foreground/50 focus:outline-none focus:ring-2 focus:ring-primary/20 focus:border-primary/50 transition-all" />
              </div>

              <div>
                <label for="email" class="block text-sm font-medium text-muted-foreground mb-3">
                  Email
                </label>
                <input id="email" v-model="form.email" type="email" required placeholder="you@example.com"
                  class="w-full px-4 py-3 bg-background/50 border border-border/50 rounded-xl text-foreground placeholder:text-muted-foreground/50 focus:outline-none focus:ring-2 focus:ring-primary/20 focus:border-primary/50 transition-all" />
              </div>

              <div>
                <label for="subject" class="block text-sm font-medium text-muted-foreground mb-3">
                  Subject
                </label>
                <select id="subject" v-model="form.subject" required
                  class="w-full px-4 py-3 bg-background/50 border border-border/50 rounded-xl text-foreground focus:outline-none focus:ring-2 focus:ring-primary/20 focus:border-primary/50 transition-all">
                  <option value="" disabled>Select a subject</option>
                  <option value="project">Project Inquiry</option>
                  <option value="collaboration">Collaboration</option>
                  <option value="job">Job Opportunity</option>
                  <option value="other">Other</option>
                </select>
              </div>

              <div>
                <label for="message" class="block text-sm font-medium text-muted-foreground mb-3">
                  Message
                </label>
                <textarea id="message" v-model="form.message" required rows="5"
                  placeholder="Tell me about your project..."
                  class="w-full px-4 py-3 bg-background/50 border border-border/50 rounded-xl text-foreground placeholder:text-muted-foreground/50 focus:outline-none focus:ring-2 focus:ring-primary/20 focus:border-primary/50 transition-all resize-none" />
              </div>

              <!-- Success Message -->
              <div v-if="successMessage" class="p-4 rounded-xl bg-success/10 border border-success/20">
                <div class="flex items-center gap-3">
                  <span class="w-2 h-2 rounded-full bg-success" />
                  <p class="text-success text-sm">{{ successMessage }}</p>
                </div>
              </div>

              <!-- Error Message -->
              <div v-if="errorMessage" class="p-4 rounded-xl bg-destructive/10 border border-destructive/20">
                <div class="flex items-center gap-3">
                  <span class="w-2 h-2 rounded-full bg-destructive" />
                  <p class="text-destructive text-sm">{{ errorMessage }}</p>
                </div>
              </div>

              <UiButton type="submit" variant="primary" size="lg" :disabled="isSubmitting" class="w-full mt-2">
                <svg v-if="isSubmitting" class="animate-spin w-5 h-5" fill="none" viewBox="0 0 24 24">
                  <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" />
                  <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4z" />
                </svg>
                <span v-if="!isSubmitting">Send Message</span>
                <span v-else>Sending...</span>
              </UiButton>
            </form>
          </div>

          <!-- Contact Info -->
          <div class="space-y-6">
            <div 
              ref="contactInfoRef"
              class="bg-card/50 backdrop-blur-xl rounded-2xl p-6 sm:p-8 border border-border/50 shadow-sm opacity-0 translate-y-6"
            >
              <div ref="contactInfoTitleWrapperRef" class="overflow-hidden mb-6">
                <UiTitle 
                  ref="contactInfoTitleRef"
                  as="h3" 
                  size="md" 
                  weight="semibold"
                  class="opacity-0"
                >
                  Contact Information
                </UiTitle>
              </div>

              <div class="space-y-6">
                <div>
                  <p class="text-sm text-muted-foreground mb-2">Email</p>
                  <a href="mailto:hello@judahsullivan.com"
                    class="text-lg font-medium hover:text-primary transition-colors">
                    hello@judahsullivan.com
                  </a>
                </div>

                <div>
                  <p class="text-sm text-muted-foreground mb-4">Social</p>
                  <div class="flex gap-3">
                    <UiButton href="https://github.com/judahbsullivan" target="_blank" variant="primary" size="sm"
                      class="w-11! h-11! p-0!">
                      <svg class="w-5 h-5" fill="currentColor" viewBox="0 0 24 24">
                        <path
                          d="M12 0C5.37 0 0 5.37 0 12c0 5.31 3.435 9.795 8.205 11.385.6.105.825-.255.825-.57 0-.285-.015-1.23-.015-2.235-3.015.555-3.795-.735-4.035-1.41-.135-.345-.72-1.41-1.23-1.695-.42-.225-1.02-.78-.015-.795.945-.015 1.62.87 1.845 1.23 1.08 1.815 2.805 1.305 3.495.99.105-.78.42-1.305.765-1.605-2.67-.3-5.46-1.335-5.46-5.925 0-1.305.465-2.385 1.23-3.225-.12-.3-.54-1.53.12-3.18 0 0 1.005-.315 3.3 1.23.96-.27 1.98-.405 3-.405s2.04.135 3 .405c2.295-1.56 3.3-1.23 3.3-1.23.66 1.65.24 2.88.12 3.18.765.84 1.23 1.905 1.23 3.225 0 4.605-2.805 5.625-5.475 5.925.435.375.81 1.095.81 2.22 0 1.605-.015 2.895-.015 3.3 0 .315.225.69.825.57A12.02 12.02 0 0024 12c0-6.63-5.37-12-12-12z" />
                      </svg>
                    </UiButton>
                    <UiButton href="https://linkedin.com/in/judahsullivan" target="_blank" variant="primary" size="sm"
                      class="w-11! h-11! p-0!">
                      <svg class="w-5 h-5" fill="currentColor" viewBox="0 0 24 24">
                        <path
                          d="M20.447 20.452h-3.554v-5.569c0-1.328-.027-3.037-1.852-3.037-1.853 0-2.136 1.445-2.136 2.939v5.667H9.351V9h3.414v1.561h.046c.477-.9 1.637-1.85 3.37-1.85 3.601 0 4.267 2.37 4.267 5.455v6.286zM5.337 7.433c-1.144 0-2.063-.926-2.063-2.065 0-1.138.92-2.063 2.063-2.063 1.14 0 2.064.925 2.064 2.063 0 1.139-.925 2.065-2.064 2.065zm1.782 13.019H3.555V9h3.564v11.452zM22.225 0H1.771C.792 0 0 .774 0 1.729v20.542C0 23.227.792 24 1.771 24h20.451C23.2 24 24 23.227 24 22.271V1.729C24 .774 23.2 0 22.222 0h.003z" />
                      </svg>
                    </UiButton>
                    <UiButton href="https://twitter.com/judahsullivan" target="_blank" variant="primary" size="sm"
                      class="w-11! h-11! p-0!">
                      <svg class="w-5 h-5" fill="currentColor" viewBox="0 0 24 24">
                        <path
                          d="M18.244 2.25h3.308l-7.227 8.26 8.502 11.24H16.17l-5.214-6.817L4.99 21.75H1.68l7.73-8.835L1.254 2.25H8.08l4.713 6.231zm-1.161 17.52h1.833L7.084 4.126H5.117z" />
                      </svg>
                    </UiButton>
                  </div>
                </div>

                <div>
                  <p class="text-sm text-muted-foreground mb-2">Location</p>
                  <p class="text-lg font-medium">Available for Remote Work</p>
                  <p class="text-muted-foreground text-sm mt-1">Based in the United States</p>
                </div>

                <div class="pt-4 border-t border-border/50">
                  <div class="flex items-center gap-3">
                    <span class="w-2.5 h-2.5 rounded-full bg-success animate-pulse" />
                    <span class="text-base font-medium">Open to new opportunities</span>
                  </div>
                </div>
              </div>
            </div>

            <!-- FAQ -->
            <div 
              ref="faqRef"
              class="bg-card/50 backdrop-blur-xl rounded-2xl p-6 sm:p-8 border border-border/50 shadow-sm opacity-0 translate-y-6"
            >
              <div ref="faqTitleWrapperRef" class="overflow-hidden mb-6">
                <UiTitle 
                  ref="faqTitleRef"
                  as="h3" 
                  size="md" 
                  weight="semibold"
                  class="opacity-0"
                >
                  Quick Questions?
                </UiTitle>
              </div>
              <div class="space-y-5">
                <div class="flex justify-between items-center py-2 border-b border-border/30">
                  <span class="text-sm text-muted-foreground">Response time</span>
                  <span class="text-sm font-medium">24-48 hours</span>
                </div>
                <div class="flex justify-between items-center py-2 border-b border-border/30">
                  <span class="text-sm text-muted-foreground">Project types</span>
                  <span class="text-sm font-medium">Web apps, APIs, consulting</span>
                </div>
                <div class="flex justify-between items-center py-2">
                  <span class="text-sm text-muted-foreground">Tech stack</span>
                  <span class="text-sm font-medium">Vue/Nuxt, .NET, PostgreSQL</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted, watch, nextTick } from 'vue'
import { useNuxtApp } from '#app'
import { useIntroLoader } from '~/composables/useIntroLoader'
import { registerExitAnimation, unregisterExitAnimation, runAllExitAnimations } from '~/composables/usePageExitAnimations'

useHead({
  title: 'Contact | Judah Sullivan',
})

definePageMeta({
  pageTransition: {
    name: 'contact-page',
    mode: 'out-in',
    onLeave: async (_el: Element, done: () => void) => {
      if (import.meta.server) return done()
      try {
        await runAllExitAnimations()
      } catch (e) {
        console.warn('[Contact] Exit animation error:', e)
      }
      done()
    },
    onEnter: async (_el: Element, done: () => void) => {
      if (import.meta.server) return done()
      
      // Scroll to top immediately before enter animation begins
      const nuxtApp = useNuxtApp()
      const smoother = (nuxtApp.$scrollSmoother as any) || (typeof window !== 'undefined' && (window as any).ScrollSmoother?.get())
      
      if (smoother && typeof smoother.scrollTop === 'function') {
        smoother.scrollTop(0) // Scroll to top
      } else {
        window.scrollTo({ top: 0, left: 0, behavior: 'instant' })
      }
      
      // Refresh ScrollTrigger when page enters
      const { ScrollTrigger } = useGSAP()
        if (ScrollTrigger) {
        nextTick(() => {
            ScrollTrigger.refresh()
            setTimeout(() => ScrollTrigger.refresh(), 100)
            setTimeout(() => ScrollTrigger.refresh(), 300)
        })
        }
        done()
    }
  }
})

const config = useRuntimeConfig()

const { isIntroLoaderComplete } = useIntroLoader()

// Refs for header section
const headerRef = ref<HTMLElement | null>(null)
const titleWrapperRef = ref<HTMLElement | null>(null)
const titleRef = ref<any>(null)
const descriptionWrapperRef = ref<HTMLElement | null>(null)
const descriptionRef = ref<HTMLElement | null>(null)

// Refs for form section
const formRef = ref<HTMLElement | null>(null)
const formTitleWrapperRef = ref<HTMLElement | null>(null)
const formTitleRef = ref<any>(null)

// Refs for contact info section
const contactInfoRef = ref<HTMLElement | null>(null)
const contactInfoTitleWrapperRef = ref<HTMLElement | null>(null)
const contactInfoTitleRef = ref<any>(null)

// Refs for FAQ section
const faqRef = ref<HTMLElement | null>(null)
const faqTitleWrapperRef = ref<HTMLElement | null>(null)
const faqTitleRef = ref<any>(null)

let hasAnimated = false
let storedTitleSplits: any = {}
let storedElements: {
  header: HTMLElement | null
  title: HTMLElement | null
  description: HTMLElement | null
  form: HTMLElement | null
  formTitle: HTMLElement | null
  contactInfo: HTMLElement | null
  contactInfoTitle: HTMLElement | null
  faq: HTMLElement | null
  faqTitle: HTMLElement | null
} = {
  header: null,
  title: null,
  description: null,
  form: null,
  formTitle: null,
  contactInfo: null,
  contactInfoTitle: null,
  faq: null,
  faqTitle: null
}

const resolveEl = (maybeEl: any): HTMLElement | null => {
  if (!maybeEl) return null
  if (maybeEl instanceof HTMLElement) return maybeEl
  if ((maybeEl as any)?.$el instanceof HTMLElement) return (maybeEl as any).$el
  if ((maybeEl as any)?.el instanceof HTMLElement) return (maybeEl as any).el
  return null
}

function animateEnter() {
  if (import.meta.server || hasAnimated) return

  const nuxtApp = useNuxtApp()
  const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
  const SplitText = nuxtApp.$SplitText as any

  if (!gsap) {
    hasAnimated = true
    return
  }

  nextTick(() => {
    const header = headerRef.value
    const titleComponent = titleRef.value as any
    const title = titleComponent?.el || titleComponent?.$el || (titleWrapperRef.value?.querySelector('h1, h2, h3, h4, h5, h6') as HTMLElement)
    const description = descriptionRef.value
    const form = formRef.value
    const formTitleComponent = formTitleRef.value as any
    const formTitle = formTitleComponent?.el || formTitleComponent?.$el || (formTitleWrapperRef.value?.querySelector('h2, h3, h4') as HTMLElement)
    const contactInfo = contactInfoRef.value
    const contactInfoTitleComponent = contactInfoTitleRef.value as any
    const contactInfoTitle = contactInfoTitleComponent?.el || contactInfoTitleComponent?.$el || (contactInfoTitleWrapperRef.value?.querySelector('h3, h4') as HTMLElement)
    const faq = faqRef.value
    const faqTitleComponent = faqTitleRef.value as any
    const faqTitle = faqTitleComponent?.el || faqTitleComponent?.$el || (faqTitleWrapperRef.value?.querySelector('h3, h4') as HTMLElement)

    if (!header || !title || !description || !form || !formTitle || !contactInfo || !contactInfoTitle || !faq || !faqTitle) {
      setTimeout(() => animateEnter(), 50)
      return
    }

    // Setup SplitText for titles
    let titleSplit: any = null
    let formTitleSplit: any = null
    let contactInfoTitleSplit: any = null
    let faqTitleSplit: any = null
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
        storedTitleSplits.title = titleSplit
      }
    } catch (e) {}

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
        storedTitleSplits.description = descriptionSplit
      }
    } catch (e) {}

    try {
      if (SplitText && formTitle) {
        formTitleSplit = new SplitText(formTitle, {
          type: 'chars',
          mask: 'chars',
          smartWrap: true,
          charsClass: 'char++',
        })
        if (formTitleSplit.chars && formTitleSplit.chars.length > 0) {
          formTitleSplit.chars.forEach((char: HTMLElement) => {
            gsap.set(char, { opacity: 0, yPercent: 120, rotationX: -90 })
          })
        }
        storedTitleSplits.formTitle = formTitleSplit
      }
    } catch (e) {}

    try {
      if (SplitText && contactInfoTitle) {
        contactInfoTitleSplit = new SplitText(contactInfoTitle, {
          type: 'chars',
          mask: 'chars',
          smartWrap: true,
          charsClass: 'char++',
        })
        if (contactInfoTitleSplit.chars && contactInfoTitleSplit.chars.length > 0) {
          contactInfoTitleSplit.chars.forEach((char: HTMLElement) => {
            gsap.set(char, { opacity: 0, yPercent: 120, rotationX: -90 })
          })
        }
        storedTitleSplits.contactInfoTitle = contactInfoTitleSplit
      }
    } catch (e) {}

    try {
      if (SplitText && faqTitle) {
        faqTitleSplit = new SplitText(faqTitle, {
          type: 'chars',
          mask: 'chars',
          smartWrap: true,
          charsClass: 'char++',
        })
        if (faqTitleSplit.chars && faqTitleSplit.chars.length > 0) {
          faqTitleSplit.chars.forEach((char: HTMLElement) => {
            gsap.set(char, { opacity: 0, yPercent: 120, rotationX: -90 })
          })
        }
        storedTitleSplits.faqTitle = faqTitleSplit
      }
    } catch (e) {}

    storedElements = {
      header,
      title,
      description,
      form,
      formTitle,
      contactInfo,
      contactInfoTitle,
      faq,
      faqTitle
    }

    const tl = gsap.timeline()

    // Animate header
    tl.fromTo(header,
      { opacity: 0, y: 20 },
      { opacity: 1, y: 0, duration: 0.55, ease: 'power3.out' }
    )

    if (titleSplit && titleSplit.chars) {
      tl.to(title, { opacity: 1, duration: 0.001 }, '-=0.35')
      tl.to(titleSplit.chars, {
        opacity: 1,
        yPercent: 0,
        rotationX: 0,
        duration: 0.5,
        stagger: { amount: 0.4, from: 'start' },
        ease: 'power3.out'
      }, '-=0.25')
    } else if (title) {
      tl.fromTo(title,
        { opacity: 0, y: 20 },
        { opacity: 1, y: 0, duration: 0.4, ease: 'power2.out' },
        '-=0.2'
      )
    }

    if (descriptionSplit && descriptionSplit.lines) {
      tl.to(description, { opacity: 1, duration: 0.001 }, '-=0.3')
      tl.to(descriptionSplit.lines, {
        opacity: 1,
        yPercent: 0,
        duration: 0.5,
        stagger: { amount: 0.3, from: 'start' },
        ease: 'power2.out'
      }, '-=0.25')
    } else if (description) {
      tl.fromTo(description,
        { opacity: 0, y: 10 },
        { opacity: 1, y: 0, duration: 0.4, ease: 'power2.out' },
        '-=0.2'
      )
    }

    // Animate form
    tl.fromTo(form,
      { opacity: 0, y: 24 },
      { opacity: 1, y: 0, duration: 0.6, ease: 'power3.out' },
      '-=0.2'
    )

    if (formTitleSplit && formTitleSplit.chars) {
      tl.to(formTitle, { opacity: 1, duration: 0.001 }, '-=0.5')
      tl.to(formTitleSplit.chars, {
        opacity: 1,
        yPercent: 0,
        rotationX: 0,
        duration: 0.4,
        stagger: { amount: 0.3, from: 'start' },
        ease: 'power3.out'
      }, '-=0.4')
    } else if (formTitle) {
      tl.fromTo(formTitle,
        { opacity: 0, y: 10 },
        { opacity: 1, y: 0, duration: 0.4, ease: 'power2.out' },
        '-=0.4'
      )
    }

    // Animate contact info
    tl.fromTo(contactInfo,
      { opacity: 0, y: 24 },
      { opacity: 1, y: 0, duration: 0.6, ease: 'power3.out' },
      '-=0.3'
    )

    if (contactInfoTitleSplit && contactInfoTitleSplit.chars) {
      tl.to(contactInfoTitle, { opacity: 1, duration: 0.001 }, '-=0.5')
      tl.to(contactInfoTitleSplit.chars, {
        opacity: 1,
        yPercent: 0,
        rotationX: 0,
        duration: 0.4,
        stagger: { amount: 0.3, from: 'start' },
        ease: 'power3.out'
      }, '-=0.4'
      )
    } else if (contactInfoTitle) {
      tl.fromTo(contactInfoTitle,
        { opacity: 0, y: 10 },
        { opacity: 1, y: 0, duration: 0.4, ease: 'power2.out' },
        '-=0.4'
      )
    }

    // Animate FAQ
    tl.fromTo(faq,
      { opacity: 0, y: 24 },
      { opacity: 1, y: 0, duration: 0.6, ease: 'power3.out' },
      '-=0.3'
    )

    if (faqTitleSplit && faqTitleSplit.chars) {
      tl.to(faqTitle, { opacity: 1, duration: 0.001 }, '-=0.5')
      tl.to(faqTitleSplit.chars, {
        opacity: 1,
        yPercent: 0,
        rotationX: 0,
        duration: 0.4,
        stagger: { amount: 0.3, from: 'start' },
        ease: 'power3.out'
      }, '-=0.4'
      )
    } else if (faqTitle) {
      tl.fromTo(faqTitle,
        { opacity: 0, y: 10 },
        { opacity: 1, y: 0, duration: 0.4, ease: 'power2.out' },
        '-=0.4'
      )
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

    const header = storedElements.header || headerRef.value
    const titleComponent = titleRef.value as any
    const title = storedElements.title || titleComponent?.el || titleComponent?.$el || (titleWrapperRef.value?.querySelector('h1, h2, h3, h4, h5, h6') as HTMLElement)
    const description = storedElements.description || descriptionRef.value
    const form = storedElements.form || formRef.value
    const formTitleComponent = formTitleRef.value as any
    const formTitle = storedElements.formTitle || formTitleComponent?.el || formTitleComponent?.$el || (formTitleWrapperRef.value?.querySelector('h2, h3, h4') as HTMLElement)
    const contactInfo = storedElements.contactInfo || contactInfoRef.value
    const contactInfoTitleComponent = contactInfoTitleRef.value as any
    const contactInfoTitle = storedElements.contactInfoTitle || contactInfoTitleComponent?.el || contactInfoTitleComponent?.$el || (contactInfoTitleWrapperRef.value?.querySelector('h3, h4') as HTMLElement)
    const faq = storedElements.faq || faqRef.value
    const faqTitleComponent = faqTitleRef.value as any
    const faqTitle = storedElements.faqTitle || faqTitleComponent?.el || faqTitleComponent?.$el || (faqTitleWrapperRef.value?.querySelector('h3, h4') as HTMLElement)

    if (!header) return resolve()

    let titleSplit = storedTitleSplits.title
    let descriptionSplit = storedTitleSplits.description
    let formTitleSplit = storedTitleSplits.formTitle
    let contactInfoTitleSplit = storedTitleSplits.contactInfoTitle
    let faqTitleSplit = storedTitleSplits.faqTitle

    // Create splits if they don't exist
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

    if (!formTitleSplit && SplitText && formTitle) {
      try {
        formTitleSplit = new SplitText(formTitle, {
          type: 'chars',
          mask: 'chars',
          smartWrap: true,
          charsClass: 'char++',
        })
      } catch (e) {
        formTitleSplit = null
      }
    }

    if (!contactInfoTitleSplit && SplitText && contactInfoTitle) {
      try {
        contactInfoTitleSplit = new SplitText(contactInfoTitle, {
          type: 'chars',
          mask: 'chars',
          smartWrap: true,
          charsClass: 'char++',
        })
      } catch (e) {
        contactInfoTitleSplit = null
      }
    }

    if (!faqTitleSplit && SplitText && faqTitle) {
      try {
        faqTitleSplit = new SplitText(faqTitle, {
          type: 'chars',
          mask: 'chars',
          smartWrap: true,
          charsClass: 'char++',
        })
      } catch (e) {
        faqTitleSplit = null
      }
    }

    const tl = gsap.timeline({
      onComplete: () => {
        try { titleSplit?.revert?.() } catch (e) {}
        try { descriptionSplit?.revert?.() } catch (e) {}
        try { formTitleSplit?.revert?.() } catch (e) {}
        try { contactInfoTitleSplit?.revert?.() } catch (e) {}
        try { faqTitleSplit?.revert?.() } catch (e) {}
        resolve()
      }
    })

    const contentDuration = 0.5

    // Exit header
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

    // Exit form
    if (formTitleSplit && formTitleSplit.chars && formTitleSplit.chars.length > 0) {
      tl.to(formTitleSplit.chars, {
        yPercent: -120,
        rotationX: 90,
        autoAlpha: 0,
        duration: contentDuration,
        stagger: { amount: 0.2, from: 'end' },
        ease: 'power2.in'
      }, 0)
    }
    if (formTitle) {
      tl.to(formTitle, { autoAlpha: 0, duration: contentDuration, ease: 'power2.in' }, 0)
    }

    // Exit contact info
    if (contactInfoTitleSplit && contactInfoTitleSplit.chars && contactInfoTitleSplit.chars.length > 0) {
      tl.to(contactInfoTitleSplit.chars, {
        yPercent: -120,
        rotationX: 90,
        autoAlpha: 0,
        duration: contentDuration,
        stagger: { amount: 0.2, from: 'end' },
        ease: 'power2.in'
      }, 0)
    }
    if (contactInfoTitle) {
      tl.to(contactInfoTitle, { autoAlpha: 0, duration: contentDuration, ease: 'power2.in' }, 0)
    }

    // Exit FAQ
    if (faqTitleSplit && faqTitleSplit.chars && faqTitleSplit.chars.length > 0) {
      tl.to(faqTitleSplit.chars, {
        yPercent: -120,
        rotationX: 90,
        autoAlpha: 0,
        duration: contentDuration,
        stagger: { amount: 0.2, from: 'end' },
        ease: 'power2.in'
      }, 0)
    }
    if (faqTitle) {
      tl.to(faqTitle, { autoAlpha: 0, duration: contentDuration, ease: 'power2.in' }, 0)
    }

    // Exit containers
    const containerDelay = contentDuration * 0.85
    if (form) {
      tl.to(form, {
        autoAlpha: 0,
        scale: 0.95,
        y: -20,
        duration: 0.45,
        ease: 'power2.in'
      }, containerDelay)
    }
    if (contactInfo) {
      tl.to(contactInfo, {
        autoAlpha: 0,
        scale: 0.95,
        y: -20,
        duration: 0.45,
        ease: 'power2.in'
      }, containerDelay)
    }
    if (faq) {
      tl.to(faq, {
        autoAlpha: 0,
        scale: 0.95,
        y: -20,
        duration: 0.45,
        ease: 'power2.in'
      }, containerDelay)
    }
    if (header) {
      tl.to(header, {
        autoAlpha: 0,
        scale: 0.9,
        y: -20,
        rotation: 2,
        duration: 0.45,
        ease: 'power2.in'
      }, containerDelay)
    }
  })
}

watch(isIntroLoaderComplete, (complete) => {
  if (complete && !hasAnimated) {
    setTimeout(() => {
      animateEnter()
    }, 150)
  }
})

onMounted(() => {
  registerExitAnimation('contactPage', animateExit)
  nextTick(() => {
    if (!storedElements.header && headerRef.value) {
      const titleComponent = titleRef.value as any
      const title = titleComponent?.el || titleComponent?.$el || (titleWrapperRef.value?.querySelector('h1, h2, h3, h4, h5, h6') as HTMLElement)
      storedElements = {
        header: headerRef.value,
        title,
        description: descriptionRef.value,
        form: formRef.value,
        formTitle: formTitleRef.value?.el || formTitleRef.value?.$el || (formTitleWrapperRef.value?.querySelector('h2, h3, h4') as HTMLElement),
        contactInfo: contactInfoRef.value,
        contactInfoTitle: contactInfoTitleRef.value?.el || contactInfoTitleRef.value?.$el || (contactInfoTitleWrapperRef.value?.querySelector('h3, h4') as HTMLElement),
        faq: faqRef.value,
        faqTitle: faqTitleRef.value?.el || faqTitleRef.value?.$el || (faqTitleWrapperRef.value?.querySelector('h3, h4') as HTMLElement)
      }
    }
  })

  if (isIntroLoaderComplete.value && !hasAnimated) {
    setTimeout(() => {
      animateEnter()
    }, 150)
  }
})

onUnmounted(() => {
  unregisterExitAnimation('contactPage')
  hasAnimated = false
  storedTitleSplits = {}
  storedElements = {
    header: null,
    title: null,
    description: null,
    form: null,
    formTitle: null,
    contactInfo: null,
    contactInfoTitle: null,
    faq: null,
    faqTitle: null
  }
})

const form = reactive({
  name: '',
  email: '',
  subject: '',
  message: '',
})

const isSubmitting = ref(false)
const successMessage = ref('')
const errorMessage = ref('')

const handleSubmit = async () => {
  isSubmitting.value = true
  errorMessage.value = ''
  successMessage.value = ''

  try {
    await $fetch(`${config.public.apiBase}/api/contact`, {
      method: 'POST',
      body: form,
    })

    successMessage.value = "Message sent successfully! I'll get back to you soon."

    // Reset form
    form.name = ''
    form.email = ''
    form.subject = ''
    form.message = ''
  } catch (error: any) {
    errorMessage.value = error?.data?.message || 'Failed to send message. Please try again.'
  } finally {
    isSubmitting.value = false
  }
}

const ctaData = {
  icon: 'heroicons:envelope',
  title: 'Ready to Start a Conversation?',
  description: 'Have a project in mind or just want to chat? I\'d love to hear from you.',
  background: 'bg-foreground dark:bg-foreground/90 text-secondary dark:text-secondary',
  primaryButton: {
    text: 'View My Work',
    to: '/projects',
    icon: 'heroicons:folder'
  },
  secondaryButton: {
    text: 'Read My Blog',
    to: '/blog',
    icon: 'heroicons:document-text'
  }
}
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
