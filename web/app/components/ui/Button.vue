<template>
  <!-- Icon Button Style (like social icons) -->
  <template v-if="iconStyle">
    <NuxtLink
      v-if="to && !href"
      :to="to"
      ref="buttonRef"
      :class="iconButtonClasses"
      v-bind="$attrs"
      @click="emit('click', $event)"
    >
      <div :class="iconBubbleClasses">
        <span :class="iconSizeClasses">
          <slot />
        </span>
      </div>
    </NuxtLink>
    <a
      v-else-if="href"
      :href="href"
      :target="target"
      ref="buttonRef"
      :class="iconButtonClasses"
      v-bind="$attrs"
      @click="emit('click', $event)"
    >
      <div :class="iconBubbleClasses">
        <span :class="iconSizeClasses">
          <slot />
        </span>
      </div>
    </a>
    <button
      v-else
      ref="buttonRef"
      :type="type"
      :disabled="disabled"
      :class="iconButtonClasses"
      v-bind="$attrs"
      @click="emit('click', $event)"
    >
      <div :class="iconBubbleClasses">
        <span :class="iconSizeClasses">
          <slot />
        </span>
      </div>
    </button>
  </template>
  
  <!-- Regular Button Style -->
  <template v-else>
    <NuxtLink
      v-if="to && !href"
      :to="to"
      ref="buttonRef"
      :class="buttonClasses"
      v-bind="$attrs"
      @click="emit('click', $event)"
    >
      <span
        ref="overlayRef"
        :class="`absolute inset-0 rounded-lg z-0 ${props.overlayColor || variantOverlayMap[props.variant] || variantOverlayMap.primary}`"
        :style="{ transform: 'scaleY(0)', transformOrigin: 'bottom center' }"
      />
      <span :class="`relative z-20 transition-colors ${variantHoverTextMap[props.variant] || variantHoverTextMap.primary}`">
        <slot />
      </span>
    </NuxtLink>
    <a
      v-else-if="href"
      :href="href"
      :target="target"
      ref="buttonRef"
      :class="buttonClasses"
      v-bind="$attrs"
      @click="emit('click', $event)"
    >
      <span
        ref="overlayRef"
        :class="`absolute inset-0 rounded-lg z-0 ${props.overlayColor || variantOverlayMap[props.variant] || variantOverlayMap.primary}`"
        :style="{ transform: 'scaleY(0)', transformOrigin: 'bottom center' }"
      />
      <span :class="`relative z-20 transition-colors ${variantHoverTextMap[props.variant] || variantHoverTextMap.primary}`">
        <slot />
      </span>
    </a>
    <button
      v-else
      ref="buttonRef"
      :type="type"
      :disabled="disabled"
      :class="buttonClasses"
      v-bind="$attrs"
      @click="emit('click', $event)"
    >
      <span
        ref="overlayRef"
        :class="`absolute inset-0 rounded-lg z-0 ${props.overlayColor || variantOverlayMap[props.variant] || variantOverlayMap.primary}`"
        :style="{ transform: 'scaleY(0)', transformOrigin: 'bottom center' }"
      />
      <span :class="`relative z-20 transition-colors ${variantHoverTextMap[props.variant] || variantHoverTextMap.primary}`">
        <slot />
      </span>
    </button>
  </template>
</template>

<script setup lang="ts">
defineOptions({ inheritAttrs: false })
import { ref, computed, onMounted, onUnmounted, nextTick } from 'vue'
import { useNuxtApp } from '#app'
import { twMerge } from 'tailwind-merge'

interface Props {
  to?: string
  href?: string
  target?: string
  type?: 'button' | 'submit' | 'reset'
  variant?: 'primary' | 'secondary' | 'outline' | 'ghost'
  size?: 'sm' | 'md' | 'lg'
  disabled?: boolean
  className?: string
  iconStyle?: boolean
  iconColor?: 'blue' | 'blue-600' | 'red' | 'yellow' | 'green' | 'purple' | 'gray' | 'foreground'
  overlayColor?: string // Custom overlay color class (e.g., 'bg-blue-500', 'bg-red-500')
}

const props = withDefaults(defineProps<Props>(), {
  type: 'button',
  variant: 'primary',
  size: 'md',
  disabled: false,
  className: '',
  iconStyle: false,
  iconColor: 'primary',
  overlayColor: undefined
})

const emit = defineEmits<{
  (e: 'click', event: MouseEvent): void
}>()

const buttonRef = ref<HTMLElement | null>(null)
const overlayRef = ref<HTMLElement | null>(null)
let cleanupFunctions: (() => void)[] = []

// Expose element ref for external animation access
defineExpose({ el: buttonRef })

const sizeClasses = computed(() => {
  const sizes = {
    sm: 'px-4 py-1.5 text-xs',
    md: 'px-6 py-2.5 text-sm',
    lg: 'px-8 py-3.5 text-base'
  }
  return sizes[props.size]
})

// Variant-based background colors (matching social icons pattern)
const variantBackgroundMap = {
  'primary': 'bg-primary/10 dark:bg-primary/5 hover:bg-primary/20 dark:hover:bg-primary/10',
  'secondary': 'bg-blue-500/10 dark:bg-blue-500/5 hover:bg-blue-500/20 dark:hover:bg-blue-500/10',
  'outline': 'bg-foreground/10 dark:bg-foreground/5 hover:bg-foreground/20 dark:hover:bg-foreground/10 border border-border/50',
  'ghost': 'bg-gray-800/10 dark:bg-gray-800/5 hover:bg-gray-800/20 dark:hover:bg-gray-800/10'
}

// Variant-based text colors for better visibility
const variantTextMap = {
  'primary': 'text-primary dark:text-primary',
  'secondary': 'text-blue-500 dark:text-blue-400',
  'outline': 'text-foreground',
  'ghost': 'text-foreground'
}

// Variant-based overlay colors (matching button background)
const variantOverlayMap = {
  'primary': 'bg-primary',
  'secondary': 'bg-blue-500',
  'outline': 'bg-foreground',
  'ghost': 'bg-gray-800'
}

// Variant-based hover text colors (for overlay visibility)
const variantHoverTextMap = {
  'primary': 'group-hover:text-white',
  'secondary': 'group-hover:text-white',
  'outline': 'group-hover:text-white dark:group-hover:text-white',
  'ghost': '' // Keep original text color for ghost
}

const buttonClasses = computed(() => {
  const baseClasses = 'group relative font-semibold uppercase tracking-wider rounded-lg inline-flex items-center justify-center overflow-hidden shrink-0 min-w-0 transition-colors'
  const backgroundClasses = variantBackgroundMap[props.variant] || variantBackgroundMap.primary
  const textClasses = variantTextMap[props.variant] || variantTextMap.primary
  
  return twMerge(
    baseClasses,
    sizeClasses.value,
    backgroundClasses,
    textClasses,
    props.disabled && 'opacity-50 cursor-not-allowed',
    props.className // This will override any conflicting classes
  )
})

// Icon button style classes (matching social icons - square container with rounded corners)
const iconButtonClasses = computed(() => {
  const colorMap = {
    'blue': 'bg-blue-500/10 dark:bg-blue-500/5 hover:bg-blue-500/20 dark:hover:bg-blue-500/10',
    'blue-600': 'bg-blue-600/10 dark:bg-blue-600/5 hover:bg-blue-600/20 dark:hover:bg-blue-600/10',
    'red': 'bg-red-500/10 dark:bg-red-500/5 hover:bg-red-500/20 dark:hover:bg-red-500/10',
    'yellow': 'bg-yellow-500/10 dark:bg-yellow-500/5 hover:bg-yellow-500/20 dark:hover:bg-yellow-500/10',
    'green': 'bg-green-500/10 dark:bg-green-500/5 hover:bg-green-500/20 dark:hover:bg-green-500/10',
    'purple': 'bg-purple-500/10 dark:bg-purple-500/5 hover:bg-purple-500/20 dark:hover:bg-purple-500/10',
    'gray': 'bg-gray-800/10 dark:bg-gray-800/5 hover:bg-gray-800/20 dark:hover:bg-gray-800/10',
    'foreground': 'bg-foreground/10 dark:bg-foreground/5 hover:bg-foreground/20 dark:hover:bg-foreground/10',
    'primary': 'bg-primary/10 dark:bg-primary/5 hover:bg-primary/20 dark:hover:bg-primary/10'
  }
  
  const sizeMap = {
    'sm': 'p-1.5',
    'md': 'p-2',
    'lg': 'p-2.5'
  }
  
  // Base classes
  const baseClasses = 'flex items-center justify-center transition-colors group rounded-lg'
  
  // Size padding
  const paddingClasses = sizeMap[props.size] || 'p-2'
  
  // Color classes
  const colorClasses = colorMap[props.iconColor] || colorMap.primary
  
  // Check if className has width/height or flex sizing classes
  const hasCustomSizing = props.className?.match(/\b(w-|h-|flex-1|flex-grow|aspect-)/) !== null
  
  // Default dimensions (only apply if className doesn't have sizing)
  const defaultDimensions = !hasCustomSizing 
    ? (props.size === 'sm' ? 'w-[44px] h-[44px]' : props.size === 'lg' ? 'w-[60px] h-[60px]' : 'w-[52px] h-[52px]')
    : ''
  
  // Merge all classes with twMerge - className prop will override defaults
  return twMerge(
    baseClasses,
    paddingClasses,
    defaultDimensions,
    colorClasses,
    props.disabled && 'opacity-50 cursor-not-allowed',
    props.className // This will override any conflicting classes
  )
})

const iconBubbleClasses = computed(() => {
  const colorMap = {
    'blue': 'bg-blue-500/20 dark:bg-blue-500/10 group-hover:bg-blue-500/30 dark:group-hover:bg-blue-500/20',
    'blue-600': 'bg-blue-600/20 dark:bg-blue-600/10 group-hover:bg-blue-600/30 dark:group-hover:bg-blue-600/20',
    'red': 'bg-red-500/20 dark:bg-red-500/10 group-hover:bg-red-500/30 dark:group-hover:bg-red-500/20',
    'yellow': 'bg-yellow-500/20 dark:bg-yellow-500/10 group-hover:bg-yellow-500/30 dark:group-hover:bg-yellow-500/20',
    'green': 'bg-green-500/20 dark:bg-green-500/10 group-hover:bg-green-500/30 dark:group-hover:bg-green-500/20',
    'purple': 'bg-purple-500/20 dark:bg-purple-500/10 group-hover:bg-purple-500/30 dark:group-hover:bg-purple-500/20',
    'gray': 'bg-gray-800/20 dark:bg-gray-800/10 group-hover:bg-gray-800/30 dark:group-hover:bg-gray-800/20',
    'foreground': 'bg-foreground/20 dark:bg-foreground/10 group-hover:bg-foreground/30 dark:group-hover:bg-foreground/20',
    'primary': 'bg-primary/20 dark:bg-primary/10 group-hover:bg-primary/30 dark:group-hover:bg-primary/20'
  }
  
  const sizeMap = {
    'sm': 'w-8 h-8',
    'md': 'w-10 h-10',
    'lg': 'w-12 h-12'
  }
  
  const baseClasses = 'rounded-full flex items-center justify-center transition-colors'
  const sizeClasses = sizeMap[props.size] || 'w-10 h-10'
  const colorClasses = colorMap[props.iconColor] || colorMap.primary
  
  return twMerge(
    baseClasses,
    sizeClasses,
    colorClasses
  )
})

const iconSizeClasses = computed(() => {
  const sizeMap = {
    'sm': 'w-4 h-4',
    'md': 'w-5 h-5',
    'lg': 'w-6 h-6'
  }
  
  const colorMap = {
    'blue': 'text-blue-500',
    'blue-600': 'text-blue-600',
    'red': 'text-red-500',
    'yellow': 'text-yellow-500 dark:text-yellow-400',
    'green': 'text-green-500',
    'purple': 'text-purple-500',
    'gray': 'text-foreground',
    'foreground': 'text-foreground',
    'primary': 'text-primary'
  }
  
  return [
    sizeMap[props.size] || 'w-5 h-5',
    colorMap[props.iconColor] || colorMap.primary
  ]
})

onMounted(() => {
  if (import.meta.server) return
  if (props.iconStyle) return // Skip animation for icon style buttons
  
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


