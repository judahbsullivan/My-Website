<template>
  <NuxtLink
    v-if="to && !href"
    :to="to"
    ref="buttonRef"
    :class="buttonClasses"
  >
    <span
      ref="overlayRef"
      class="absolute inset-0 bg-primary rounded-full"
      :style="{ transform: 'scaleY(0)', transformOrigin: 'bottom center' }"
    />
    <span class="relative z-10">
      <slot />
    </span>
  </NuxtLink>
  <a
    v-else-if="href"
    :href="href"
    :target="target"
    ref="buttonRef"
    :class="buttonClasses"
  >
    <span
      ref="overlayRef"
      class="absolute inset-0 bg-primary rounded-full"
      :style="{ transform: 'scaleY(0)', transformOrigin: 'bottom center' }"
    />
    <span class="relative z-10">
      <slot />
    </span>
  </a>
  <button
    v-else
    ref="buttonRef"
    :type="type"
    :disabled="disabled"
    :class="buttonClasses"
  >
    <span
      ref="overlayRef"
      class="absolute inset-0 bg-primary rounded-full"
      :style="{ transform: 'scaleY(0)', transformOrigin: 'bottom center' }"
    />
    <span class="relative z-10">
      <slot />
    </span>
  </button>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted, nextTick } from 'vue'
import { useNuxtApp } from '#app'

interface Props {
  to?: string
  href?: string
  target?: string
  type?: 'button' | 'submit' | 'reset'
  variant?: 'primary' | 'secondary' | 'outline' | 'ghost'
  size?: 'sm' | 'md' | 'lg'
  disabled?: boolean
  className?: string
}

const props = withDefaults(defineProps<Props>(), {
  type: 'button',
  variant: 'primary',
  size: 'md',
  disabled: false,
  className: ''
})

const buttonRef = ref<HTMLElement | null>(null)
const overlayRef = ref<HTMLElement | null>(null)
let cleanupFunctions: (() => void)[] = []

const sizeClasses = computed(() => {
  const sizes = {
    sm: 'px-4 py-1.5 text-xs',
    md: 'px-6 py-2.5 text-sm',
    lg: 'px-8 py-3.5 text-base'
  }
  return sizes[props.size]
})

const buttonClasses = computed(() => [
  'group relative bg-muted text-foreground font-semibold uppercase tracking-wider rounded-full inline-flex items-center justify-center overflow-hidden shrink-0 min-w-0',
  sizeClasses.value,
  { 'opacity-50 cursor-not-allowed': props.disabled },
  props.className
])

onMounted(() => {
  if (import.meta.server) return
  
  // Use nextTick to ensure DOM is fully rendered
  nextTick(() => {
    const nuxtApp = useNuxtApp()
    const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
    
    if (!gsap || !buttonRef.value || !overlayRef.value) {
      setTimeout(() => {
        if (buttonRef.value && overlayRef.value) {
          setupAnimation()
        }
      }, 100)
      return
    }
    
    setupAnimation()
  })
})

function setupAnimation() {
  if (import.meta.server) return
  
  const nuxtApp = useNuxtApp()
  const gsap = nuxtApp.$gsap as typeof import('gsap').gsap
  
  if (!gsap || !buttonRef.value || !overlayRef.value) return
  
  // Get the actual DOM element - NuxtLink might be a component instance
  // Get the actual DOM element - NuxtLink might be a component instance
  const button = buttonRef.value && (buttonRef.value instanceof HTMLElement
    ? buttonRef.value
    : (buttonRef.value as any)?.$el ?? buttonRef.value)
  const overlay = overlayRef.value

  if (!button || !overlay) return
  // Set initial state - overlay hidden at bottom
  gsap.set(overlay, {
    scaleY: 0,
    transformOrigin: 'bottom center'
  })
  
  // Enter animation - from bottom
  const enterAnimation = () => {
    gsap.to(overlay, {
      scaleY: 1,
      transformOrigin: 'bottom center',
      duration: 0.3,
      ease: 'power2.out'
    })
  }
  
  // Exit animation - to top, then reset to bottom
  const exitAnimation = () => {
    gsap.to(overlay, {
      scaleY: 0,
      transformOrigin: 'top center',
      duration: 0.3,
      ease: 'power2.in',
      onComplete: () => {
        // Reset to bottom after exit
        gsap.set(overlay, {
          transformOrigin: 'bottom center'
        })
      }
    })
  }
  
  if (button && typeof button.addEventListener === 'function') {
    button.addEventListener('mouseenter', enterAnimation)
    button.addEventListener('mouseleave', exitAnimation)
    
    // Store cleanup function
    cleanupFunctions.push(() => {
      if (button && typeof button.removeEventListener === 'function') {
        button.removeEventListener('mouseenter', enterAnimation)
        button.removeEventListener('mouseleave', exitAnimation)
      }
    })
  }
}

// Cleanup on unmount
onUnmounted(() => {
  cleanupFunctions.forEach(cleanup => cleanup())
  cleanupFunctions = []
})
</script>


